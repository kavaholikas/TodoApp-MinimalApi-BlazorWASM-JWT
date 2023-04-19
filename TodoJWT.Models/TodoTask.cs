using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoJWT.Models
{
    public class TodoTask
    {
        public int TodoTaskID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleated { get; set; }
        public bool IsArchived { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public TodoTask CreateFromDto(TodoTaskDto dto)
        {
            return new TodoTask()
            {
                Name = dto.Name,
                Description = dto.Description,
                IsCompleated = false,
                IsArchived = false
            };
        }

        public TodoTaskDto CreateDto()
        {
            return new TodoTaskDto()
            {
                TodoTaskID = TodoTaskID,
                Name = this.Name,
                Description = this.Description,
                IsCompleated = this.IsCompleated,
                IsArchived = this.IsArchived
            };
        }
    }
}
