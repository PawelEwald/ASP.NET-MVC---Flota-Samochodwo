using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracaWSIZ.Models
{
    public class PrzypomnienieViewModel
    {

        public Tankowanie tankowanie { get; set; }
        public CzynnosciAutoCoDotyczy czynnosciAuto { get; set; }
        public CzynnosciWykonane CzynnosciWykonane { get; set; }
        
    }
}

