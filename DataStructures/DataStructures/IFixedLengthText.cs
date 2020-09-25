using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public interface IFixedLengthText
    {
       
        string ToFixedLengthText();
        IComparable Key{ get; set; }
        void ToTObj(string line);

    }
}
