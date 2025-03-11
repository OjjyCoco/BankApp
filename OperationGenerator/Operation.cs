

namespace OperationGenerator
{

    public enum OperationType
    {
        RetraitDAB,
        FactureCB,
        DepotGuichet
    }

   

    public class Operation
    {
        public string NumCarte { get; set; }
        public double Montant { get; set; }
        public OperationType OperationType { get; set; }
        public DateTime Date { get; set; }
        public string Devise { get; set; }
        public decimal TauxDeChange { get; set; }

        
    }
}
