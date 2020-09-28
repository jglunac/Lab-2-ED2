using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures
{
    class DiskBNode<T> : IFixedLengthText where T : IFixedLengthText, IComparable
    {
        public int ID;

        public int Parent;
        public DiskBSortedList<T> BNodeValues;
        public Stack<int> BNodeSons = new Stack<int>();
        int Degree;
        public IComparable Key { get; set; }
        int ValueLength;
        public int NodeLength;
        DiskBTree<T>.ToTObj delegate_toT;

        public void CreateNode(int _id, int _parent)
        {
            ID = _id;
            Parent = _parent;
        }
        public DiskBNode(DiskBTree<T>.ToTObj _algorithm, int TLength, int degree)
        {
            delegate_toT = _algorithm;
            ValueLength = TLength;
            Degree = degree;
            NodeLength = 8 + 4 * (Degree) + (Degree - 1) * ValueLength;
            BNodeValues = new DiskBSortedList<T>(Degree);
        }
        public int GetSonID(int index)
        {
            //Todo valor tiene un hijo derecho y un izquierdo
            Stack<int> AuxStack = new Stack<int>();
            int sonID = 0;
            for (int k = 0; k <= index; k++)
            {
                sonID = BNodeSons.Peek();
                AuxStack.Push(BNodeSons.Pop());
            }
            int count = AuxStack.Count;
            for (int j = 0; j < count; j++)
            {
                BNodeSons.Push(AuxStack.Pop());
            }
            return sonID;
        }
        public void ToTObj(string Line)
        {
            int index;
            for (int i = 0; i < Degree - 1; i++)
            {
                index = Line.Length - ValueLength;
                if (Line.Substring(index) != "‡".PadLeft(ValueLength, '-'))
                {

                    BNodeValues.Enlist(delegate_toT(Line.Substring(index)));

                }

                Line = Line.Remove(index);

            }
            for (int i = 0; i <= Degree + 1; i++)
            {
                index = Line.Length - 4;
                if (i < Degree)
                {
                    if (Convert.ToInt32(Line.Substring(index)) != 0)
                    {
                        BNodeSons.Push(Convert.ToInt32(Line.Substring(index)));
                    }
                }
                else if (i == Degree)
                {
                    Parent = Convert.ToInt32(Line.Substring(index));
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
            string response = $"{ID:0000}{Parent:0000}";
            int _nullSons = Degree - BNodeSons.Count;
            int count;
            if (BNodeSons.Count < Degree)
            {
                count = BNodeSons.Count;
            }
            else
            {
                count = Degree;
            }
            for (int j = 0; j < count; j++)
            {
                response += $"{BNodeSons.Pop():0000}";
            }

            for (int j = 0; j < _nullSons; j++)
            {
                response += "0000";
            }
            int i = 0;
            while (!BNodeValues.IsEmpty())
            {
                response += BNodeValues.Get().ToFixedLengthText();
                i++;
            }
            while (i != Degree - 1)
            {
                response += "‡".PadLeft(ValueLength, '-');
                i++;
            }
            return response;
        }
        public bool Insert(T NewValue)
        {
            return BNodeValues.Enlist(NewValue);

        }
        public void Insert(int _index, T nValue)
        {
            BNodeValues.Enlist(_index, nValue);

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

        string IFixedLengthText.ToFixedLengthText()
        {
            throw new NotImplementedException();
        }

        void IFixedLengthText.ToTObj(string line)
        {
            throw new NotImplementedException();
        }
    }
}

