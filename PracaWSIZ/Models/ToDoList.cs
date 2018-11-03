using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracaWSIZ.Models
{
    public class ToDoList
    {
        public string nazwaCzynnosci { get; set; }
        public int? coIleKm { get; set; }
        public int? coIleMcy { get; set; }
        public int? max_Km { get; set; }
        public DateTime? max_Data { get; set; }
        public int? stan_Km { get; set; }
        public int? doZrobieniaKm { get; set; }
        public int? doZrobieniaMcy { get; set; }
    }
}