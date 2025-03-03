using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using QuickBite.Models;

namespace QuickBite.Controllers
{
    public class CustomerController : Controller
    {
        private myContext _context;
        private IWebHostEnvironment _env;

        public CustomerController(myContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Category> category = _context.tbl_Category.ToList();
            ViewData["category"] = category;

            List<Product> products = _context.tbl_Product.ToList();
            ViewData["product"] = products;

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
        [HttpPost]
        public IActionResult updateCustomerProfile(Customer customer)
        {
            _context.tbl_Customer.Update(customer);
            _context.SaveChanges();
            return RedirectToAction("customerProfile");
        }

        public IActionResult changeProfileImage(Customer customer, IFormFile customer_image)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "customer_images", customer_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            customer_image.CopyTo(fs);
            customer.Customer_image = customer_image.FileName;
            _context.tbl_Customer.Update(customer);
            _context.SaveChanges();
            return RedirectToAction("customerProfile");
        }

        public IActionResult feedback()
        {
            List<Category> category = _context.tbl_Category.ToList();
            ViewData["category"] = category;
            return View();
        }
        [HttpPost]
        public IActionResult feedback(Feedback feedback)
        {
            TempData["messege"] = "Thank you for your feedback";
            _context.tbl_Feedback.Add(feedback);
            _context.SaveChanges();
            return RedirectToAction("feedback");
        }

        public IActionResult fetchAllProducts()
        {
            List<Category> category = _context.tbl_Category.ToList();
            ViewData["category"] = category;

            List<Product> products = _context.tbl_Product.ToList();
            ViewData["product"] = products;

            return View();
        }

        public IActionResult productDetails(int id)
        {
            List<Category> category = _context.tbl_Category.ToList();
            ViewData["category"] = category;

            var products = _context.tbl_Product.Where(p => p.product_id == id).ToList();

            return View(products);
        }

        public IActionResult AddToCart(int prod_id, Cart cart)
        {
            string isLogin = HttpContext.Session.GetString("customerSession");
            if (isLogin != null)
            {
                cart.prod_id = prod_id;
                cart.cust_id = int.Parse(isLogin);
                cart.product_quantity = 1;
                cart.cart_status = 0;
                _context.tbl_Cart.Add(cart);
                _context.SaveChanges();
                TempData["message"] = "Product Successfully Added in Cart";
                return RedirectToAction("fetchAllProducts");
            }
            else
            {
                return RedirectToAction("customerLogin");

            }
        }
        







    }
}
