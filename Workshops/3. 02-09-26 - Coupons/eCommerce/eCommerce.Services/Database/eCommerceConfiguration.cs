using Microsoft.EntityFrameworkCore;

namespace eCommerce.Services.Database
{
    public partial class ECommerceDbContext : DbContext
    {

        private void CreateConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.ChildCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ProductCategory relationships
            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure UserRole relationships
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Asset>()
               .HasOne(a => a.Product)
               .WithMany(p => p.Assets)
               .HasForeignKey(a => a.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Order)
                .WithMany(o => o.ProductReviews)
                .HasForeignKey(pr => pr.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CouponIB180079>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<CouponUsageIB180079>()
                .HasIndex(u => new { u.CouponId, u.UserId })
                .IsUnique();

            modelBuilder.Entity<CouponUsageIB180079>()
                .HasOne(u => u.Coupon)
                .WithMany(c => c.Usages)
                .HasForeignKey(u => u.CouponId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CouponUsageIB180079>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add any additional model configurations here
        }
    }
}
