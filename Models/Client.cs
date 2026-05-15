using System.ComponentModel.DataAnnotations;

namespace TechMoveGLMS.Models
{
    public class Client
    {
        public int Id { get; set; }
        [Display(Name = "Full Names")]
        [Required(ErrorMessage = "Please enter full names")]
        public string Name { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please add valid email address")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Please add valid phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Region")]
        [Required(ErrorMessage = "Please add client region")]
        public string Region { get; set; }

        public ICollection<Contract> Contracts { get; set; }
    }
}
