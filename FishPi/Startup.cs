using FishPi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FishPi.Startup))]
namespace FishPi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			ConfigureAuth(app);
			createRolesandUsers();
		}

		private void createRolesandUsers()
		{
			WebsiteDB context = new WebsiteDB();

			var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
			var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


			// In Startup iam creating first Admin Role and creating a default Admin User    
			if (!roleManager.RoleExists("Admin"))
			{

				// first we create Admin rool   
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Admin";
				roleManager.Create(role);

				//Here we create a Admin super user who will maintain the website                  

				var user = new ApplicationUser();
				user.UserName = "admin";
				user.Email = "admin@gmail.com";

				string userPWD = "Adm1n!23";

				var chkUser = userManager.Create(user, userPWD);

				//Add default User to Role Admin   
				if (chkUser.Succeeded)
				{
					var result1 = userManager.AddToRole(user.Id, "Admin");

				}
			}

			// creating Creating Manager role    
			if (!roleManager.RoleExists("Manager"))
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Manager";
				roleManager.Create(role);

			}

			// creating Creating Employee role    
			if (!roleManager.RoleExists("Student"))
			{
				var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
				role.Name = "Student";
				roleManager.Create(role);

			}
		}
	}
}
