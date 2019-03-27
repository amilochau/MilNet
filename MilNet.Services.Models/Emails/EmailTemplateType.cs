namespace MilNet.Services.Models
{
    public enum EmailTemplateType
    {
        NotSet = 0,

        NewAccount = 1,
        PasswordReset = 2,
        EmailChanged = 4,
        AccountClosed = 5,
        NewAccountFromAdministration = 6,

        Forms = 10
    }
}
