using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class UserSite : EntityBase
    {
        #region Mapped

        public int UserSiteId { get; set; }
        public int UserId { get; set; }
        public int SiteId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("SiteId")]
        public virtual Site Site { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return UserSiteId; }
        }

        public string UserDetails
        {
            get { return User != null ? User.FirstName + " " + User.LastName : string.Empty; }
        }

        public string Name
        {
            get { return Site != null ? Site.Name : string.Empty; }
        }

        #endregion
    }
}