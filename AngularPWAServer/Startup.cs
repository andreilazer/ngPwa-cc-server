using AngularPWAServer.Services;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AngularPWAServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PushNotificationsOptions>(Configuration.GetSection("PushNotifications"));
            services.AddSingleton<IPushSubscriptionsService, PushSubscriptionsService>();
            services.AddHttpClient<PushServiceClient>();
            services.AddControllers();

            string[] origins = new string[] { "http://localhost:4200", "http://localhost:5000" };
            services.AddCors(options =>
            {
                options.AddPolicy("the cors",
                builder =>
                {
                    builder.WithOrigins(origins)
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AngularPWAServer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AngularPWAServer");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
