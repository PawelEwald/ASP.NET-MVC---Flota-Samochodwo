using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracaWSIZ.Models
{
    public class DetailsViewModel
    {
        public CzynnosciAutoCoDotyczy CzynnosciAutoCoDotyczyVM { get; set; }
        public IEnumerable<CzynnosciAutoCoDotyczy> czynnosciAcutCoDotyczyLista { get; set; }
        public CzynnosciWykonane czynnosciWykonane { get; set; }
        public IEnumerable<CzynnosciWykonane> czynnosciWykonanesList { get; set; }

        public Tankowanie tankowanie { get; set; }
        public IEnumerable<Tankowanie> tankowaniesList { get; set; }
    }
}