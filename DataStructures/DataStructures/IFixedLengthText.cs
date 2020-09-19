using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public interface IFixedLengthText
    {
        int FixedLenght { get; set; }
        string ToFixedLengthText();
    }
}
