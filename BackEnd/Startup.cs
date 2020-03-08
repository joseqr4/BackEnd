using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyModel;
using System.IO;
using System.Reflection;
using BackEnd.Services;

namespace BackEnd
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
            services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

 

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddCookie()
                .AddJwtBearer(options =>
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = "aprovechapp.com",
                     ValidAudience = "aprovechapp.com",
                     IssuerSigningKey = new SymmetricSecurityKey(
                    //Encoding.UTF8.GetBytes(Configuration["Llave_super_secreta"])),
                    Encoding.UTF8.GetBytes("somethingyouwantwhichissecurewillworkk")),
                     ClockSkew = TimeSpan.Zero
                 });
            //services.AddSingleton<IEcommerceRepository, MyEcommerceRepository>();

            //services.AddSwaggerGen(c => {
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "Aprovechapp",
            //        Version = "v1",
            //        Description = "Proyecto de Grado",
            //        Contact = new OpenApiContact()
            //        {
            //            Name = "Jose Curti",
            //            Email = "josecurti.jc@gmail.com"
            //        }

            //    });
            //});

            services.AddSwaggerGen(config => 

            config.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
            {
                Title = "Aprovechapp",
                Version = "1.0.0"
            }));

            //var basePath = AppContext.BaseDirectory;
            //var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
            //var fileName = System.IO.Path.GetFileName(assemblyName + ".xml");

            //services.AddSwaggerGen(config =>

            //config.IncludeXmlComments(System.IO.Path.Combine(basePath, fileName)));
      



            services.AddMvc(options =>
        options.EnableEndpointRouting = false)
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<EmailSenderOptions>(Configuration.GetSection("EmailSenderOptions"));


         

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext contex, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

         
            app.UseMvc();
            
            app.UseSwagger();
            app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json","Gnexus"));
            CreateRoles(serviceProvider);
        


        }


        private void CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            Task<IdentityResult> roleResult;
            

            //Check that there is an Administrator role and create if not
            Task<bool> hasAdminRole = roleManager.RoleExistsAsync("Member");
            hasAdminRole.Wait();

            if (!hasAdminRole.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("Member"));
                roleResult.Wait();
            }

            Task<bool> hasAdminRole2 = roleManager.RoleExistsAsync("Company");
            hasAdminRole.Wait();

            if (!hasAdminRole2.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("Company"));
                roleResult.Wait();
            }


            Task<bool> hasAdminRole3 = roleManager.RoleExistsAsync("Administrator");
            hasAdminRole.Wait();

            if (!hasAdminRole3.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("Administrator"));
                roleResult.Wait();
            }


            Task<bool> hasAdminRole4 = roleManager.RoleExistsAsync("User");
            hasAdminRole.Wait();

            if (!hasAdminRole4.Result)
            {
                roleResult = roleManager.CreateAsync(new IdentityRole("User"));
                roleResult.Wait();
            }

        }

    }
}
