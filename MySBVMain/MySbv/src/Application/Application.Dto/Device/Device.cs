using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Device
{
    public class DeviceDto
    {
        public int DeviceId { get; set; }

        public int? DeviceTypeId { get; set; }
        
        [Display(Name = "Device Name")]
        public string Name { get; set; }

        [Display(Name = "Device Serial Number")]
        public string SerialNumber { get; set; }

        public string Description { get; set; }
        public string LookUpKey { get; set; }

        public bool IsActive { get; set; }
    }
}
