using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Extensions
{
    public static class MyAwesomeExtensions
    {
        public static void AddAwesomeness(this IServiceCollection services, Action<MyAwesomeOptions>? options = null) 
        {
            services.Configure(options);
        }
    }


   public class MyAwesomeOptions
    {
        public string? Prefix { get; set; }
        public int AgeFilter { get; set; } = 50;
    }
}
