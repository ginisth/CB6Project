﻿using System;

namespace CB6Project
{
    class MainMenu
    {
        private  User _username { get; set; }
        private readonly RoleType _roleType;
        private bool remain = true;
        private ConsoleKey key;

        public MainMenu(User username)
        {
            _username = username;
            _roleType = _username.Role;
        }


        public void UsersMenu()
        {
           var option = new MenuOptions(_username);

            do
            {
                Console.Clear();
                MainMenuHeader();
                // Check for unread messages.
                option.UnreadMessages();

                Console.WriteLine("-->Press the corresponding key");
                switch (_roleType)
                {
                    case RoleType.MOD:
                        Console.WriteLine("#1.Inbox \n#2.Outbox \n#3.Send Message \n#4.View all messages" +
                            "\n#Esc.Logout");
                        break;
                    case RoleType.SuperMOD:
                        Console.WriteLine("#1.Inbox \n#2.Outbox \n#3.Send Message \n#4.View all messages" +
                            "\n#5.Edit message\n#Esc.Logout");
                        break;
                    case RoleType.Admin:
                        Console.WriteLine("#1.Inbox \n#2.Outbox \n#3.Send Message \n#4.View all messages" +
                            "\n#5.Edit message\n#6.Delete message\n#7.View users\n#8.Add user" +
                            "\n#9.Delete user\n#Q.Update role\n#W.Update user\n#Esc.Logout");
                        break;
                    case RoleType.CustomUser:
                    default:
                        Console.WriteLine("#1.Inbox \n#2.Outbox \n#3.Send Message \n#Esc.Logout");
                        break;
                }



                switch (key = Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        option.Inbox();
                        break;
                    case ConsoleKey.D2:
                        option.Outbox();
                        break;
                    case ConsoleKey.D3:
                        option.SendMessage();
                        break;
                    case ConsoleKey.D4 when _roleType != RoleType.CustomUser:
                        option.ShowMessages();
                        break;
                    case ConsoleKey.D5 when _roleType == RoleType.SuperMOD || _roleType == RoleType.Admin:
                        option.EditMessage();
                        break;
                    case ConsoleKey.D6 when _roleType == RoleType.Admin:
                        option.DeleteMessage();
                        break;
                    case ConsoleKey.D7 when _roleType == RoleType.Admin:
                        option.ShowUsers();
                        break;
                    case ConsoleKey.D8 when _roleType == RoleType.Admin:
                        option.AddUser();
                        break;
                    case ConsoleKey.D9 when _roleType == RoleType.Admin:
                        option.DeleteUser();
                        break;
                    case ConsoleKey.Q when _roleType == RoleType.Admin:
                        option.UpdateRole();
                        break;
                    case ConsoleKey.W when _roleType == RoleType.Admin:
                        option.UpdateUser();
                        break;
                    case ConsoleKey.Escape:
                        remain = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("\nNot a valid option.\nPress any key to refresh the menu.");
                        Console.ReadKey();
                        break;

                }
            } while (remain);

        }
        static void MainMenuHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@" _______  _______ _________ _                  _______  _______  _                
(       )(  ___  )\__   __/( (    /|          (       )(  ____ \( (    /||\     /|
| () () || (   ) |   ) (   |  \  ( |          | () () || (    \/|  \  ( || )   ( |
| || || || (___) |   | |   |   \ | |          | || || || (__    |   \ | || |   | |
| |(_)| ||  ___  |   | |   | (\ \) |          | |(_)| ||  __)   | (\ \) || |   | |
| |   | || (   ) |   | |   | | \   |          | |   | || (      | | \   || |   | |
| )   ( || )   ( |___) (___| )  \  |          | )   ( || (____/\| )  \  || (___) |
|/     \||/     \|\_______/|/    )_)          |/     \|(_______/|/    )_)(_______)
                                                                                  
");
            Console.ResetColor();
        }


    }
}
