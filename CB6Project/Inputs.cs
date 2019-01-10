using System;

namespace CB6Project
{
    public static class Inputs
    {

        public static string InputForUsername()
        {
            bool remain = false;
            string input;
            do
            {
                Console.WriteLine("Enter minimum 5 and maximum 20 chars.");
                input = Console.ReadLine();
                if (input.Length > 20 || input.Length < 5)
                    Console.WriteLine("The string length is too small/big,re-enter with minimum 5 " +
                        "and maximum 20 chars.");
                else
                    remain = true;
            } while (remain == false);
            return input;

        }

        public static string InputForMessages()
        {
            bool remain = false;
            string input;
            do
            {
                Console.WriteLine("Enter maximum 250 chars.");
                input = Console.ReadLine();
                if (input.Length > 250)
                    Console.WriteLine("The string length is too big,re-enter with maximum of 250 chars.");
                else
                    remain = true;
            } while (remain == false);
            return input;
        }

        public static bool CheckPoint()
        {
            ConsoleKey key = Console.ReadKey().Key;
            if (key == ConsoleKey.Escape)
                return false;
            else
                return true;
        }

        public static string InputForPassword()
        {
            bool remain = false;
            string input;
            Console.WriteLine("Enter minimum 5 and maximum 20 chars.");
            do
            {
                bool remain2 = false;
                input = string.Empty;
                do
                {
                    // Masking the passwod
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        input += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                        {
                            input = input.Substring(0, (input.Length - 1));
                            Console.Write("\b \b");
                        }
                        else if (key.Key == ConsoleKey.Enter)
                            remain2 = true;
                    }
                } while (remain2 == false);
                if (input.Length > 20 || input.Length < 5)
                    Console.WriteLine("\nThe string length is too small/big,re-enter with minimum 5" +
                        " and maximum 20 chars.");
                else
                    remain = true;
            } while (remain == false);

            return input;
        }
    }
}
