using AutoMapper;
using BooksAP;
using BooksAP.Interfcae;
using BooksAP.Services;
using BooksAPI.DTOs;
using BooksAPI.DTOs.Interface;
using BooksAPI.DTOs.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
    });

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("LocalSqlServer"), sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
        });
    });
    var mapperConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new AutoMapperProfile());
    });

    builder.Services.AddTransient<IBookInterface, BookRepository>();
    builder.Services.AddTransient<ICategoryInterface, CategoryRepository>();
    builder.Services.AddTransient<IUnitOfWork, UnitOfWork>(); 
    builder.Services.AddTransient(typeof(IRepositories<>), typeof(Repository<>));
    builder.Services.AddTransient<IBookService, BookService>();
    builder.Services.AddTransient<ICategoryService, CategoryService>();

    IMapper mapper = mapperConfig.CreateMapper();
    builder.Services.AddSingleton(mapper);
    builder.Host.UseSerilog();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}