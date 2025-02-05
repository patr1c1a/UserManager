using Microsoft.EntityFrameworkCore;
using UserManager.Data;
using UserManager.Services;
using UserManager.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=UserManager.db"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(c =>
    c.EnableAnnotations());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
    	options.TokenValidationParameters = new TokenValidationParameters
    	{
        	ValidateIssuerSigningKey = true,
        	IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)),
        	ValidateIssuer = true,
        	ValidIssuer = jwtSettings["Issuer"],
        	ValidateAudience = true,
        	ValidAudience = jwtSettings["Audience"],
        	ValidateLifetime = true,
        	ClockSkew = TimeSpan.Zero
    	};
	});

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDbContext>(); 
    var userRepository = services.GetRequiredService<IUserRepository>();
    var roleRepository = services.GetRequiredService<IRoleRepository>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    var adminRole = roleRepository.GetRoleByName("Admin");
    if (adminRole == null)
    {
        logger.LogInformation("Creating Admin role...");
        adminRole = new Role { Name = "Admin", Users = new List<User>() };
        roleRepository.AddRole(adminRole);
        dbContext.SaveChanges();
        logger.LogInformation("Admin role created.");
    }

    var existingAdmin = userRepository.GetUserByUsername("admin");
    if (existingAdmin == null)
    {
        logger.LogInformation("Creating admin user...");
        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@example.com",
            Password = "Admin123!",
            RoleId = adminRole.Id
        };

        userRepository.AddUser(adminUser);
        dbContext.SaveChanges();
        logger.LogInformation("Admin user created.");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();  

app.Run();
