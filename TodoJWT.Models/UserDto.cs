using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoJWT.Models
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
    }
}
