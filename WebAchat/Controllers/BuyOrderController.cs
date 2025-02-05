using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAchat.Data;
using WebAchat.Models;
using WebAchat.Models.ViewModels;

namespace WebAchat.Controllers
{
    [Authorize(Roles = "PurchaseManager")]
    public class BuyOrderController : Controller
    {
        private readonly ApplicationDbContext _tempContext;
        private readonly ErpDbContext _mainContext;
        private const string SessionKey = "CurrentTempOrderId";

        public BuyOrderController(ApplicationDbContext tempContext, ErpDbContext mainContext)
        {
            _tempContext = tempContext;
            _mainContext = mainContext;
        }
        private TempBuyOrder GetCurrentOrder()
        {
            var orderId = HttpContext.Session.GetInt32(CurrentOrderKey);
            return orderId.HasValue
                ? _tempDb.TempBuyOrders
                    .Include(o => o.Products)
                    .ThenInclude(p => p.TempProduct)
                    .FirstOrDefault(o => o.TempBuyOrderId == orderId.Value)
                : null;
        }
        public IActionResult CurrentOrder()
        {
            var order = GetCurrentOrder();
            var vm = new OrderViewModel();

            if (order != null)
            {
                vm.TempOrderId = order.TempBuyOrderId;
                vm.Date = order.CreatedDate;

                foreach (var op in order.OrderProducts
                    .Include(op => op.TempProduct)
                    .ToList())
                {
                    if (op.ProductId.HasValue)
                    {
                        var product = _mainContext.Products
                            .Include(p => p.Category)
                            .Include(p => p.Manufacturer)
                            .First(p => p.ProductId == op.ProductId);

                        vm.Items.Add(new OrderProductViewModel
                        {
                            TempOrderProductId = op.TempOrderProductId,
                            ProductName = product.Name,
                            Price = product.Price,
                            Quantity = op.Quantity,
                            IsTemporary = false
                        });
                    }
                    else if (op.TempProductId.HasValue)
                    {
                        vm.Items.Add(new OrderProductViewModel
                        {
                            TempOrderProductId = op.TempOrderProductId,
                            ProductName = $"[NEW] {op.TempProduct.Name}",
                            Price = op.TempProduct.Price,
                            Quantity = op.Quantity,
                            IsTemporary = true
                        });
                    }
                }
                vm.TotalAmount = vm.Items.Sum(i => i.Total);
            }

            ViewBag.Suppliers = _mainContext.Suppliers.ToList();
            ViewBag.Categories = _mainContext.Categories.ToList();
            ViewBag.Manufacturers = _mainContext.Manufacturers.ToList();

            return View(vm);
        }

        [HttpPost]
        public IActionResult ValidateOrder(int supplierId)
        {
            var tempOrder = GetCurrentOrder();
            if (tempOrder == null || !tempOrder.OrderProducts.Any())
            {
                TempData["Error"] = "Cannot validate empty order";
                return RedirectToAction("CurrentOrder");
            }

            // Create main order
            var buyOrder = new WebAchat.Models.Order
            {
                SupplierId = supplierId,
                CreationDate = DateTime.Now,
                Status = StockManagement.Models.OrderStatus.Pending,
                Type = StockManagement.Models.OrderType.Purchase,
                TotalAmount = tempOrder.OrderProducts.Sum(op =>
                    op.ProductId.HasValue ?
                    _mainContext.Products.Find(op.ProductId).Price * op.Quantity :
                    op.TempProduct.Price * op.Quantity)
            };

            _mainContext.Orders.Add(buyOrder);
            _mainContext.SaveChanges();

            // Process order products
            foreach (var tempOp in tempOrder.OrderProducts
                .Include(op => op.TempProduct)
                .ToList())
            {
                int finalProductId;

                if (tempOp.ProductId.HasValue)
                {
                    finalProductId = tempOp.ProductId.Value;
                }
                else
                {
                    // Convert temp product to real product
                    var newProduct = new StockManagement.Models.Product
                    {
                        Name = tempOp.TempProduct.Name,
                        Description = tempOp.TempProduct.Description,
                        Price = tempOp.TempProduct.Price,
                        StockQuantity = 0,
                        CategoryId = tempOp.TempProduct.CategoryId,
                        ManufacturerId = tempOp.TempProduct.ManufacturerId
                    };
                    _mainContext.Products.Add(newProduct);
                    _mainContext.SaveChanges();
                    finalProductId = newProduct.ProductId;
                }

                _mainContext.OrderProducts.Add(new StockManagement.Models.OrderProducts
                {
                    OrderId = buyOrder.OrderId,
                    ProductId = finalProductId,
                    Quantity = tempOp.Quantity
                });
            }

            _mainContext.SaveChanges();
            ClearCurrentOrder();
            return RedirectToAction("OrderConfirmed", new { id = buyOrder.OrderId });
        }

        // Rest of the controller remains the same as previous version
        // ... [Other action methods]
    }
}