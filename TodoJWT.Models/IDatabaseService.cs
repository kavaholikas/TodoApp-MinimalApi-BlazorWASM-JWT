using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoJWT.Models
{
    public interface IDatabaseService
    {
        public User? GetUser(int id);
        public User? GetUser(string username);
        public void CreateUser(User user);
        public void UpdateUser(User user);
        public void RemoveUser(User user);
        public bool UsernameTaken(string username);
        public bool PromoteUser(int id);
        public List<User> GetUsers();

        public void CreateUserTask(User user, TodoTask task);
        public List<TodoTask> GetUserTasks(User user);
        public List<TodoTask> GetUserArchivedTasks(User user);

        public bool CompleteTask(int id);
        public bool RemoveTask(int id);
        public bool ArchiveTask(int id);
    }
}
