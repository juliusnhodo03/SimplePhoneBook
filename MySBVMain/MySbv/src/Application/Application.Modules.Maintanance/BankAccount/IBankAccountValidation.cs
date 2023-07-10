using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto.Account;
using Application.Dto.Site;
using Domain.Data.Model;
using Utility.Core;

namespace Application.Modules.Maintanance.BankAccount
{
    public interface IBankAccountValidation
    {
        /// <summary>
        /// Add A Account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Add(BankAccountHolderDto accountsList, string username);

        /// <summary>
        /// Add A Account
        /// </summary>
        /// <param name="account"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> AddOnContinue(BankAccountHolderDto accountsList, string username);

        /// <summary>
        /// Update a new Account
        /// </summary>
        /// <param name="accountDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(AccountDto accountDto, string username);

        /// <summary>
        /// Submit a Account
        /// </summary>
        /// <param name="accountDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Submit(AccountDto accountDto, string username);

        /// <summary>
        /// Delete a Account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int id, string username);

        /// <summary>
        /// Generates Beneficiary Code for the Account
        /// </summary>
        /// <returns></returns>
        string GenerateBeneficiaryCode();

        /// <summary>
        /// Generates Task Reference Number for Approval Process
        /// </summary>
        /// <returns></returns>
        string GenerateTaskRefNumber();

        /// <summary>
        /// Get ApprovalObjectId By RefNumber
        /// </summary>
        /// <param name="referenceNumber"></param>
        /// <returns></returns>
        int GetApprovalObjectIdByRefNumber(string referenceNumber);

        /// <summary>
        /// Get the Task Reference number
        /// </summary>
        /// <returns></returns>
        string GetTaskRef(int accountId);

        /// <summary>
        /// Find a Account by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<AccountDto> Find(int id);
        
        /// <summary>
        /// Find an Account By Approval Object ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<AccountDto> ApprovalFind(int id);

        /// <summary>
        /// Find a Account by id
        /// </summary>
        /// <param name="rejectParameters"></param>
        /// <param name="bankAccountDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        bool IsBankAccountRejected(RejectAccountArgumentsDto rejectParameters, AccountDto bankAccountDto, string username);

        /// <summary>
        /// Return a list of all Account
        /// </summary>
        /// <returns></returns>
        IEnumerable<ListAccountDto> All();

        /// <summary>
        /// Check if the person trying to Approve a task is the one who submitted it for Approval
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        bool UserApprovingOwnTask(int userId, int accountId);

        /// <summary>
        /// Get the Account by the Benefiary Code
        /// </summary>
        /// <param name="beneficiaryCode"></param>
        /// <returns></returns>
        int GetAccountByBeneficiaryCode(string beneficiaryCode);

        /// <summary>
        /// Check if Bank Account is still in PENDING OR DECLINED State
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        bool IsBankAccountInApprovalState(int accountId);


        /// <summary>
        /// Check if site already has a Default Account
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        bool DefaultAccountAlreadyExist(int siteId);


        /// <summary>
        /// Check is the account is Default
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        bool IsAccountDefault(int accountId, int siteId);


        /// <summary>
        /// Check If there is a Default Account in the system
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        bool AreThereDefaultAccount(int accountId, int siteId);


        /// <summary>
        /// Check If there is a Default Account in the system with a Pending Status
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="siteId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        bool DefaultAccountByStatus(int accountId, int siteId, int statusId);


        /// <summary>
        /// Check if there are Duplicate account Numbers for the current site
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="siteId"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        bool DuplicateBankAccounts(int siteId, string accountNumber, int bankId);


        /// <summary>
        /// Check if The bank account is already in use by the current site
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="siteId"></param>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        bool AccountInUseByCurrentSite(int accountId, int siteId, string accountNumber);


        /// <summary>
        /// Get Site Bank Accounts
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        List<string> SiteBankAccounts(int siteId);

    }
}
