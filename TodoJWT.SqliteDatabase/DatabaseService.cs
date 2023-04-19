using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TodoJWT.Models;

namespace TodoJWT.SqliteDatabase
{
    public class DatabaseService : IDatabaseService
    {
        public void CreateUser(User user)
        {
            using (UserContext db = new UserContext())
            {
                db.Add(user);
                db.SaveChanges();
            }
        }

        public User? GetUser(int id)
        {
            using (UserContext db = new UserContext())
            {
                try
                {
                    return db.Users.First(x => x.UserID == id);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public User? GetUser(string username)
        {
            using (UserContext db = new UserContext())
            {
                try
                {
                    return db.Users.First(x => x.Username == username);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public void RemoveUser(User user)
        {
            if (user.IsAdmin)
            {
                return;
            }

            using (UserContext db = new UserContext())
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            using (UserContext db = new UserContext())
            {
                db.Users.Update(user);
                db.SaveChanges();
            }
        }

        public bool UsernameTaken(string username)
        {
            using (UserContext db = new UserContext())
            {
                try
                {
                    User user = db.Users.First(u => u.Username == username);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool PromoteUser(int id)
        {
            using (UserContext db = new UserContext())
            {
                try
                {
                    User user = db.Users.Find(id);
                    user.IsAdmin = true;
                    db.SaveChanges();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public List<User> GetUsers()
        {
            using (UserContext db = new UserContext())
            {
                return db.Users.ToList();
            }
        }

        public void CreateUserTask(User user, TodoTask task)
        {
            using (UserContext db = new UserContext())
            {
                User? u = db.Users.Find(user.UserID);

                if (u != null)
                {
                    u.Tasks.Add(task);
                    db.SaveChanges();
                }
            }
        }

        public List<TodoTask> GetUserTasks(User user)
        {
            using (UserContext db = new UserContext())
            {
                return db.Tasks.Where(t => t.UserID == user.UserID && !t.IsArchived).ToList();
            }
        }

        public List<TodoTask> GetUserArchivedTasks(User user)
        {
            using (UserContext db = new UserContext())
            {
                return db.Tasks.Where(t => t.UserID == user.UserID && t.IsArchived).ToList();
            }
        }

        public bool CompleteTask(int id)
        {
            using (UserContext db = new UserContext())
            {
                try
                {
                    var task = db.Tasks.Find(id);
                    task.IsCompleated = true;
                    db.SaveChanges();
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
        }

        public bool RemoveTask(int id)
        {
            using (UserContext db = new UserContext())
            {
                try
                {
                    var task = db.Tasks.Find(id);
                    db.Tasks.Remove(task);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool ArchiveTask(int id)
        {
            using (UserContext db = new UserContext())
            {
                try
                {
                    var task = db.Tasks.Find(id);
                    task.IsArchived = true;
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
