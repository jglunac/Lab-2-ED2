using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    class DiskBSortedList<T> where T:IComparable, IFixedLengthText
    {
        int degree;
        Node<T> Head;
        public DiskBSortedList(int dgree)
        {
            degree = dgree;
        }
        public bool IsEmpty()
        {
            return Head == null;
        }

        public int GetLength()
        {
            if (Head == null)
            {
                return 0;
            }
            else
            {
                Node<T> aux = Head;
                int length = 1;
                while (aux.next != null)
                {
                    aux = aux.next;
                    length++;
                }
                return length;
            }
        }
        public bool Enlist(T newT_value)
        {

            if (IsEmpty())
            {
                Head = new Node<T>();
                Head.t_object = newT_value;
                return true;
            }
            else
            {
                Node<T> newNode = new Node<T>();
                newNode.t_object = newT_value;
                Node<T> aux = new Node<T>();
                aux = Head;
                bool exit = false;
                do
                {
                    if (newNode.t_object.CompareTo(aux.t_object) > 0)
                    {
                        if (aux.next != null)
                        {
                            aux = aux.next;
                        }
                        else
                        {
                            exit = true;
                        }

                    }
                    else if (newNode.t_object.CompareTo(aux.t_object) < 0)
                    {
                        if (aux.prev == null)
                        {
                            Node<T> TempNode = new Node<T>();
                            TempNode.t_object = Head.t_object;
                            TempNode.next = Head.next;

                            Head = newNode;
                            Head.next = TempNode;
                            if (TempNode.next != null)
                            {
                                TempNode.next.prev = TempNode;
                            }

                            TempNode.prev = Head;

                        }
                        else
                        {
                            Node<T> NodoTemp = new Node<T>();
                            NodoTemp.t_object = aux.t_object;
                            NodoTemp.next = aux.next;
                            NodoTemp.prev = aux.prev;

                            newNode.next = aux;
                            newNode.prev = aux.prev;
                            aux.prev.next = newNode;
                            aux.prev = newNode;

                        }
                        return true;


                    }
                    else
                    {
                        return false;

                    }

                } while (!exit);
                aux.next = newNode;
                newNode.prev = aux;
                return true;

            }
        }
        public void Enlist(int index, T value)
        {
            Node<T> Aux = new Node<T>();
            Node<T> NewNode = new Node<T>();
            NewNode.t_object = value;
            Aux = Head;
            int i = 0;
            while (Aux.next != null && i < index )
            {
                Aux = Aux.next;
                i++;
            }
            if (i==index)
            {
                NewNode.next = Aux;
                NewNode.prev = Aux.prev;
                if (Aux.prev != null)
                {
                    Aux.prev.next = NewNode;
                }
                else
                {
                    Head = NewNode;
                }
                Aux.prev = NewNode;
            }
            else
            {
                Aux.next = NewNode;
                NewNode.prev = Aux;
            }

        }
        public bool IsFull()
        {
            if (GetLength() == degree - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public T Get()
        {
            Node<T> aux = new Node<T>();
            aux = Head;
            if (Head != null)
            {
                T Ans = Head.t_object;
                if (Head.next != null)
                {
                    Head = Head.next;
                    if(Head.next!= null) Head.next.prev = Head;
                    Head.prev = null;
                }
                else
                {
                    Head = null;
                }
                return Ans;
            }
            aux = null;
            return default(T);
        }
        public T GetHead()
        {
            Node<T> aux = new Node<T>();
            aux = Head;
            bool deleteHead = true;
            while (aux.next != null)
            {
                deleteHead = false;
                aux = aux.next;
            }
            Delete(aux, deleteHead);
            T Ans = aux.t_object;
            aux = null;
            return Ans;
        }
        public void Delete(Node<T> aux, bool deleteHead)
        {
            if (!deleteHead)
            {
                aux.prev.next = aux.next;
                if (aux.next != null)
                {
                    aux.next.prev = aux.prev;
                }
            }
            else
            {
                Head = Head.next;
                if (Head != null)
                {
                    Head.prev = null;
                    if (Head.next != null)
                    {
                        Head.next.prev = Head;
                    }
                }
            }



            aux.prev = null;
            aux.next = null;
        }
        public int GetSonIndex(T T_value)
        {
            if (IsEmpty())
            {
                return -1;
            }
            else
            {
                return RecursiveGetSonIndex(T_value, Head, 0);
                
            }
        }
        public int GetSonIndex(IComparable valueID)
        {
            if (IsEmpty())
            {
                return -1;
            }
            else
            {
                return RecursiveGetSonIndex(valueID, Head, 0);

            }
        }
        
        public T GetByIndex(int index)
        {
            if (index < GetLength())
            {
                Node<T> aux = new Node<T>();
                aux = Head;
                bool deleteHead = true;
                for (int i = 0; i < index; i++)
                {
                    deleteHead = false;
                    aux = aux.next;
                }
                Delete(aux, deleteHead);

                return aux.t_object;
            }
            else
                return default(T);
        }
        int RecursiveGetSonIndex(IComparable tKey, Node<T> ActualNode, int Sonindex)
        {

            if (tKey.CompareTo(ActualNode.t_object.Key) > 0)
            {
                if (ActualNode.next != null)
                {

                    Sonindex++;
                    return RecursiveGetSonIndex(tKey, ActualNode.next, Sonindex);
                }
                else
                {
                    Sonindex++;
                    return Sonindex;
                }

            }
            else if (tKey.CompareTo(ActualNode.t_object.Key) < 0)
            {
                return Sonindex;


            }
            else
            {
                return -1;

            }
        }
        int RecursiveGetSonIndex(T t_value, Node<T> ActualNode, int Sonindex)
        {
            
            if (t_value.CompareTo(ActualNode.t_object) > 0)
            {
                if (ActualNode.next != null)
                {
                    
                    Sonindex++;
                    return RecursiveGetSonIndex(t_value, ActualNode.next, Sonindex);
                }
                else
                {
                    Sonindex++;
                    return Sonindex;
                }

            }
            else if (t_value.CompareTo(ActualNode.t_object) < 0)
            {
                return Sonindex;


            }
            else
            {
                return -1;

            }
        }

    }
}
