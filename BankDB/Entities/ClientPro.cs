using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Data.Entities
{
    public class ClientPro : Client
    {
        public string Siret { get; set; }

        public string StatutJuridique { get; set; }

        public string AdresseSiege { get; set; }

        public string ComplementSiege { get; set; }

        public string CodePostalSiege { get; set; }

        public string VilleSiege { get; set; }
    }
}
