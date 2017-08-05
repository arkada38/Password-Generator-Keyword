using Models;
using static System.Console;

namespace CA
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string serviceName;
            string keyword;
            int passwordLength;
            var useSpecialSymbols = false;

            if (args.Length == 4)
            {
                serviceName = args[0];
                keyword = args[1];
                passwordLength = int.Parse(args[2]);
                useSpecialSymbols = bool.Parse(args[3]);
                GeneratePassword(serviceName, keyword, passwordLength, useSpecialSymbols);
            }
            else
            {
                WriteLine("Input service name:");
                serviceName = ReadLine();

                WriteLine("Input keyword:");
                keyword = ReadLine();

                WriteLine("Input length of password:");
                passwordLength = int.Parse(ReadLine());

                WriteLine("Do you want password with special symbols (yes/no)?");
                var answer = ReadLine().ToLower();

                if (answer == "yes" || answer == "y" || answer == "1")
                    useSpecialSymbols = true;

                GeneratePassword(serviceName, keyword, passwordLength, useSpecialSymbols);

                WriteLine("Press any key for exit...");
                ReadKey();
            }
        }

        private static void GeneratePassword(string serviceName, string keyword, int passwordLength, bool useSpecialSymbols)
        {
            WriteLine($"\nService name: {serviceName}\nKeyword: {keyword}\nLength of password: {passwordLength}\nWith special symbols: {useSpecialSymbols}\n");
            WriteLine($"Password: {Generator.GeneratePassword(serviceName, keyword, passwordLength, useSpecialSymbols)}\n");
        }
    }
}
