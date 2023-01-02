using Batvpn.Persistence;
using BatVpn.Application;
using BatVpn.Infrastructure;
using BatVpn.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();



#region DataBaseService and other repositories
//var BatVpnconnectionString = builder.Configuration.GetConnectionString("BatVpnConnectionString");
//var IdentityconnectionString = builder.Configuration.GetConnectionString("IdentityConnectionString");
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var BatVpnconnectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";
var IdentityconnectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";
PersistenceBootstrapper.Configure(builder.Services, BatVpnconnectionString, IdentityconnectionString);
#endregion


#region Adding Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

#endregion


#region Adding Jwt Bearer 

.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
#endregion


#region Adding Cors Policy

builder.Services.AddCors(o => o.AddPolicy("BatVpnPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
#endregion


#region Other Services
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(Assembly.Load("BatVpn.Application"));

InfrastructureBootstrapper.Configure(builder.Services);
ApplicationBootstrapper.Configure(builder.Services);


#endregion



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    //scope.ServiceProvider.GetRequiredService<T>().Database.SetCommandTimeout(160);
    scope.ServiceProvider.GetRequiredService<BatVpnDbContext>().Database.Migrate();
}

app.UseCors("BatVpnPolicy");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    //mvc
    endpoints.MapControllerRoute(
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
