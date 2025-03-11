using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Datas.Entities
{
    public abstract class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Nom { get; set; }

        public string Adresse { get; set; }

        public string Complement { get; set; }

        public string CodePostal { get; set; }

        public string Ville { get; set; }

        [Required, EmailAddress]
        public string Mail { get; set; }

    }
}
