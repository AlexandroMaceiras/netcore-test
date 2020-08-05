using AlbertEinstein.Models;
using AlbertEinstein.Services;
using Consultas.Services;
using Medicos.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pacientes.Services;

namespace AlbertEinstein
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
            services.AddDbContext<AlbertEinsteinContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Registrando a DI (Injeção de Dependência)
             services.AddTransient<IConsultaItemService, ConsultaItemService>(); 
             services.AddTransient<IMedicoItemService, MedicoItemService>(); 
             services.AddTransient<IPacienteItemService, PacienteItemService>(); 
             services.AddTransient<IOutrosItemService, OutrosItemService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                OpenApiContact Contato = new OpenApiContact()
                {
                    Email = "alexandro@maceiras.com.br",
                    Name = "Click Here"
                };

                OpenApiLicense Licenca = new OpenApiLicense();
                Licenca.Name = "Copyright(C)";

                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "AlbertEinstein", 
                    Version = "Versão 1.0.0", 
                    Description = "Aplicação de Teste do Albert Einstein",
                    Contact = Contato,
                    License = Licenca
                });
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

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WEB.API Versão 1.0.0");
                c.RoutePrefix = string.Empty;
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
