using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoJWT.Models
{
    public class TodoTaskDto
    {
        public int TodoTaskID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleated { get; set; }
        public bool IsArchived { get; set; }
    }
}
