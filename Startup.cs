
using CardWebApi.Extensions;
using CardWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace CardWebApi
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerDocumentation();
            services.AddCors();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddDbContext<PaymentDetailContext>(options =>
                options.UseNpgsql(_config["Data:DefaultConnection:ConnectionString"])
            );
            services.AddControllers()
                   .AddNewtonsoftJson(options =>
                   {
                       options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                       options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                   });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(options =>
                options.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c
.SwaggerEndpoint("/swagger/v1/swagger.json", "Modulr Microservice API v1");
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
