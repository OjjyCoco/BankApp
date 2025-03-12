using System.ComponentModel.DataAnnotations;

namespace Bank.Datas.Entities
{
    public enum Sexe
    {
        M = 'M',
        F = 'F'
    }

    public class ClientPart : Client
    {
        public DateTime DateNaissance { get; set; }

        [Required, MaxLength(50)]
        public string Prenom { get; set; }

        public Sexe Sexe { get; set; }
    }
}