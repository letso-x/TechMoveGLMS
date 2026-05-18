using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TechMoveGLMS.Models.ContractStatus;

namespace TechMoveGLMS.Models
{
    public class Contract
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please input valid start date")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please input valid end date")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Please select a status")]
        public ContractStatus Status { get; set; }

       
        public string? SignedAgreement { get; set; }

        [Required(ErrorMessage = "Please enter service level")]
        public string ServiceLevel { get; set; }

        [Required(ErrorMessage = "Please select a client")]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public Client? Client { get; set; }

        public ICollection<ServiceRequest>? ServiceRequests { get; set; }
    }
}
