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

        public ContractStatus Status { get; set; }

        public string SignedAgreement { get; set; }

        public string ServiceLevel { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public Client Client { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}
