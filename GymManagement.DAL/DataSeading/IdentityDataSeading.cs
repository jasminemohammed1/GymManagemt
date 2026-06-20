using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.DataSeading
{
    public static class IdentityDataSeading
    {
        public static async Task SeadIdentityAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger logger, CancellationToken ct = default)
        {


            try
            {
                bool hasUsers = userManager.Users.Any();
                bool hasRoles = roleManager.Roles.Any();
                if (hasUsers && hasRoles) return;


                var roles = new List<IdentityRole>()
            {
                new IdentityRole("Admin"),
                new IdentityRole("SuperAdmin")
            };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        var res = await roleManager.CreateAsync(role);
                        if (!res.Succeeded)
                        {
                            logger.LogError($"Faild to create Role: {role.Name} : withh error {string.Join(',', res.Errors.Select(x => x.Description))}");
                        }
                    }
                }

                if (!hasUsers)
                {

                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "aliaa",
                        LastName = "tarek",
                        Email = "aliaa@gmail.com",
                        PhoneNumber = "01234567811",
                        UserName = "aliaatarek"
                    };
                    var res = await userManager.CreateAsync(MainAdmin, "Passw0rd@123");
                    if (res.Succeeded)
                    {
                        await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");
                    }
                    else
                    {
                        logger.LogError("Failed to create MainAdmin: {Errors}",
                            string.Join(',', res.Errors.Select(x => x.Description)));
                    }
                    await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");
                    var Admin = new ApplicationUser()
                    {
                        FirstName = "mohamed",
                        LastName = "tarek",
                        Email = "mohamed@gmail.com",
                        PhoneNumber = "01234567822",
                        UserName = "mohamedtarek"
                    };
                 var res2 =    await userManager.CreateAsync(Admin, "Passw0rd@123");
                    if (res2.Succeeded)
                    {
                        await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");
                    }
                    else
                    {
                        logger.LogError("Failed to create MainAdmin: {Errors}",
                            string.Join(',',res2.Errors.Select(x => x.Description)));
                    }


                    await userManager.AddToRoleAsync(Admin, "Admin");

                    logger.LogInformation("Identity Seaded ");
                }
                return;

            }
            catch (Exception ex)
            {




                logger.LogError(ex, "identity seading failed");
                return;



            }
        }
    }
}
