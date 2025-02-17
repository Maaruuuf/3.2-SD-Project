using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickBite.Models;

namespace QuickBite.Controllers
{
    public class AdminController : Controller
    {
        private myContext _context;
        private IWebHostEnvironment _env;
        public AdminController(myContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public IActionResult Index()
        {
           string admin_session = HttpContext.Session.GetString("admin_session");
            if (admin_session != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login");
            }
            
        }

        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login(string adminEmail, string adminPassword)
        {
           var row =  _context.tbl_Admin.FirstOrDefault(a => a.admin_email == adminEmail);
            if(row != null && row.admin_password == adminPassword)
            {
                HttpContext.Session.SetString("admin_session",row.admin_id.ToString());
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.message = "Incorrect Username or password";
                return View();
            }
           
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("admin_session");
            return RedirectToAction("login");
        }

        public IActionResult Profile()
        {
           var adminId =  HttpContext.Session.GetString("admin_session");
            var row = _context.tbl_Admin.Where(a=>a.admin_id == int.Parse(adminId)).ToList();
            return View(row);
        }

        [HttpPost]
        public IActionResult Profile(Admin admin)
        {
            _context.tbl_Admin.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }
        [HttpPost]
        public IActionResult ChangeProfileImage(IFormFile admin_image,Admin admin)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "admin_image", admin_image.FileName);
            FileStream fs = new FileStream(ImagePath,FileMode.Create);
            admin_image.CopyTo(fs);
            admin.admin_image = admin_image.FileName;
            _context.tbl_Admin.Update(admin);
            _context.SaveChanges();
            return RedirectToAction("Profile");
        }
        public IActionResult fetchCustomer()
        {
            return View(_context.tbl_Customer.ToList());
        }
        public IActionResult customerDetails(int id)
        {
            return View(_context.tbl_Customer.FirstOrDefault(c => c.Customer_id == id));
        }
        public IActionResult updateCustomer(int id)
        {
            return View(_context.tbl_Customer.Find(id));
        }
        public IActionResult deleteCustomer(int id)
        {
            var customer = _context.tbl_Customer.Find(id);
            _context.tbl_Customer.Remove(customer);
            _context.SaveChanges();
            return RedirectToAction("fetchCustomer");
        }

    }
}
