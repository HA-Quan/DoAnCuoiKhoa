using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Services.Repository;
using Services.Service;
using Services.Service.Base;
using System.Text;

namespace Services;

public static class ServiceExtensions
{
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
    {
        string connectionString = config.GetConnectionString("DBconnection");
        services.AddDbContext<RepositoryContext>(
            options => options.UseSqlServer(connectionString));

        //services.AddDbContextPool<RepositoryContext>(o => o.UseMySql(connectionString, serverVersion), 300);
    }

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration config)
    {
        //JWT
        var secretKey = config["JWT:SecretKey"];
        var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(otp =>
        {
            otp.TokenValidationParameters = new TokenValidationParameters
            {
                // Tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                // ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero
            };
        });
        services.AddAuthorizationCore(config =>
        {
            config.AddPolicy("IsManagement", policy => policy.RequireClaim("Roles", "Management"));
            config.AddPolicy("IsStaff", policy => policy.RequireClaim("Roles", "Staff"));
            config.AddPolicy("IsMember", policy => policy.RequireClaim("Roles", "Member"));
        });
    }

    public static void ConfigureCustomServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IServiceBase<>), typeof(BaseService<>));
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IRefreshService, RefreshService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IChipService, ChipService>();
        services.AddScoped<IDisplayService, DisplayService>();
        services.AddScoped<IGiftService, GiftService>();
        services.AddScoped<IGiftByProductService, GiftByProductService>();
        services.AddScoped<IImportProductService, ImportProductService>();
        services.AddScoped<IMemoryService, MemoryService>();
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<IOrderDetailService, OrderDetailService>();
        services.AddScoped<IOrderProductService, OrderProductService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IPromotionService, PromotionService>();
        services.AddScoped<IRateService, RateService>();
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IStatisticService, StatisticService>();

        //services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IChipRepository, ChipRepository>();
        services.AddScoped<IDisplayRepository, DisplayRepository>();
        services.AddScoped<IGiftRepository, GiftRepository>();
        services.AddScoped<IGiftByProductRepository, GiftByProductRepository>();
        services.AddScoped<IImportProductRepository, ImportProductRepository>();
        services.AddScoped<IMemoryRepository, MemoryRepository>();
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPromotionRepository, PromotionRepository>();
        services.AddScoped<IRateRepository, RateRepository>();
        services.AddScoped<IStorageRepository, StorageRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();

        

        
        services.AddSwaggerGen();
        //services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserValidator>());
        //services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
    }

    public static void ConfigureExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
