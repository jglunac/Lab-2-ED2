using DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestConsole
{
    public class BTreeInt : IComparable, IFixedLengthText
    {
        int numero;
        
        public BTreeInt(int valor)
        {
            numero = valor;
        }
        public int CompareTo(object comparer)
        {
            var comparator = (BTreeInt)comparer;

            if (numero.CompareTo(comparator.numero)==1)
            {
                return 1;
            }
            else if(numero.CompareTo(comparator.numero)==-1)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public string ToFixedLengthText()
        {
            return $"{numero:0000}";
        }

        public void ToTObj(string line)
        {
            numero = int.Parse(line);
        }
    }
}
