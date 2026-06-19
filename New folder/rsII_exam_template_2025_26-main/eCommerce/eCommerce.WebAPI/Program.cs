using eCommerce.Common.Services.CryptoService;
using eCommerce.Model.Requests;
using eCommerce.Model.Responses;
using eCommerce.Services;
using eCommerce.Services.Database;
using eCommerce.Services.ProductStateMachine;
using eCommerce.Services.QueryOptimization;
using eCommerce.Services.Validators;
using eCommerce.WebAPI.Filters;
using eCommerce.WebAPI.Services;
using eCommerce.WebAPI.Services.AccessManager;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthenticatedUserAccessor, HttpAuthenticatedUserAccessor>();

builder.Services.AddControllers(
   options => options.Filters.Add<ExceptionFilter>()
);

// Add Entity Framework Core DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddMapster();


// register Mapster for object mapping — configure custom maps BEFORE AddMapster()
TypeAdapterConfig<Product, ProductResponse>.NewConfig().IgnoreNullValues(true);


TypeAdapterConfig<Category, CategoryResponse>.NewConfig().IgnoreNullValues(true);
TypeAdapterConfig<User, UserResponse>.NewConfig().IgnoreNullValues(true);
TypeAdapterConfig<UserUpdateRequest, User>.NewConfig().IgnoreNullValues(true);
TypeAdapterConfig<ProductType, ProductTypeResponse>.NewConfig().IgnoreNullValues(true);
TypeAdapterConfig<UnitOfMeasure, UnitOfMeasureResponse>.NewConfig().IgnoreNullValues(true);
TypeAdapterConfig<Asset, AssetResponse>.NewConfig().IgnoreNullValues(true);
TypeAdapterConfig<ProductReview, ProductReviewResponse>.NewConfig()
    .Map(dest => dest.ReviewerDisplayName, src => $"{src.User.FirstName} {src.User.LastName}".Trim());
TypeAdapterConfig<Order, OrderResponse>.NewConfig()
    .Map(dest => dest.Status, src => (int)src.Status);
TypeAdapterConfig<OrderItem, OrderItemResponse>.NewConfig()
    .Map(dest => dest.ProductName, src => src.Product != null ? src.Product.Name : string.Empty);
TypeAdapterConfig<Activity, ActivityResponse>.NewConfig().IgnoreNullValues(true);
TypeAdapterConfig<RewardRule, RewardRuleResponse>.NewConfig().IgnoreNullValues(true);



//TypeAdapterConfig<UserActivity, UserActivityResponse>.NewConfig()
//    .Map(dest => dest.UserFullName, src => $"{src.User.FirstName} {src.User.LastName}".Trim())
//    .Map(dest => dest.ActivityName, src => src.Activity.Name)
//    .Map(dest => dest.DueDate, src => src.Activity.DueDate)
//    .Map(dest => dest.NumberOfPoints, src => src.Activity!.RewardRule!.NumberOfPoints)
//;


TypeAdapterConfig<UserActivity, UserActivityResponse>.NewConfig()
    .MapWith(src => new UserActivityResponse
    {
        ActivityName = src.Activity.Name,
        UserFullName = $"{src.User.FirstName} {src.User.LastName}".Trim(),
        DueDate = src.Activity.DueDate,
        NumberOfPoints = src.Activity.RewardRule.NumberOfPoints
    }
    )
;










//TypeAdapterConfig<UserActivity, UserActivityResponse>.NewConfig()
//    .MapWith(src => new UserActivityResponse
//    {
//        Id = src.Id,
//        Status = src.Status,
//        UserFullName = src.User != null ? $"{src.User.FirstName} {src.User.LastName}".Trim() : string.Empty,
//        ActivityName = src.Activity.Name,
//        DueDate = src.Activity.DueDate,
//        NumberOfPoints = src.Activity.RewardRule.NumberOfPoints,
//    });


















//TypeAdapterConfig<UserActivity, UserActivityResponse>.NewConfig()
//    .Map(dest => dest.UserFullName, src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}".Trim() : string.Empty)
//    .Map(dest => dest.ActivityName, src => src.Activity != null ? src.Activity.Name : string.Empty)
//    .Map(dest => dest.DueDate, src => src.Activity != null ? src.Activity.DueDate : DateTime.MinValue)
//    .Map(dest => dest.NumberOfPoints, src => src.Activity != null && src.Activity.RewardRule != null ? src.Activity.RewardRule.NumberOfPoints : 0);


// register application services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<BaseProductState>();
builder.Services.AddScoped<InitialProductState>();
builder.Services.AddScoped<DraftProductState>();
builder.Services.AddScoped<ActiveProductState>();

// category service
builder.Services.AddScoped<ICategoryService, CategoryService>();
// product type service
builder.Services.AddScoped<IProductTypeService, ProductTypeService>();
// unit of measure service
builder.Services.AddScoped<IUnitOfMeasureService, UnitOfMeasureService>();
// user service
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAssetService, AssetService>();


builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

builder.Services.AddScoped<IAccessManager, AccessManager>();

builder.Services.AddScoped<ICryptoService, CryptoService>();

builder.Services.AddScoped<IQueryOptimizationService, QueryOptimizationService> ();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductReviewService, ProductReviewService>();

builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IRewardRuleService, RewardRuleService>();
builder.Services.AddScoped<IUserActivityService, UserActivityService>();

builder.Services.AddScoped<IGenderService, GenderService>();

builder.Services.AddScoped<IValidator<ProductTypeInsertRequest>, ProductTypeInsertValidator>();
builder.Services.AddScoped<IValidator<ProductTypeUpdateRequest>, ProductTypeUpdateValidator>();
builder.Services.AddScoped<IValidator<UnitOfMeasureInsertRequest>, UnitOfMeasureInsertValidator>();
builder.Services.AddScoped<IValidator<UnitOfMeasureUpdateRequest>, UnitOfMeasureUpdateValidator>();
builder.Services.AddScoped<IValidator<CategoriesInsertRequest>, CategoryInsertValidator>();
builder.Services.AddScoped<IValidator<CategoriesUpdateRequest>, CategoryUpdateValidator>();
builder.Services.AddScoped<IValidator<UserInsertRequest>, UserInsertValidator>();
builder.Services.AddScoped<IValidator<UserUpdateRequest>, UserUpdateValidator>();
builder.Services.AddScoped<IValidator<AssetInsertRequest>, AssetInsertValidator>();
builder.Services.AddScoped<IValidator<AssetUpdateRequest>, AssetUpdateValidator>();
builder.Services.AddScoped<IValidator<ProductReviewInsertRequest>, ProductReviewInsertValidator>();
builder.Services.AddScoped<IValidator<ProductReviewUpdateRequest>, ProductReviewUpdateValidator>();

builder.Services.AddScoped<IValidator<ActivityInsertRequest>, ActivityInsertValidator>();
builder.Services.AddScoped<IValidator<ActivityUpdateRequest>, ActivityUpdateValidator>();
builder.Services.AddScoped<IValidator<RewardRuleInsertRequest>, RewardRuleInsertValidator>();
builder.Services.AddScoped<IValidator<RewardRuleUpdateRequest>, RewardRuleUpdateValidator>();
builder.Services.AddScoped<IValidator<UserActivityInsertRequest>, UserActivityInsertValidator>();
builder.Services.AddScoped<IValidator<UserActivityUpdateRequest>, UserActivityUpdateValidator>();

builder.Services.AddScoped<IValidator<GenderInsertRequest>, GenderInsertValidator>();
builder.Services.AddScoped<IValidator<GenderUpdateRequest>, GenderUpdateValidator>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(options => // dodavanje authentfikacije i autorizacije u projekat
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtToken:Issuer"],
        ValidAudience = builder.Configuration["JwtToken:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtToken:SecretKey"] ?? string.Empty)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = "v1",
            Title = "eCommerce API",
            Description = "API for managing products and categories in the eCommerce application"
        });

        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));

        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();


    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
