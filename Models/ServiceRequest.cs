using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TechMoveGLMS.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "")]
        public string Description { get; set; }

        [ForeignKey("Contract")]
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
    }
}
