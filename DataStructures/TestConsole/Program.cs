using System;
using System.Collections.Generic;
using DataStructures;
using System.IO;
namespace TestConsole

{
    class Program
    {
        static void Main(string[] args)
        {
            BTreeInt template = new BTreeInt(9);
            BTreeInt template2 = new BTreeInt(10);
            BTreeInt template3 = new BTreeInt(12);
            List<BTreeInt> temp = new List<BTreeInt>();
            List<BTreeInt> auxiliar = new List<BTreeInt>();

            temp.Add(template);
            temp.Add(template2);
            temp.Add(template3);
            int[] cosas = { 64, 31, 24, 77, 20, 99, 34, 58, 51, 4, 48, 80, 22, 15, 72, 7, 75, 53, 33, 18, 16, 78, 44, 62, 68, 84, 14, 92, 93, 17 };
            var delegado = new DiskBTree<BTreeInt>.ToTObj(Convert);

            DiskBTree<BTreeInt> arbolito = new DiskBTree<BTreeInt>(4, 5, @"C:\Users\brazi\Desktop\Tree.txt", delegado);

            

            foreach (var cosa in cosas)
            {
                BTreeInt inter = new BTreeInt(cosa);
                arbolito.Insert(inter);
            }
            //Caso de eliminación: hoja en underflow, unión de padre y hermanos, padre en underflow
            //arbolito.Delete(16);
            //arbolito.Delete(20);
            //arbolito.Delete(4);
            //arbolito.Delete(22);
            //arbolito.Delete(24);
            //arbolito.Delete(18);
            //arbolito.Delete(14);

            arbolito.Delete(99);
            arbolito.Delete(64);
            arbolito.Delete(75);
            arbolito.Delete(14);
            arbolito.Delete(31);
            arbolito.Delete(48);
            arbolito.Delete(20);
            arbolito.Delete(84);
            arbolito.Delete(62);
            arbolito.Delete(15);
            arbolito.Delete(18);
            arbolito.Delete(17);
            arbolito.Delete(16);




            List<BTreeInt> recorrido = new List<BTreeInt>();
            Console.WriteLine("1. InOrder");
            Console.WriteLine("2. PreOrder");
            Console.WriteLine("3. PostOrder");
            Console.WriteLine("Ingrese recorrido deseado: ");
            int respuesta = int.Parse(Console.ReadLine());
            switch (respuesta)
            {
                case 1:
                    arbolito.InOrder(recorrido);
                    foreach (var item in recorrido)
                    {
                        Console.WriteLine(item.Key);
                    }
                    break;
                case 2:
                    arbolito.PreOrder(recorrido);
                    foreach (var item in recorrido)
                    {
                        Console.WriteLine(item.Key);
                    }
                    break;
                case 3:
                    arbolito.PostOrder(recorrido);
                    foreach (var item in recorrido)
                    {
                        Console.WriteLine(item.Key);
                    }
                    break;

                default:
                    break;
            }

            Console.ReadKey();


        }
        static BTreeInt Convert(string line)
        {
            BTreeInt entero = new BTreeInt(int.Parse(line));
            return entero;
        }
    }
}
