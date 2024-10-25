using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace scrubsAPI
{
    public class DoctorEditModel
    {
        [Required] public string email { get; set; }
        [Required] public string name { get; set; }
        public DateTime? birthday { get; set; }
        [Required] public Gender gender { get; set; }
        public string? phone { get; set; }
    }
}
