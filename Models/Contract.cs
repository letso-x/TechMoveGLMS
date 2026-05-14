using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMoveGLMS.Models
{
    public class Contract
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string  ContractStatus Status { get; set; }

        public string AgreementFilePath { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public Client Client { get; set; }

        public ICollection<ServiceRequest> ServiceRequests { get; set; }
    }
}
