using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Passbook.SampleWebService.Repository;
using Passbook.SampleWebService.Services;

namespace Passbook.SampleWebService
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var generatorConfig = new PassGeneratorConfiguration();
            Configuration.Bind("PassGenerator", generatorConfig);
            services.AddSingleton(generatorConfig);

            services.AddSingleton<IWebServiceHandler, InMemoryHandler>();
            services.AddSingleton<IPassService, PassService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Pass}/{action=Index}/{id?}");
            });
        }
    }
}
