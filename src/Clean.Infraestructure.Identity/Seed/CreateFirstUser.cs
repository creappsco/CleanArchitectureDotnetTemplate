using System.Net; 
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Clean.Infraestructure.Identity.Models;
using Clean.Core.Application.Models.Authentication;

namespace Clean.Infraestructure.Identity.Seed
{
    public static class UserCreator
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var result = await roleManager.CreateAsync(new IdentityRole(AuthRoles.Administrator.ToString())); 
            await roleManager.CreateAsync(new IdentityRole(AuthRoles.User.ToString())); 

            var applicationUser = new ApplicationUser
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe@cleantemplate.co",
                Email = "johndoe@cleantemplate.co", 
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(applicationUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(applicationUser, "P@55w0rd1");
                await userManager.AddToRoleAsync(applicationUser, AuthRoles.Administrator.ToString());
            }
        }
    }
}