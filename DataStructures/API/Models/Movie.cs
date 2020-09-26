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

        public IComparable Key { get; set; }

        public string title { get; set; }

        public Movie ToTObj(string line)
        {
            Movie mov = new Movie();
            line = line.Replace("Ꮄ", "");
            string[] item = line.Split("¬");
            mov.title = item[0];
            mov.director = item[1];
            mov.imdbRating = double.Parse(item[2]);
            mov.releaseDate = item[3];
            mov.genre = item[4];
            mov.rottenTomatoesRating = int.Parse(item[5]);
            return mov;
        }

        public string ToFixedLengthText()
        {
            return $"{title.PadLeft(50, 'Ꮄ')}¬{director.PadLeft(50, 'Ꮄ')}¬{imdbRating:0.0}¬{releaseDate.PadLeft(11, 'Ꮄ')}¬{genre.PadLeft(20, 'Ꮄ')}¬{rottenTomatoesRating:00}";
        }

        public Movie()
        {
            Key = title.ToString() + releaseDate.ToString();
        }
        public int CompareTo(object obj)
        {
            var comparator = (Movie)obj;
           
                if (Key.CompareTo(comparator.Key)>0)
                {
                    return 1;
                }
                else if(Key.CompareTo(comparator.Key)<0)
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
