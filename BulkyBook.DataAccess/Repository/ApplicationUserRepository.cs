using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace BulkyBook.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public ApplicationUser Get(string id)
        {
            return dbSet.Find(id);
        }

        public IdentityRole GetRole(ApplicationUser user)
        {
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
            if (roles.FirstOrDefault() != null)
                return roles.FirstOrDefault(u => u.Id == roleId);
            else
                return new IdentityRole()
                {
                    Name = ""
                };
        }
    }
}
