using Blog.Application.Interfaces;
using Blog.Application.Interfaces.Authentication;
using Blog.Application.Services;
using Blog.Application.Services.Authentication;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Infrastructure.Authentication.Configurations;
using Blog.Infrastructure.Authentication.Services;
using Blog.Persistence.Contexts;
using Blog.Persistence.Repositories;
using Blog.Persistence.Transactions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// افزودن DbContext با استفاده از کانکشن  
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// افزودن سایر سرویس‌ها

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IReactionService, ReactionService>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<JwtService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));


//dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
//dotnet add package Microsoft.IdentityModel.Tokens
//dotnet add package System.IdentityModel.Tokens.Jwt

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var jwtService = serviceProvider.GetRequiredService<JwtService>();

    options.TokenValidationParameters = new TokenValidationParameters
    {      
        //سازنده توکن (پروژه ای که این را ساخته) اعتبار سنجی کن)

        ValidateIssuer = true,
        //مقصدی که این توکن به دستش می رسه را اعتبار سنجی کن
        ValidateAudience = true,
        //مدت اعتبار توکن را اعتبار سنجی کن
        ValidateLifetime = true,
        // امضای توکن را اعتبار سنجی کن
        ValidateIssuerSigningKey = true,
        // زمان اضافی برای اعتبار سنجی توکن (به طور پیش فرض 5 دقیقه است)- زمان را دقیق محاسبه می کند

        // سازنده توکن معریفی می کنه
        //این اطلاعات از appsetting می گیره
        ClockSkew = TimeSpan.Zero,
        // سازنده توکن معریفی می کنه
        //این اطلاعات از appsetting می گیره
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var token = (context.SecurityToken as JsonWebToken)?.EncodedToken;
            if (token != null)
            {
                bool isValid = await jwtService.IsTokenValidAsync(token);
                if (!isValid)
                {
                    context.Fail("Token is not valid in the database.");
                }
            }
            else
            {
                context.Fail("Invalid token format.");
            }
        },

    };
});


builder.Services.AddAuthorization();


builder.Services.AddControllers();
// Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Blog API",
        Version = "v1",
        Description = "API Documentation for Blazor Blog"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "توکن خود را اینجا وارد نمایید"

    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



//add cors => برای اجازه اینکه پروژه 
//Blog.PanelAdmin اجازه دسترسی به پروژه
//api داشته باشع

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:7173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor Blog API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (UnauthorizedAccessException)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized");
    }
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
