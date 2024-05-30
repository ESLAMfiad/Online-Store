using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Store.APIs.Errors;
using Store.APIs.Extentions;
using Store.APIs.helpers;
using Store.APIs.Middlewares;
using Store.Core.Models;
using Store.Core.Models.identity;
using Store.Core.Repository;
using Store.Repository;
using Store.Repository.Data;
using Store.Repository.identity;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureService :Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); //allowed dependancy injection
});
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});
builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
{
    var Connection = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(Connection);
});
builder.Services.AddApplicationServices();


builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddCors(Options =>
{
    Options.AddPolicy("MyPolicy", options =>
    {
        options.AllowAnyHeader();
        options.AllowAnyMethod();
        options.WithOrigins(builder.Configuration["FrontBaseUrl"]);
    });
});
#endregion


var app = builder.Build();


#region Update database
//StoreContext dbContext = new StoreContext(); //invalid
//await dbContext.Database.MigrateAsync();
using var Scope = app.Services.CreateScope(); //group of services lifetime scooped "mnhom el dbcontext"
var Services = Scope.ServiceProvider; //mskt elservices nfsha
var LoggerFactory= Services.GetRequiredService<ILoggerFactory>();
try
{
    //using bt3mlo dispose wtqflo b3d man5ls
    
    var dbContext = Services.GetRequiredService<StoreContext>(); //ask clr for creating obj from dbcontext explicitly
    await dbContext.Database.MigrateAsync(); //UPDATE DATABASE 

    var IdentityDbContext= Services.GetRequiredService<AppIdentityDbContext>();
    await IdentityDbContext.Database.MigrateAsync();

    var UserManager=Services.GetRequiredService<UserManager<AppUser>>();
    await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
    await StoreContextSeed.SeedAsync(dbContext);
}
catch (Exception ex)
{
    var Logger = LoggerFactory.CreateLogger<Program>();
    Logger.LogError(ex, "error occured during updating database");
}


#endregion
#region Configure -the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<ExceptionMiddlewares>();
    app.UseSwaggerMiddlewares();
}
app.UseStatusCodePagesWithReExecute("/errors/{0}"); //0 bt7gz mkan llcode  //el ReExecute btbqqa asr3 mn elredirect
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
#endregion

app.Run();
