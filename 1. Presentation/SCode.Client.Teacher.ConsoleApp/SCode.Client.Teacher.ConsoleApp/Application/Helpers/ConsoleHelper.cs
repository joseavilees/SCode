using System;

namespace SCode.Client.Teacher.ConsoleApp.Application.Helpers
{
    public class ConsoleHelper
    {
        public static void PrintWelcome()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(@"
                _____  _____ ____  _____  ______ 
               / ____|/ ____/ __ \|  __ \|  ____|
              | (___ | |   | |  | | |  | | |__   
               \___ \| |   | |  | | |  | |  __|  
               ____) | |___| |__| | |__| | |____ 
              |_____/ \_____\____/|_____/|______|
                                     
             ");

            Console.ResetColor();
        }

        public static void WriteBlackLine()
        {
            Console.WriteLine();
        }
    }
}