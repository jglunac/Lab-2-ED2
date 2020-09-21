using System;
using System.Collections.Generic;
using DataStructures;
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
            int[] cosas = {2,3,5,8,1,4};
            DiskBTree<BTreeInt> arbolito = new DiskBTree<BTreeInt>(4,3, @"C:\Users\joseg\Desktop\Consola\Consola\bin\Debug\Tree.txt", temp, auxiliar);
            foreach (var cosa in cosas)
            {
                BTreeInt inter = new BTreeInt(cosa);
                arbolito.Insert(inter);
            }
        }
    }
}
