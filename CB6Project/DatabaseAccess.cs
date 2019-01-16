using System.Collections.Generic;
using System.Linq;

namespace CB6Project
{
    public static class DatabaseAccess
    {
        public static User FindUser(string username)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var person = context.Users.Find(username);
                return person;
            }
        }

        public static void AddUser(User user)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var person = context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static List<User> ShowUsers()
        {
            using (var context = new ProjectDatabaseEntities())
            {
                return context.Users.ToList().OrderBy(x => x.Role).ToList();
            }
        }

        public static User ReturnValidUser(string username, string password)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                User user = context.Users.Where(x => x.Username == username && x.Password == password)
                                        .SingleOrDefault();
                var pass = context.Users.Where(x => x.Username == username && x.Password == password)
                                        .Select(x => x.Password)
                                        .SingleOrDefault();

                // Using two queries to check the password sensibility
                if (user != null && pass == password)
                    return user;
                else
                    return null;
            }
        }



        public static void UpdateRole(User user, RoleType roleType)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var result = context.Users.Where(x => x.Username == user.Username).First();
                result.Role = roleType;
                context.SaveChanges();
            }

        }

        public static void DeleteUser(User user)
        {

            using (var context = new ProjectDatabaseEntities())
            {
                var person = context.Users.Where(x => x.Username == user.Username).FirstOrDefault();

                var msg = context.Messages.Where(x => x.Sender == user.Username).ToList();

                foreach (var m in msg)
                {
                    m.Sender = null;
                }

                var msg2 = context.Messages.Where(x => x.Receiver == user.Username).ToList();

                foreach (var m2 in msg2)
                {
                    m2.Receiver = null;
                }

                context.Users.Remove(person);
                context.SaveChanges();
            }

        }
        public static void UpdateUser(User oldUser, User newUser)
        {

            using (var context = new ProjectDatabaseEntities())
            {
                var person = context.Users.Where(x => x.Username == oldUser.Username).FirstOrDefault();

                var msg = context.Messages.Where(x => x.Sender == oldUser.Username).ToList();

                foreach (var m in msg)
                {
                    m.Sender = newUser.Username;
                }

                var msg2 = context.Messages.Where(x => x.Receiver == oldUser.Username).ToList();

                foreach (var m2 in msg2)
                {
                    m2.Receiver = newUser.Username;
                }

                // Removing the old User.
                context.Users.Remove(person);

                // Adding a new updated User.
                var addNewuser = context.Users.Add(newUser);

                context.SaveChanges();

            }

        }

        public static bool FirstTimeRegister()
        {
            using (var context = new ProjectDatabaseEntities())
            {
                if (context.Users.Count() == 0)
                {
                    context.Users.Add(new User
                    {
                        Username = "admin",
                        Password = "admin",
                        Role = RoleType.Admin
                    });
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
        }

        public static List<Message> ShowMessages()
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.OrderBy(x => x.SubscriptionDate).ToList();
                return msg;
            }
        }

        public static void SendMessage(Message message)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.Add(message);
                context.SaveChanges();
            }


        }

        public static List<Message> Inbox(string receiver)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.Where(x => x.Receiver == receiver)
                                            .OrderByDescending(x => x.Various == "Unread")
                                           .ThenByDescending(x => x.SubscriptionDate)
                                           .ToList();
                return msg;
            }

        }

        public static void MarkAsRead(Message message)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.Where(x => x.Message_id == message.Message_id).FirstOrDefault();
                msg.Various = "Read";
                context.SaveChanges();
            }
        }

        public static List<Message> Outbox(string sender)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.Where(x => x.Sender == sender)
                                          .OrderBy(x => x.SubscriptionDate)
                                          .ToList();
                return msg;
            }
        }

        public static Message FindMessage(int message)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.Find(message);
                return msg;
            }
        }

        public static void EditMessage(int editMsg, string messageData)
        {

            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.Where(x => x.Message_id == editMsg).SingleOrDefault();
                msg.Message_Data = messageData;
                context.SaveChanges();
            }
        }

        public static int CountUnreadMessage(string receiver)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.Count(x => x.Receiver == receiver && x.Various == "Unread");
                return msg;
            }
        }

        public static void DeleteMessage(int deleteMsg)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.Where(x => x.Message_id == deleteMsg).SingleOrDefault();
                context.Messages.Remove(msg);
                context.SaveChanges();
            }
        }

    }
}
