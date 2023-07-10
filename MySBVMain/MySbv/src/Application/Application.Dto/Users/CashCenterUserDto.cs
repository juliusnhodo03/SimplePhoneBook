using System;
using System.ComponentModel.DataAnnotations;
using Application.Dto.ValidationAttributes;

namespace Application.Dto.Users
{
    public class CashCenterUserDto
    {
       
        [Display(Name = "Cash Centre")]
        [DropDownValidation(ErrorMessage = "Select Cash Centre")]
        public int CashCenterId { get; set; }

        public string CashCenterName { get; set; }

        public UserDto UserDto { get; set; }

    }
}