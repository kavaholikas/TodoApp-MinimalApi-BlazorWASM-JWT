using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoJWT.Models
{
    public class ChangePasswordDto
    {
        public int UserID { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set;}
    }
}
