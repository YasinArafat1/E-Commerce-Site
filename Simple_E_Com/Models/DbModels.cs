using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_E_Com.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; } = default!;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        public int Role { get; set; }
        public int IsActive { get; set; }

    }
    public class Product
    {
        public Product()
        {
            this.SalesItems = new List<SalesItem>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string Unit { get; set; } = default!;
        public double Price { get; set; }
        public double Quantity { get; set; }
        public string? Image { get; set; }= default!;
        public virtual ICollection<SalesItem> SalesItems { get; set; }
    }
    public class Customer
    {
        public Customer()
        {
            this.SalesOrders = new List<SalesOrder>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Mobile { get; set; } = default!;
        public string Address { get; set; } = default!;
        //nev
        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
    public class SalesOrder
    {
        public SalesOrder()
        {
            this.SalesItems = new List<SalesItem>();
        }

        public int Id { get; set; }
        public string OrderNo { get; set; } = default!;
        public DateTime OrderDate { get; set; }

        //fk
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        //nev
        public virtual ICollection<SalesItem>? SalesItems { get; set; }
    }
    public class SalesItem
    {
        public int Id { get; set; }
        [ForeignKey("SalesOrder")]
        public int SalesOrderId { get; set; }
        public virtual SalesOrder? SalesOrder { get; set; }


        public double UnitPrice { get; set; }
        public double Quantity { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }


    }
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesItem> SalesItems { get; set; }

    }
}
