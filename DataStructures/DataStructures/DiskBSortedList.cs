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
        public bool Enlist(T t)
        {

            if (IsEmpty())
            {
                Head = new Node<T>();
                Head.t_object = t;
                return true;
            }
            else
            {
                Node<T> nuevoNode = new Node<T>();
                nuevoNode.t_object = t;
                Node<T> aux = new Node<T>();
                aux = Head;
                bool exit = false;
                do
                {
                    if (nuevoNode.t_object.CompareTo(aux.t_object) > 0)
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
                    else if (nuevoNode.t_object.CompareTo(aux.t_object) < 0)
                    {
                        if (aux.prev == null)
                        {
                            Node<T> NodoTemp = new Node<T>();
                            NodoTemp.t_object = Head.t_object;
                            NodoTemp.next = Head.next;

                            Head = nuevoNode;
                            Head.next = NodoTemp;
                            if (NodoTemp.next != null)
                            {
                                NodoTemp.next.prev = NodoTemp;
                            }

                            NodoTemp.prev = Head;

                        }
                        else
                        {
                            Node<T> NodoTemp = new Node<T>();
                            NodoTemp.t_object = aux.t_object;
                            NodoTemp.next = aux.next;
                            NodoTemp.prev = aux.prev;

                            nuevoNode.next = aux;
                            nuevoNode.prev = aux.prev;
                            aux.prev.next = nuevoNode;
                            aux.prev = nuevoNode;

                        }
                        return true;


                    }
                    else
                    {
                        return false;

                    }

                } while (!exit);
                aux.next = nuevoNode;
                nuevoNode.prev = aux;
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
        public void Delete(Node<T> aux, bool hd)
        {
            if (!hd)
            {
                aux.prev.next = null;
            }
            else
            {
                Head = null;
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
        public int GetSonIndex(IComparable vId)
        {
            if (IsEmpty())
            {
                return -1;
            }
            else
            {
                return RecursiveGetSonIndex(vId, Head, 0);

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
        int RecursiveGetSonIndex(IComparable t_id, Node<T> _actual, int Sonindex)
        {

            if (t_id.CompareTo(_actual.t_object.ID) > 0)
            {
                if (_actual.next != null)
                {

                    Sonindex++;
                    return RecursiveGetSonIndex(t_id, _actual.next, Sonindex);
                }
                else
                {
                    Sonindex++;
                    return Sonindex;
                }

            }
            else if (t_id.CompareTo(_actual.t_object.ID) < 0)
            {
                return Sonindex;


            }
            else
            {
                return -1;

            }
        }
        int RecursiveGetSonIndex(T t_value, Node<T> _actual, int Sonindex)
        {
            
            if (t_value.CompareTo(_actual.t_object) > 0)
            {
                if (_actual.next != null)
                {
                    
                    Sonindex++;
                    return RecursiveGetSonIndex(t_value, _actual.next, Sonindex);
                }
                else
                {
                    Sonindex++;
                    return Sonindex;
                }

            }
            else if (t_value.CompareTo(_actual.t_object) < 0)
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
