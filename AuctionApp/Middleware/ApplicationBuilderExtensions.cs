using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app, string root)
        {
            string path = Path.Combine(root,"node_modules");
            PhysicalFileProvider fileprovider = new PhysicalFileProvider(path);
            StaticFileOptions options = new StaticFileOptions();
            options.RequestPath = "/node_modules";
            options.FileProvider = fileprovider;
            app.UseStaticFiles(options);
            return app;
        }
    }
}
