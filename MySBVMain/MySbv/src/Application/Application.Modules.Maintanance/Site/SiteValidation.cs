using System;
using System.Activities.Statements;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto.Product;
using Application.Dto.Site;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Domain.Serializer;
using Utility.Core;
using State = Domain.Data.Core.State;
using Task = Domain.Data.Model.Task;


namespace Application.Modules.Maintanance.Site
{
    public class SiteValidation : ISiteValidation
    {
        #region Fields

        private readonly IUserAccountValidation _userAccountValidation;
        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IAsyncRepository _asyncRepository;

        #endregion

        #region Constructor

        public SiteValidation(IRepository repository, IMapper mapper, IAsyncRepository asyncRepository, 
            ILookup lookup, IUserAccountValidation userAccountValidation)
        {
            _mapper = mapper;
            _repository = repository;
            _asyncRepository = asyncRepository;
            _lookup = lookup;
            _userAccountValidation = userAccountValidation;
        }

        #endregion

        #region ISiteValidation

        public IEnumerable<ListSiteDto> All()
        {
            var sites = _repository.Query<Domain.Data.Model.Site>(a => a.IsNotDeleted, s => s.Status, b => b.Merchant, c => c.Address);
            return sites.Select(site => _mapper.Map<Domain.Data.Model.Site, ListSiteDto>(site)).ToList();
        }

        public MethodResult<SiteDto> Find(int id)
        {
            Domain.Data.Model.Site site = _repository.Query<Domain.Data.Model.Site>(a => a.SiteId == id,
                                            m => m.Merchant,
                                            a => a.Address,
                                            a => a.Accounts,
                                            a => a.Accounts.Select(b => b.Bank),
                                            a => a.Accounts.Select(b => b.AccountType),
                                            a => a.Accounts.Select(b => b.TransactionType),
                                            a => a.Accounts.Select(b => b.Status),
                                            u => u.SiteContainers,
                                            o => o.SiteContainers.Select(a => a.ContainerType),
                                            cta => cta.SiteContainers.Select(a => a.ContainerType.ContainerTypeAttributes)
                                            ).FirstOrDefault(a => a.SiteId == id);


            if (site != null)
            {
                var mappedSite = _mapper.Map<Domain.Data.Model.Site, SiteDto>(site);
                mappedSite.ContainerTypeIds = site.SiteContainers.Where(b => b.IsNotDeleted).Select(a => a.ContainerTypeId).ToList();

                return new MethodResult<SiteDto>(MethodStatus.Successful, mappedSite);
            }
            return new MethodResult<SiteDto>(MethodStatus.Error, null, "Site Not Found.");
        }
        
        public MethodResult<int> Add(SiteDto siteDto, string username)
        {
            var userId = _userAccountValidation.UserByName(username).UserId;

            var address = new Address
            {
                AddressTypeId = 2,
                AddressLine1 = siteDto.Address.AddressLine1,
                AddressLine2 = siteDto.Address.AddressLine2,
                AddressLine3 = siteDto.Address.AddressLine3,
                PostalCode = siteDto.Address.PostalCode,
                Latitude = siteDto.Address.Latitude,
                Longitude = siteDto.Address.Longitude,
                LastChangedById = userId,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = userId,
                IsNotDeleted = true,
                EntityState = State.Added
            };

            var mappedSite = _mapper.Map<SiteDto, Domain.Data.Model.Site>(siteDto);
            mappedSite.Address = address;

            mappedSite.CreatedById = userId;
            mappedSite.LastChangedById = userId;
            mappedSite.StatusId = _lookup.GetStatusId("ACTIVE"); // Saved 

            var result = _repository.Add(mappedSite) > 0;

            //Add ContainerTypes
            if (result)
            {
                siteDto.SiteId = mappedSite.SiteId;
                AddNewSiteContainers(siteDto, mappedSite, userId);
            }

            return result ? new MethodResult<int>(MethodStatus.Successful, mappedSite.SiteId) :
                new MethodResult<int>(MethodStatus.Error, 0, "Site Not added. Server Error.");
        }


        private void AddNewSiteContainers(SiteDto siteDto, Domain.Data.Model.Site mappedSite, int loggedInUserId)
        {
          
            foreach (int containerTypeId in siteDto.ContainerTypeIds)
            {
                var siteId = mappedSite.SiteId;
                var date = DateTime.Now;

                SiteContainerDto siteContainerDto = new SiteContainerDto
                {
                    SiteId = siteId,
                    ContainerTypeId = containerTypeId,
                };

                var mappedSiteContainer = _mapper.Map<SiteContainerDto, SiteContainer>(siteContainerDto);
                ((IEntity)mappedSiteContainer).WalkObjectGraph(o =>
                {
                    o.CreatedById = loggedInUserId;
                    o.CreateDate = date;
                    o.LastChangedById = loggedInUserId;
                    o.LastChangedDate = date;
                    o.IsNotDeleted = true;
                    o.EntityState = State.Added;
                    return false;
                }, a => { });

                if (_repository != null) _repository.Add(mappedSiteContainer);
            }
        }


        private static void AddNewSiteContainersOnUpdate(IEnumerable<int> added, Domain.Data.Model.Site mappedSite, int loggedInUserId)
        {
            foreach (int containerTypeId in added)
            {
                mappedSite.SiteContainers.Add(new SiteContainer
                {
                    ContainerTypeId = containerTypeId,
                    SiteId = mappedSite.SiteId,
                    IsNotDeleted = true,
                    LastChangedById = loggedInUserId,
                    CreatedById = loggedInUserId,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    EntityState = State.Added
                });
            }
        }

        private static void MarkRemovedSiteContainers(IEnumerable<int> removed, Domain.Data.Model.Site mappedSite)
        {
            foreach (int i in removed)
            {
                mappedSite.SiteContainers.FirstOrDefault(a => a.SiteId == mappedSite.SiteId && a.ContainerTypeId == i).IsNotDeleted = false;
                mappedSite.SiteContainers.FirstOrDefault(a => a.SiteId == mappedSite.SiteId && a.ContainerTypeId == i).EntityState = State.Modified;
            }
        }
        
        public MethodResult<int> Submit(SiteDto siteDto, string username)
        {
            var userId = _userAccountValidation.UserByName(username).UserId;

            var address = new Address
            {
                AddressTypeId = 2,
                AddressLine1 = siteDto.Address.AddressLine1,
                AddressLine2 = siteDto.Address.AddressLine2,
                AddressLine3 = siteDto.Address.AddressLine3,
                PostalCode = siteDto.Address.PostalCode,
                Latitude = siteDto.Address.Latitude,
                Longitude = siteDto.Address.Longitude,
                LastChangedById = userId,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = userId,
                IsNotDeleted = true,
                EntityState = State.Added
            };

            var mappedSite = _mapper.Map<SiteDto, Domain.Data.Model.Site>(siteDto);
            mappedSite.Address = address;

            mappedSite.CreatedById = userId;
            mappedSite.CreateDate = DateTime.Now;
            mappedSite.LastChangedById = userId;
            mappedSite.LastChangedDate = DateTime.Now;
            mappedSite.StatusId = _lookup.GetStatusId("ACTIVE"); // Saved 

            var result = _repository.Add(mappedSite) > 0;

            //Add ContainerTypes
            if (result)
            {
                siteDto.SiteId = mappedSite.SiteId;
                AddNewSiteContainers(siteDto, mappedSite, userId);
            }

            return result ? new MethodResult<int>(MethodStatus.Successful, mappedSite.SiteId) :
                new MethodResult<int>(MethodStatus.Error, 0, "Site Not added. Server Error.");
        }

        public MethodResult<bool> Edit(SiteDto siteDto, string username)
        {
            //Get the logged on user id 
            var userId = _userAccountValidation.UserByName(username).UserId;
            Domain.Data.Model.Site tmp = _repository.Query<Domain.Data.Model.Site>(a => a.SiteId == siteDto.SiteId,
                                            m => m.Merchant,
                                            a => a.Address,
                                            u => u.SiteContainers).FirstOrDefault(a => a.SiteId == siteDto.SiteId);

            if (tmp == null)
                return new MethodResult<bool>(MethodStatus.Error, false, "Error, Site Not Found.");

            var address = new Address
            {
                AddressTypeId = 2,
                AddressLine1 = siteDto.Address.AddressLine1,
                AddressLine2 = siteDto.Address.AddressLine2,
                AddressLine3 = siteDto.Address.AddressLine3,
                PostalCode = siteDto.Address.PostalCode,
                Latitude = siteDto.Address.Latitude,
                Longitude = siteDto.Address.Longitude,
                LastChangedById = userId,
                LastChangedDate = DateTime.Now,
                IsNotDeleted = true,
                EntityState = State.Added
            };
            
            var site = _mapper.Map<SiteDto, Domain.Data.Model.Site>(siteDto);
            site.SiteContainers = tmp.SiteContainers;
            site.Address = address;
            site.StatusId = _lookup.GetStatusId("ACTIVE");

            IEnumerable<int> added = siteDto.ContainerTypeIds.Except(tmp.SiteContainers.Select(a => a.ContainerTypeId));
            IEnumerable<int> removed = site.SiteContainers.Select(a => a.ContainerTypeId).Except(siteDto.ContainerTypeIds.Select(a => a));

            AddNewSiteContainersOnUpdate(added, site, userId);
            MarkRemovedSiteContainers(removed, site);

            ((IEntity)site).WalkObjectGraph(o =>
            {
                site.CreateDate = tmp.CreateDate;
                site.CreatedById = tmp.CreatedById;
                site.LastChangedById = userId;
                site.LastChangedDate = DateTime.Now;
                site.IsNotDeleted = true;
                site.EntityState = State.Modified;
                return false;
            }, o => { });

            foreach (var siteContainer in site.SiteContainers)
            {
                siteContainer.Site = null;
            }

            return (_repository.Update(site) > 0)
                            ? new MethodResult<bool>(MethodStatus.Successful, true, "Site was successfully Updated.")
                            : new MethodResult<bool>(MethodStatus.Error, false, "Site was not successfully Updated.");
        }

        public MethodResult<Task> Delete(int id, string username)
        {
            //Get the logged on user 
            var userId = _userAccountValidation.UserByName(username).UserId;
            var site = _repository.Query<Domain.Data.Model.Site>(a => a.SiteId == id).FirstOrDefault();

            if (site != null)
            {
                int addressId = site.AddressId;
                if (_repository.Delete<Domain.Data.Model.Site>(id, userId) > 0 &&
                    _repository.Delete<Address>(addressId, userId) > 0)
                {
                    return new MethodResult<Task>(MethodStatus.Successful, new Task(),
                        "Site was successfully deleted.");
                }
            }
            return new MethodResult<Task>(MethodStatus.Error, null, "Site was not deleted successfully.");
        }


        public void DeleteSiteContainers(int siteId, string username)
        {
            int userId = _userAccountValidation.UserByName(username).UserId;

            var siteContainers = _repository.Query<SiteContainer>(a => a.SiteId == siteId)
                       .Select(a => _mapper.Map<SiteContainer, SiteContainerDto>(a))
                       .ToList();

            foreach (var siteContainer in siteContainers)
            {
                SiteContainerDto siteContainerDto = new SiteContainerDto
                {
                    SiteId = siteId,
                    ContainerTypeId = siteContainer.ContainerTypeId,
                };

                SiteContainer mappedProductFee = _mapper.Map<SiteContainerDto, SiteContainer>(siteContainerDto);
                mappedProductFee.EntityState = State.Modified;
                mappedProductFee.LastChangedById = userId;
                mappedProductFee.IsNotDeleted = false;
                mappedProductFee.LastChangedDate = DateTime.Now;

                _repository.Update(mappedProductFee);
            }
        }


        public bool CitCodeExists(string citCode)
        {
            return _repository.All<Domain.Data.Model.Site>().Any(o => o.CitCode.ToLower() == citCode.ToLower() && o.IsNotDeleted);
        }

        public bool SiteNameExists(string siteName)
        {
            return _repository.All<Domain.Data.Model.Site>().Any(o => o.Name.ToLower() == siteName.ToLower() && o.IsNotDeleted);
        }

        public bool IsThereAccount(int siteId)
        {
            return _repository.All<Account>().Any(o => o.SiteId == siteId && o.IsNotDeleted);
        }

        public bool IsThereMySbvProduct(int siteId)
        {
            return _repository.All<Domain.Data.Model.Product>().Any(o => o.SiteId == siteId && o.IsNotDeleted);
        }

        public string GetAllHeadOfficeUsersEmailAddress()
        {
            var users = _repository.All<User>().Where(e => e.UserType != null);
            return users.Where(e => e.UserType.Name == "HeadOfficeUser").Select(user => _mapper.Map<User, User>(user)).Aggregate("", (current, item) => current + item.EmailAddress);
        }

        public MethodResult<bool> IsRejected(RejectSiteArgumentsDto rejectParameters, SiteDto siteDto, string username)
        {
            int userId = _userAccountValidation.UserByName(username).UserId;
            var mappedSite = _mapper.Map<SiteDto, Domain.Data.Model.Site>(siteDto);

            mappedSite.Comments = rejectParameters.Comments;
            mappedSite.LastChangedById = userId;
            mappedSite.LastChangedDate = DateTime.Now;
            mappedSite.EntityState = State.Modified;

            var result = _repository.Update(mappedSite) > 0;
            return result ? new MethodResult<bool>(MethodStatus.Successful, true, "Site was successfully. Rejected") :
                new MethodResult<bool>(MethodStatus.Error, false, "Site Not Rejected. Server Error");
        }

        #endregion

        #region Helpers

        public MethodResult<bool> IsInUse(int id)
        {
            return _repository.Any<Account>(a => a.SiteId == id && a.IsNotDeleted) || _repository.Any<Domain.Data.Model.Product>(a => a.SiteId == id && a.IsNotDeleted) ?
                new MethodResult<bool>(MethodStatus.Error, true, "Cannot delete a site that is linked to an active account or device.") :
                new MethodResult<bool>(MethodStatus.Successful);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<List<SiteDto>> GetSitesAsync(int merchantId)
        {
            var sites = await _asyncRepository.GetAsync<Domain.Data.Model.Site>(e => e.MerchantId == merchantId);

            var collection = new List<SiteDto>();
            foreach (var site in sites)
            {
                var mapped = _mapper.Map<Domain.Data.Model.Site, SiteDto>(site);
                collection.Add(mapped);
            }
            return collection;
        }


        public bool IsSiteNameInUse(string name)
        {
            return _repository.Any<Domain.Data.Model.Site>(a => a.Name.ToLower() == name.ToLower());
        }

        public bool NameUsedByAnotherSite(string name, int id)
        {
            return _repository.Any<Domain.Data.Model.Site>(a => a.Name.ToLower() == name.ToLower() && a.SiteId != id);
        }

        public bool CitCodeUsedByAnotherSite(string citCode, int id)
        {
            return _repository.All<Domain.Data.Model.Site>().Any(o => o.CitCode.ToLower() == citCode.ToLower() && o.SiteId != id);
        }


        public MethodResult<Task> Delete(int id, int addressId)
        {
            throw new NotImplementedException();
        }

        #endregion



    }
}
