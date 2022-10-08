using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using ProiectDAW2.Models;

[assembly: OwinStartupAttribute(typeof(ProiectDAW2.Startup))]
namespace ProiectDAW2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            // Se apeleaza o metoda in care se adauga contul de administrator si rolurile aplicatiei
            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new
            RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new
            UserStore<ApplicationUser>(context));
            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("Admin"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // Primul admin
                var user1 = new ApplicationUser();
                user1.UserName = "admin1@gmail.com";
                user1.Email = "admin1@gmail.com";
                var adminCreated1 = UserManager.Create(user1, "Admin1!");
                if (adminCreated1.Succeeded)
                {
                    UserManager.AddToRole(user1.Id, "Admin");
                }

                // Al doilea admin
                var user2 = new ApplicationUser();
                user2.UserName = "admin2@gmail.com";
                user2.Email = "admin2@gmail.com";
                var adminCreated2 = UserManager.Create(user2, "Admin2!");
                if (adminCreated2.Succeeded)
                {
                    UserManager.AddToRole(user2.Id, "Admin");
                }

            }
            if (!roleManager.RoleExists("Colaborator"))
            {
                var role = new IdentityRole();
                role.Name = "Colaborator";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}
