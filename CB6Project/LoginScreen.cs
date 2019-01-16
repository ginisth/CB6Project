using System;

namespace CB6Project
{
    public class LoginScreen
    {
        public void LoginMenu()
        {
            try
            {
                do
                {
                    Console.Clear();

                    LoginScreenHeader();
                    Console.WriteLine("-->Press the corresponding key\n#1.Login\n#2.Register\n#Esc.Exit");
                    ConsoleKey key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.D1:
                            Login();
                            break;
                        case ConsoleKey.D2:
                            Register();
                            break;
                        case ConsoleKey.Escape:
                            Environment.Exit(1);
                            break;
                        default:
                            break;

                    }
                } while (true);
            }catch(Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong with the application.Restart.");
                Console.ResetColor();
            }
        }

        private void Login()
        {
            Console.Clear();
            Console.WriteLine("--> Give username.");
            string inputUsername = Inputs.InputForUsername();
            Console.WriteLine("-->Give password.");
            string inputPassword = Inputs.InputForPassword();
            User user = DatabaseAccess.ReturnValidUser(inputUsername, inputPassword);
            if (user == null)
            {
                Console.WriteLine("\nWrong username or password.Press any key to return back.");
                Console.ReadKey();
            }
            else
            {
                WelcomeHeader();
                new MainMenu(user).UsersMenu();
            }
        }

        private void Register()
        {
            Console.Clear();
            // Administrator is auto-created if there is no user stored in the database.
            bool firstTimeRegister = DatabaseAccess.FirstTimeRegister();
            if ( firstTimeRegister== true)
            {
                Console.WriteLine("Succecfully created Administrator!");
                Console.WriteLine("From the Login screen,login with username:admin and password:admin" +
                    " as administrator");
            }
            else
            {
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
            }
            Console.ReadKey();
        }

        static void LoginScreenHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@" _______  _______        _______  _______ _________ _       
(  ____ \(  ____ \      (       )(  ___  )\__   __/( \      
| (    \/| (    \/      | () () || (   ) |   ) (   | (      
| |      | |            | || || || (___) |   | |   | |      
| | ____ | | ____       | |(_)| ||  ___  |   | |   | |      
| | \_  )| | \_  )      | |   | || (   ) |   | |   | |      
| (___) || (___) |      | )   ( || )   ( |___) (___| (____/\
(_______)(_______)      |/     \||/     \|\_______/(_______/
                                                            
");
            Console.ResetColor();
        }


        static void WelcomeHeader()
        {
            Console.Clear();
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
            Console.WriteLine("You have logged in successfully.Press any key to view the main menu");
            Console.ReadKey();
        }

    }
}

