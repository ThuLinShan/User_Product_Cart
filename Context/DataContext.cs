using Microsoft.EntityFrameworkCore;
using User_Product_Cart.Models;

namespace User_Product_Cart.Context
{
    public class DataContext: DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventProduct> EventsProduct { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryProduct> CategoryProducts { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<BrandProduct> BrandProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>().HasKey(x => new { x.UserId, x.ProductId});
            modelBuilder.Entity<Promotion>()
                        .Property(e => e.startDate)
                        .HasColumnType("date");
            modelBuilder.Entity<Promotion>()
                        .Property(e => e.endDate)
                        .HasColumnType("date");

            modelBuilder.Entity<EventProduct>()
                .HasKey(ep => new {ep.EventId, ep.ProductId});
            modelBuilder.Entity<EventProduct>()
                .HasOne(ep => ep.Event)
                .WithMany(p => p.EventProducts)
                .HasForeignKey(ep => ep.EventId);
            modelBuilder.Entity<EventProduct>()
                .HasOne(ep => ep.Product)
                .WithMany(e => e.EventProducts)
                .HasForeignKey(ep => ep.ProductId);

            modelBuilder.Entity<CategoryProduct>()
                .HasKey(cp => new { cp.CategoryId, cp.ProductId });
            modelBuilder.Entity<CategoryProduct>()
                .HasOne(cp => cp.Category)
                .WithMany(c => c.CategoryProducts)
                .HasForeignKey(cp => cp.CategoryId);
            modelBuilder.Entity<CategoryProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p =>p.CategoryProducts)
                .HasForeignKey(cp => cp.ProductId);


            modelBuilder.Entity<BrandProduct>()
                .HasKey(bp => new { bp.BrandId, bp.ProductId });
            modelBuilder.Entity<BrandProduct>()
                .HasOne(bp => bp.Brand)
                .WithMany(b => b.BrandProducts)
                .HasForeignKey(bp => bp.BrandId);
            modelBuilder.Entity<BrandProduct>()
                .HasOne(bp => bp.Product)
                .WithMany(p => p.BrandProducts)
                .HasForeignKey(bp => bp.ProductId);
        }
    }
}
