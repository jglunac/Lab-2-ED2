using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DataStructures;

namespace API.Models
{
    public class Movie : IComparable, IFixedLengthText
    {
        public string Director { get; set; }
        public double ImdbRating { get; set; }
        public string Genre { get; set; }
        public string ReleaseDate { get; set; }
        public int RottenTomatoesRating { get; set; }

        [JsonIgnore] public IComparable Key { get; set; }

        public string Title { get; set; }

        public void ToTObj(string line)
        {
            Title = line.Substring(0, 50).Replace("Ꮄ", ""); 
            Director = line.Substring(50, 50).Replace("Ꮄ", "");
            ImdbRating = double.Parse(line.Substring(100, 3).Replace("Ꮄ", ""));
            ReleaseDate = line.Substring(103, 11).Replace("Ꮄ", "");
            Genre = line.Substring(114, 20).Replace("Ꮄ", "");
            RottenTomatoesRating = int.Parse(line.Substring(134, 3).Replace("Ꮄ", ""));
            string titulo = "NA";
            string release = "NA";
            if (Title != null)
            {
                titulo = Title;
            }
            if (ReleaseDate != null)
            {
                release = ReleaseDate;
            }
            release = release.Substring(release.Length - 4, 4);
            Key = titulo + "-" + release;
        }

        public string ToFixedLengthText()
        {
            if (Title==null)
            {
                Title = "nulltitle";
            }
            if (Director == null)
            {
                Director = "nulldirector";
            }
            if (ReleaseDate == null)
            {
                ReleaseDate = "nulldate";
            }
            if (Genre == null)
            {
                Genre = "nullgenre";
            }
            return $"{Title.PadLeft(50, 'Ꮄ')}{Director.PadLeft(50, 'Ꮄ')}{ImdbRating:0.0}{ReleaseDate.PadLeft(11, 'Ꮄ')}{Genre.PadLeft(20, 'Ꮄ')}{RottenTomatoesRating:000}";
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
