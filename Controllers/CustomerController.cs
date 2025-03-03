using System.Drawing;
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
            ViewBag.checkSession = HttpContext.Session.GetString("customerSession");
            return View();
        }
        public IActionResult customerLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult customerLogin(String customerEmail, String customerPassword)
        {
            var customer = _context.tbl_Customer.FirstOrDefault(c => c.Customer_email == customerEmail);
            if (customer != null && customer.Customer_password == customerPassword)
            {
                HttpContext.Session.SetString("customerSession",
                   customer.Customer_id.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.message = "Incorrect Username or Password";
                return View();
            }
        }
        public IActionResult customerRegistration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult customerRegistration(Customer customer)
        {
            _context.tbl_Customer.Add(customer);
            _context.SaveChanges();
            return RedirectToAction("customerLogin");
        }
        public IActionResult customerLogout()
        {
            HttpContext.Session.Remove("customerSession");
            return RedirectToAction("Index");
        }
        public IActionResult customerProfile()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("customerSession")))
            {
                return RedirectToAction("customerLogin");
            }
            else
            {
                List<Category> category = _context.tbl_Category.ToList();
                ViewData["category"] = category;
                var customerId = HttpContext.Session.GetString("customerSession");
                var row = _context.tbl_Customer.Where(c => c.Customer_id == int.Parse(customerId)).ToList();
                return View(row);

            }
        }


    }
}
