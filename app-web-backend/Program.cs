using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using app_web_backend.Models;

namespace app_web_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Seed admin user if no users exist
            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    if (!context.Usuarios.Any())
                    {
                        var admin = new Usuario
                        {
                            Nome = "admin",
                            Senha = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                            Perfil = Perfil.Admin
                        };

                        context.Usuarios.Add(admin);
                        context.SaveChanges();

                        Console.WriteLine($"Seeded admin user: Id={admin.Id}, Nome={admin.Nome}, senha=Admin@123");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao seedar usuário: {ex.Message}");
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
