using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Data.Entities
{
    public class Operation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Type { get; set; }

        public double Montant { get; set; }

        public string Devise { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("CarteBancaire")]
        public string NumCarte { get; set; }
        public CarteBancaire CarteBancaire { get; set; }
    }
}
