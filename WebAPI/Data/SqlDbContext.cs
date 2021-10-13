using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;
using SharedLibrary.Models.CustomerModels;
using SharedLibrary.Models.OrderModels;
using SharedLibrary.Models.ProductModels;

#nullable disable

namespace WebAPI.Data
{
    public partial class SqlDbContext : DbContext
    {
        public SqlDbContext()
        {
        }

        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductColor> ProductColors { get; set; }
        public virtual DbSet<ProductSize> ProductSizes { get; set; }
        public virtual DbSet<ProductTag> ProductTags { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.BrandDescription).IsRequired();

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Color>(entity =>
            {
                entity.Property(e => e.ColorName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.Property(e => e.CouponCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CouponDiscount).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.PasswordSalt).IsRequired();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SecondaryAddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__Customer__04AFB25B");

                entity.HasOne(d => d.PaymentMethod)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__PaymentM__0697FACD");

                entity.HasOne(d => d.ShippingMethod)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShippingMethodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__Shipping__05A3D694");
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK__OrderPro__08D097A377D77B3D");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderProd__Order__17C286CF");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderProd__Produ__18B6AB08");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.PaymentMethodName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.AdditionalInfo).IsRequired();

                entity.Property(e => e.Category).IsRequired();

                entity.Property(e => e.DateTimeCreated).HasColumnType("datetime");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ProductDescription).IsRequired();

                entity.Property(e => e.ProductImage).IsRequired();

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Products__BrandI__09746778");
            });

            modelBuilder.Entity<ProductColor>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ColorId })
                    .HasName("PK__ProductC__7CD6B0B933C0AA2D");

                entity.HasOne(d => d.Color)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.ColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductCo__Color__14E61A24");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductColors)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductCo__Produ__13F1F5EB");
            });

            modelBuilder.Entity<ProductSize>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.SizeId })
                    .HasName("PK__ProductS__0C37165AF3F1BC5D");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductSizes)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductSi__Produ__10216507");

                entity.HasOne(d => d.Size)
                    .WithMany(p => p.ProductSizes)
                    .HasForeignKey(d => d.SizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductSi__SizeI__11158940");
            });

            modelBuilder.Entity<ProductTag>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.TagId })
                    .HasName("PK__ProductT__625B095705AD66E0");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductTags)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductTa__Produ__0D44F85C");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ProductTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProductTa__TagId__0C50D423");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.ReviewText).IsRequired();

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reviews__Custome__1B9317B3");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reviews__Product__1C873BEC");
            });

            modelBuilder.Entity<ShippingMethod>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ShippingMethodName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.Property(e => e.SizeName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasKey(e => new { e.CustomerId, e.ProductId })
                    .HasName("PK__Wishlist__6FEEA8B472E329D1");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Wishlists__Custo__1F63A897");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Wishlists)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Wishlists__Produ__2057CCD0");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
