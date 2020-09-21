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
            
            if (RootID == -1)
            {
                NewRoot(newValue, ref ans);
                return true;
            }
            else
            {
                
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
        bool RecursiveInsert(int ID, T _newValue, bool MiddleVLift)
        {
            DiskBNode<T> Actual = new DiskBNode<T>(aux, ValueLength, Degree);
            string Line = FindNode(ID);

            Actual.ToTObj(Line);
            if (!Actual.HasSons()||MiddleVLift)
            {
                bool isFull = Actual.BNodeValues.IsFull();
                if (Actual.Insert(_newValue))
                {
                    
                   
                    if (isFull)
                    {
                        for (int i = 0; i < Degree / 2; i++)
                        {
                            //vaciar GreatestValues
                            GreatestValues.Push(Actual.BNodeValues.Get());
                        }
                        MiddleValue = Actual.BNodeValues.Get();
                        DadID = Actual.Dad;
                        ActualID = Actual.ID;
                        if (MiddleVLift)
                        {
                            //llenar GreatestSons
                        }
                        RewriteNode(ActualID, Actual.ToFixedLengthText());
                        return DivideNode();

                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                int SonIndex =Actual.BNodeValues.GetSonIndex(_newValue);
                int SonID = -1;
                Stack<int> AuxStack = new Stack<int>();
                for (int i = 0; i <= SonIndex; i++)
                {
                    SonID = Actual.BNodeSons.Peek();
                    AuxStack.Push(Actual.BNodeSons.Pop());
                }
                foreach (var item in AuxStack)
                {
                    Actual.BNodeSons.Push(AuxStack.Pop());
                }
                if (SonID != -1)
                {
                    return RecursiveInsert(SonID, _newValue, false);
                }
                else
                {
                    return false;
                }
                
            }

           
        }
        void RewriteNode(int ID, string ToWrite)
        {

        }
        void WriteNode(int ID, string ToWrite)
        {

        }
        bool DivideNode()
        {
            DiskBNode<T> BroNode = new DiskBNode<T>(aux, ValueLength, Degree);
            BroNode.CreateNode(AvailableID, DadID);
            AvailableID++;
            foreach (var item in GreatestSons)
            {
                BroNode.BNodeSons.Push(GreatestSons.Pop());
            }
            foreach (var item in GreatestValues)
            {
                BroNode.BNodeValues.Enlist(GreatestValues.Pop());
            }
            WriteNode(BroNode.ID, BroNode.ToFixedLengthText());
            DadUpdate(BroNode.ID);
            return InsertInDad();
        }
        bool InsertInDad(int index)
        {

        }
        void DadUpdate(int BroID)
        {
            DiskBNode<T> DadNode = new DiskBNode<T>(aux, ValueLength, Degree);
            string Line = FindNode(DadID);
            Stack<int> AuxStack = new Stack<int>();
            DadNode.ToTObj(Line);
            while (DadNode.BNodeSons.Peek() != ActualID)
            {
                AuxStack.Push(DadNode.BNodeSons.Pop());
            }
            DadNode.BNodeSons.Push(BroID);
            foreach (var item in AuxStack)
            {
                DadNode.BNodeSons.Push(AuxStack.Pop());
            }
            RewriteNode(DadID, DadNode.ToFixedLengthText());
        }
    }
}
