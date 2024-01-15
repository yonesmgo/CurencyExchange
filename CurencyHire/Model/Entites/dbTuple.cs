using System.ComponentModel.DataAnnotations;

namespace CurencyHire.Model.Entites
{
    public class DbTuple
    {
        [Required]
        public string from { get; set; }
        [Required]
        public string to { get; set; }
        [Required]
        public double rate { get; set; }
    }
}
