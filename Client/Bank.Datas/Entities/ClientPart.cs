using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Datas.Entities
{
    public class ClientPart : Client
    {
        public DateTime DateNaissance { get; set; }

        public string Prenom { get; set; }

        public char Sexe { get; set; }
    }
}
