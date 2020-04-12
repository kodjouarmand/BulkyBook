using BulkyBook.Models;
using Microsoft.AspNetCore.Identity;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        public ApplicationUser Get(string id);
        public IdentityRole GetRole(ApplicationUser user);
    }
}
