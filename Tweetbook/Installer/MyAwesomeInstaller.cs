using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Extensions;

namespace Tweetbook.Installer
{
    public class MyAwesomeInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAwesomeness(option =>
            {
                option.Prefix = "_alex";
                option.AgeFilter = 49;
            });
        }
    }
}
