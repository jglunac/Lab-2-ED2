﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures
{
    class DiskBNode<T> : IFixedLengthText where T : IFixedLengthText, IComparable
    {
        public int ID;
        public int Dad;
        public DiskBSortedList<T> BNodeValues = new DiskBSortedList<T>();
        public Stack<int> BNodeSons = new Stack<int>();
        int Degree;
        List<T> AuxList;
        List<T> Aux;
        int ValueLength;
        public int NodeLength;
        public void CreateNode(int _id, int _dad)
        {
            ID = _id;
            Dad = _dad;
        }
        public DiskBNode(List<T> EmptyObject, int TLength, int degree, List<T> Auxiliar)
        {
            AuxList = Auxiliar;
            Aux = EmptyObject;
            ValueLength = TLength;
            Degree = degree;
            NodeLength = 8 + 4 * (Degree) + (Degree-1) * ValueLength;
        }
       public void ToTObj(string Line)
        {
            int index;
            for (int i = 0; i < Degree-1; i++)
            {
                index = Line.Length - ValueLength;
                if (Line.Substring(index)!= "‡".PadLeft(ValueLength, '-'))
                {
                    Aux.First().ToTObj(Line.Substring(index));
                    BNodeValues.Enlist(Aux.First());
                    AuxList.Add(Aux.First());
                    Aux.Remove(Aux.First());
                }
                
                Line = Line.Remove(index);

            }
            for (int i = 0; i <= Degree+1; i++)
            {
                index = Line.Length - 4;
                if (i<Degree)
                {
                    BNodeSons.Push(Convert.ToInt32(Line.Substring(index)));
                }
                else if(i==Degree)
                {
                    Dad = Convert.ToInt32(Line.Substring(index));
                }
                else
                {
                    ID = Convert.ToInt32(Line.Substring(index));
                }
                Line = Line.Remove(index);
            }
            
        }
        
        
        public string ToFixedLengthText()
        {
            string response = $"{ID:0000}{Dad:0000}";
            int _nullSons = Degree - BNodeSons.Count;
            int sonsCount = BNodeSons.Count;
            for (int j = 0; j < sonsCount; j++)
            {
                response += $"{BNodeSons.Pop():0000}";
            }
           
            for (int j = 0; j < _nullSons; j++)
            {
                response += "0000";
            }
            int i = 0;
            while(!BNodeValues.IsEmpty())
            {
                response += BNodeValues.Get().ToFixedLengthText();
                i++;
            }
            if (i != Degree-1)
            {
                response += "‡".PadLeft(ValueLength, '-');
            }
            return response;
        }
        public bool Insert(T NewValue)
        {
            return BNodeValues.Enlist(NewValue);
            
        }
        public void Insert(int nindex, T nValue)
        {
            BNodeValues.Enlist(nindex, nValue);
            
        }
       
       public bool HasSons()
        {
            
            foreach (var item in BNodeSons)
            {
                if (item != 0)
                {
                    return true;
                }
            }
            return false;
        }
        

    }
}
