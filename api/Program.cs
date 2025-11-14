

//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
//using SmartBudgetTracker.Data;
//using SmartBudgetTracker.Repositories.Implementations;
//using SmartBudgetTracker.Repositories.Interfaces;
//using System.Text;
//using Npgsql.EntityFrameworkCore.PostgreSQL;

//var builder = WebApplication.CreateBuilder(args);



//builder.Services.AddScoped<IBudgetGoalRepository, BudgetGoalRepository>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
//builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<IAuthService, AuthService>();


//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });



//builder.Services.AddControllers()
//    .AddJsonOptions(x =>
//    {
//        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
//        x.JsonSerializerOptions.WriteIndented = true;
//    });


//builder.Services.AddDbContext<BudgetDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Smart Budget Tracker API",
//        Version = "v1",
//        Description = "Backend API for Smart Budget Tracker"
//    });


//    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Enter JWT token like: Bearer eyJhbGciOiJIUzI1NiIs...",
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer",
//        BearerFormat = "JWT"
//    });

//    //  Apply security globally
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});

//// ============ 6. CORS ============
////  Replace the URL below with your real Vue Render URL once deployed

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowVueClient", policy =>
//    {
//        policy.WithOrigins(
//            "http://localhost:5173",                        // for local Vue dev
//            "https://your-vue-frontend.onrender.com"        // for Render frontend
//        )
//        .AllowAnyHeader()
//        .AllowAnyMethod();
//    });
//});

//var app = builder.Build();

//// ============ 7. Middleware Order ============

//app.UseCors("AllowVueClient");

//app.UseSwagger();
//app.UseSwaggerUI();

//app.UseHttpsRedirection();

//app.UseAuthentication();   //  Must come before UseAuthorization
//app.UseAuthorization();

//app.MapControllers();

//app.Run();


using FirebaseAdmin;
using Google;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SmartBudgetTracker.Data;
using SmartBudgetTracker.Repositories;
using SmartBudgetTracker.Repositories.Implementations;
using SmartBudgetTracker.Repositories.Interfaces;
using SmartBudgetTracker.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBudgetGoalRepository, BudgetGoalRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
try
{
    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("FIREBASE_CONFIG")))
    {
        // Production - read from environment variable
        var firebaseConfig = Environment.GetEnvironmentVariable("FIREBASE_CONFIG");
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromJson(firebaseConfig)
        });
    }
    else if (File.Exists("firebase-adminsdk.json"))
    {
        // Development - read from file if it exists
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("firebase-adminsdk.json")
        });
    }
    // If neither exists, skip Firebase initialization (for migrations)
}
catch (Exception ex)
{
    // Log but don't crash during startup/migrations
    Console.WriteLine($"Firebase initialization skipped: {ex.Message}");
}

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = true;
    });


builder.Services.AddDbContext<BudgetDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Smart Budget Tracker API",
        Version = "v1",
        Description = "Backend API for Smart Budget Tracker"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer eyJhbGciOiJIUzI1NiIs...",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
//FirebaseApp.Create(new AppOptions()
//{
//    Credential = GoogleCredential.FromFile("path/to/firebase-adminsdk.json")
//});
// Firebase initialization - optional for migrations
try
{
    if (File.Exists("firebase-adminsdk.json"))
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile("firebase-adminsdk.json")
        });
    }
}
catch
{
    // Skip Firebase during migrations
}
// CORS - Allow same origin since we're serving frontend from same domain
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueClient", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5173",                        
            "https://financeflow-2esa.onrender.com"        
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BudgetDbContext>();
    db.Database.Migrate(); 
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BudgetDbContext>();
        context.Database.Migrate(); // Applies pending migrations automatically
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.UseCors("AllowVueClient");

// Serve static files from wwwroot (your Vue build)
app.UseStaticFiles();
app.UseDefaultFiles();  // Serves index.html by default

// Swagger only in development

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// API routes
app.MapControllers();

// SPA fallback - serves index.html for Vue Router (client-side routing)
app.MapFallbackToFile("index.html");

app.Run();
