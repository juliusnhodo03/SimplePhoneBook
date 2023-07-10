using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Cluster
{
    public class ClusterDto
    {
        [ScaffoldColumn(false)]
        public int ClusterId { get; set; }

        public int RegionManagerId { get; set; }

        [Required(ErrorMessage = "Cluster Name is missing")]
        [StringLength(20, ErrorMessage = "Cluster Name cannot be longer than 20 characters.")]
        [Display(Name = "Cluster Name")]
        public string Name { get; set; }
        
        [Display(Name = "Cluster Description")]
        public string Description { get; set; }
        
    }
}