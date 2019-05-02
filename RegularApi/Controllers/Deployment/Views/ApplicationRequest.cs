using System.ComponentModel.DataAnnotations;

namespace RegularApi.Services.Deployment.Views
{
    public class ApplicationRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(250)]
        public string Name { get; set; }
        
        [Required]
        [MinLength(1)]
        [MaxLength(250)]
        public string Tag { get; set; }
    }
}