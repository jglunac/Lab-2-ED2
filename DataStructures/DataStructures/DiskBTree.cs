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
        int BroSon;
        //List<T> aux;
        Stack<T> auxiliar= new Stack<T>();
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

        public bool Delete(IComparable ValueID)
        {
            if (RootID==0)
            {
                return false;
            }
            else
            {
                return RecursiveDelete(ValueID, RootID);
            }
            
        }
        bool RecursiveDelete(IComparable v_id, int NodeID)
        {
            DiskBNode<T> Actual = new DiskBNode<T>(toT, ValueLength, Degree);
            string Line = FindNode(NodeID);
            bool hasSons = Actual.HasSons();
            Actual.ToTObj(Line);
            bool isFull = Actual.BNodeValues.IsFull();
            int count = Actual.BNodeValues.GetLength();
            Stack<int> AuxStack = new Stack<int>();
            int sonID = -1;
            bool exit = false;
            int i = 0;
            while (!Actual.BNodeValues.IsEmpty()&& !exit)
            {
                auxiliar.Push(Actual.BNodeValues.GetHead());
                if (v_id.CompareTo(auxiliar.Peek().Key) == 0)
                {
                    //eliminar valor
                    auxiliar.Pop();
                    exit = true;
                    if (hasSons)
                    {
                        //i es index del valor
                        for (int k = 0; k <= i+1; k++)
                        {
                            sonID = Actual.BNodeSons.Peek();
                            AuxStack.Push(Actual.BNodeSons.Pop());
                        }
                        count = AuxStack.Count;
                        for (int j = 0; j < count; j++)
                        {
                            Actual.BNodeSons.Push(AuxStack.Pop());
                        }
                        //FindMinor requiere ID de hijo izquierdo
                        T DadReplacement = FindMinor(sonID);
                        if (DadReplacement == null)
                        {

                        }
                        else
                        {
                            Actual.BNodeValues.Enlist(DadReplacement);
                        }
                    }
                    
                }
                i++;
            }
            count = auxiliar.Count;
            for (int j = 0; j < count; j++)
            {
                Actual.BNodeValues.Enlist(auxiliar.Pop());
            }
            
            if (exit)
            {
                if (Actual.BNodeValues.GetLength() < Math.Round((Degree / 2.00) - 1))
                {
                    bool Rotation=false;
                    ActualID = Actual.ID;
                    DadID = Actual.Dad;
                    //Rotación de valores
                    T replacement = ValueRotation(ref Rotation);
                    if (Rotation)
                    {
                        Actual.Insert(replacement);
                        
                    }
                    else
                    {
                        //Unión de hermanos y padre
                        Actual.ID = 0;
                        while (!Actual.BNodeValues.IsEmpty())
                        {
                            GreatestValues.Push(Actual.BNodeValues.GetHead());
                        }
                        count = Actual.BNodeSons.Count;
                        for (int j = 0; j < count; j++)
                        {
                            GreatestSons.Push(Actual.BNodeSons.Pop());
                        }
                        DeleteNode();
                        
                    
                    }
                    

                }
                RewriteNode(NodeID, Actual.ToFixedLengthText());
                return true;
            }
            else if (hasSons)
            {
                int SonIndex = Actual.BNodeValues.GetSonIndex(v_id);
                sonID = -1;

                for (int h = 0; h <= SonIndex; h++)
                {
                    sonID = Actual.BNodeSons.Peek();
                    AuxStack.Push(Actual.BNodeSons.Pop());
                }
                count = AuxStack.Count;
                for (int j = 0; j < count; j++)
                {
                    Actual.BNodeSons.Push(AuxStack.Pop());
                }
                if (sonID != -1)
                {
                    return RecursiveDelete(v_id, sonID);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            //if (isFull)
            //{
            //    for (int i = 0; i < Degree / 2; i++)
            //    {
            //        //vaciar GreatestValues
            //        GreatestValues.Push(Actual.BNodeValues.GetHead());
            //    }
            //    MiddleValue = Actual.BNodeValues.GetHead();
            //    if (Actual.Dad == 0)
            //    {
            //        Actual.Dad = AvailableID + 1;
            //    }
            //    DadID = Actual.Dad;
            //    ActualID = Actual.ID;

            //    RewriteNode(ActualID, Actual.ToFixedLengthText());
            //    DivideNode();
            //    return true;

            //}
            //else
            //{
            //    RewriteNode(ID, Actual.ToFixedLengthText());
            //    return true;
            //}
        }
        void DeleteNode()
        {
            DiskBNode<T> Dad = new DiskBNode<T>(toT, ValueLength, Degree);
            DiskBNode<T> Bro = new DiskBNode<T>(toT, ValueLength, Degree);
            string Line = FindNode(DadID);
            Dad.ToTObj(Line);
            Stack<int> temp = new Stack<int>();
            Stack<int> temp2 = new Stack<int>();
            int count;
            //volver a llenar BNodeSons
            do
            {
                temp.Push(Dad.BNodeSons.Pop());
            } while (temp.Peek() != ActualID);
            int DadValueIndex = temp.Count - 1;
            if (Dad.BNodeSons.Count != 0)
            {
                Line = FindNode(Dad.BNodeSons.Peek());
                Bro.ToTObj(Line);
                count = GreatestValues.Count();
                Bro.Insert(Dad.BNodeValues.GetByIndex(DadValueIndex));
                for (int i = 0; i < count; i++)
                {
                    Bro.Insert(GreatestValues.Pop());
                }
                temp.Pop();
                count = temp.Count;
                for (int i = 0; i < count; i++)
                {
                    Dad.BNodeSons.Push(temp.Pop());
                }
                count = Bro.BNodeSons.Count;
                for (int i = 0; i < count; i++)
                {
                    temp2.Push(Bro.BNodeSons.Pop());
                }
                count = GreatestSons.Count;
                for (int i = 0; i < count; i++)
                {
                    Bro.BNodeSons.Push(GreatestSons.Pop());
                }
                count = temp2.Count;
                for (int i = 0; i < count; i++)
                {
                    Bro.BNodeSons.Push(temp2.Pop());
                }
            }
            else
            {
                Dad.BNodeSons.Push(temp.Pop());
                Line = FindNode(temp.Peek());
                Bro.ToTObj(Line);
                count = GreatestValues.Count();
                Bro.Insert(Dad.BNodeValues.GetByIndex(DadValueIndex));
                for (int i = 0; i < count; i++)
                {
                    Bro.Insert(GreatestValues.Pop());
                }
                temp.Pop();
                count = temp.Count;
                for (int i = 0; i < count; i++)
                {
                    Dad.BNodeSons.Push(temp.Pop());
                }
                count = GreatestSons.Count;
                for (int i = 0; i < count; i++)
                {
                    Bro.BNodeSons.Push(GreatestSons.Pop());
                }
            }
            RewriteNode(Dad.ID, Dad.ToFixedLengthText());
            RewriteNode(Bro.ID, Bro.ToFixedLengthText());
        }
        T FindMinor(int _sonID)
        {
            DiskBNode<T> Actual = new DiskBNode<T>(toT, ValueLength, Degree);
            string Line = FindNode(_sonID);
            Actual.ToTObj(Line);
            
            if (Actual.HasSons())
            {

            }
            else
            {
                if (Actual.BNodeValues.GetLength()>Math.Round((Degree/2.00)-1))
                {
                    return Actual.BNodeValues.Get();
                }
                else
                {

                }
            }

        }

        T ValueRotation(ref bool success)
        {
            DiskBNode<T> Dad = new DiskBNode<T>(toT, ValueLength, Degree);
            DiskBNode<T> Bro = new DiskBNode<T>(toT, ValueLength, Degree);
            string Line = FindNode(DadID);
            Dad.ToTObj(Line);
            T ReturnValue;
            Stack<int> temp = new Stack<int>();
            //volver a llenar BNodeSons
            do
            {
                temp.Push(Dad.BNodeSons.Pop());
            } while (temp.Peek() != ActualID) ;
            int DadValueIndex = temp.Count - 1;
            if (Dad.BNodeSons.Count != 0)
            {
                Line = FindNode(Dad.BNodeSons.Peek());
                Bro.ToTObj(Line);
                
                if (Bro.BNodeValues.GetLength() > Math.Round((Degree / 2.00) - 1))
                {
                    ReturnValue = Dad.BNodeValues.GetByIndex(DadValueIndex);
                    Dad.BNodeValues.Enlist(Bro.BNodeValues.Get());
                    success = true;
                    int count = temp.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Dad.BNodeSons.Push(temp.Pop());
                    }
                    RewriteNode(Dad.ID, Dad.ToFixedLengthText());
                    RewriteNode(Bro.ID, Bro.ToFixedLengthText());
                    return ReturnValue;

                }
                else
                {
                    Dad.BNodeSons.Push(temp.Pop());
                    if (temp.Count != 0)
                    {
                        Line = FindNode(temp.Peek());
                        Bro.ToTObj(Line);
                        if (Bro.BNodeValues.GetLength() > Math.Round((Degree / 2.00) - 1))
                        {
                            ReturnValue = Dad.BNodeValues.GetByIndex(DadValueIndex-1);
                            Dad.BNodeValues.Enlist(Bro.BNodeValues.GetHead());
                            success = true;
                            int count = temp.Count;
                            for (int i = 0; i < count; i++)
                            {
                                Dad.BNodeSons.Push(temp.Pop());
                            }
                            RewriteNode(Dad.ID, Dad.ToFixedLengthText());
                            RewriteNode(Bro.ID, Bro.ToFixedLengthText());
                            return ReturnValue;

                        }
                        else
                        {
                            int count = temp.Count;
                            for (int i = 0; i < count; i++)
                            {
                                Dad.BNodeSons.Push(temp.Pop());
                            }
                            //Unión con padre y hermano derecho

                            success = false;
                            return default(T);
                        }
                    }
                    else
                    {
                        //Unión con padre y hermano derecho
                        success = false;
                        return default(T);
                    }
                }
            }
            else
            {
                Dad.BNodeSons.Push(temp.Pop());
                Line = FindNode(temp.Peek());
                Bro.ToTObj(Line);
                if (Bro.BNodeValues.GetLength() > Math.Round((Degree / 2.00) - 1))
                {
                    ReturnValue = Dad.BNodeValues.GetByIndex(DadValueIndex - 1);
                    Dad.BNodeValues.Enlist(Bro.BNodeValues.GetHead());
                    success = true;
                    int count = temp.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Dad.BNodeSons.Push(temp.Pop());
                    }
                    RewriteNode(Dad.ID, Dad.ToFixedLengthText());
                    RewriteNode(Bro.ID, Bro.ToFixedLengthText());
                    return ReturnValue;

                }
                else
                {
                    //Unión con padre y hermano izquierdo
                    int count = temp.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Dad.BNodeSons.Push(temp.Pop());
                    }
                    success = false;
                    return default(T);
                }
            }
            
        }
    }   
}

