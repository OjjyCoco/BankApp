using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Datas.Entities
{
    public class CompteBancaire
    {
        [Key]
        public string NumCompte { get; set; }

        public DateTime DateOuverture { get; set; }

        public double Solde { get; set; } = 1000.00;

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
