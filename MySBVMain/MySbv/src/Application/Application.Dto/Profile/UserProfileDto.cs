using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.Dto.ValidationAttributes;

namespace Application.Dto.Profile
{
    public class UserProfileDto
    {
        #region User Information

        public int UserId { get; set; }

        public int UserTypeId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "UserName")]
        [RegularExpression(@"^[a-zA-Z0-9._-]*$", ErrorMessage = "Invalid Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Title")]
        [DropDownValidation(ErrorMessage = "Title must be selected")]
        public int TitleId { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        [MaxLength(20, ErrorMessage = "Maximum First name length cannot be more that 20 characters")]
        [RegularExpression(@"^[a-zA-Z _-]*$", ErrorMessage = "Invalid First Name.")]
        [MinLength(2, ErrorMessage = "First name cannot be less than 2 characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        [MaxLength(20, ErrorMessage = "Maximum Last name length cannot be more that 20 characters")]
        [RegularExpression(@"^[a-zA-Z _-]*$", ErrorMessage = "Invalid Last Name.")]
        [MinLength(2, ErrorMessage = "Last name cannot be less than 2 characters")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [RegularExpression(
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Identity Number")]
        [IsValidId(ErrorMessage = "Invalid SA Identity Number")]
        public string IdNumber { get; set; }

        [Display(Name = "Passport Number")]
        [RegularExpression(@"^[0-9a-zA-Z]*$", ErrorMessage = "Passport Number cannot include special characters.")]
        public string PassportNumber { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Office Number")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Office number must be [0000000000]")]
        [MaxLength(10, ErrorMessage = "Office Number must be 10 characters long")]
        public string OfficeNumber { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Cell Number")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Cell number must be [0000000000]")]
        [MaxLength(10, ErrorMessage = "Cell Number must be 10 characters long")]
        public string CellNumber { get; set; }

        [Display(Name = "Fax Number")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Invalid  Fax Number")]
        [MinLength(10, ErrorMessage = "Fax Number must be 10 characters long")]
        public string FaxNumber { get; set; }

        #endregion

        #region SBV Role Information

        [Display(Name = "Teller")]
        public bool IsTeller { get; set; }

        [Display(Name = "Teller Supervisor")]
        public bool IsTellerSupervisor { get; set; }

        [Display(Name = "Approver")]
        public bool IsApprover { get; set; }

        [Display(Name = "Recon")]
        public bool IsRecon { get; set; }

        [Display(Name = "Finance Reviewer")]
        public bool IsFinanceReviewer { get; set; }

        [Display(Name = "Admin")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Data Capture")]
        public bool IsDataCapture { get; set; }

        #endregion

        #region Retail User Role Infomation

        [Display(Name = "User")]
        public bool IsUser { get; set; }

        [Display(Name = "Viewer")]
        public bool IsViewer { get; set; }

        [Display(Name = "Supervisor")]
        public bool IsSupervisor { get; set; }

        #endregion

        #region Account Infomation

        public string CashCenterName { get; set; }

        public string LockedStatus { get; set; }

        #endregion

        #region Notification Types

        [Display(Name = "Email")]
        public bool IsEmailNotificationType { get; set; }

        [Display(Name = "SMS")]
        public bool IsSmsNotificationType { get; set; }

        [Display(Name = "FAX")]
        public bool IsFaxNotificationType { get; set; }

        #endregion

        #region Merchant Information

        public int? MerchantId { get; set; }

        public int? CashCentreId { get; set; }

        public string MerchantName { get; set; }

        public List<int> SiteIds { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreatedById { get; set; }

        public int LastChangedById { get; set; }

        public bool ActiveStatus { get; set; }

        public bool CanMakeVaultPayment { get; set; }

        #endregion
    }
}