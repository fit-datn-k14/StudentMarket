using StudentMarket.API.Hubs;
using Microsoft.AspNetCore.Mvc;
using StudentMarket.BL.CategoryBL;
using StudentMarket.BL.CommentBL;
using StudentMarket.BL.LocationBL;
using StudentMarket.BL.MessageBL;
using StudentMarket.BL.NotificationBL;
using StudentMarket.BL.PostBL;
using StudentMarket.BL.UserBL;
using StudentMarket.BL;
using StudentMarket.DL.CategoryDL;
using StudentMarket.DL.CommentDL;
using StudentMarket.DL.LocationDL;
using StudentMarket.DL.MessageDL;
using StudentMarket.DL.NotificationDL;
using StudentMarket.DL.PostDL;
using StudentMarket.DL.UserDL;
using StudentMarket.DL;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace StudentMarket.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy
                    {
                        ProcessDictionaryKeys = true,
                        OverrideSpecifiedNames = true,
                        ProcessExtensionDataNames = true
                    }
                };
            });
            // Cấu hình các dịch vụ sử dụng trong ứng dụng
            services.AddSignalR();
            services.AddScoped<NotificationHub>();
            services.AddScoped<ChatHub>();
            services.AddScoped<IUserBL, UserBL>();
            services.AddScoped<IUserDL, UserDL>();
            services.AddScoped<ICategoryBL, CategoryBL>();
            services.AddScoped<ICategoryDL, CategoryDL>();
            services.AddScoped<ICommentBL, CommentBL>();
            services.AddScoped<ICommentDL, CommentDL>();
            services.AddScoped<ILocationBL, LocationBL>();
            services.AddScoped<ILocationDL, LocationDL>();
            services.AddScoped<IMessageBL, MessageBL>();
            services.AddScoped<IMessageDL, MessageDL>();
            services.AddScoped<INotificationBL, NotificationBL>();
            services.AddScoped<INotificationDL, NotificationDL>();
            services.AddScoped<IPostBL, PostBL>();
            services.AddScoped<IPostDL, PostDL>();
            services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
            services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            DBContext.connectionString = config.GetConnectionString("MySQL");
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:8081")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:8082")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notification");
                endpoints.MapHub<ChatHub>("/chat");
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

        }
    }
}
