using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.Persistence.Database.Extensions;

namespace Sheaft.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseInfrastructure(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var contextFactory = serviceScope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
                var context = contextFactory.CreateDbContext();
                
                if (!context.AllMigrationsApplied())
                {
                    context.Migrate();
                }

                var settingsEnum = Enum.GetValues(typeof(SettingKind)).Cast<SettingKind>().ToList();
                var settings = context.Settings.ToList();
                var missingSettings = settingsEnum.Except(settings.Select(s => s.Kind));
                foreach (var missingSetting in missingSettings)
                {
                    context.Add(new Setting(missingSetting.ToString("G"), missingSetting));
                    context.SaveChanges();
                }

                var removedSettings = settings.Select(s => s.Kind).Except(settingsEnum);
                foreach (var removedSetting in removedSettings)
                {
                    var setting = settings.FirstOrDefault(s => s.Kind == removedSetting);
                    if (setting == null)
                        continue;

                    context.Remove(setting);
                    context.SaveChanges();
                }
            }
        }
    }
}