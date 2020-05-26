using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqliteLib.Model;

namespace webServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DateTime dateTime = DateTime.Parse("05/01/2019 06:32:00");
            string str = dateTime.ToString();

            FileInfo fileInfo = new FileInfo(NoteContext.FilePath);
            if(!File.Exists(fileInfo.FullName))
            {
                NoteContext noteContext = new NoteContext(true);
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
