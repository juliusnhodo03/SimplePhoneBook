using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto.Site;
using Utility.Core;
using Task = Domain.Data.Model.Task;

namespace Application.Modules.Maintanance.Site
{
    public interface ISiteValidation
    {
        /// <summary>
        /// Return a list of all Site
        /// </summary>
        /// <returns></returns>
        IEnumerable<ListSiteDto> All();

        /// <summary>
        /// Check if CitCode already Exist in the system
        /// </summary>
        /// <returns></returns>
        bool CitCodeExists(string citCode);

        /// <summary>
        /// Check if SiteName used by another Site in the system
        /// </summary>
        ///  /// <param name="citCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CitCodeUsedByAnotherSite(string citCode, int id);

        /// <summary>
        /// Check if SiteName already Exist in the system
        /// </summary>
        /// <returns></returns>
        bool SiteNameExists(string siteName);

        /// <summary>
        /// Check if SiteName used by another Site in the system
        /// </summary>
        ///  /// <param name="siteName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool NameUsedByAnotherSite(string siteName, int id);

        /// <summary>
        /// Check if there is a an Account for this site 
        /// before it could be send for approval
        /// </summary>
        /// <returns></returns>
        bool IsThereAccount(int siteId);

        /// <summary>
        /// Check if there is a a Product for this site 
        /// before it could be send for approval
        /// </summary>
        /// <returns></returns>
        bool IsThereMySbvProduct(int siteId);

        /// <summary>
        /// Add A New Site
        /// </summary>
        /// <param name="siteDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<int> Add(SiteDto siteDto, string username);

        /// <summary>
        /// Submit a Site for approval
        /// </summary>
        /// <param name="siteDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<int> Submit(SiteDto siteDto, string username);

        /// <summary>
        /// Update a new Site
        /// </summary>
        /// <param name="siteDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(SiteDto siteDto, string username);

        /// <summary>
        /// Delete a Site
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<Task> Delete(int id, string username);

        /// <summary>
        /// Find a Site by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<SiteDto> Find(int id);

        /// <summary>
        /// Get Current Approval User Details
        /// </summary>
        /// <returns></returns>
        //User GetCurrentApprovalUser(string username);

        /// <summary>
        /// Get all HeadOffice Users Email Address
        /// </summary>
        /// <returns></returns>
        string GetAllHeadOfficeUsersEmailAddress();

        /// <summary>
        /// Reject Site Approval, specifying the Comment on 
        /// why the Site has been Rejected
        /// </summary>
        /// <returns></returns>
        MethodResult<bool> IsRejected(RejectSiteArgumentsDto rejectParameters, SiteDto siteDeto, string username);

        /// <summary>
        /// Check if the site is in Use
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<bool> IsInUse(int id);

        Task<List<SiteDto>> GetSitesAsync(int merchantId);
    }
}