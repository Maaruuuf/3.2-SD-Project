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
        [HttpPost]
        public IActionResult updateCustomer(Customer customer,IFormFile customer_image)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "customer_images",customer_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            customer_image.CopyTo(fs);
            customer.Customer_image = customer_image.FileName;
            _context.tbl_Customer.Update(customer);
            _context.SaveChanges();
            return RedirectToAction("fetchCustomer");
        }
        public IActionResult deletePermission(int id)
        {
            return View(_context.tbl_Customer.FirstOrDefault(c => c.Customer_id == id));
        }
        public IActionResult deleteCustomer(int id)
        {
            var customer = _context.tbl_Customer.Find(id);
            _context.tbl_Customer.Remove(customer);
            _context.SaveChanges();
            return RedirectToAction("fetchCustomer");
        }

        public IActionResult fetchCategory()
        {
            return View(_context.tbl_Category.ToList());
        }
        public IActionResult addCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult addCategory(Category cat)
        {
            _context.tbl_Category.Add(cat);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }

        public IActionResult updateCategory(int id)
        {
            var category = _context.tbl_Category.Find(id);
            return View(category);
        }
        [HttpPost]
        public IActionResult updateCategory(Category cat)
        {
            _context.tbl_Category.Update(cat);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }
        public IActionResult deletePermissionCategory(int id)
        {
            return View(_context.tbl_Category.FirstOrDefault(c => c.category_id == id));
        }
        public IActionResult deleteCategory(int id)
        {
            var category = _context.tbl_Category.Find(id);
            _context.tbl_Category.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("fetchCategory");
        }

        public IActionResult fetchProduct()
        {  
            return View(_context.tbl_Product.ToList());
        }

        public IActionResult addProduct()
        {
            List<Category>categories = _context.tbl_Category.ToList();
            ViewData["category"] = categories;
            return View();
        }
        [HttpPost]
        public IActionResult addProduct(Product prod,IFormFile product_image)
        {
            string imageName = Path.GetFileName(product_image.FileName);
            string imagePath = Path.Combine(_env.WebRootPath,"product_images",imageName);
            FileStream fs = new FileStream(imagePath,FileMode.Create);
            product_image.CopyTo(fs);
            prod.product_image = imageName;



            _context.tbl_Product.Add(prod);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");
        }
        public IActionResult productDetails(int id)
        {
            return View(_context.tbl_Product.Include(p=>p.category).FirstOrDefault(p => p.product_id == id));
        }
        public IActionResult deletePermissionProduct(int id)
        {
            return View(_context.tbl_Product.FirstOrDefault(p =>p.product_id == id));
        }
        public IActionResult deleteProduct(int id)
        {
            var product = _context.tbl_Product.Find(id);
            _context.tbl_Product.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");
        }
        public IActionResult updateProduct(int id)
        {
            List<Category> categories = _context.tbl_Category.ToList();
            ViewData["category"] = categories;

            var product = _context.tbl_Product.Find(id);
            ViewBag.selectedCategoryId = product.cat_id;
            return View(product);
        }
        [HttpPost]
        public IActionResult updateProduct(Product product)
        {
            _context.tbl_Product.Update(product);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");
        }
        [HttpPost]
        public IActionResult ChangeProductImage(IFormFile product_image, Product product)
        {
            string ImagePath = Path.Combine(_env.WebRootPath, "product_images", product_image.FileName);
            FileStream fs = new FileStream(ImagePath, FileMode.Create);
            product_image.CopyTo(fs);

            product.product_image = product_image.FileName;
            _context.tbl_Product.Update(product);
            _context.SaveChanges();
            return RedirectToAction("fetchProduct");
        }

        public IActionResult fetchFeedback()
        {

            return View(_context.tbl_Feedback.ToList());
        }

        public IActionResult deletePermissionFeedback(int id)
        {
            return View(_context.tbl_Feedback.FirstOrDefault(f => f.feedback_id == id));
        }
       public IActionResult deleteFeedback(int id)
        {
            var feedback = _context.tbl_Feedback.Find(id);
            _context.tbl_Feedback.Remove(feedback);
            _context.SaveChanges();
            return RedirectToAction("fetchFeedback");
        }
        public IActionResult fetchCart()
        {
            var cart = _context.tbl_Cart.Include(c => c.products).Include(c => c.customers).ToList();
            return View(cart);
        }
        public IActionResult deletePermissionCart(int id)
        {
            return View(_context.tbl_Cart.FirstOrDefault(c => c.cart_id == id));
        }
        public IActionResult deleteCart(int id)
        {
            var cart = _context.tbl_Cart.Find(id);
            _context.tbl_Cart.Remove(cart);
            _context.SaveChanges();
            return RedirectToAction("fetchCart");
        }
    }
}
