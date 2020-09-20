using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    class DiskBTree<T> where T:IComparable, IFixedLengthText
    {
        int DadID;
        Stack<T> GreatestValues = new Stack<T>();
        Stack<int> GreatestSons = new Stack<int>();
        T MiddleValue;
        int ActualID;
        int RootID = -1;
        int AvailableID = 1;
        int ValueLength;
        int Degree;
        T aux;
        public DiskBTree( int TLength, int degree)
        {
            //Crear archivo
            ValueLength = TLength;
            Degree = degree;
        }
        public bool Insert(T newValue, ref string ans)
        {
            DiskBNode<T> AuxNode = new DiskBNode<T>(aux, ValueLength, Degree);
            if (RootID == -1)
            {
                NewRoot(newValue, ref ans);
                return true;
            }
            else
            {
                string Line = FindNode(RootID);
                AuxNode.ToTObj(Line);
            }
        }
        string FindNode(int ID)
        {
            return "";
        }
        void NewRoot(T _newValue, ref string ans)
        {
            RootID = AvailableID;
            DiskBNode<T> AuxNode = new DiskBNode<T>(aux, ValueLength, Degree);
            AuxNode.CreateNode(AvailableID, -1);
            AuxNode.Insert(_newValue);
            ans = AuxNode.ToFixedLengthText();//Escribir en archivo
            AvailableID++;
        }
        bool FindLeaf(DiskBNode<T> Actual, T _newValue)
        {
            

            bool isFull = Actual.BNodeValues.IsFull();
            Actual.Insert(_newValue);
            if (isFull)
            {
                for (int i = 0; i < Degree/2; i++)
                {
                    GreatestValues.Push(Actual.BNodeValues.Get());
                }
                MiddleValue = Actual.BNodeValues.Get();
                DadID = Actual.Dad;
                ActualID = Actual.ID;

            }
        }
    }
}
