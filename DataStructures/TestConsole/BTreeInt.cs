using DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestConsole
{
    public class BTreeInt : IComparable, IFixedLengthText
    {
        
        public IComparable Key { get; set; }
        
        public BTreeInt(int valor)
        {
            Key = valor;
        }
        public int CompareTo(object comparer)
        {
            var comparator = (BTreeInt)comparer;

            if (Key.CompareTo(comparator.Key)==1)
            {
                return 1;
            }
            else if(Key.CompareTo(comparator.Key)==-1)
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
            return $"{Key:0000}";
        }

        public void ToTObj(string line)
        {
            Key = int.Parse(line);
        }
    }
}
