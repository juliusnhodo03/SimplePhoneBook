namespace Application.Modules.Common
{
    public enum ApplicationRoles
    {
        SBVAdmin = 1,
        RetailUser,
        RetailViewer,
        RetailSupervisor,
        SBVTeller,
        SBVTellerSupervisor,
        SBVApprover,
        SBVRecon,
        SBVFinanceReviewer,
        SbvDataCapture
    }

    public enum ApplicationNotificationType
    {
        SMS = 1,
        FAX = 2,
        EMAIL = 3
    }

    public enum Titles
    {
        Mr = 1,
        Mrs,
        Miss,
        Dr,
        Other
    }

    public enum VaultSource
    {
        GPT,
        WEBFLO,
        MYSBV,
        GREYSTONE
    }

}