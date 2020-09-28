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

            DiskBTree<BTreeInt> arbolito = new DiskBTree<BTreeInt>(4, 5, @"C:\Users\joseg\Desktop\Tree.txt", delegado);

            

            foreach (var cosa in cosas)
            {
                BTreeInt inter = new BTreeInt(cosa);
                arbolito.Insert(inter);
            }
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


            Console.ReadKey();
       
            List<BTreeInt> recorrido = new List<BTreeInt>();
            arbolito.InOrder(recorrido);


           
        }
        static BTreeInt Convert(string line)
        {
            BTreeInt entero = new BTreeInt(int.Parse(line));
            return entero;
        }
    }
}
