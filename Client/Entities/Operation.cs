using Bank.Datas.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Operation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Type { get; set; }

    public string NumCarte { get; set; }

    public double Montant { get; set; }

    public DateTime Date { get; set; }

    [ForeignKey("CompteBancaire")]
    public string NumCompte { get; set; }
    public CompteBancaire CompteBancaire { get; set; }
}