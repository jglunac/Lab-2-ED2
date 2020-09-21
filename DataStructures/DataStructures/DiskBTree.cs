using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace DataStructures
{
    class DiskBTree<T> where T : IComparable, IFixedLengthText
    {
        string Path;
        int DadID;
        Stack<T> GreatestValues = new Stack<T>();
        Stack<int> GreatestSons = new Stack<int>();
        T MiddleValue;
        int ActualID;
        int RootID = 0;
        int AvailableID = 1;
        int ValueLength;
        int Degree;
        T aux;
        public DiskBTree(int TLength, int degree, string ruta)
        {
            //Crear archivo
            ValueLength = TLength;

            if (degree > 2)//Preguntar cual es el grado mínimo de los árboles B
            {
                Degree = degree;
            }
            else
            {
                Degree = 3;

                Degree = degree;
                Path = ruta + "Tree.txt";
                using (StreamWriter writer = new StreamWriter(Path))
                {
                    writer.WriteLine();

                }
            }
            bool Insert(T newValue)
            {

                if (RootID == 0)
                {
                    NewRoot(newValue, 0);
                    return true;
                }
                else
                {
                    return RecursiveInsert(RootID, newValue);
                }
            }

            string FindNode(int ID)
            {
                string linea = "";
                using (var content = new MemoryStream())
                {
                    StreamReader lector = new StreamReader(Path);
                    lector.ReadLine();
                    for (int i = 0; i < ID; i++)
                    {
                        linea = lector.ReadLine();
                    }
                }
                return linea;
            }
            void NewRoot(T _newValue, int BroID)
            {
                RootID = AvailableID;
                DiskBNode<T> AuxNode = new DiskBNode<T>(aux, ValueLength, Degree);
                AuxNode.CreateNode(AvailableID, 0);
                AuxNode.Insert(_newValue);
                AuxNode.BNodeSons.Push(ActualID);
                AuxNode.BNodeSons.Push(BroID);
                WriteNode(RootID, AuxNode.ToFixedLengthText());
                AvailableID++;
            }
            bool RecursiveInsert(int ID, T _newValue)
            {
                DiskBNode<T> Actual = new DiskBNode<T>(aux, ValueLength, Degree);
                string Line = FindNode(ID);

                Actual.ToTObj(Line);
                if (!Actual.HasSons())
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

                            RewriteNode(ActualID, Actual.ToFixedLengthText());
                            DivideNode();
                            return true;

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
                    int SonIndex = Actual.BNodeValues.GetSonIndex(_newValue);
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
                        return RecursiveInsert(SonID, _newValue);
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
            void DivideNode()
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

            }

            void DadUpdate(int BroID)
            {
                DiskBNode<T> DadNode = new DiskBNode<T>(aux, ValueLength, Degree);
                if (DadID > 0)
                {
                    string Line = FindNode(DadID);
                    Stack<int> AuxStack = new Stack<int>();
                    DadNode.ToTObj(Line);
                    while (DadNode.BNodeSons.Peek() != ActualID)
                    {
                        AuxStack.Push(DadNode.BNodeSons.Pop());
                    }
                    int valueIndex = DadNode.BNodeSons.Count - 1;
                    DadNode.BNodeSons.Push(BroID);
                    foreach (var item in AuxStack)
                    {
                        DadNode.BNodeSons.Push(AuxStack.Pop());
                    }
                    bool isFull = DadNode.BNodeValues.IsFull();
                    DadNode.Insert(valueIndex, MiddleValue);
                    if (isFull)
                    {
                        for (int i = 0; i < Degree / 2; i++)
                        {
                            //vaciar GreatestValues
                            GreatestValues.Push(DadNode.BNodeValues.Get());
                        }
                        MiddleValue = DadNode.BNodeValues.Get();
                        DadID = DadNode.Dad;
                        ActualID = DadNode.ID;
                        int firstSons = DadNode.BNodeSons.Count - (Degree / 2 + 1);
                        for (int i = 0; i < firstSons; i++)
                        {
                            AuxStack.Push(DadNode.BNodeSons.Pop());
                        }
                        foreach (var item in DadNode.BNodeSons)
                        {
                            GreatestSons.Push(DadNode.BNodeSons.Pop());
                        }
                        foreach (var item in AuxStack)
                        {
                            DadNode.BNodeSons.Push(AuxStack.Pop());
                        }

                        DivideNode();


                    }


                    RewriteNode(DadID, DadNode.ToFixedLengthText());
                }
                else
                {
                    NewRoot(MiddleValue, BroID);
                }

            }
        }
    }
}
