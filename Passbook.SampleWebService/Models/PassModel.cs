using System.ComponentModel.DataAnnotations;

namespace Passbook.SampleWebService.Models
{
    public class PassModel
    {
        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Secret { get; set; }
    }
}
