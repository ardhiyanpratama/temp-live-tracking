using BackendService.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Data.DataSeed
{
    public class SeedData
    {
        public static void Seed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            applicationDbContext.Database.Migrate();

            if (!applicationDbContext.AdditionalInformation.Any())
            {
                var additionalInformation = new List<AdditionalInformation> {
                    new AdditionalInformation
                    {
                        PlateNumber = "L 123 ABC",
                        Helper= "budi@gmail.com",
                    },
                    new AdditionalInformation
                    {
                        PlateNumber = "L 345 CBD",
                        Helper= "bambang@gmail.com",
                    },
                    new AdditionalInformation
                    {
                        PlateNumber = "L 678 EFG",
                        Helper= "armand@gmail.com",
                    },
                };

                applicationDbContext.AddRange(additionalInformation);
                applicationDbContext.SaveChanges();
            }


        }

    }
}
