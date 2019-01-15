using System;
using System.IO;

namespace CB6Project
{
    public static class LogFile
    {
        public static void Log(string logSender, string logReceiver, string logData)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                path = path + @"\Log.txt";
                if (File.Exists(path))
                {
                    StreamWriter writer = File.AppendText(path);
                    writer.WriteLine($"Date:{DateTime.Now}--from:{logSender}--to:{logReceiver}--data:{logData}");
                    writer.Close();
                }
                else
                {
                    // .Dispose closes the stream
                    File.Create(path).Dispose();
                    var writer = new StreamWriter(path);
                    writer.WriteLine($"Date:{DateTime.Now}--from:{logSender}--to:{logReceiver}--data:{logData}");
                    writer.Close();

                }
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong with the text file creation.");
                Console.ResetColor();
            }  
        }

    }
}
