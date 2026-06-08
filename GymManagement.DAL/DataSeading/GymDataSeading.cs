using GYMProject.DBContexts;
using GYMProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeading
{
    public static  class GymDataSeading
    { 
        public static  async Task SaedData(GymDBContext db , string SaedFolderPath ,ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if (!await db.Plans.AnyAsync(ct))
                {
                    var plans = ReadDataFromFile<Plan>(SaedFolderPath, "plans.json");
                    if(plans.Any())
                    {
                        db.Plans.AddRange(plans);
                  
                        logger.LogInformation($"Data Seadeed with count {plans.Count()}");

                    }
                    if(db.ChangeTracker.HasChanges())
                    {
                        await db.SaveChangesAsync(ct);
                    }
                    else
                    {
                        logger.LogInformation("Data Already Seaded");
                    }




                }
            }catch(Exception ex)
            {

                logger.LogError("Gym Data Seading Failed ");
                throw;

            }
        }


        private static  List<T> ReadDataFromFile<T>(string seadFolderPath , string FileName)
        {
            var filePath = Path.Combine(seadFolderPath, FileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Sead Data File Not Exists: {filePath}");
            }

            var data = File.ReadAllText(filePath);
            var opts = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            var plans = JsonSerializer.Deserialize<List<T>>(data, opts) ?? [];
            return plans;
        }
    }
}
