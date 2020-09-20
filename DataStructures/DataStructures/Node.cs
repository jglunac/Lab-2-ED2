using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    class Node<T> where T : IComparable
    {
        public Node<T> next;
        public Node<T> prev;
    
        public T t_object;
        
    }
}
