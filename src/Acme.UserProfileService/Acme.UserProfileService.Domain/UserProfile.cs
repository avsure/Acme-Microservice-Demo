using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.UserProfileService.Domain
{
     public class UserProfile
        {
            public Guid Id { get; private set; }
            public string Name { get; private set; }
            public string Email { get; private set; }
            public int Age { get; private set; }

            private UserProfile() { }

            public UserProfile(string name, string email, int age)
            {
                Id = Guid.NewGuid();
                Name = name;
                Email = email;
                Age = age;
            }

            public void Update(string name, string email, int age)
            {
                Name = name;
                Email = email;
                Age = age;
            }
        }
}
