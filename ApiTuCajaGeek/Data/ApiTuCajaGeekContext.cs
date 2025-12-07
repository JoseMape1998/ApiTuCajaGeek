using Microsoft.EntityFrameworkCore;
using ApiTuCajaGeek.Models;

namespace ApiTuCajaGeek.Data
{
    public class ApiTuCajaGeekContext : DbContext
    {
        public ApiTuCajaGeekContext(DbContextOptions<ApiTuCajaGeekContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Images_Category> Images_Category { get; set; }
        public DbSet<Images_Product> Images_Product { get; set; }
        public DbSet<Images_rating> Images_rating { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Purchase_data> Purchase_data { get; set; }
        public DbSet<Purchase_type> Purchase_Type { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Shopping_cart> Shopping_cart { get; set; }
        public DbSet<User_rating> User_rating { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(eb =>
            {
                eb.HasKey(e => e.Category_Id);
                eb.Property(e => e.Category_name).HasMaxLength(500).IsRequired().HasColumnType("varchar(500)");
                eb.Property(e => e.Category_description).HasColumnType("nvarchar(4000)").IsRequired(false);
            });

            modelBuilder.Entity<Images_Category>(eb =>
            {
                eb.HasKey(e => e.Image_category_Id);
                eb.Property(e => e.Image_Url).HasMaxLength(500).IsRequired().HasColumnType("varchar(500)");
                eb.HasOne(e => e.Category)
                  .WithMany(c => c.ImagesCategory)
                  .HasForeignKey(e => e.Category_Id)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Images_Product>(eb =>
            {
                eb.HasKey(e => e.Image_product_Id);
                eb.Property(e => e.Image_Url).HasMaxLength(500).IsRequired().HasColumnType("varchar(500)");
                eb.HasOne(e => e.Product)
                  .WithMany(p => p.Images)
                  .HasForeignKey(e => e.Product_Id)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Images_rating>(eb =>
            {
                eb.HasKey(e => e.Image_rating_Id);
                eb.Property(e => e.Image_Url).HasMaxLength(500).IsRequired().HasColumnType("varchar(500)");
                eb.HasOne(e => e.Rating)
                  .WithMany(r => r.Images)
                  .HasForeignKey(e => e.Rating_Id)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Products>(eb =>
            {
                eb.HasKey(e => e.Product_Id);
                eb.Property(e => e.SKU).HasDefaultValueSql("newid()").IsRequired();
                eb.Property(e => e.Product_name).HasMaxLength(500).IsRequired().HasColumnType("varchar(500)");
                eb.Property(e => e.Product_description).HasMaxLength(4000).HasColumnType("varchar(4000)").IsRequired(false);
                eb.Property(e => e.Product_value_before_discount).HasColumnType("decimal(18,0)").IsRequired();
                eb.Property(e => e.Product_value_after_discount).HasColumnType("decimal(18,0)").IsRequired();
                eb.Property(e => e.Number_existences).IsRequired();
                eb.Property(e => e.Stock_State).IsRequired();
                eb.Property(e => e.Weight).HasColumnType("decimal(18,0)").IsRequired(false);
                eb.Property(e => e.Height).HasColumnType("decimal(18,0)").IsRequired(false);
                eb.Property(e => e.Width).HasColumnType("decimal(18,0)").IsRequired(false);
                eb.Property(e => e.Depth).HasColumnType("decimal(18,0)").IsRequired(false);
                eb.Property(e => e.Product_State).IsRequired();
                eb.HasOne(e => e.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(e => e.Category_Id)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Purchase_type>(eb =>
            {
                eb.HasKey(e => e.Purchase_type_Id);
                eb.Property(e => e.Name_purchase_type).HasMaxLength(500).IsRequired().HasColumnType("varchar(500)");
                eb.Property(e => e.Requires_card_information).IsRequired();
            });

            modelBuilder.Entity<Purchase_data>(eb =>
            {
                eb.HasKey(e => e.UserId);
                eb.Property(e => e.PurchaseAddress).HasMaxLength(150).IsRequired().HasColumnType("varchar(150)");
                eb.HasOne(e => e.User)
                  .WithOne(u => u.PurchaseData)
                  .HasForeignKey<Purchase_data>(p => p.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
                eb.HasOne(e => e.PurchaseType)
                  .WithMany(t => t.PurchaseDatas)
                  .HasForeignKey(e => e.PurchaseTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Rol>(eb =>
            {
                eb.HasKey(e => e.Rol_id);
                eb.Property(e => e.Name_Rol).HasMaxLength(200).IsRequired().HasColumnType("varchar(200)");
                eb.Property(e => e.Rol_id).HasDefaultValueSql("newid()");
            });

            modelBuilder.Entity<Users>(eb =>
            {
                eb.HasKey(e => e.User_Id);
                eb.Property(e => e.User_Id).HasDefaultValueSql("newid()");
                eb.Property(e => e.Email_user).HasMaxLength(100).IsRequired().HasColumnType("varchar(100)");
                eb.Property(e => e.Name_user).HasMaxLength(100).IsRequired().HasColumnType("varchar(100)");
                eb.Property(e => e.LastName_user).HasMaxLength(300).IsRequired().HasColumnType("varchar(300)");
                eb.Property(e => e.Cell_number_user).IsRequired(false);
                eb.Property(e => e.Password_hash).HasMaxLength(200).IsRequired().HasColumnType("nvarchar(200)");
                eb.Property(e => e.User_state).IsRequired();
                eb.HasOne(e => e.Rol)
                  .WithMany(r => r.Users)
                  .HasForeignKey(e => e.Rol_id)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Shopping_cart>(eb =>
            {
                eb.HasKey(e => e.Shopping_cart_Id);
                eb.Property(e => e.Unit_value).HasColumnType("decimal(18,0)").IsRequired();
                eb.Property(e => e.Amount).IsRequired();
                eb.Property(e => e.Date_added).HasDefaultValueSql("getdate()").IsRequired();
                eb.Property(e => e.Subtotal).HasColumnType("decimal(18,0)").HasComputedColumnSql("[Unit_value]*[Amount]", stored: true);
                eb.HasOne(e => e.User)
                  .WithMany(u => u.ShoppingCarts)
                  .HasForeignKey(e => e.User_Id)
                  .OnDelete(DeleteBehavior.Cascade);
                eb.HasOne(e => e.Product)
                  .WithMany(p => p.ShoppingCarts)
                  .HasForeignKey(e => e.Product_Id)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User_rating>(eb =>
            {
                eb.HasKey(e => e.Rating_Id);
                eb.Property(e => e.Purchase_comment).HasColumnType("nvarchar(2000)").IsRequired(false);
                eb.HasOne(e => e.Product)
                  .WithMany(p => p.User_ratings)
                  .HasForeignKey(e => e.Product_Id)
                  .OnDelete(DeleteBehavior.Restrict);
                eb.HasOne(e => e.User)
                  .WithMany(u => u.User_ratings)
                  .HasForeignKey(e => e.User_Id)
                  .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
