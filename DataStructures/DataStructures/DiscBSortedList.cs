using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    class DiscBSortedList<T> where T:IComparable
    {
        int degree;
        Node<T> Head;
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
            bool shiftHead = true;
            while(aux.next != null)
            {
                shiftHead = false;
                aux = aux.next;
            }
            Shift(aux, shiftHead);
            T Ans = aux.t_object;
            aux = null;
            return Ans;
        }
        public void Shift(Node<T> aux, bool hd)
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
                Node<T> aux = new Node<T>();
                aux = Head;
                bool exit = false;
                int Sonindex = 0;
                if (T_value.CompareTo(aux.t_object) > 0)
                {
                    if (aux.next != null)
                    {
                        aux = aux.next;
                        Sonindex++;
                    }
                    else
                    {
                        Sonindex++;
                        return Sonindex;
                    }

                }
                else if (T_value.CompareTo(aux.t_object) < 0)
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
}
