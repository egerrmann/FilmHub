using System.Collections.Generic;

namespace FilmHub.Models
{
    public class FilmViewModel
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Summary { get; set; }
        public string Time { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public List<string> Actors { get; set; }
        public string Image { get; set; }
    }
}