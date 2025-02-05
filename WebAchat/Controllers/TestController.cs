using Microsoft.AspNetCore.Mvc;
using WebAchat.Models;

namespace WebAchat.Controllers;

public class TestController : Controller

{
    private readonly ErpDbContext _db = new ErpDbContext();
    
    
    public IActionResult Index()
    {
        var client = _db.Clients.FirstOrDefault();
        return Json(client);
    }
    
    
}