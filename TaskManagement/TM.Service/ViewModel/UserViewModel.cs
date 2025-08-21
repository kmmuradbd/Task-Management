using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Domain.DomainObject;

namespace TM.Service.ViewModel
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string FullName { get; set; }
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6)]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "UserRole is required.")]
        public int UserRoleMasterId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsArchived { get; set; }

        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6)]
        [Display(Name = "Old Password")]
        [Required(ErrorMessage = "Old Password is required.")]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6)]
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("Password", ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; }

        #region Audit
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        #endregion

        #region Public Method
        public User ToEntity()
        {
            User user = new User();

            user.Id = this.Id;
            user.FullName = this.FullName;
            user.Password = this.Password;
            user.UserRoleMasterId = this.UserRoleMasterId;
            user.IsActive = this.IsActive;
            user.IsArchived = this.IsArchived;
            user.FullName = this.FullName;
            user.Password = this.Password;
            user.CreatedBy = this.CreatedBy;
            user.UpdatedBy = this.UpdatedBy;
            user.CreatedDate = this.CreatedDate;
            user.UpdatedDate = this.UpdatedDate;

            return user;
        }
        #endregion
    }
}
