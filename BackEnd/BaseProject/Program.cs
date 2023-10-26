using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Services;
using Services.Repository;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web host");
    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    var builder = WebApplication.CreateBuilder(args);

    //builder.Services.AddCors(options =>
    //{
    //    options.AddPolicy(MyAllowSpecificOrigins,
    //                          policy =>
    //                          {
    //                              policy.WithOrigins("http://localhost:8080")
    //                                                  .AllowAnyHeader()
    //                                                  .AllowAnyMethod();
    //                          });
    //});

    builder.Services.AddCors(option =>
    {
        option.AddPolicy(name: MyAllowSpecificOrigins,
                        policy =>
                        {
                            policy.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                        });
    });

    // Add services to the container.

    builder.Services.AddControllers();

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console());

    // JWT

    builder.Services.ConfigureJWT(builder.Configuration);

    // DB
    builder.Services.ConfigureSqlContext(builder.Configuration);
    // Repository
    builder.Services.ConfigureCustomServices();

    var app = builder.Build();

    // exception middleware
    app.ConfigureExceptionMiddleware();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors(MyAllowSpecificOrigins);
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

    });


    app.MapFallbackToFile("index.html");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
