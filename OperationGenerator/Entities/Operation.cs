using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationGenerator.Entities
{

    public enum OperationType
    {
        RetraitDAB,
        FactureCB,
        DepotGuichet
    }

   

    public class Operation
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NumCarte { get; set; }
        public double Montant { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string Devise { get; set; }
        public decimal TauxDeChange { get; set; }        
    }
}
