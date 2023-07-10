using System.ComponentModel.DataAnnotations;

namespace Application.Dto.SalesArea
{
    public class SalesAreaDto
    {
        [ScaffoldColumn(false)]
        public int SalesAreaId { get; set; }

        [Required(ErrorMessage = "Area Name is missing")]
        [Display(Name = "Area Name")]
        public string Name { get; set; }

        [Display(Name = "Area Description")]
        public string Description { get; set; }
    }
}