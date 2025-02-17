using Microsoft.EntityFrameworkCore;

namespace QuickBite.Models
{
    public class myContext : DbContext
    {
        public myContext(DbContextOptions<myContext> options) : base(options)
        {
        }
        public DbSet<Admin> tbl_Admin { get; set; }
        public DbSet<Customer> tbl_Customer { get; set; }
        public DbSet<Category> tbl_Category { get; set; }
        public DbSet<Product> tbl_Product { get; set; }
        public DbSet<Cart> tbl_Cart { get; set; }
        public DbSet<Feedback> tbl_Feedback { get; set; }
        public DbSet<Faqs> tbl_Faqs { get; set; }



    }
}
