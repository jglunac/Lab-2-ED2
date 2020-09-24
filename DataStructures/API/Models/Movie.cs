using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStructures;

namespace API.Models
{
    public class Movie : IComparable, IFixedLengthText
    {
        public string director { get; set; }
        public double imdbRating { get; set; }
        public string genre { get; set; }
        public string releaseDate { get; set; }
        public int rottenTomatoesRating { get; set; }

        public string ID { get; set; }

        public string title { get; set; }

        public void ToTObj(string line)
        {
            ID = (line).ToString();
        }

        public string ToFixedLengthText()
        {
            return $"{ID:0000}";
        }

        public Movie()
        {
            ID = title.ToString() + releaseDate.ToString();
        }
        public int CompareTo(object obj)
        {
            var comparator = (Movie)obj;
           
                if (ID.CompareTo(comparator.ID)>0)
                {
                    return 1;
                }
                else if(ID.CompareTo(comparator.ID)<0)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            
        }


    }
}
