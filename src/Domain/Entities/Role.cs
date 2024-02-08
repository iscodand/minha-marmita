﻿using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Role : IdentityRole<string>
    {
        public string Description { get; set; }

        public static Role Create(string name, string description)
        {
            return new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                NormalizedName = name.Trim().ToUpper(),
                Description = description
            };
        }
    }
}
