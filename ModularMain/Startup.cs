using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ModularMain.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModularMain
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        List<Assembly> _dynamicallyLoadedLibraries = new List<Assembly>();
        /// <summary>
        /// To be used with Razor class libraries loaded dynamically
        /// </summary>
        void LoadDynamicLibraries(ApplicationPartManager PartManager)
        {
            // get the output folder of this application
            string moduleSource = Configuration["ModulesPath"];
            moduleSource = Path.GetDirectoryName(moduleSource);

            // get the full filepath of any dll starting with the rcl_ prefix
            string Prefix = "Mod_";
            string SearchPattern = $"{Prefix}*.dll";
            string[] LibraryPaths = Directory.GetFiles(moduleSource, SearchPattern);

            if (LibraryPaths != null && LibraryPaths.Length > 0)
            {
                // create the load context
                LibraryLoadContext LoadContext = new LibraryLoadContext(moduleSource);

                Assembly Assembly;
                ApplicationPart ApplicationPart;
                foreach (string LibraryPath in LibraryPaths)
                {
                    // load each assembly using its filepath
                    Assembly = LoadContext.LoadFromAssemblyPath(LibraryPath);

                    // create an application part for that assembly
                    ApplicationPart = LibraryPath.EndsWith(".Views.dll") ? new CompiledRazorAssemblyPart(Assembly) as ApplicationPart : new AssemblyPart(Assembly);

                    // register the application part
                    PartManager.ApplicationParts.Add(ApplicationPart);

                    // if it is NOT the *.Views.dll add it to a list for later use
                    if (!LibraryPath.EndsWith(".Views.dll"))
                        _dynamicallyLoadedLibraries.Add(Assembly);
                }
            }

        }
        /// <summary>
        /// Registers a <see cref="CompositeFileProvider"/> for each dynamically loaded assembly.
        /// </summary>
        void RegisterDynamicLibariesStaticFiles(IWebHostEnvironment env)
        {
            IFileProvider FileProvider;
            foreach (Assembly A in _dynamicallyLoadedLibraries)
            {
                // create a "web root" file provider for the embedded static files found on wwwroot folder
                FileProvider = new ManifestEmbeddedFileProvider(A, "wwwroot");

                // register a new composite provider containing
                // the old web root file provider
                // and the new one we just created
                env.WebRootFileProvider = new CompositeFileProvider(env.WebRootFileProvider, FileProvider);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .ConfigureApplicationPartManager((PartManager) => {
                    LoadDynamicLibraries(PartManager);      // dynamic RCLs
                })
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            if (_dynamicallyLoadedLibraries.Count > 0)
            {
                RegisterDynamicLibariesStaticFiles(env);
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
