using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class UserRole : IdentityUserRole<string>
    {
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
    }
}