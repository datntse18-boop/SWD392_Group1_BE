using Microsoft.EntityFrameworkCore;
using Backend_CycleTrust.DAL.Entities;

namespace Backend_CycleTrust.DAL.Data
{
    public class CycleTrustDbContext : DbContext
    {
        public CycleTrustDbContext(DbContextOptions<CycleTrustDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<BikeImage> BikeImages { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InspectionReport> InspectionReports { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== Enum conversions (store as string) =====
            modelBuilder.Entity<User>()
                .Property(u => u.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Bike>()
                .Property(b => b.BikeCondition)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Bike>()
                .Property(b => b.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<InspectionReport>()
                .Property(ir => ir.InspectionStatus)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<Report>()
                .Property(r => r.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // ===== Unique constraints =====
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Brand>()
                .HasIndex(b => b.BrandName)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            modelBuilder.Entity<Wishlist>()
                .HasIndex(w => new { w.BuyerId, w.BikeId })
                .IsUnique();

            // ===== User relationships =====
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Bike relationships =====
            modelBuilder.Entity<Bike>()
                .HasOne(b => b.Seller)
                .WithMany(u => u.Bikes)
                .HasForeignKey(b => b.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bike>()
                .HasOne(b => b.Brand)
                .WithMany(br => br.Bikes)
                .HasForeignKey(b => b.BrandId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Bike>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Bikes)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===== BikeImage =====
            modelBuilder.Entity<BikeImage>()
                .HasOne(bi => bi.Bike)
                .WithMany(b => b.BikeImages)
                .HasForeignKey(bi => bi.BikeId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== InspectionReport =====
            modelBuilder.Entity<InspectionReport>()
                .HasOne(ir => ir.Bike)
                .WithMany(b => b.InspectionReports)
                .HasForeignKey(ir => ir.BikeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InspectionReport>()
                .HasOne(ir => ir.Inspector)
                .WithMany(u => u.InspectionReports)
                .HasForeignKey(ir => ir.InspectorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Message =====
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Bike)
                .WithMany(b => b.Messages)
                .HasForeignKey(m => m.BikeId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===== Order =====
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Bike)
                .WithMany(b => b.Orders)
                .HasForeignKey(o => o.BikeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany(u => u.BuyerOrders)
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Seller)
                .WithMany(u => u.SellerOrders)
                .HasForeignKey(o => o.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Report =====
            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Bike)
                .WithMany(b => b.Reports)
                .HasForeignKey(r => r.BikeId)
                .OnDelete(DeleteBehavior.SetNull);

            // ===== Review =====
            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.Order)
                .WithMany(o => o.Reviews)
                .HasForeignKey(rv => rv.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.Buyer)
                .WithMany(u => u.BuyerReviews)
                .HasForeignKey(rv => rv.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(rv => rv.Seller)
                .WithMany(u => u.SellerReviews)
                .HasForeignKey(rv => rv.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Wishlist =====
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Buyer)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Bike)
                .WithMany(b => b.Wishlists)
                .HasForeignKey(w => w.BikeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Seed data =====
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "ADMIN" },
                new Role { RoleId = 2, RoleName = "BUYER" },
                new Role { RoleId = 3, RoleName = "SELLER" },
                new Role { RoleId = 4, RoleName = "INSPECTOR" }
            );

            // Users: 1 Admin, 1 Buyer, 1 Seller, 1 Inspector
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, FullName = "Admin CycleTrust", Email = "admin@cycletrust.com", Password = "admin123", Phone = "0901000001", Address = "Ho Chi Minh City", RoleId = 1, Status = Enums.UserStatus.ACTIVE, CreatedAt = seedDate },
                new User { UserId = 2, FullName = "Nguyen Van Buyer", Email = "buyer@cycletrust.com", Password = "buyer123", Phone = "0901000002", Address = "Ha Noi", RoleId = 2, Status = Enums.UserStatus.ACTIVE, CreatedAt = seedDate },
                new User { UserId = 3, FullName = "Tran Thi Seller", Email = "seller@cycletrust.com", Password = "seller123", Phone = "0901000003", Address = "Da Nang", RoleId = 3, Status = Enums.UserStatus.ACTIVE, CreatedAt = seedDate },
                new User { UserId = 4, FullName = "Le Van Inspector", Email = "inspector@cycletrust.com", Password = "inspector123", Phone = "0901000004", Address = "Ho Chi Minh City", RoleId = 4, Status = Enums.UserStatus.ACTIVE, CreatedAt = seedDate }
            );

            // Brands
            modelBuilder.Entity<Brand>().HasData(
                new Brand { BrandId = 1, BrandName = "Giant" },
                new Brand { BrandId = 2, BrandName = "Trek" },
                new Brand { BrandId = 3, BrandName = "Specialized" },
                new Brand { BrandId = 4, BrandName = "Cannondale" },
                new Brand { BrandId = 5, BrandName = "Merida" }
            );

            // Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Road Bike" },
                new Category { CategoryId = 2, CategoryName = "Mountain Bike" },
                new Category { CategoryId = 3, CategoryName = "City Bike" },
                new Category { CategoryId = 4, CategoryName = "Folding Bike" },
                new Category { CategoryId = 5, CategoryName = "Electric Bike" }
            );

            // Bikes (seller_id = 3)
            modelBuilder.Entity<Bike>().HasData(
                new Bike { BikeId = 1, SellerId = 3, Title = "Giant Contend AR 1 2025", Description = "Xe đạp đường trường Giant Contend AR 1, khung nhôm nhẹ, groupset Shimano 105.", Price = 25000000m, BrandId = 1, CategoryId = 1, FrameSize = "M", BikeCondition = Enums.BikeCondition.USED_LIKE_NEW, Status = Enums.BikeStatus.APPROVED, CreatedAt = seedDate },
                new Bike { BikeId = 2, SellerId = 3, Title = "Trek Marlin 7 2024", Description = "Xe đạp leo núi Trek Marlin 7, phuộc RockShox, 27.5 inch.", Price = 18000000m, BrandId = 2, CategoryId = 2, FrameSize = "L", BikeCondition = Enums.BikeCondition.USED_GOOD, Status = Enums.BikeStatus.APPROVED, CreatedAt = seedDate },
                new Bike { BikeId = 3, SellerId = 3, Title = "Specialized Sirrus X 4.0", Description = "Xe đạp thành phố cao cấp, khung carbon, phù hợp đi làm và tập luyện.", Price = 35000000m, BrandId = 3, CategoryId = 3, FrameSize = "M", BikeCondition = Enums.BikeCondition.NEW, Status = Enums.BikeStatus.PENDING, CreatedAt = seedDate },
                new Bike { BikeId = 4, SellerId = 3, Title = "Cannondale Quick 5", Description = "Xe đạp fitness nhẹ, phù hợp di chuyển hàng ngày trong thành phố.", Price = 12000000m, BrandId = 4, CategoryId = 3, FrameSize = "S", BikeCondition = Enums.BikeCondition.USED_FAIR, Status = Enums.BikeStatus.APPROVED, CreatedAt = seedDate }
            );


            // Inspection Report (Inspector id=4 inspects bike id=1)
            modelBuilder.Entity<InspectionReport>().HasData(
                new InspectionReport { ReportId = 1, BikeId = 1, InspectorId = 4, FrameCondition = "Khung nhôm còn mới, không trầy xước.", BrakeCondition = "Phanh đĩa hoạt động tốt.", DrivetrainCondition = "Hệ thống truyền động Shimano 105 mượt mà.", OverallComment = "Xe trong tình trạng rất tốt, đạt tiêu chuẩn.", InspectionStatus = Enums.InspectionStatus.APPROVED, InspectedAt = seedDate },
                new InspectionReport { ReportId = 2, BikeId = 2, InspectorId = 4, FrameCondition = "Khung nhôm có vài vết trầy nhỏ.", BrakeCondition = "Phanh cần điều chỉnh lại.", DrivetrainCondition = "Xích hơi giãn, cần thay sớm.", OverallComment = "Xe ổn, cần bảo dưỡng nhẹ.", InspectionStatus = Enums.InspectionStatus.APPROVED, InspectedAt = seedDate }
            );

            // Order (buyer id=2 buys bike id=1 from seller id=3)
            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, BikeId = 1, BuyerId = 2, SellerId = 3, TotalAmount = 25000000m, DepositAmount = 5000000m, Status = Enums.OrderStatus.COMPLETED, CreatedAt = seedDate },
                new Order { OrderId = 2, BikeId = 4, BuyerId = 2, SellerId = 3, TotalAmount = 12000000m, DepositAmount = 2000000m, Status = Enums.OrderStatus.PENDING, CreatedAt = seedDate }
            );

            // Review (buyer id=2 reviews completed order)
            modelBuilder.Entity<Review>().HasData(
                new Review { ReviewId = 1, OrderId = 1, BuyerId = 2, SellerId = 3, Rating = 5, Comment = "Xe rất đẹp, đúng mô tả. Seller giao hàng nhanh!", CreatedAt = seedDate }
            );

            // Message (buyer chats with seller about bike)
            modelBuilder.Entity<Message>().HasData(
                new Message { MessageId = 1, SenderId = 2, ReceiverId = 3, BikeId = 2, Content = "Chào bạn, xe Trek Marlin 7 còn không ạ?", SentAt = seedDate },
                new Message { MessageId = 2, SenderId = 3, ReceiverId = 2, BikeId = 2, Content = "Chào bạn, xe vẫn còn nhé. Bạn muốn xem trực tiếp không?", SentAt = seedDate }
            );

            // Wishlist (buyer saves bike id=2)
            modelBuilder.Entity<Wishlist>().HasData(
                new Wishlist { WishlistId = 1, BuyerId = 2, BikeId = 2, CreatedAt = seedDate }
            );

            // Report (buyer reports a listing)
            modelBuilder.Entity<Report>().HasData(
                new Report { ReportId = 1, ReporterId = 2, BikeId = 3, Reason = "Giá niêm yết không hợp lý so với tình trạng xe.", Status = Enums.ReportStatus.PENDING, CreatedAt = seedDate }
            );
        }
    }
}
