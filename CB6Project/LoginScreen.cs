using System;

namespace CB6Project
{
    public class LoginScreen
    {
        public void LoginMenu()
        {
            try
            {
                bool remain = true;
                // A user with admin privileges is created if there is no user stored in the database
                UserDatabaseAccess.FirstTimeRegister();

                do
                {
                    Console.Clear();
                    
                    Header();
                    Console.WriteLine("-->Press the corresponding key\n#1.Login\n#2.Register\n#Esc.Exit");
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.D1:
                            (string checkUsername, bool remain2) = UserDatabaseAccess.ReturnValidUser();
                            // Deconstruction of the tuple.If bool remain2=true the User can login.
                            if (remain2 == false)
                                break;
                            Console.Clear();
                            new MainMenu(checkUsername).UsersMenu();
                            break;
                        case ConsoleKey.D2:
                            UserDatabaseAccess.AddUser();
                            break;
                        case ConsoleKey.Escape:
                            Environment.Exit(1);
                            break;
                        default:
                            break;

                    }
                } while (remain);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred.Restart the app.\n\n" + e.Message);
                Console.ResetColor();
                Console.ReadKey();
            }
        }
        static void Header()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@" _______  ______    ______    _______  _______  _______ _________ _______  _______ _________
(  ____ \(  ___ \  / ____ \  (  ____ )(  ____ )(  ___  )\__    _/(  ____ \(  ____ \\__   __/
| (    \/| (   ) )( (    \/  | (    )|| (    )|| (   ) |   )  (  | (    \/| (    \/   ) (   
| |      | (__/ / | (____    | (____)|| (____)|| |   | |   |  |  | (__    | |         | |   
| |      |  __ (  |  ___ \   |  _____)|     __)| |   | |   |  |  |  __)   | |         | |   
| |      | (  \ \ | (   ) )  | (      | (\ (   | |   | |   |  |  | (      | |         | |   
| (____/\| )___) )( (___) )  | )      | ) \ \__| (___) ||\_)  )  | (____/\| (____/\   | |   
(_______/|/ \___/  \_____/   |/       |/   \__/(_______)(____/   (_______/(_______/   )_(   
                                                                                            
");
            Console.ResetColor();
        }

    }
}

