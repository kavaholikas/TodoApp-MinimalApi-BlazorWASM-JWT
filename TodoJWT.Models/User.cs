using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TodoJWT.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }

        public List<TodoTask> Tasks { get; set; } = new List<TodoTask>();

        public UserDto CreateDto(User user)
        {
            return new UserDto()
            {
                UserID = user.UserID,
                Username = user.Username,
                IsAdmin = user.IsAdmin
            };
        }
    }
}
