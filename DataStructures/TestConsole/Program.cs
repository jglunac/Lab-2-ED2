using System;
using System.Collections.Generic;
using DataStructures;
namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] cosas = {2,3,5,8,1,4};
            BTreeInt template = new BTreeInt(9);
            DiskBTree<BTreeInt> arbolito = new DiskBTree<BTreeInt>(4,3, @"C:\Users\joseg\Desktop\Consola\Consola\bin\Debug\Tree.txt", template);
            foreach (var cosa in cosas)
            {
                BTreeInt inter = new BTreeInt(cosa);
                arbolito.Insert(inter);
            }
        }
    }
}
