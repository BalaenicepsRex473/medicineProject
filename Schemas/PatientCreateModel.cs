using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;


namespace scrubsAPI
{
    public class PatientCreateModel
    {
        [Required][MinLength(1), MaxLength(1000)]public string name { get; set; }
        public DateTime? birthday { get; set; }
        [Required] public Gender gender { get; set; }
    }
}
