using Microsoft.AspNetCore.Mvc;
using QuickBite.Models;

namespace QuickBite.Controllers
{
    public class CustomerController : Controller
    {
        private myContext _context;
       
        public CustomerController(myContext context)
        {
            _context = context;
            

        }
        public IActionResult Index()
        {
            List<Category> category = _context.tbl_Category.ToList();
            ViewData["category"] = category;
            return View();
        }
    }
}
