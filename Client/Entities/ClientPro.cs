using System.ComponentModel.DataAnnotations;

namespace Bank.Datas.Entities
{
    public enum StatutJuridique
    {
        SARL,
        SA,
        SAS,
        EURL
    }

    public class ClientPro : Client
    {
        [Required]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "Le SIRET doit contenir exactement 14 chiffres.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "Le SIRET doit être composé uniquement de 14 chiffres.")]
        public string Siret { get; set; }

        public StatutJuridique StatutJuridique { get; set; }

        public string AdresseSiege { get; set; }

        public string ComplementSiege { get; set; }

        public string CodePostalSiege { get; set; }

        public string VilleSiege { get; set; }
    }
}