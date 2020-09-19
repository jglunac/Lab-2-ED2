using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    class DiskBNode<T> : IFixedLengthText where T : IFixedLengthText, IComparable
    {
        int ID;
        int Dad;

        int Grade;
        T Value;
       
        int FixedLength => 8 + 4 * (Grade + 1) + Grade * Value.FixedLenght;
        public string ToFixedLengthText()
        {
            return $"{ID:0000}{Dad:0000}";
        }
        
    }
}
