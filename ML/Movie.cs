using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Movie
    {
        public int id { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
        public string backdrop_path { get; set; }
        public string vote_average { get; set; }
        public string original_language { get; set; }
        public string poster_path { get; set; }
        public List<object> Movies { get; set; }
    }
}
