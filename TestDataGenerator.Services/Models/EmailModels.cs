using TestDataGenerator.Resources;

namespace TestDataGenerator.Services.Models
{
    public abstract class EmailModel
    {
        public string To { get; set; }

        public string Subject { get; set; }

        public string FullName { get; set; }

        public EmailModel()
            : this(subject: Messages.EmailModel_Default_Subject)
        {

        }

        public EmailModel(string to = "", string subject = "", string fullName = "")
        {
            this.To = to;
            this.Subject = subject;
            this.FullName = string.IsNullOrEmpty(fullName) ? Messages.EmailModel_Default_FullName : fullName;
        }
    }

    public class LayoutEmailModel : EmailModel
    {
        public string EmailBody { get; set; }
    }

    public class RegSuccessEmailModel : EmailModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public RegSuccessEmailModel()
            : base(subject: Messages.EmailModel_Default_RegSuccessSubject)
        {

        }
    }

    public class ForgottenPasswordEmailModel : EmailModel
    {
        public string VerifyLink { get; set; }

        public string ExpirationDate { get; set; }

        public ForgottenPasswordEmailModel()
            : base(subject: Messages.EmailModel_Default_ForgottenPasswordSubject)
        {

        }
    }
}