using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public interface IFixedLengthText
    {
       
        string ToFixedLengthText();
        IComparable ID { get; set; }
        void ToTObj(string line);

    }
}
