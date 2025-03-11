using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Data.Entities
{
    public class CarteBancaire
    {
        [Key]
        public string NumCarte { get; set; }

        public DateTime DateExpiration { get; set; }

        [ForeignKey("CompteBancaire")]
        public string NumCompte { get; set; }
        public CompteBancaire CompteBancaire { get; set; }
    }
}
