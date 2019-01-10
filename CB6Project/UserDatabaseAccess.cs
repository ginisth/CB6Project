using System;
using System.Linq;

namespace CB6Project
{
    public static class UserDatabaseAccess
    {
        public static string FindUser(string username)
        {
            User person = null;
            using (var context = new ProjectDatabaseEntities())
            {
                Console.WriteLine("Type the username.");
                person = context.Users.Find(Inputs.InputForUsername());

                // User can't find his own username to avoid delete himself or send message to himself
                // Only exception in the UpdateUser function where the FindUser method parameter is passed
                // as null
                if (person != null && person.Username != username)
                    return person.Username;
                else
                {
                    Console.WriteLine("The user doesn't exists.Press any key to return at the Main menu.");
                    Console.ReadKey();
                    return null;
                }
            }
        }

        public static void AddUser()
        {
            Console.Clear();
            using (var context = new ProjectDatabaseEntities())
            {
                bool remain = false;
                string inputUsername;
                do
                {
                    Console.WriteLine("Give username");
                    inputUsername = Inputs.InputForUsername();
                    if (context.Users.Any(x => x.Username == inputUsername))
                        Console.WriteLine("Username already exists");
                    else
                        remain = true;
                } while (remain == false);
                Console.WriteLine("Give password");
                var person = context.Users.Add(new User
                {
                    Username = inputUsername,
                    Password = Inputs.InputForPassword(),
                    Role = RoleType.CustomUser
                });

                context.SaveChanges();
                Console.WriteLine($"\n{inputUsername} added successfully!Press any key to return at the Main menu");
            }
            Console.ReadKey();
        }

        public static void ShowUsers()
        {
            Console.Clear();
            using (var context = new ProjectDatabaseEntities())
            {
                var users = context.Users.ToList().OrderBy(x => x.Role);
                Console.WriteLine("Username <--> Role \n----------------------");
                foreach (var u in users)
                    Console.WriteLine($"{u.Username} <--> {u.Role}");
            }
            Console.WriteLine("-->Press any key to return at the Main menu");
            Console.ReadKey();
        }

        public static (string, bool) ReturnValidUser()
        {
            using (var context = new ProjectDatabaseEntities())
            {
                bool remain = true;
                string user;
                do
                {
                    Console.Clear();
                    Console.WriteLine("--> Give username.");
                    string inputUsername = Inputs.InputForUsername();
                    Console.WriteLine("-->Give password.");
                    string inputPassword = Inputs.InputForPassword();

                    user = context.Users.Where(x => x.Username == inputUsername && x.Password == inputPassword)
                                        .Select(x => x.Username).SingleOrDefault();
                    var pass = context.Users.Where(x => x.Username == inputUsername && x.Password == inputPassword)
                                         .Select(x => x.Password).SingleOrDefault();

                    //using double condition due to password sensibility

                    if (user != null && pass == inputPassword)
                    {
                        Console.Clear();
                        WelcomeHeader();
                        Console.WriteLine("Username and password match!Press any key to continue");
                        Console.ReadKey();
                        return (user, true);
                    }
                    else
                    {
                        Console.WriteLine("\nWrong username or password.");
                        Console.WriteLine("-->Press Esc to go back or any other key to continue");
                        remain = Inputs.CheckPoint();
                    }

                } while (remain);
                return (user, remain);
            }
        }


        public static RoleType FindRole(string username)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var person = context.Users.Find(username);
                RoleType role = person.Role;
                return role;

            }

        }

        public static void UpdateRole(string username)
        {
            Console.Clear();
            Console.WriteLine("Which user you want to update?");
            string RoleToUpdate = FindUser(username);
            if (RoleToUpdate != null)
            {
                using (var context = new ProjectDatabaseEntities())
                {
                    var result = context.Users.Where(x => x.Username == RoleToUpdate).First();
                    Console.WriteLine($"What role would you like to give to {RoleToUpdate}?");
                    Console.WriteLine("1.Super Moderator \n2.Moderator \n3.Custom User");
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.D1:
                            result.Role = RoleType.SuperMOD;
                            Console.WriteLine($"{RoleToUpdate} updated to Super Moderator!" +
                                $"Press any key to go back to the main menu!");
                            break;
                        case ConsoleKey.D2:
                            result.Role = RoleType.MOD;
                            Console.WriteLine($"{RoleToUpdate} updated to Moderator!" +
                                $"Press any key to go back to the main menu!");
                            break;
                        case ConsoleKey.D3:
                            result.Role = RoleType.CustomUser;
                            Console.WriteLine($"{RoleToUpdate} updated to Custom User!" +
                                $"Press any key to go back to the main menu!");
                            break;
                        default:
                            Console.WriteLine("Wrong key! Retry.");
                            break;
                    }
                    context.SaveChanges();
                    Console.ReadKey();
                }
            }
        }

        public static void DeleteUser(string username)
        {
            Console.Clear();
            Console.WriteLine("Which user you want to delete?");
            string userToDelete = FindUser(username);
            if (userToDelete != null)
            {
                using (var context = new ProjectDatabaseEntities())
                {
                    var user = context.Users.Where(x => x.Username == userToDelete).FirstOrDefault();
                    var msg = context.Messages.Where(x => x.Sender == userToDelete).ToList();
                    foreach (var m in msg)
                    {
                        m.Sender = null;
                    }
                    var msg2 = context.Messages.Where(x => x.Receiver == userToDelete).ToList();
                    foreach (var m2 in msg2)
                    {
                        m2.Receiver = null;
                    }

                    context.Users.Remove(user);
                    context.SaveChanges();
                    Console.WriteLine($"{userToDelete} has been deleted!Press any key to continue.");
                    Console.ReadKey();
                }
            }
        }
        public static void UpdateUser(string username)
        {
            Console.Clear();
            Console.WriteLine("Which user you want to update?");
            string userToUpdate =FindUser(username);
            if (userToUpdate != null)
            {
                Console.WriteLine("Give a new username");
                string newName = Inputs.InputForUsername();
                Console.WriteLine("Give a new password");
                string newPassword = Inputs.InputForPassword();
                using (var context = new ProjectDatabaseEntities())
                {
                    var user = context.Users.Where(x => x.Username == userToUpdate).FirstOrDefault();

                    // Holding the same Role.
                    RoleType existingRole = user.Role;

                    var msg = context.Messages.Where(x => x.Sender == userToUpdate).ToList();
                    foreach (var m in msg)
                    {
                        m.Sender = newName;
                    }
                    var msg2 = context.Messages.Where(x => x.Receiver == userToUpdate).ToList();
                    foreach (var m2 in msg2)
                    {
                        m2.Receiver = newName;
                    }

                    // Removing the old User.
                    context.Users.Remove(user);

                    // Adding a new updated User.
                    var newuser = context.Users.Add(new User
                    {
                        Username = newName,
                        Password = newPassword,
                        Role = existingRole
                    });
                    context.SaveChanges();
                    Console.WriteLine($"\n{newName} has been updated!Press any key to continue.");
                    Console.ReadKey();
                }
            }
        }

        public static void FirstTimeRegister()
        {
            Console.Clear();
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
                    Console.WriteLine("Succecfully created Administrator!");
                    Console.WriteLine("From the Login screen,login with username:admin and password:admin" +
                        " as administrator");
                    context.SaveChanges();
                    Console.ReadKey();
                }
            }
        }

        static void WelcomeHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"          _______  _        _______  _______  _______  _______  _  _ 
|\     /|(  ____ \( \      (  ____ \(  ___  )(       )(  ____ \( )( )
| )   ( || (    \/| (      | (    \/| (   ) || () () || (    \/| || |
| | _ | || (__    | |      | |      | |   | || || || || (__    | || |
| |( )| ||  __)   | |      | |      | |   | || |(_)| ||  __)   | || |
| || || || (      | |      | |      | |   | || |   | || (      (_)(_)
| () () || (____/\| (____/\| (____/\| (___) || )   ( || (____/\ _  _ 
(_______)(_______/(_______/(_______/(_______)|/     \|(_______/(_)(_)
                                                                     
");
            Console.ResetColor();
        }


    }
}
