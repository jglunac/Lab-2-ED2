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
        int ParentID;
        Stack<T> GreatestValues = new Stack<T>();
        Stack<int> GreatestSons = new Stack<int>();
        T MiddleValue;
        int ActualID;
        int RootID = 0;
        int AvailableID = 1;
        int ValueLength;
        int Degree;
        int nodeLenght;

        Stack<T> auxiliar= new Stack<T>();
        public delegate T ToTObj(string line);
        ToTObj delegate_toT;
        public DiskBTree(int TLength, int degree, string Route, ToTObj Algorithm)
        {

            //Crear archivo
            ValueLength = TLength;
            delegate_toT = Algorithm;
            Path = Route + "BTree.txt";
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
            nodeLenght = 8 + 4 * (Degree) + (Degree - 1) * ValueLength;
        }

        public bool Insert(T newValue)
        {

            if (RootID == 0)
            {
                NewRoot(newValue, 0);
                UpdateHeader();
                return true;
            }
            else
            {
                bool result = RecursiveInsert(RootID, newValue);
                UpdateHeader();
                return result;
            }
        }
        string FindNode(int ID)
        {
            string Line = "";
            using (StreamReader Reader = new StreamReader(Path))
            {
                Reader.ReadLine();
                for (int i = 0; i < ID; i++)
                {
                    Line = Reader.ReadLine();
                }
            }
            return Line;
        }
        void NewRoot(T _newValue, int BrotherID)
        {
            RootID = AvailableID;
            DiskBNode<T> AuxNode = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            AuxNode.CreateNode(AvailableID, 0);
            AuxNode.Insert(_newValue);
            AuxNode.BNodeSons.Push(BrotherID);
            AuxNode.BNodeSons.Push(ActualID);
            Stack<int> AuxStack = new Stack<int>();
            int count = AuxNode.BNodeSons.Count;
            for (int i = 0; i < count; i++)
            {
                if (AuxNode.BNodeSons.Peek() != 0)
                {
                    SonsUpdate(AuxNode.BNodeSons.Peek(), AuxNode.ID);
                    AuxStack.Push(AuxNode.BNodeSons.Pop());
                }
            }
            count = AuxStack.Count;
            for (int i = 0; i < count; i++)
            {
                AuxNode.BNodeSons.Push(AuxStack.Pop());
            }
            WriteNode(RootID, AuxNode.ToFixedLengthText());
            AvailableID++;
        }
        bool RecursiveInsert(int ID, T _newValue)
        {
            DiskBNode<T> Actual = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
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
                        if (Actual.Parent == 0)
                        {
                            Actual.Parent = AvailableID + 1;
                        }
                        ParentID = Actual.Parent;
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

            List<string> PreviousFile = new List<string>();
            using (StreamReader reader = new StreamReader(Path))
            {
                string NextLine;
                do
                {
                    NextLine = reader.ReadLine();
                    PreviousFile.Add(NextLine);
                } while (NextLine != null);

            }

            int LineCount = PreviousFile.Count;
            using (StreamWriter writer = new StreamWriter(Path))
            {
                for (int i = 0; i < LineCount; i++)
                {
                    if (i == ID)
                    {
                        writer.WriteLine(ToWrite);
                        PreviousFile.Remove(PreviousFile.First());
                    }
                    else
                    {
                        writer.WriteLine(PreviousFile.First());
                        PreviousFile.Remove(PreviousFile.First());
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
            DiskBNode<T> BrotherNode = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            BrotherNode.CreateNode(AvailableID, ParentID);
            AvailableID++;
            int count = GreatestSons.Count;
            for (int i = 0; i < count; i++)
            {
                BrotherNode.BNodeSons.Push(GreatestSons.Pop());
            }
            count = GreatestValues.Count;
            for (int i = 0; i < count; i++)
            {
                BrotherNode.BNodeValues.Enlist(GreatestValues.Pop());
            }
            WriteNode(BrotherNode.ID, BrotherNode.ToFixedLengthText());
            ParentUpdate(BrotherNode.ID);

        }

        void ParentUpdate(int BrotherID)
        {
            DiskBNode<T> ParentNode = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            if (ParentID != AvailableID)
            {
                string Line = FindNode(ParentID);
                Stack<int> AuxStack = new Stack<int>();
                ParentNode.ToTObj(Line);
                bool exit = false;
                while (!exit)
                {
                    if (ParentNode.BNodeSons.Peek() == ActualID) exit = true;
                    AuxStack.Push(ParentNode.BNodeSons.Pop());
                }
                int valueIndex = AuxStack.Count-1;
                ParentNode.BNodeSons.Push(BrotherID);
                int count = AuxStack.Count;
                for( int i = 0; i<count; i++)
                {
                    ParentNode.BNodeSons.Push(AuxStack.Pop());
                }
                bool isFull = ParentNode.BNodeValues.IsFull();
                ParentNode.Insert(valueIndex, MiddleValue);
                if (isFull)
                {
                    for (int i = 0; i < Degree / 2; i++)
                    {
                        //vaciar GreatestValues
                        GreatestValues.Push(ParentNode.BNodeValues.GetHead());
                    }
                    MiddleValue = ParentNode.BNodeValues.GetHead();
                    if (ParentNode.Parent == 0)
                    {
                        ParentNode.Parent = AvailableID + 1;
                    }
                    ParentID = ParentNode.Parent;
                    ActualID = ParentNode.ID;
                    int firstSons = ParentNode.BNodeSons.Count - (Degree / 2 + 1);
                    for (int i = 0; i < firstSons; i++)
                    {
                        AuxStack.Push(ParentNode.BNodeSons.Pop());
                    }
                    count = ParentNode.BNodeSons.Count;
                    for (int i = 0; i < count; i++)
                    {
                        GreatestSons.Push(ParentNode.BNodeSons.Pop());
                    }
                    count = AuxStack.Count;
                    for (int i = 0; i < count; i++)
                    {
                        ParentNode.BNodeSons.Push(AuxStack.Pop());
                    }
                    RewriteNode(ActualID, ParentNode.ToFixedLengthText());
                    DivideNode();
                }
                else
                {
                    count = ParentNode.BNodeSons.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (ParentNode.BNodeSons.Peek() != 0)
                        {
                            SonsUpdate(ParentNode.BNodeSons.Peek(), ParentNode.ID);
                            AuxStack.Push(ParentNode.BNodeSons.Pop());
                        }
                    }
                    count = AuxStack.Count;
                    for (int i = 0; i < count; i++)
                    {
                        ParentNode.BNodeSons.Push(AuxStack.Pop());
                    }
                    RewriteNode(ParentID, ParentNode.ToFixedLengthText());

                }

            }
            else
            {
                NewRoot(MiddleValue, BrotherID);
            }
        }
        void SonsUpdate(int _actualID, int parent_id)
        {
            DiskBNode<T> ActualNode = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            string Line = FindNode(_actualID);
            Stack<int> AuxStack = new Stack<int>();
            ActualNode.ToTObj(Line);
            ActualNode.Parent = parent_id;
            int count = ActualNode.BNodeSons.Count;
            for (int i = 0; i < count; i++)
            {
                if (ActualNode.BNodeSons.Peek()!=0)
                {
                    SonsUpdate(ActualNode.BNodeSons.Peek(), ActualNode.ID);
                    AuxStack.Push(ActualNode.BNodeSons.Pop());
                }
            }
            count = AuxStack.Count;
            for (int i = 0; i < count; i++)
            {
                ActualNode.BNodeSons.Push(AuxStack.Pop());
            }
            RewriteNode(ActualNode.ID, ActualNode.ToFixedLengthText());


        }

        public bool Delete(IComparable ValueID)
        {
            if (RootID == 0)
            {
                UpdateHeader();
                return false;
            }
            else
            {
                bool result = RecursiveDelete(ValueID, RootID, false);
                UpdateHeader();
                return result;
            }
            
        }
        bool RecursiveDelete(IComparable ValueKey, int NodeID, bool backReview)
        {
            DiskBNode<T> Actual = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            string Line = FindNode(NodeID);
            Actual.ToTObj(Line);
            bool hasSons;
            
            if (!backReview) hasSons = Actual.HasSons();
            else hasSons = false;

            bool isFull = Actual.BNodeValues.IsFull();
            int count = Actual.BNodeValues.GetLength();
            Stack<int> AuxStack = new Stack<int>();
            int sonID = -1;
            bool exit = false;
            int i = 0;
            T deletedValue;
            while (!Actual.BNodeValues.IsEmpty() && !exit)
            {
                auxiliar.Push(Actual.BNodeValues.Get());
                if (ValueKey.CompareTo(auxiliar.Peek().Key) == 0)
                {
                    //eliminar valor
                    deletedValue =auxiliar.Pop();
                    exit = true;
                    if (hasSons)
                    {
                        //i es index del valor
                        sonID = Actual.GetSonID(i + 1);
                        //FindMinor requiere ID de hijo izquierdo
                        bool Right = true;
                        bool Underflow = false;
                        T ParentReplacement = FindMinor(sonID, ref Underflow, Right);
                        if (Underflow)
                        {
                            sonID= Actual.GetSonID(i);
                            Right = false;
                            ParentReplacement = FindMinor(sonID, ref Underflow, Right);
                            if (Underflow)
                            {
                                ActualID = Actual.GetSonID(i);
                                ParentID = Actual.ID;
                                
                                
                                sonUnion(Actual.GetSonID(i+1));
                                do
                                {
                                    AuxStack.Push(Actual.BNodeSons.Pop());
                                } while (AuxStack.Peek() != ActualID);
                                AuxStack.Pop();
                                count = AuxStack.Count;
                                for (int k = 0; k < count; k++)
                                {
                                    Actual.BNodeSons.Push(AuxStack.Pop());
                                }
                            }
                            else
                            {
                                Actual.BNodeValues.Enlist(ParentReplacement);
                            }
                        }
                        else
                        {
                            Actual.BNodeValues.Enlist(ParentReplacement);
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
                if (Actual.Parent != 0)
                {
                    if (Actual.BNodeValues.GetLength() < Math.Round((Degree / 2.00) - 1))
                    {
                        bool Rotation = false;
                        bool fromRightBrother = false;
                        ActualID = Actual.ID;
                        ParentID = Actual.Parent;
                        //Rotación de valores
                        T replacement = ValueRotation(ref Rotation, ref fromRightBrother);
                       

                        if (Rotation)
                        {
                            Actual.Insert(replacement);
                            if (GreatestSons.Count != 0 && GreatestSons.Peek()!=0)
                            {
                                DiskBNode<T> ToAddSon = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
                                Line = FindNode(GreatestSons.Peek());
                                ToAddSon.ToTObj(Line);
                                ToAddSon.Parent = Actual.ID;
                                RewriteNode(GreatestSons.Peek(), ToAddSon.ToFixedLengthText());
                                if (fromRightBrother)
                                {
                                    count = Actual.BNodeSons.Count;
                                    for (int j = 0; j < count; j++)
                                    {
                                        if (Actual.BNodeSons.Peek() == 0)
                                        {
                                            Actual.BNodeSons.Pop();
                                        }
                                        else
                                        {
                                            AuxStack.Push(Actual.BNodeSons.Pop());
                                        }
                                    }
                                    
                                    Actual.BNodeSons.Push(GreatestSons.Pop());
                                    count = AuxStack.Count;
                                    for (int k = 0; k < count; k++)
                                    {
                                        Actual.BNodeSons.Push(AuxStack.Pop());
                                    }
                                }
                                else
                                {
                                    Actual.BNodeSons.Push(GreatestSons.Pop());
                                }
                            }
                        }
                        else
                        {
                            //Unión de hermanos y padre
                            Actual.Parent = 0;
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
                }
                else
                {
                    if (Actual.BNodeValues.IsEmpty())
                    {
                        count = Actual.BNodeSons.Count;
                        for (int k = 0; k < count; k++)
                        {
                            Actual.BNodeSons.Pop();
                        }
                        if (backReview)
                        {
                            RewriteNode(NodeID, Actual.ToFixedLengthText());
                            UpdateHeader();
                            return false;
                        }
                        
                    }
                    
                }
                RewriteNode(NodeID, Actual.ToFixedLengthText());

                return true;
            }
            else if (hasSons)
            {
                int SonIndex = Actual.BNodeValues.GetSonIndex(ValueKey);
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
                    return RecursiveDelete(ValueKey, sonID, false);
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
           
        }
        void DeleteNode()
        {
            DiskBNode<T> Parent = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            DiskBNode<T> Brother = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            string Line = FindNode(ParentID);
            Parent.ToTObj(Line);
            Stack<int> temp = new Stack<int>();
            Stack<int> temp2 = new Stack<int>();
            int count;
            T ParentValue;
            int ParentValueIndex;
            //volver a llenar BNodeSons
            do
            {
                temp.Push(Parent.BNodeSons.Pop());
            } while (temp.Peek() != ActualID);
            ParentValueIndex = temp.Count - 1;
            if (Parent.BNodeSons.Count != 0 && Parent.BNodeSons.Peek() != 0)
            {
                Line = FindNode(Parent.BNodeSons.Peek());
                Brother.ToTObj(Line);
                count = GreatestValues.Count();
                ParentValue = Parent.BNodeValues.GetByIndex(ParentValueIndex);
                Brother.Insert(ParentValue);
                Parent.Insert(ParentValue);
                

                for (int i = 0; i < count; i++)
                {
                    Brother.Insert(GreatestValues.Pop());
                }
                temp.Pop();
                count = temp.Count;
                for (int i = 0; i < count; i++)
                {
                    Parent.BNodeSons.Push(temp.Pop());
                }
                
                count = GreatestSons.Count;
                for (int i = 0; i < count; i++)
                {
                    Brother.BNodeSons.Push(GreatestSons.Pop());
                }
                count = temp2.Count;
                for (int i = 0; i < count; i++)
                {
                    Brother.BNodeSons.Push(temp2.Pop());
                }
            }
            else
            {
                Parent.BNodeSons.Push(temp.Pop());
                Line = FindNode(temp.Peek());
                Brother.ToTObj(Line);
                count = GreatestValues.Count();
                ParentValue = Parent.BNodeValues.GetByIndex(ParentValueIndex);
                Brother.Insert(ParentValue);
                Parent.Insert(ParentValue);
                for (int i = 0; i < count; i++)
                {
                    Brother.Insert(GreatestValues.Pop());
                }
                temp.Pop();
                count = temp.Count;
                for (int i = 0; i < count; i++)
                {
                    Parent.BNodeSons.Push(temp.Pop());
                }
                count = Brother.BNodeSons.Count;
                for (int i = 0; i < count; i++)
                {
                    temp2.Push(Brother.BNodeSons.Pop());
                }
                count = GreatestSons.Count;
                for (int i = 0; i < count; i++)
                {
                    Brother.BNodeSons.Push(GreatestSons.Pop());
                }
            }

            RewriteNode(Parent.ID, Parent.ToFixedLengthText());
            bool newRoot = !RecursiveDelete(ParentValue.Key, Parent.ID, true);
            if (newRoot)
            {
                RootID = Brother.ID;
                Brother.Parent = 0;
            }
            RewriteNode(Brother.ID, Brother.ToFixedLengthText());
        }
        void sonUnion(int leftBrotherID)
        {
           
            DiskBNode<T> Brother = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            string Line = FindNode(ActualID);
           Brother.ToTObj(Line);
            Brother.Parent = 0;
            
            while (!Brother.BNodeValues.IsEmpty())
            {
                GreatestValues.Push(Brother.BNodeValues.Get());
            }
            RewriteNode(Brother.ID, Brother.ToFixedLengthText());
            Line = FindNode(leftBrotherID);
            
            Brother.ToTObj(Line);
            int count = GreatestValues.Count;
            for (int i = 0; i < count; i++)
            {
                Brother.Insert(GreatestValues.Pop());
            }
            RewriteNode(Brother.ID, Brother.ToFixedLengthText());

        }
        T FindMinor(int _sonID, ref bool underflow, bool RightSon)
        {
            DiskBNode<T> Actual = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            string Line = FindNode(_sonID);
            Actual.ToTObj(Line);

            if (Actual.HasSons())
            {

                return RecursiveFindMinor(Actual.BNodeSons.Peek());
            }
            else
            {
                if (Actual.BNodeValues.GetLength() > Math.Round((Degree / 2.00) - 1))
                {
                    underflow = false;
                    if (RightSon)
                    {
                        T ToReturn = Actual.BNodeValues.Get();
                        RewriteNode(Actual.ID, Actual.ToFixedLengthText());
                        return ToReturn;
                    }
                    else
                    {
                        T ToReturn = Actual.BNodeValues.GetHead();
                        RewriteNode(Actual.ID, Actual.ToFixedLengthText());
                        return ToReturn;
                    }
                }
                else
                {
                    underflow = true;
                    return default(T);
                }

                    
            }

        }
        T RecursiveFindMinor(int son_id)
        {
            DiskBNode<T> Actual = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            string Line = FindNode(son_id);
            Actual.ToTObj(Line);
            if (Actual.HasSons())
            {

                return RecursiveFindMinor(Actual.BNodeSons.Peek());
            }
            else
            {

                T ToReturn = Actual.BNodeValues.Get();
                Actual.Insert(ToReturn);
                RecursiveDelete(ToReturn.Key, Actual.ID, false);
                return ToReturn;
            }
        }

        T ValueRotation(ref bool success, ref bool rightBrother)
        {
            DiskBNode<T> Parent = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            DiskBNode<T> Brother = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
            string Line = FindNode(ParentID);
            Parent.ToTObj(Line);
            T ReturnValue;
            Stack<int> temp = new Stack<int>();
            //volver a llenar BNodeSons
            do
            {
                temp.Push(Parent.BNodeSons.Pop());
            } while (temp.Peek() != ActualID);
            int ParentValueIndex = temp.Count - 1;
            if (Parent.BNodeSons.Count != 0 && Parent.BNodeSons.Peek()!=0)
            {
                Line = FindNode(Parent.BNodeSons.Peek());
                Brother.ToTObj(Line);

                if (Brother.BNodeValues.GetLength() > Math.Round((Degree / 2.00) - 1))
                {
                    rightBrother = true;
                    ReturnValue = Parent.BNodeValues.GetByIndex(ParentValueIndex);
                    Parent.BNodeValues.Enlist(Brother.BNodeValues.Get());
                    success = true;
                    int count = temp.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Parent.BNodeSons.Push(temp.Pop());
                    }
                    GreatestSons.Push(Brother.BNodeSons.Pop());
                    RewriteNode(Parent.ID, Parent.ToFixedLengthText());
                    RewriteNode(Brother.ID, Brother.ToFixedLengthText());
                    return ReturnValue;

                }
                else
                {
                    Parent.BNodeSons.Push(temp.Pop());
                    if (temp.Count != 0 && temp.Peek() != 0)
                    {
                        Line = FindNode(temp.Peek());
                        while (!Brother.BNodeValues.IsEmpty())
                        {
                            Brother.BNodeValues.Get();
                        }
                        int count = Brother.BNodeSons.Count;
                        for (int i = 0; i < count; i++)
                        {
                            Brother.BNodeSons.Pop();
                        }
                        Brother.ToTObj(Line);
                        if (Brother.BNodeValues.GetLength() > Math.Round((Degree / 2.00) - 1))
                        {
                            rightBrother = false;
                            ReturnValue = Parent.BNodeValues.GetByIndex(ParentValueIndex - 1);
                            Parent.BNodeValues.Enlist(Brother.BNodeValues.GetHead());
                            success = true;
                            count = temp.Count;
                            for (int i = 0; i < count; i++)
                            {
                                Parent.BNodeSons.Push(temp.Pop());
                            }
                            count = Brother.BNodeSons.Count;
                            for (int i = 0; i < count; i++)
                            {
                                if (Brother.BNodeSons.Peek() == 0)
                                {
                                    Brother.BNodeSons.Pop();
                                }
                                else
                                {
                                    temp.Push(Brother.BNodeSons.Pop());
                                }
                            }
                            if(temp.Count != 0) GreatestSons.Push(temp.Pop());
                            count = temp.Count;
                            for (int i = 0; i <count; i++)
                            {
                                Brother.BNodeSons.Push(temp.Pop());
                            }
                            RewriteNode(Parent.ID, Parent.ToFixedLengthText());
                            RewriteNode(Brother.ID, Brother.ToFixedLengthText());
                            return ReturnValue;

                        }
                        else
                        {
                            count = temp.Count;
                            for (int i = 0; i < count; i++)
                            {
                                Parent.BNodeSons.Push(temp.Pop());
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
                Parent.BNodeSons.Push(temp.Pop());
                Line = FindNode(temp.Peek());
                while (!Brother.BNodeValues.IsEmpty())
                {
                    Brother.BNodeValues.Get();
                }
                int count = Brother.BNodeSons.Count;
                for (int i = 0; i < count; i++)
                {
                    Brother.BNodeSons.Pop();
                }
                Brother.ToTObj(Line);
                if (Brother.BNodeValues.GetLength() > Math.Round((Degree / 2.00) - 1))
                {
                    rightBrother = false;
                    ReturnValue = Parent.BNodeValues.GetByIndex(ParentValueIndex - 1);
                    Parent.BNodeValues.Enlist(Brother.BNodeValues.GetHead());
                    success = true;
                    count = temp.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Parent.BNodeSons.Push(temp.Pop());
                    }
                    count = Brother.BNodeSons.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (Brother.BNodeSons.Peek()==0)
                        {
                            Brother.BNodeSons.Pop();
                        }
                        else
                        {
                            temp.Push(Brother.BNodeSons.Pop());
                        }
                    }
                    
                    if(temp.Count!=0)GreatestSons.Push(temp.Pop());
                    count = temp.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Brother.BNodeSons.Push(temp.Pop());
                    }
                    RewriteNode(Parent.ID, Parent.ToFixedLengthText());
                    RewriteNode(Brother.ID, Brother.ToFixedLengthText());
                    return ReturnValue;

                }
                else
                {
                    //Unión con padre y hermano izquierdo
                    count = temp.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Parent.BNodeSons.Push(temp.Pop());
                    }
                    success = false;
                    return default(T);
                }
            }

        }

        public void InOrder(List<T> TargetList)
        {
            RecursiveInOrder(TargetList, RootID);
        }
        void RecursiveInOrder(List<T> TargetList, int actualNode)
        {
            string Line = FindNode(actualNode);
            if (actualNode != 0)
            {
                DiskBNode<T> actual = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
                
                actual.ToTObj(Line);
                Stack<int> aux = new Stack<int>();
                if (actual.HasSons())
                {
                    while (actual.BNodeSons.Count != 0)
                    {
                        aux.Push(actual.BNodeSons.Peek());
                        RecursiveInOrder(TargetList, actual.BNodeSons.Pop());
                        T Value = actual.BNodeValues.GetByIndex(aux.Count - 1);
                        if (Value != null)
                        {
                            TargetList.Add(Value);
                            actual.BNodeValues.Enlist(Value);
                        }

                    }
                }
                else
                {
                    while (!actual.BNodeValues.IsEmpty())
                    {
                        TargetList.Add(actual.BNodeValues.Get());
                    }
                }
            }
           
        }

        public void PreOrder(List<T> TargetList)
        {
            RecursivePreOrder(TargetList, RootID);
        }

        //RID
        void RecursivePreOrder(List<T> TargetList, int actualNode)
        {
            string Line = FindNode(actualNode);
            if (actualNode != 0)
            {
                DiskBNode<T> actual = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
                actual.ToTObj(Line);
                Stack<int> aux = new Stack<int>();
                while (!actual.BNodeValues.IsEmpty())
                {
                    T Value = actual.BNodeValues.Get();
                    if (Value != null)
                    {
                        TargetList.Add(Value);
                    }
                }
                if (actual.HasSons())
                {
                    while (actual.BNodeSons.Count != 0)
                    {
                        aux.Push(actual.BNodeSons.Peek());
                        RecursivePreOrder(TargetList, actual.BNodeSons.Pop());
                    }

                }
            }
        }

        public void PostOrder(List<T> TargetList)
        {
            RecursivePostOrder(TargetList, RootID);
        }

        //IDR
        void RecursivePostOrder(List<T> TargetList, int actualNode)
        {
            string Line = FindNode(actualNode);
            if (actualNode != 0)
            {
                DiskBNode<T> actual = new DiskBNode<T>(delegate_toT, ValueLength, Degree);
                actual.ToTObj(Line);
                Stack<int> aux = new Stack<int>();

                if (actual.HasSons())
                {
                    while (actual.BNodeSons.Count != 0)
                    {
                        aux.Push(actual.BNodeSons.Peek());
                        RecursivePreOrder(TargetList, actual.BNodeSons.Pop());
                    }

                }

                while (!actual.BNodeValues.IsEmpty())
                {
                    T Value = actual.BNodeValues.Get();
                    if (Value != null)
                    {
                        TargetList.Add(Value);
                    }
                }
            }
        }
        void UpdateHeader()
        {
            
            
            List<string> PreviousFile = new List<string>();
            using (StreamReader reader = new StreamReader(Path))
            {
                string NextLine;
                do
                {
                    NextLine = reader.ReadLine();
                    PreviousFile.Add(NextLine);
                } while (NextLine != null);
            }
            
            string newHeader = "Root:" + RootID + " Available Node: " + AvailableID.ToString() + " Node Lenght: " + nodeLenght;
            int largo = PreviousFile.Count;
            using (StreamWriter writer = new StreamWriter(Path))
            {
                for (int i = 0; i < largo; i++)
                {

                    if (i == 0)
                    {
                        writer.WriteLine(newHeader);
                        PreviousFile.Remove(PreviousFile.First());
                    }
                    else
                    {
                        writer.WriteLine(PreviousFile.First());
                        PreviousFile.Remove(PreviousFile.First());
                    }
                }
            }           
        }

        public string DeleteTree()
        {
            File.Delete(Path);
            return "Ok";
        }
    }
}

