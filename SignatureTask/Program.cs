using System;

namespace Signature
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "";
            int blockSize = 0;

            if (args.Length < 2)
            {
                Console.WriteLine("Введите путь к файлу: ");
                filePath = Console.ReadLine();
                Console.WriteLine("Введите размер блока: ");
                while (!int.TryParse(Console.ReadLine(), out blockSize))
                {
                    Console.WriteLine("Некорректный ввод числа, введите число еще раз: ");
                }
            }
            else
            {
                filePath = args[0];
                if (!int.TryParse(args[1], out blockSize))
                {
                    Console.WriteLine("Параметр 2 задан неверно, введите размер блока: ");
                    while (!int.TryParse(Console.ReadLine(), out blockSize))
                    {
                        Console.WriteLine("Размер блока задан не верно, введите размер блока еще раз: ");
                    }
                }
                
            }
            HashCalculator calculator = new HashCalculator();
            try
            {
                calculator.Calculate(filePath, blockSize);
            }
            catch (Exception e)
            {
                Console.WriteLine("В приложении возникло исключение: {0} \nStack trace:\n{1}", e.Message, e.StackTrace);
            }            
        }
    }
}
