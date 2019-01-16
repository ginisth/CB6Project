using System;

namespace CB6Project
{
    public class MenuOptions 
    {
        private User _logedUser { get; set; }

        public MenuOptions(User logedUser)
        {
            _logedUser = logedUser;
        }

        public void AddUser()
        {
            Console.Clear();
            Console.WriteLine("Type the username of the new user");
            string newUsername = Inputs.InputForUsername();
            User checkUser = DatabaseAccess.FindUser(newUsername);
            if (checkUser == null)
            {
                Console.WriteLine("Type the password of the new user");
                string newPassword = Inputs.InputForPassword();
                User newUser = new User()
                {
                    Username = newUsername,
                    Password = newPassword,
                    Role = RoleType.CustomUser
                };
                DatabaseAccess.AddUser(newUser);
                Console.WriteLine($"\n{newUsername} successfully created!");
            }
            else
                Console.WriteLine("Username already exists.");
            Console.ReadKey();
        }

        public void ShowUsers()
        {
            Console.Clear();
            var usersList = DatabaseAccess.ShowUsers();
            Console.WriteLine("Username <--> Role \n----------------------");
            foreach (var u in usersList)
                Console.WriteLine($"{u.Username} <--> {u.Role}");
            Console.WriteLine("-->Press any key to return at the Main menu");
            Console.ReadKey();
        }

        public void UpdateRole()
        {

            Console.Clear();
            Console.WriteLine("Which user you want to update?");
            string username = Inputs.InputForUsername();
            User RoleToUpdate = DatabaseAccess.FindUser(username);
            if (RoleToUpdate!=null && RoleToUpdate.Username!=_logedUser.Username)
            {
                Console.WriteLine($"Which role would you like to assign to {username}?Press the corresponding key:");
                Console.WriteLine("1.Super Moderator \n2.Moderator \n3.Custom User");
                ConsoleKey key = Console.ReadKey(true).Key;
                RoleType role = RoleToUpdate.Role;
                switch (key)
                {
                    case ConsoleKey.D1:
                        role = RoleType.SuperMOD;
                        break;
                    case ConsoleKey.D2:
                        role = RoleType.MOD;
                        break;
                    case ConsoleKey.D3:
                        role = RoleType.CustomUser;
                        break;
                    default:
                        Console.WriteLine("Wrong key! Retry.");
                        break;
                }
                DatabaseAccess.UpdateRole(RoleToUpdate, role);
                Console.WriteLine($"{username} is now a {role.ToString()}!" +
                            $"Press any key to go back to the main menu!");
            }
            else
                Console.WriteLine("The user doesn't exists.");
            Console.ReadKey();
        }

        public void DeleteUser()
        {
            Console.Clear();
            Console.WriteLine("Which user you want to delete?");
            string username = Inputs.InputForUsername();
            User userToDelete = DatabaseAccess.FindUser(username);
            if (userToDelete != null && userToDelete.Username!=_logedUser.Username)
            {
                DatabaseAccess.DeleteUser(userToDelete);
                Console.WriteLine($"{username} has been deleted!Press any key to continue.");
            }
            else
                Console.WriteLine("The user doesn't exists.");
            Console.ReadKey();
        }

        public void UpdateUser()
        {
            Console.Clear();
            Console.WriteLine("Which user you want to update?");
            string username = Inputs.InputForUsername();
            User userToUpdate = DatabaseAccess.FindUser(username);
            if (userToUpdate != null)
            {
                Console.WriteLine("Give a new username");
                string newName = Inputs.InputForUsername();
                Console.WriteLine("Give a new password");
                string newPassword = Inputs.InputForPassword();
                var newUser = new User()
                {
                    Username = newName,
                    Password = newPassword,
                    Role = userToUpdate.Role
                };
                DatabaseAccess.UpdateUser(userToUpdate, newUser);
                Console.WriteLine($"\n{newName} has been updated!Press any key to continue.");
            }
            else
                Console.WriteLine("The user doesn't exists.");
            Console.ReadKey();
        }

        public void UnreadMessages()
        {
            int msg = DatabaseAccess.CountUnreadMessage(_logedUser.Username);
            if (msg > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You have {msg} new messages!Check your Inbox.");
                Console.ResetColor();
            }
        }

        public void ShowMessages()
        {
            Console.Clear();
            var messageList = DatabaseAccess.ShowMessages();
            if (messageList.Count == 0)
            {
                Console.WriteLine("No messages");
                Console.ReadKey();
                return;
            }
            foreach (var c in messageList)
                Console.WriteLine($"\nMsg Id: {c.Message_id}\nDatetime: {c.SubscriptionDate}" +
                    $"\nFrom :{c.Sender}\nTo:{c.Receiver}\nMessage:{c.Message_Data}\nStatus:{c.Various}");
            Console.ReadKey();
        }

        public void SendMessage()
        {
            Console.Clear();
            Console.WriteLine("Where would you like to send a message?");
            string username = Inputs.InputForUsername();
            User receiver = DatabaseAccess.FindUser(username);
            if (receiver != null &&receiver.Username!=_logedUser.Username)
            {
                Console.WriteLine("Type your message and press enter:");
                string Data = Inputs.InputForMessages();
                Message message = new Message()
                {
                    Sender = _logedUser.Username,
                    Receiver = receiver.Username,
                    Message_Data = Data,
                    SubscriptionDate = DateTime.Now,
                    Various = "Unread"
                };
                DatabaseAccess.SendMessage(message);
                Console.WriteLine("Your message has been successfully sent!!\nPress any key to continue.");
                LogFile.Log(_logedUser.Username, receiver.Username, Data);
            }
            else
                Console.WriteLine("User doesn't exists.");
            Console.ReadKey();
        }

        public void Inbox()
        {
            Console.Clear();
            var messageList = DatabaseAccess.Inbox(_logedUser.Username);
            if (messageList.Count == 0)
            {
                Console.WriteLine("No messages");
                Console.ReadKey();
                return;
            }

            int count = 0;
            foreach (var c in messageList)
            {
                if (c.Sender != null)
                    Console.WriteLine($"\nMsg Id: {c.Message_id}\nDatetime: {c.SubscriptionDate}" +
                        $"\nFrom :{c.Sender}\nMessage:{c.Message_Data}\nStatus:{c.Various}");
                else
                    Console.WriteLine($"\nMsg Id: {c.Message_id}\nDatetime: {c.SubscriptionDate}" +
                        $"\nFrom:Deleted User\nMessage:{c.Message_Data}\nStatus:{c.Various}");

                // Mark as read
                DatabaseAccess.MarkAsRead(c);

                // Break the foreach loop when the message limit is reached.
                count++;
                if (count == messageList.Count)
                {
                    Console.WriteLine("-->No more messages.Press any key to return at the Main menu.");
                    Console.ReadKey();
                    break;
                }

                // The user can choose if he want to continue reading
                Console.WriteLine("-->Press Esc to go back or any other key to continue reading messages");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    break;
            }
        }

        public void Outbox()
        {
            Console.Clear();
            var messageList = DatabaseAccess.Outbox(_logedUser.Username);
            if (messageList.Count == 0)
            {
                Console.WriteLine("No messages");
                Console.ReadKey();
                return;
            }

            foreach (var c in messageList)
            {
                if (c.Receiver != null)
                    Console.WriteLine($"\nMsg Id: {c.Message_id}\nDatetime: {c.SubscriptionDate}" +
                        $"\nTo:{c.Receiver}\nMessage:{c.Message_Data}\nStatus:{c.Various}");
                else
                    Console.WriteLine($"\nMsg Id: {c.Message_id}\nDatetime: {c.SubscriptionDate}" +
                        $"\nTo:Deleted User \nMessage:{c.Message_Data}\nStatus:{c.Various}");
            }
            Console.WriteLine("\nPress any key to return at the main menu.");
            Console.ReadKey();
        }

        public void EditMessage()
        {
            Console.Clear();
            Console.WriteLine("Which message would you like to edit?Type the ID");
            // Check if the input is an integer.
            bool checkInput = int.TryParse(Console.ReadLine(), out int messageNumber);
            if (checkInput == true)
            {
                Message message = DatabaseAccess.FindMessage(messageNumber);
                if (message != null)
                {
                    Console.WriteLine($"You selected the message number {message.Message_id} " +
                        $"= {message.Message_Data}");
                    Console.WriteLine("Type the new message ");
                    string newData = Inputs.InputForMessages();
                    DatabaseAccess.EditMessage(messageNumber, newData);
                    Console.WriteLine("Message Updated");
                }
                else
                    Console.WriteLine("The message don't exist.Press any key to return at the Main Menu");
            }
            else
                Console.WriteLine("You have to type an integer.Press any key to return at the Main Menu");
            Console.ReadKey();
        }

        public void DeleteMessage()
        {
            Console.Clear();
            Console.WriteLine("Which message would you like to delete?Type the ID");
            bool checkInput = int.TryParse(Console.ReadLine(), out int messageNumber);
            if (checkInput == true)
            {
                Message message = DatabaseAccess.FindMessage(messageNumber);
                if(message!=null)
                {
                    DatabaseAccess.DeleteMessage(messageNumber);
                    Console.WriteLine("Message Deleted");
                }
                else
                    Console.WriteLine("The message don't exist.Press any key to return at the Main Menu");
            }
            else
                Console.WriteLine("You have to type an integer.Press any key to return at the Main Menu");
            Console.ReadKey();
        }

      
    }
}
