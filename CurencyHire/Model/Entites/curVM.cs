using System.ComponentModel.DataAnnotations;

namespace CurencyHire.Model.Entites
{
    public class CurVM
    {
        [Required]
        public string fromCurrency { get; set; }
        [Required]
        public string toCurrency { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}
