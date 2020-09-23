using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace DataStructures
{
    public class DiskBTree<T> where T : IComparable, IFixedLengthText
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
        //List<T> aux;
        //List<T> auxiliar;
        public delegate T ToTObj(string line);
        ToTObj toT;
        public DiskBTree(int TLength, int degree, string ruta, ToTObj Algorithm)
        {
            //Crear archivo
            ValueLength = TLength;
            toT = Algorithm;
            Path = ruta + "Tree.txt";
            using (StreamWriter writer = new StreamWriter(Path))
            {
                writer.WriteLine("ENCABEZADO");

            }

            if (degree > 2)//Preguntar cual es el grado mínimo de los árboles B
            {
                Degree = degree;
            }
            else
            {
                Degree = 3;
                Degree = degree;
            }
        }

        public bool Insert(T newValue)
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
            using (StreamReader lector = new StreamReader(Path))
            {
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
            DiskBNode<T> AuxNode = new DiskBNode<T>(toT, ValueLength, Degree);
            AuxNode.CreateNode(AvailableID, 0);
            AuxNode.Insert(_newValue);
            AuxNode.BNodeSons.Push(BroID);
            AuxNode.BNodeSons.Push(ActualID);
            WriteNode(RootID, AuxNode.ToFixedLengthText());
            AvailableID++;
        }
        bool RecursiveInsert(int ID, T _newValue)
        {
            DiskBNode<T> Actual = new DiskBNode<T>(toT, ValueLength, Degree);
            string Line = FindNode(ID);

            Actual.ToTObj(Line);
            if (!Actual.HasSons())
            {
                bool isFull = Actual.BNodeValues.IsFull();
                bool inserted = Actual.Insert(_newValue);
                if (inserted)
                {


                    if (isFull)
                    {
                        for (int i = 0; i < Degree / 2; i++)
                        {
                            //vaciar GreatestValues
                            GreatestValues.Push(Actual.BNodeValues.GetHead());
                        }
                        MiddleValue = Actual.BNodeValues.GetHead();
                        if (Actual.Dad == 0)
                        {
                            Actual.Dad = AvailableID + 1;
                        }
                        DadID = Actual.Dad;
                        ActualID = Actual.ID;

                        RewriteNode(ActualID, Actual.ToFixedLengthText());
                        DivideNode();
                        return true;

                    }
                    else
                    {
                        RewriteNode(ID, Actual.ToFixedLengthText());
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
                int count = AuxStack.Count;
                for (int j = 0; j < count; j++)
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

            List<string> previous = new List<string>();
            using (StreamReader reader = new StreamReader(Path))
            {
                string siguiente;
                do
                {
                    siguiente = reader.ReadLine();
                    previous.Add(siguiente);
                } while (siguiente != null);

            }

            int largo = previous.Count;
            using (StreamWriter writer = new StreamWriter(Path))
            {
                for (int i = 0; i < largo; i++)
                {

                    if (i == ID)
                    {
                        writer.WriteLine(ToWrite);
                        previous.Remove(previous.First());
                    }
                    else
                    {
                        writer.WriteLine(previous.First());
                        previous.Remove(previous.First());
                    }
                }
            }



        }
        void WriteNode(int ID, string ToWrite)
        {
            List<string> previous = new List<string>();
            using (StreamReader reader = new StreamReader(Path))
            {
                string siguiente;
                do
                {
                    siguiente = reader.ReadLine();
                    previous.Add(siguiente);
                } while (siguiente != null);
            }
            int largo = previous.Count;
            using (StreamWriter writer = new StreamWriter(Path))
                for (int i = 0; i < largo; i++)
                {

                    if (i == ID)
                    {
                        writer.WriteLine(ToWrite);
                        previous.Remove(previous.First());
                    }
                    else
                    {
                        writer.WriteLine(previous.First());
                        previous.Remove(previous.First());
                    }
                }
        }
        void DivideNode()
        {
            DiskBNode<T> BroNode = new DiskBNode<T>(toT, ValueLength, Degree);
            BroNode.CreateNode(AvailableID, DadID);
            AvailableID++;
            int count = GreatestSons.Count;
            for (int i = 0; i < count; i++)
            {
                BroNode.BNodeSons.Push(GreatestSons.Pop());
            }
            count = GreatestValues.Count;
            for (int i = 0; i < count; i++)
            {
                BroNode.BNodeValues.Enlist(GreatestValues.Pop());
            }
            WriteNode(BroNode.ID, BroNode.ToFixedLengthText());
            DadUpdate(BroNode.ID);

        }

        void DadUpdate(int BroID)
        {
            DiskBNode<T> DadNode = new DiskBNode<T>(toT, ValueLength, Degree);
            if (DadID != AvailableID)
            {
                string Line = FindNode(DadID);
                Stack<int> AuxStack = new Stack<int>();
                DadNode.ToTObj(Line);
                bool exit = false;
                while (!exit)
                {
                    if (DadNode.BNodeSons.Peek() == ActualID) exit = true;
                    AuxStack.Push(DadNode.BNodeSons.Pop());
                }
                int valueIndex = DadNode.BNodeSons.Count;
                DadNode.BNodeSons.Push(BroID);
                int count = AuxStack.Count;
                for( int i = 0; i<count; i++)
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
                        GreatestValues.Push(DadNode.BNodeValues.GetHead());
                    }
                    MiddleValue = DadNode.BNodeValues.GetHead();
                    DadID = DadNode.Dad;
                    ActualID = DadNode.ID;
                    int firstSons = DadNode.BNodeSons.Count - (Degree / 2 + 1);
                    for (int i = 0; i < firstSons; i++)
                    {
                        AuxStack.Push(DadNode.BNodeSons.Pop());
                    }
                    count = DadNode.BNodeSons.Count;
                    for (int i = 0; i < count; i++)
                    {
                        GreatestSons.Push(DadNode.BNodeSons.Pop());
                    }
                    count = AuxStack.Count;
                    for (int i = 0; i < count; i++)
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

