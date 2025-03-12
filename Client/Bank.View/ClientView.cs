using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Bank.Datas.Entities;

namespace Bank.Views
{

    public class ClientView
    {
        public void AfficherClients(List<Client> clients)
        {
            Console.WriteLine("Liste des clients :");
            foreach (var client in clients)
            {
                Console.WriteLine($"ID: {client.Id} | Nom: {client.Nom} | Mail: {client.Mail}");

                if (client is ClientPart part)
                {
                    Console.WriteLine($"Particulier: {part.Prenom}, Né(e) le {part.DateNaissance.ToShortDateString()}");
                }
                else if (client is ClientPro pro)
                {
                    Console.WriteLine($"Professionnel: SIRET {pro.Siret}, {pro.StatutJuridique}");
                }
            }
        }
    }


}
