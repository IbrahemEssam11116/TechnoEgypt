using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;

namespace TechnoEgypt.Services
{
    public  class SeedData
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task InitializeAsync()
        {
            await SeedRolesAsync(_roleManager);
        }

        private  async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "ADMIN", "STAGE", "BRANCH", "COURSE", "TRACK","APPLICATION", "MESSAGE" , "STAFF" };

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // Check if the role doesn't exist, then create it
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    // Create the role and seed it to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

                    // You can add additional logic here, e.g., check if the role creation was successful
                }
            }
        }
    }
}
    
