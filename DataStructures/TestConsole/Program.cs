using System;
using System.Collections.Generic;
namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string linea = "Hola Mundo";
            string linea2 = linea.Substring(6);
            string linea3 = linea.Remove(6);
            Queue<int> lista = new Queue<int>();
           
            Console.WriteLine(linea3);
            Console.WriteLine(linea2);
            Console.ReadKey();
        }
    }
}
