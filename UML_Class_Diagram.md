# CycleTrust - UML Class Diagram

## 1) Domain Model (Core Business Classes)

```mermaid
classDiagram
    class Role {
        +int RoleId
        +string RoleName
    }

    class User {
        +int UserId
        +string FullName
        +string Email
        +string Password
        +string Phone
        +string Address
        +int RoleId
        +UserStatus Status
        +bool PendingSellerUpgrade
        +DateTime CreatedAt
    }

    class Brand {
        +int BrandId
        +string BrandName
    }

    class Category {
        +int CategoryId
        +string CategoryName
    }

    class Bike {
        +int BikeId
        +int SellerId
        +int? BrandId
        +int? CategoryId
        +string Title
        +string Description
        +decimal Price
        +string FrameSize
        +BikeCondition BikeCondition
        +BikeStatus Status
        +DateTime CreatedAt
    }

    class BikeImage {
        +int ImageId
        +int BikeId
        +string ImageUrl
    }

    class InspectionReport {
        +int ReportId
        +int BikeId
        +int InspectorId
        +string FrameCondition
        +string BrakeCondition
        +string DrivetrainCondition
        +string OverallComment
        +string? ReportFile
        +InspectionStatus InspectionStatus
        +DateTime InspectedAt
    }

    class Message {
        +int MessageId
        +int SenderId
        +int ReceiverId
        +int? BikeId
        +string Content
        +DateTime SentAt
    }

    class Order {
        +int OrderId
        +int BikeId
        +int BuyerId
        +int SellerId
        +decimal TotalAmount
        +decimal? DepositAmount
        +OrderStatus Status
        +DateTime CreatedAt
    }

    class Payment {
        +int PaymentId
        +int OrderId
        +decimal Amount
        +string PaymentMethod
        +PaymentStatus Status
        +DateTime CreatedAt
    }

    class Review {
        +int ReviewId
        +int OrderId
        +int BuyerId
        +int SellerId
        +int Rating
        +string? Comment
        +DateTime CreatedAt
    }

    class Report {
        +int ReportId
        +int ReporterId
        +int? BikeId
        +string Reason
        +List~string~ ImageUrls
        +ReportStatus Status
        +DateTime CreatedAt
    }

    class Wishlist {
        +int WishlistId
        +int BuyerId
        +int BikeId
        +DateTime CreatedAt
        <<Unique(BuyerId,BikeId)>>
    }

    Role "1" --> "0..*" User : assigns
    User "1" --> "0..*" Bike : seller
    Brand "1" --> "0..*" Bike : classifies
    Category "1" --> "0..*" Bike : classifies

    Bike "1" --> "0..*" BikeImage : contains
    Bike "1" --> "0..*" InspectionReport : inspected
    User "1" --> "0..*" InspectionReport : inspector

    User "1" --> "0..*" Message : sends
    User "1" --> "0..*" Message : receives
    Bike "0..1" --> "0..*" Message : context

    User "1" --> "0..*" Order : buyer
    User "1" --> "0..*" Order : seller
    Bike "1" --> "0..*" Order : ordered

    Order "1" --> "0..*" Payment : paidBy
    Order "1" --> "0..*" Review : reviewed
    User "1" --> "0..*" Review : writesAsBuyer
    User "1" --> "0..*" Review : receivesAsSeller

    User "1" --> "0..*" Report : reports
    Bike "0..1" --> "0..*" Report : target

    User "1" --> "0..*" Wishlist : owns
    Bike "1" --> "0..*" Wishlist : wished
```

## 2) Application Layer (Clean Layered Structure)

```mermaid
classDiagram
    class CycleTrustDbContext {
        +DbSet~User~ Users
        +DbSet~Role~ Roles
        +DbSet~Bike~ Bikes
        +DbSet~BikeImage~ BikeImages
        +DbSet~Brand~ Brands
        +DbSet~Category~ Categories
        +DbSet~InspectionReport~ InspectionReports
        +DbSet~Message~ Messages
        +DbSet~Order~ Orders
        +DbSet~Payment~ Payments
        +DbSet~Report~ Reports
        +DbSet~Review~ Reviews
        +DbSet~Wishlist~ Wishlists
        #OnModelCreating(modelBuilder)
    }

    class IBikeService
    class IOrderService
    class IUserService
    class IMessageService
    class IPaymentService
    class IReviewService
    class IReportService
    class IWishlistService
    class IInspectionReportService
    class IBikeImageService
    class IBrandService
    class ICategoryService
    class IRoleService
    class ICloudinaryService

    class BikeService
    class OrderService
    class UserService
    class MessageService
    class PaymentService
    class ReviewService
    class ReportService
    class WishlistService
    class InspectionReportService
    class BikeImageService
    class BrandService
    class CategoryService
    class RoleService
    class CloudinaryService

    class BikesController
    class OrdersController
    class UsersController
    class MessagesController
    class PaymentController
    class ReviewsController
    class ReportsController
    class WishlistsController
    class InspectionReportsController
    class BikeImagesController
    class BrandsController
    class CategoriesController
    class RolesController
    class AuthController

    IBikeService <|.. BikeService
    IOrderService <|.. OrderService
    IUserService <|.. UserService
    IMessageService <|.. MessageService
    IPaymentService <|.. PaymentService
    IReviewService <|.. ReviewService
    IReportService <|.. ReportService
    IWishlistService <|.. WishlistService
    IInspectionReportService <|.. InspectionReportService
    IBikeImageService <|.. BikeImageService
    IBrandService <|.. BrandService
    ICategoryService <|.. CategoryService
    IRoleService <|.. RoleService
    ICloudinaryService <|.. CloudinaryService

    BikesController --> IBikeService
    BikesController --> ICloudinaryService
    BikesController --> IBikeImageService
    BikesController --> IInspectionReportService

    OrdersController --> IOrderService
    UsersController --> IUserService
    MessagesController --> IMessageService
    PaymentController --> IPaymentService
    ReviewsController --> IReviewService
    ReportsController --> IReportService
    WishlistsController --> IWishlistService
    InspectionReportsController --> IInspectionReportService
    BikeImagesController --> IBikeImageService
    BrandsController --> IBrandService
    CategoriesController --> ICategoryService
    RolesController --> IRoleService

    BikeService --> CycleTrustDbContext
    OrderService --> CycleTrustDbContext
    UserService --> CycleTrustDbContext
    MessageService --> CycleTrustDbContext
    PaymentService --> CycleTrustDbContext
    ReviewService --> CycleTrustDbContext
    ReportService --> CycleTrustDbContext
    WishlistService --> CycleTrustDbContext
    InspectionReportService --> CycleTrustDbContext
    BikeImageService --> CycleTrustDbContext
    BrandService --> CycleTrustDbContext
    CategoryService --> CycleTrustDbContext
    RoleService --> CycleTrustDbContext
```

## 3) Notes for System Architecture Report

- Architecture style: Layered Architecture (WebAPI -> BLL Service/Interface -> DAL DbContext/Entity).
- Persistence: Entity Framework Core + PostgreSQL.
- Constraint highlights: unique Email, BrandName, CategoryName, RoleName, and Wishlist(BuyerId, BikeId).
- Delete strategies in model: Restrict, Cascade, SetNull (configured in DbContext).
- Security boundary: Role-based authorization at controller level (ADMIN/BUYER/SELLER/INSPECTOR).
