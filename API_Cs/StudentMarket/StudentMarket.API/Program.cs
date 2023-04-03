using Microsoft.AspNetCore.Mvc;
using StudentMarket.DL;
using StudentMarket.BL;
using StudentMarket.DL.UserDL;
using StudentMarket.BL.UserBL;
using StudentMarket.DL.CategoryDL;
using StudentMarket.BL.NotificationBL;
using StudentMarket.DL.NotificationDL;
using StudentMarket.BL.PostBL;
using StudentMarket.DL.PostDL;
using StudentMarket.BL.MessageBL;
using StudentMarket.DL.MessageDL;
using StudentMarket.DL.LocationDL;
using StudentMarket.BL.LocationBL;
using StudentMarket.DL.CommentDL;
using StudentMarket.BL.CommentBL;
using StudentMarket.BL.CategoryBL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
            .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

// Add services to the container.
builder.Services.AddScoped<IUserBL, UserBL>();
builder.Services.AddScoped<IUserDL, UserDL>();
builder.Services.AddScoped<ICategoryBL, CategoryBL>();
builder.Services.AddScoped<ICategoryDL, CategoryDL>();
builder.Services.AddScoped<ICommentBL, CommentBL>();
builder.Services.AddScoped<ICommentDL, CommentDL>();
builder.Services.AddScoped<ILocationBL, LocationBL>();
builder.Services.AddScoped<ILocationDL, LocationDL>();
builder.Services.AddScoped<IMessageBL, MessageBL>();
builder.Services.AddScoped<IMessageDL, MessageDL>();
builder.Services.AddScoped<INotificationBL, NotificationBL>();
builder.Services.AddScoped<INotificationDL, NotificationDL>();
builder.Services.AddScoped<IPostBL, PostBL>();
builder.Services.AddScoped<IPostDL, PostDL>();
builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

DBContext.connectionString = builder.Configuration.GetConnectionString("MySQL");

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                      });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
