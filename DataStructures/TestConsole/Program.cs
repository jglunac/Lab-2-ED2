﻿using System;
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
            Stack<int> pila = new Stack<int>();
            pila.Push(1);
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine(pila.Pop());
            }
            Console.WriteLine(linea3);
            Console.WriteLine(linea2);
            Console.ReadKey();
        }
    }
}
