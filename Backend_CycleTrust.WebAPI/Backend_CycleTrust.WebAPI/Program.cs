using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.BLL.Services;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Interfaces;
using Backend_CycleTrust.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ===== DbContext =====
            builder.Services.AddDbContext<CycleTrustDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ===== Repositories =====
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // ===== Services =====
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IBikeService, BikeService>();
            builder.Services.AddScoped<IBikeImageService, BikeImageService>();
            builder.Services.AddScoped<IInspectionReportService, InspectionReportService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IReportService, ReportService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IWishlistService, WishlistService>();

            // ===== Controllers & Swagger =====
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ===== CORS =====
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
