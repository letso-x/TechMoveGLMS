using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechMoveGLMS.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please add a description")]
        public string Description { get; set; }

        [ForeignKey("Contract")]
        public int ContractId { get; set; }

        [Required(ErrorMessage = "Please enter a cost")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public decimal Cost { get; set; }

        public ServiceRequestStatus Status { get; set; }

        public Contract? Contract { get; set; }
    }
}
