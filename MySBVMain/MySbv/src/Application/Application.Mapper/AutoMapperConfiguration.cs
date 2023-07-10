using System;
using System.Linq;
using Application.Dto.Account;
using Application.Dto.Address;
using Application.Dto.Bank;
using Application.Dto.Carrier;
using Application.Dto.CashCenter;
using Application.Dto.CashDeposit;
using Application.Dto.CashOrder;
using Application.Dto.CashProcessing;
using Application.Dto.Cluster;
using Application.Dto.Product;
using Application.Dto.Device;
using Application.Dto.DeviceType;
using Application.Dto.Merchant;
using Application.Dto.Profile;
using Application.Dto.SalesArea;
using Application.Dto.Site;
using Application.Dto.CashOrderTask;
using Application.Dto.Task;
using Application.Dto.Users;
using Application.Dto.FailedVaultTransaction;
using Domain.Data.Model;
using ProductTypeDto = Application.Dto.ProductType.ProductTypeDto;
using Application.Dto.Vault;
using AutoMapper;

namespace Application.Mapper
{
    public class AutoMapperConfigProfile
    {
        public AutoMapperConfigProfile()
        {
            // Cash Ordering
            AutoMapper.Mapper.CreateMap<CashOrderDto, CashOrder>().ReverseMap();
            AutoMapper.Mapper.CreateMap<CashOrderTypeDto, CashOrderType>().ReverseMap();
            AutoMapper.Mapper.CreateMap<CashOrderContainerDto, CashOrderContainer>().ReverseMap();
            AutoMapper.Mapper.CreateMap<CashOrderContainerDropDto, CashOrderContainerDrop>().ReverseMap();
            AutoMapper.Mapper.CreateMap<CashOrderContainerDropItemDto, CashOrderContainerDropItem>().ReverseMap();
            AutoMapper.Mapper.CreateMap<DenominationDto, Denomination>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Status, StatusDto>().ReverseMap();

            AutoMapper.Mapper.CreateMap<VaultContainerDto, VaultContainer>()
                .ForMember(dest => dest.VaultContainerId, src => src.MapFrom(o => o.VaultContainerId))
                .ForMember(dest => dest.ContainerId, src => src.MapFrom(o => o.ContainerId))
                .ForMember(dest => dest.CashDepositId, src => src.MapFrom(o => o.CashDepositId))
                .ForMember(dest => dest.SiteId, src => src.MapFrom(o => o.SiteId))
                .ForMember(dest => dest.SupervisorId, src => src.MapFrom(o => o.SupervisorId))
                .ForMember(dest => dest.CitCode, src => src.MapFrom(o => o.CitCode))
                .ForMember(dest => dest.DeviceId, src => src.MapFrom(o => o.DeviceId))
                .ForMember(dest => dest.DiscrepancyReasonId, src => src.MapFrom(o => o.DiscrepancyReasonId))
                .ForMember(dest => dest.SerialNumber, src => src.MapFrom(o => o.SerialNumber))
                .ForMember(dest => dest.Comment, src => src.MapFrom(o => o.Comment))
                .ForMember(dest => dest.Amount, src => src.MapFrom(o => o.Amount))
                .ForMember(dest => dest.ActualAmount, src => src.MapFrom(o => o.ActualAmount))
                .ForMember(dest => dest.DiscrepancyAmount, src => src.MapFrom(o => o.DiscrepancyAmount))
                .ForMember(dest => dest.HasDiscrepancy, src => src.MapFrom(o => o.HasDiscrepancy))
                .ForMember(dest => dest.ProcessedById, src => src.MapFrom(o => o.ProcessedById))
                .ForMember(dest => dest.ProcessedDateTime, src => src.MapFrom(o => o.ProcessedDateTime))
                .ForMember(dest => dest.VaultContainerDrops, src => src.MapFrom(o => o.VaultContainerDrops));


            AutoMapper.Mapper.CreateMap<VaultContainerDropDto, VaultContainerDrop>()
                .ForMember(dest => dest.VaultContainerId, src => src.MapFrom(o => o.VaultContainerId))
                .ForMember(dest => dest.DenominationId, src => src.MapFrom(o => o.DenominationId))
                .ForMember(dest => dest.ValueInCents, src => src.MapFrom(o => o.ValueInCents))
                .ForMember(dest => dest.Count, src => src.MapFrom(o => o.Count))
                .ForMember(dest => dest.Value, src => src.MapFrom(o => o.Value))
                .ForMember(dest => dest.DiscrepancyCount, src => src.MapFrom(o => o.DiscrepancyCount))
                .ForMember(dest => dest.DiscrepancyValue, src => src.MapFrom(o => o.DiscrepancyValue))
                .ForMember(dest => dest.ActualCount, src => src.MapFrom(o => o.ActualCount))
                .ForMember(dest => dest.ActualValue, src => src.MapFrom(o => o.ActualValue))
                .ForMember(dest => dest.HasDiscrepancy, src => src.MapFrom(o => o.HasDiscrepancy))
                .ForMember(dest => dest.DenominationType, src => src.MapFrom(o => o.DenominationType));

            AutoMapper.Mapper.CreateMap<CashOrder, ListCashOrderDto>()
                .ForMember(dest => dest.CashOrderId, src => src.MapFrom(o => o.CashOrderId))
                .ForMember(dest => dest.ReferenceNumber, src => src.MapFrom(o => o.ReferenceNumber))
                .ForMember(dest => dest.MerchantName, src => src.MapFrom(o => o.Site.Merchant.Name))
                .ForMember(dest => dest.SiteName, src => src.MapFrom(o => o.SiteName))
                .ForMember(dest => dest.CitCode, src => src.MapFrom(o => o.Site.CitCode))
                .ForMember(dest => dest.DeliveryDate, src => src.MapFrom(o => o.DeliveryDate))
                .ForMember(dest => dest.BagNumber, src => src.MapFrom(o => o.ContainerNumberWithCashForExchange))
                .ForMember(dest => dest.OrderType, src => src.MapFrom(o => o.CashOrderType.Name))
                .ForMember(dest => dest.CashOrderAmount, src => src.MapFrom(o => o.CashOrderAmount)).ReverseMap();

            AutoMapper.Mapper.CreateMap<AddressDto, Address>().ReverseMap();

            AutoMapper.Mapper.CreateMap<AddressTypeDto, AddressType>().ReverseMap();
            AutoMapper.Mapper.CreateMap<UserDto, User>()
                .ForMember(dest => dest.LockedStatus, o => o.MapFrom(src => src.IsLocked));

            AutoMapper.Mapper.CreateMap<BankDto, Bank>().ReverseMap();
            AutoMapper.Mapper.CreateMap<CitCarrierDto, CitCarrier>().ReverseMap();
            AutoMapper.Mapper.CreateMap<ClusterDto, Cluster>().ReverseMap();

            AutoMapper.Mapper.CreateMap<SalesAreaDto, SalesArea>().ReverseMap();

            AutoMapper.Mapper.CreateMap<UserSiteDto, UserSite>().ReverseMap();

            AutoMapper.Mapper.CreateMap<User, UserDto>()
                .ForMember(dest => dest.Password, o => o.MapFrom(src => "pass#123"))
                .ForMember(dest => dest.ConfirmPassword, o => o.MapFrom(src => "pass#123"))
                .ForMember(dest => dest.IsLocked, src => src.MapFrom(o => o.LockedStatus)).ReverseMap();

            AutoMapper.Mapper.CreateMap<User, MerchantUserDto>()
                .ForMember(dest => dest.MerchantId, o => o.MapFrom(src => src.MerchantId))
                .ForMember(dest => dest.MerchantNumber, o => o.MapFrom(src => src.Merchant.Number))
                .ForMember(dest => dest.UserSites, o => o.MapFrom(src => src.UserSites))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Merchant.Name))
                .ForMember(dest => dest.CanMakeVaultPayment, o => o.MapFrom(src => src.CanMakeVaultPayment))
                .ForMember(dest => dest.UserDto, o => o.MapFrom(src => src));

            AutoMapper.Mapper.CreateMap<User, CashCenterUserDto>()
                .ForMember(dest => dest.CashCenterId, o => o.MapFrom(src => src.CashCenterId))
                .ForMember(dest => dest.CashCenterName, o => o.MapFrom(src => src.CashCenterName))
                .ForMember(dest => dest.UserDto, o => o.MapFrom(src => src));

            AutoMapper.Mapper.CreateMap<CashCenterUserDto, User>()
                .ForMember(dest => dest.UserId, o => o.MapFrom(src => src.UserDto.UserId))
                .ForMember(dest => dest.TitleId, o => o.MapFrom(src => src.UserDto.TitleId))
                .ForMember(dest => dest.CashCenterId, o => o.MapFrom(src => src.CashCenterId))
                .ForMember(dest => dest.UserTypeId, o => o.MapFrom(src => src.UserDto.UserTypeId))
                .ForMember(dest => dest.UserName, o => o.MapFrom(src => src.UserDto.UserName))
                .ForMember(dest => dest.FirstName, o => o.MapFrom(src => src.UserDto.FirstName))
                .ForMember(dest => dest.LastName, o => o.MapFrom(src => src.UserDto.LastName))
                .ForMember(dest => dest.IdNumber, o => o.MapFrom(src => src.UserDto.IdNumber))
                .ForMember(dest => dest.PassportNumber, o => o.MapFrom(src => src.UserDto.PassportNumber))
                .ForMember(dest => dest.EmailAddress, o => o.MapFrom(src => src.UserDto.EmailAddress))
                .ForMember(dest => dest.CellNumber, o => o.MapFrom(src => src.UserDto.CellNumber))
                .ForMember(dest => dest.FaxNumber, o => o.MapFrom(src => src.UserDto.FaxNumber))
                .ForMember(dest => dest.OfficeNumber, o => o.MapFrom(src => src.UserDto.OfficeNumber))
                .ForMember(dest => dest.LockedStatus, o => o.MapFrom(src => src.UserDto.IsLocked))
                .ForMember(dest => dest.IsNotDeleted, o => o.MapFrom(src => src.UserDto.IsNotDeleted))
                .ForMember(dest => dest.CreatedById, o => o.MapFrom(src => src.UserDto.CreatedById))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.UserDto.CreateDate));

            AutoMapper.Mapper.CreateMap<MerchantUserDto, User>()
                .ForMember(dest => dest.UserId, o => o.MapFrom(src => src.UserDto.UserId))
                .ForMember(dest => dest.TitleId, o => o.MapFrom(src => src.UserDto.TitleId))
                .ForMember(dest => dest.MerchantId, o => o.MapFrom(src => src.MerchantId))
                .ForMember(dest => dest.UserTypeId, o => o.MapFrom(src => src.UserDto.UserTypeId))
                .ForMember(dest => dest.UserName, o => o.MapFrom(src => src.UserDto.UserName))
                .ForMember(dest => dest.FirstName, o => o.MapFrom(src => src.UserDto.FirstName))
                .ForMember(dest => dest.LastName, o => o.MapFrom(src => src.UserDto.LastName))
                .ForMember(dest => dest.IdNumber, o => o.MapFrom(src => src.UserDto.IdNumber))
                .ForMember(dest => dest.PassportNumber, o => o.MapFrom(src => src.UserDto.PassportNumber))
                .ForMember(dest => dest.EmailAddress, o => o.MapFrom(src => src.UserDto.EmailAddress))
                .ForMember(dest => dest.CellNumber, o => o.MapFrom(src => src.UserDto.CellNumber))
                .ForMember(dest => dest.FaxNumber, o => o.MapFrom(src => src.UserDto.FaxNumber))
                .ForMember(dest => dest.OfficeNumber, o => o.MapFrom(src => src.UserDto.OfficeNumber))
                .ForMember(dest => dest.LockedStatus, o => o.MapFrom(src => src.UserDto.IsLocked))
                .ForMember(dest => dest.IsNotDeleted, o => o.MapFrom(src => src.UserDto.IsNotDeleted))
                .ForMember(dest => dest.CreatedById, o => o.MapFrom(src => src.UserDto.CreatedById))
                .ForMember(dest => dest.CanMakeVaultPayment, o => o.MapFrom(src => src.CanMakeVaultPayment))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.UserDto.CreateDate));

            AutoMapper.Mapper.CreateMap<User, UserProfileDto>()
                .ForMember(dest => dest.UserId, o => o.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, o => o.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserTypeId, o => o.MapFrom(src => src.UserTypeId))
                .ForMember(dest => dest.TitleId, o => o.MapFrom(src => src.TitleId))
                .ForMember(dest => dest.FirstName, o => o.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, o => o.MapFrom(src => src.LastName))
                .ForMember(dest => dest.EmailAddress, o => o.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.IdNumber, o => o.MapFrom(src => src.IdNumber))
                .ForMember(dest => dest.PassportNumber, o => o.MapFrom(src => src.PassportNumber))
                .ForMember(dest => dest.OfficeNumber, o => o.MapFrom(src => src.OfficeNumber))
                .ForMember(dest => dest.CellNumber, o => o.MapFrom(src => src.CellNumber))
                .ForMember(dest => dest.FaxNumber, o => o.MapFrom(src => src.FaxNumber))
                .ForMember(dest => dest.CashCenterName, o => o.MapFrom(src => src.CashCenterName))
                .ForMember(dest => dest.LockedStatus, o => o.MapFrom(src => src.LockedStatus))
                .ForMember(dest => dest.SiteIds, o => o.MapFrom(src => src.UserSites.Select(a => a.SiteId)))
                .ForMember(dest => dest.MerchantId, o => o.MapFrom(src => src.MerchantId))
                .ForMember(dest => dest.CashCentreId, o => o.MapFrom(src => src.CashCenterId))
                .ForMember(dest => dest.CreatedById, o => o.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.ActiveStatus, o => o.MapFrom(src => src.IsNotDeleted))
                .ForMember(dest => dest.CanMakeVaultPayment, o => o.MapFrom(src => src.CanMakeVaultPayment))
                .ForMember(dest => dest.MerchantName, o => o.MapFrom(src => src.Merchant.Name))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.CreateDate));

            AutoMapper.Mapper.CreateMap<UserProfileDto, User>()
                .ForMember(dest => dest.UserId, o => o.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, o => o.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserTypeId, o => o.MapFrom(src => src.UserTypeId))
                .ForMember(dest => dest.TitleId, o => o.MapFrom(src => src.TitleId))
                .ForMember(dest => dest.FirstName, o => o.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, o => o.MapFrom(src => src.LastName))
                .ForMember(dest => dest.EmailAddress, o => o.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.IdNumber, o => o.MapFrom(src => src.IdNumber))
                .ForMember(dest => dest.PassportNumber, o => o.MapFrom(src => src.PassportNumber))
                .ForMember(dest => dest.OfficeNumber, o => o.MapFrom(src => src.OfficeNumber))
                .ForMember(dest => dest.CellNumber, o => o.MapFrom(src => src.CellNumber))
                .ForMember(dest => dest.FaxNumber, o => o.MapFrom(src => src.FaxNumber))
                .ForMember(dest => dest.LockedStatus, o => o.MapFrom(src => src.LockedStatus))
                .ForMember(dest => dest.MerchantId, o => o.MapFrom(src => src.MerchantId))
                .ForMember(dest => dest.CanMakeVaultPayment, o => o.MapFrom(src => src.CanMakeVaultPayment))
                .ForMember(dest => dest.CreatedById, o => o.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.IsNotDeleted, o => o.MapFrom(src => src.ActiveStatus))
                .ForMember(dest => dest.CashCenterId, o => o.MapFrom(src => src.CashCentreId))
                .ForMember(dest => dest.LastChangedById, o => o.MapFrom(src => src.LastChangedById));


            AutoMapper.Mapper.CreateMap<CashCentreDto, CashCenter>().ReverseMap();
            AutoMapper.Mapper.CreateMap<ListCashCenterDto, CashCenter>().ReverseMap();
            AutoMapper.Mapper.CreateMap<AddressDto, CashCenter>().ReverseMap();

            AutoMapper.Mapper.CreateMap<CashDeposit, CashDeposit>();

            AutoMapper.Mapper.CreateMap<CashDeposit, CashDepositDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Container, ContainerDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Container, Container>().ReverseMap();
            AutoMapper.Mapper.CreateMap<ContainerDrop, ContainerDropDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<ContainerDropItem, ContainerDropItemDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<DepositType, DepositTypeDto>().ReverseMap();

            AutoMapper.Mapper.CreateMap<CashDeposit, ListCashDepositDto>()
                .ForMember(dest => dest.CashDepositId, o => o.MapFrom(src => src.CashDepositId))
                .ForMember(dest => dest.TransactionReference, o => o.MapFrom(src => src.TransactionReference))
                .ForMember(dest => dest.ContainerSerialNumber, o => o.MapFrom(src => src.Containers.FirstOrDefault().SerialNumber))
                .ForMember(dest => dest.CashDepositType, o => o.MapFrom(src => src.DepositTypeName))
                .ForMember(dest => dest.TotalDepositAmount, o => o.MapFrom(src => src.DepositedAmount))
                .ForMember(dest => dest.SiteName, o => o.MapFrom(src => src.SiteName))
                .ForMember(dest => dest.BankAccount, o => o.MapFrom(src => src.Account.AccountNumber))
                .ForMember(dest => dest.StatusName, o => o.MapFrom(src => src.Status.Name))
                .ForMember(dest => dest.CaptureDate, o => o.MapFrom(src => src.CreateDate.Value));


            AutoMapper.Mapper.CreateMap<ContainerType, ContainerTypeDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<ContainerTypeDto, ContainerType>().ReverseMap();

            AutoMapper.Mapper.CreateMap<ContainerTypeAttribute, ContainerTypeAttributeDto>().ReverseMap();

            AutoMapper.Mapper.CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.PreviousComments, o => o.MapFrom(src => src.Comments))
                .ForMember(dest => dest.DefaultAccount, o => o.MapFrom(src => src.DefaultAccount))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductTypeId, o => o.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.DeviceId, o => o.MapFrom(src => src.DeviceId))
                .ForMember(dest => dest.DeviceTypeId, o => o.MapFrom(src => src.DeviceTypeId))
                .ForMember(dest => dest.SiteId, o => o.MapFrom(src => src.SiteId))
                .ForMember(dest => dest.ServiceTypeId, o => o.MapFrom(src => src.ServiceTypeId))
                .ForMember(dest => dest.SettlementTypeId, o => o.MapFrom(src => src.SettlementTypeId))
                .ForMember(dest => dest.StatusId, o => o.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.PublicHolidayInclInFeeFlag, o => o.MapFrom(src => src.PublicHolidayInclInFeeFlag))
                .ForMember(dest => dest.ImplementationDate, o => o.MapFrom(src => Convert.ToDateTime(src.ImplementationDateString)))
                .ForMember(dest => dest.ServiceType, o => o.MapFrom(src => src.ServiceType))
                .ForMember(dest => dest.SettlementType, o => o.MapFrom(src => src.SettlementType))
                .ForMember(dest => dest.DeviceType, o => o.MapFrom(src => src.DeviceType))
                .ForMember(dest => dest.Status, o => o.MapFrom(src => src.Status))
                .ForMember(dest => dest.ProductType, o => o.MapFrom(src => src.ProductType))
                .ForMember(dest => dest.ProductTypeName, o => o.MapFrom(src => src.ProductTypeName))
                .ForMember(dest => dest.ServiceTypeName, o => o.MapFrom(src => src.ServiceTypeName))
                .ForMember(dest => dest.SettlementTypeName, o => o.MapFrom(src => src.SettlementTypeName))
                .ForMember(dest => dest.StatusName, o => o.MapFrom(src => src.StatusName))
                .ForMember(dest => dest.SiteName, o => o.MapFrom(src => src.SiteName))
                .ForMember(dest => dest.ProductFees, o => o.MapFrom(src => src.ProductFees));

            AutoMapper.Mapper.CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductTypeId, o => o.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.DeviceId, o => o.MapFrom(src => src.DeviceId))
                .ForMember(dest => dest.DeviceTypeId, o => o.MapFrom(src => src.DeviceTypeId))
                .ForMember(dest => dest.SiteId, o => o.MapFrom(src => src.SiteId))
                .ForMember(dest => dest.ServiceTypeId, o => o.MapFrom(src => src.ServiceTypeId))
                .ForMember(dest => dest.SettlementTypeId, o => o.MapFrom(src => src.SettlementTypeId))
                .ForMember(dest => dest.StatusId, o => o.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.PublicHolidayInclInFeeFlag, o => o.MapFrom(src => src.PublicHolidayInclInFeeFlag))
                .ForMember(dest => dest.ImplementationDateString, o => o.MapFrom(src => src.ImplementationDate.ToString("dd MMMM yyyy")))
                .ForMember(dest => dest.TerminationDateString, o => o.MapFrom(src => src.TerminationDate.HasValue ? src.TerminationDate.Value.ToString("dd MMMM yyyy") : string.Empty))
                .ForMember(dest => dest.ServiceType, o => o.MapFrom(src => src.ServiceType))
                .ForMember(dest => dest.SettlementType, o => o.MapFrom(src => src.SettlementType))
                .ForMember(dest => dest.DeviceType, o => o.MapFrom(src => src.DeviceType))
                .ForMember(dest => dest.Status, o => o.MapFrom(src => src.Status))
                .ForMember(dest => dest.ProductType, o => o.MapFrom(src => src.ProductType))
                .ForMember(dest => dest.ProductTypeName, o => o.MapFrom(src => src.ProductTypeName))
                .ForMember(dest => dest.ServiceTypeName, o => o.MapFrom(src => src.ServiceTypeName))
                .ForMember(dest => dest.SettlementTypeName, o => o.MapFrom(src => src.SettlementTypeName))
                .ForMember(dest => dest.StatusName, o => o.MapFrom(src => src.StatusName))
                .ForMember(dest => dest.SiteName, o => o.MapFrom(src => src.SiteName))
                .ForMember(dest => dest.ProductFees, o => o.MapFrom(src => src.ProductFees));


            AutoMapper.Mapper.CreateMap<SiteProductDto, Product>().ReverseMap();


            AutoMapper.Mapper.CreateMap<ProductTypeDto, ProductType>()
                .ForMember(dest => dest.ProductTypeId, o => o.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.LookUpKey, o => o.MapFrom(src => src.LookUpKey))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<ProductFeeDto, ProductFee>()
                 .ForMember(dest => dest.ProductId, o => o.MapFrom(src => src.ProductId))
                 .ForMember(dest => dest.FeeId, o => o.MapFrom(src => src.FeeId))
                 .ReverseMap();

            AutoMapper.Mapper.CreateMap<TaskDto, Task>()
                 .ReverseMap();

            AutoMapper.Mapper.CreateMap<ListTaskDto, Task>()
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<ServiceTypeDto, ServiceType>().ReverseMap();
            AutoMapper.Mapper.CreateMap<SettlementTypeDto, SettlementType>().ReverseMap();

            AutoMapper.Mapper.CreateMap<Device, DeviceDto>().ReverseMap();

            AutoMapper.Mapper.CreateMap<DeviceType, DeviceTypeDto>().ReverseMap();

            AutoMapper.Mapper.CreateMap<SiteContainerDto, SiteContainer>().ReverseMap();

            AutoMapper.Mapper.CreateMap<MerchantDto, Merchant>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Merchant, MerchantDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<ListMerchantDto, Merchant>().ReverseMap();

            AutoMapper.Mapper.CreateMap<SiteDto, Site>()
                .ForMember(dest => dest.Address, src => src.MapFrom(o => o.Address))
                .ForMember(dest => dest.Accounts, src => src.MapFrom(o => o.Accounts))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<Device, SiteDeviceModel>()
                .ForMember(dest => dest.DeviceId, src => src.MapFrom(o => o.DeviceId))
                .ForMember(dest => dest.Name, src => src.MapFrom(o => o.Name));

            AutoMapper.Mapper.CreateMap<ListSiteDto, Site>().ReverseMap();

            AutoMapper.Mapper.CreateMap<AddressDto, Site>().ReverseMap();

            AutoMapper.Mapper.CreateMap<AddressDto, Address>().ReverseMap();
            AutoMapper.Mapper.CreateMap<AddressTypeDto, AddressType>().ReverseMap();

            AutoMapper.Mapper.CreateMap<AccountDto, Account>().ReverseMap();
            AutoMapper.Mapper.CreateMap<ListAccountDto, Account>().ReverseMap();

            AutoMapper.Mapper.CreateMap<CashDeposit, CashProcessingDto>()
                .ForMember(dest => dest.CashDepositId, o => o.MapFrom(src => src.CashDepositId))
                .ForMember(dest => dest.DepositTypeId, o => o.MapFrom(src => src.DepositTypeId))
                .ForMember(dest => dest.SiteId, o => o.MapFrom(src => src.SiteId))
                .ForMember(dest => dest.AccountId, o => o.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.ProductTypeId, o => o.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.DeviceId, o => o.MapFrom(src => src.DiscrepancyAmount))
                .ForMember(dest => dest.ProductType, o => o.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.Narrative, o => o.MapFrom(src => src.Narrative))
                .ForMember(dest => dest.TransactionReference, o => o.MapFrom(src => src.TransactionReference))
                .ForMember(dest => dest.DepositedAmount, o => o.MapFrom(src => src.DepositedAmount))
                .ForMember(dest => dest.ActualAmount, o => o.MapFrom(src => src.ActualAmount))
                .ForMember(dest => dest.DiscrepancyAmount, o => o.MapFrom(src => src.DiscrepancyAmount))
                .ForMember(dest => dest.DepositTypeName, o => o.MapFrom(src => src.DepositTypeName))
                .ForMember(dest => dest.SiteName, o => o.MapFrom(src => src.SiteName))
                .ForMember(dest => dest.ProductTypeName, o => o.MapFrom(src => src.ProductTypeName))
                .ForMember(dest => dest.MerchantName, o => o.MapFrom(src => src.Site.Merchant.Name))
                .ForMember(dest => dest.ContractNumber, o => o.MapFrom(src => src.Site.Merchant.ContractNumber))
                .ForMember(dest => dest.iTramsUserName, o => o.MapFrom(src => src.iTramsUserName))
                .ForMember(dest => dest.SubmitDateTime, o => o.MapFrom(src => src.SubmitDateTime))
                .ForMember(dest => dest.CitDateTime, o => o.MapFrom(src => src.CitDateTime))
                .ForMember(dest => dest.IsNotDeleted, o => o.MapFrom(src => src.IsNotDeleted))
                .ForMember(dest => dest.CreatedById, o => o.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.LastChangedById, o => o.MapFrom(src => src.LastChangedById))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.LastChangedDate, o => o.MapFrom(src => src.LastChangedDate))
                .ForMember(dest => dest.VaultAmount, o => o.MapFrom(src => src.VaultAmount))
                .ForMember(dest => dest.VaultSource, o => o.MapFrom(src => src.VaultSource))
                .ForMember(dest => dest.SettlementIdentifier, o => o.MapFrom(src => src.SettlementIdentifier))
                .ForMember(dest => dest.CitDateTime, o => o.MapFrom(src => src.CitDateTime))
                .ForMember(dest => dest.SendDateTime, o => o.MapFrom(src => src.SendDateTime))
                .ForMember(dest => dest.Confirm, o => o.MapFrom(src => src.IsConfirmed))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<Container, ProcessingContainerDto>()
                .ForMember(dest => dest.ContainerId, o => o.MapFrom(src => src.ContainerId))
                .ForMember(dest => dest.CashDepositId, o => o.MapFrom(src => src.CashDepositId))
                .ForMember(dest => dest.ContainerTypeId, o => o.MapFrom(src => src.ContainerTypeId))
                .ForMember(dest => dest.ReferenceNumber, o => o.MapFrom(src => src.ReferenceNumber))
                .ForMember(dest => dest.SerialNumber, o => o.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.DiscrepancyAmount, o => o.MapFrom(src => src.DiscrepancyAmount))
                .ForMember(dest => dest.SealNumber, o => o.MapFrom(src => src.SealNumber))
                .ForMember(dest => dest.ActualAmount, o => o.MapFrom(src => src.ActualAmount))
                .ForMember(dest => dest.ContainerTypeName, o => o.MapFrom(src => src.ContainerType.Name))
                .ForMember(dest => dest.DiscrepancyAmount, o => o.MapFrom(src => src.DiscrepancyAmount))
                .ForMember(dest => dest.IsPrimaryContainer, o => o.MapFrom(src => src.IsPrimaryContainer))
                .ForMember(dest => dest.IsNotDeleted, o => o.MapFrom(src => src.IsNotDeleted))
                .ForMember(dest => dest.CreatedById, o => o.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.LastChangedById, o => o.MapFrom(src => src.LastChangedById))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.LastChangedDate, o => o.MapFrom(src => src.LastChangedDate))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<ContainerDrop, ProcessingContainerDropDto>()
                .ForMember(dest => dest.ContainerId, o => o.MapFrom(src => src.ContainerId))
                .ForMember(dest => dest.ContainerDropId, o => o.MapFrom(src => src.ContainerDropId))
                .ForMember(dest => dest.DiscrepancyReasonId, o => o.MapFrom(src => src.DiscrepancyReasonId))
                .ForMember(dest => dest.Narrative, o => o.MapFrom(src => src.Narrative))
                .ForMember(dest => dest.HasDiscrepancy, o => o.MapFrom(src => src.HasDiscrepancy))
                .ForMember(dest => dest.ReferenceNumber, o => o.MapFrom(src => src.ReferenceNumber))
                .ForMember(dest => dest.BagSerialNumber, o => o.MapFrom(src => src.BagSerialNumber))
                .ForMember(dest => dest.Amount, o => o.MapFrom(src => src.Amount))
                .ForMember(dest => dest.DiscrepancyAmount, o => o.MapFrom(src => src.DiscrepancyAmount))
                .ForMember(dest => dest.ActualAmount, o => o.MapFrom(src => src.ActualAmount))
                .ForMember(dest => dest.Comment, o => o.MapFrom(src => src.Comment))
                .ForMember(dest => dest.Number, o => o.MapFrom(src => src.Number))
                .ForMember(dest => dest.IsNotDeleted, o => o.MapFrom(src => src.IsNotDeleted))
                .ForMember(dest => dest.CreatedById, o => o.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.LastChangedById, o => o.MapFrom(src => src.LastChangedById))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.LastChangedDate, o => o.MapFrom(src => src.LastChangedDate))
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<ContainerDropItem, ProcessingContainerDropItemDto>()
                .ForMember(dest => dest.ContainerDropItemId, o => o.MapFrom(src => src.ContainerDropItemId))
                .ForMember(dest => dest.ContainerDropId, o => o.MapFrom(src => src.ContainerDropId))
                .ForMember(dest => dest.DenominationId, o => o.MapFrom(src => src.DenominationId))
                .ForMember(dest => dest.ValueInCents, o => o.MapFrom(src => src.ValueInCents))
                .ForMember(dest => dest.Count, o => o.MapFrom(src => src.Count))
                .ForMember(dest => dest.Value, o => o.MapFrom(src => src.Value))
                .ForMember(dest => dest.DiscrepancyCount, o => o.MapFrom(src => src.DiscrepancyCount))
                .ForMember(dest => dest.DiscrepancyValue, o => o.MapFrom(src => src.DiscrepancyValue))
                .ForMember(dest => dest.ActualCount, o => o.MapFrom(src => src.ActualCount))
                .ForMember(dest => dest.ActualValue, o => o.MapFrom(src => src.ActualValue))
                .ForMember(dest => dest.IsNotDeleted, o => o.MapFrom(src => src.IsNotDeleted))
                .ForMember(dest => dest.CreatedById, o => o.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.LastChangedById, o => o.MapFrom(src => src.LastChangedById))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.DenominationType, o => o.MapFrom(src => src.DenominationType))
                .ForMember(dest => dest.DenominationName, o => o.MapFrom(src => src.DenominationName))
                .ForMember(dest => dest.LastChangedDate, o => o.MapFrom(src => src.LastChangedDate))
                .ReverseMap();


            // Failed Vault Transactions
            AutoMapper.Mapper.CreateMap<VaultTransactionXml, VaultTransactionXmlDto>()
                .ForMember(dest => dest.VaultTransactionXmlId, o => o.MapFrom(src => src.VaultTransactionXmlId))
                .ForMember(dest => dest.StatusId, o => o.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.BagSerialNumber, o => o.MapFrom(src => src.BagSerialNumber))
                .ForMember(dest => dest.ErrorMessages, o => o.MapFrom(src => src.ErrorMessages))
                .ForMember(dest => dest.XmlMessage, o => o.MapFrom(src => src.XmlMessage))
                .ForMember(dest => dest.XmlAwaitingApproval, o => o.MapFrom(src => src.XmlAwaitingApproval))
                .ForMember(dest => dest.ApprovedById, o => o.MapFrom(src => src.ApprovedById))
                .ForMember(dest => dest.ApprovedDate, o => o.MapFrom(src => src.ApprovedDate))
                .ForMember(dest => dest.IsNotDeleted, o => o.MapFrom(src => src.IsNotDeleted))
                .ForMember(dest => dest.LastChangedById, o => o.MapFrom(src => src.LastChangedById))
                .ForMember(dest => dest.LastChangedDate, o => o.MapFrom(src => src.LastChangedDate))
                .ForMember(dest => dest.CreatedById, o => o.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.CreateDate));

            AutoMapper.Mapper.CreateMap<VaultTransactionXmlDto, VaultTransactionXml>()
                .ForMember(dest => dest.VaultTransactionXmlId, o => o.MapFrom(src => src.VaultTransactionXmlId))
                .ForMember(dest => dest.StatusId, o => o.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.BagSerialNumber, o => o.MapFrom(src => src.BagSerialNumber))
                .ForMember(dest => dest.ErrorMessages, o => o.MapFrom(src => src.ErrorMessages))
                .ForMember(dest => dest.XmlMessage, o => o.MapFrom(src => src.XmlMessage))
                .ForMember(dest => dest.XmlAwaitingApproval, o => o.MapFrom(src => src.XmlAwaitingApproval))
                .ForMember(dest => dest.ApprovedById, o => o.MapFrom(src => src.ApprovedById))
                .ForMember(dest => dest.ApprovedDate, o => o.MapFrom(src => src.ApprovedDate))
                .ForMember(dest => dest.IsNotDeleted, o => o.MapFrom(src => src.IsNotDeleted))
                .ForMember(dest => dest.LastChangedById, o => o.MapFrom(src => src.LastChangedById))
                .ForMember(dest => dest.LastChangedDate, o => o.MapFrom(src => src.LastChangedDate))
                .ForMember(dest => dest.CreatedById, o => o.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.CreateDate, o => o.MapFrom(src => src.CreateDate));

            AutoMapper.Mapper.CreateMap<CurrencyDto, Currency>()
                .ForMember(dest => dest.CurrencyId, o => o.MapFrom(src => src.CurrencyId))
                .ForMember(dest => dest.CountryId, o => o.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.Code, o => o.MapFrom(src => src.Code))
                .ForMember(dest => dest.Symbol, o => o.MapFrom(src => src.Symbol))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description))
                .ForMember(dest => dest.LookUpKey, o => o.MapFrom(src => src.LookUpKey));

            AutoMapper.Mapper.CreateMap<Currency, CurrencyDto>()
                .ForMember(dest => dest.CurrencyId, o => o.MapFrom(src => src.CurrencyId))
                .ForMember(dest => dest.CountryId, o => o.MapFrom(src => src.CountryId))
                .ForMember(dest => dest.Code, o => o.MapFrom(src => src.Code))
                .ForMember(dest => dest.Symbol, o => o.MapFrom(src => src.Symbol))
                .ForMember(dest => dest.Name, o => o.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description))
                .ForMember(dest => dest.LookUpKey, o => o.MapFrom(src => src.LookUpKey));

            AutoMapper.Mapper.CreateMap<VaultTransactionTypeDto, VaultTransactionType>()
                .ForMember(dest => dest.Code, o => o.MapFrom(src => src.Code))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description));

            AutoMapper.Mapper.CreateMap<VaultTransactionType, VaultTransactionTypeDto>()
                .ForMember(dest => dest.Code, o => o.MapFrom(src => src.Code))
                .ForMember(dest => dest.Description, o => o.MapFrom(src => src.Description));


            AutoMapper.Mapper.CreateMap<CashOrderTask, ListCashOrderTaskDto>()
                .ReverseMap();

            AutoMapper.Mapper.CreateMap<CashOrderTask, CashOrderTaskDto>()
                .ReverseMap();
        }
        
    }
}