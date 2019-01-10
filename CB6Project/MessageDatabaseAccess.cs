using System;
using System.Linq;

namespace CB6Project
{
    public static class MessageDatabaseAccess
    {
        public static void ShowMessages()
        {
            Console.Clear();
            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.OrderBy(x => x.SubscriptionDate);
                foreach (var c in msg)
                    Console.WriteLine($"\nMsg Id: {c.Message_id}\nDatetime: {c.SubscriptionDate}" +
                        $"\nFrom :{c.Sender}\nTo:{c.Receiver}\nMessage:{c.Message_Data}\nStatus:{c.Various}");
                Console.ReadKey();
            }
        }

        public static void SendMessage(string sender)
        {
            Console.Clear();
            Console.WriteLine("Where would you like to send a message?");
            string receiver = UserDatabaseAccess.FindUser(sender);
            if (receiver != null)
            {
                using (var context = new ProjectDatabaseEntities())
                {
                    Console.WriteLine("Type your message and press enter:");
                    string Data = Inputs.InputForMessages();
                    var msg = context.Messages.Add(new Message
                    {
                        Sender = sender,
                        Receiver = receiver,
                        Message_Data = Data,
                        SubscriptionDate = DateTime.Now,
                        Various = "Unread"
                    });
                    context.SaveChanges();
                    Console.WriteLine("Your message has been successfully sent!!\nPress any key to continue.");
                    LogFile.Log(sender, receiver, Data);
                    Console.ReadKey();
                }
            }

        }

        public static void Inbox(string receiver)
        {
            Console.Clear();
            using (var context = new ProjectDatabaseEntities())
            {
                int msg1 = context.Messages.Where(x => x.Receiver == receiver).Count();
                if (msg1 == 0)
                {
                    Console.WriteLine("No messages.Press any key to return at the Main menu.");
                    Console.ReadKey();
                }
                else
                {
                    var msg2 = context.Messages.Where(x => x.Receiver == receiver)
                                               .OrderByDescending(x => x.SubscriptionDate);
                    int count = 0;
                    foreach (var c in msg2)
                    {
                        if (c.Sender != null)
                            Console.WriteLine($"\nMsg Id: {c.Message_id}\nDatetime: {c.SubscriptionDate}" +
                                $"\nFrom :{c.Sender}\nMessage:{c.Message_Data}\nStatus:{c.Various}");
                        else
                            Console.WriteLine($"\nMsg Id: {c.Message_id}\nDatetime: {c.SubscriptionDate}" +
                                $"\nFrom:Deleted User\nMessage:{c.Message_Data}\nStatus:{c.Various}");
                        // Mark message as read
                        c.Various = "Read";

                        // Break the foreach loop when the message limit is reached.
                        count++;
                        if (count == msg1)
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
                context.SaveChanges();
            }

        }

        public static void Outbox(string sender)
        {
            Console.Clear();
            using (var context = new ProjectDatabaseEntities())
            {
                int msg1 = context.Messages.Where(x => x.Sender == sender).Count();
                if (msg1 == 0)
                    Console.WriteLine("No messages.Press any key to return at the Main menu.");
                else
                {
                    var msg = context.Messages.Where(x => x.Sender == sender)
                                              .OrderBy(x => x.SubscriptionDate);
                    foreach (var c in msg)
                    {
                        if (c.Receiver != null)
                            Console.WriteLine($"\nMsg Id: {c.Message_id}\nDatetime: {c.SubscriptionDate}" +
                                $"\nTo:{c.Receiver}\nMessage:{c.Message_Data}\nStatus:{c.Various}");
                        else
                            Console.WriteLine($"\nMsg Id: {c.Message_id}\nDatetime: {c.SubscriptionDate}" +
                                $"\nTo:Deleted User \nMessage:{c.Message_Data}\nStatus:{c.Various}");
                    }

                }
            }
            Console.ReadKey();

        }

        public static void EditMessage(string username)
        {
            Console.Clear();
            Console.WriteLine("Which message would you like to edit?Type the ID");
            bool checkInput = int.TryParse(Console.ReadLine(), out int editMsg);
            if (checkInput == true)
            {
                using (var context = new ProjectDatabaseEntities())
                {

                    var msg = context.Messages.Where(x => x.Message_id == editMsg).SingleOrDefault();
                    if (msg != null)
                    {
                        Console.WriteLine(msg.Message_id + ":" + msg.Message_Data + "\n");
                        msg.Message_Data = Inputs.InputForMessages();
                        msg.Various = $"Edited by {username}";
                        context.SaveChanges();
                        Console.WriteLine("Message Updated");
                    }
                    else
                        Console.WriteLine("The message don't exist.Press any key to return at the Main Menu");
                }
            }
            else
                Console.WriteLine("You have to type an integer.Press any key to return at the Main Menu");

            Console.ReadKey();
        }

        public static void UnreadMessage(string receiver)
        {
            using (var context = new ProjectDatabaseEntities())
            {
                var msg = context.Messages.Count(x => x.Receiver == receiver && x.Various == "Unread");
                if (msg > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"You have {msg} new messages!Check your Inbox.");
                    Console.ResetColor();
                }
            }
        }

        public static void DeleteMessage()
        {
            Console.Clear();
            Console.WriteLine("Which message would you like to delete?Type the ID");
            // Check if the input is an integer.
            bool checkInput = int.TryParse(Console.ReadLine(), out int deleteMsg);
            if (checkInput == true)
            {
                using (var context = new ProjectDatabaseEntities())
                {

                    var msg = context.Messages.Where(x => x.Message_id == deleteMsg).SingleOrDefault();
                    
                    if (msg != null)
                    {
                        context.Messages.Remove(msg);
                        context.SaveChanges();
                        Console.WriteLine("Message Deleted");
                    }
                    else
                        Console.WriteLine("The message don't exist.Press any key to return at the Main Menu");
                }
            }
            else
                Console.WriteLine("You have to type an integer.Press any key to return at the Main Menu");

            Console.ReadKey();
        }
    }
}
