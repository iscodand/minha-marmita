using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    [Table("USERS")]
    public class User : IdentityUser<string>
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company UserCompany { get; set; }
        public int CompanyId { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Meal> Meals { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public static User Create(string name,
                                  string email,
                                  string username,
                                  int companyId)
        {
            User user = new()
            {
                Name = name,
                NormalizedName = name.Trim().ToUpper(),
                Email = email,
                IsActive = true,
                UserName = username,
                CompanyId = companyId
            };

            return user;
        }

        public User Deactivate()
        {
            IsActive = false;
            return this;
        }

        public User Activate()
        {
            IsActive = true;
            return this;
        }
    }
}