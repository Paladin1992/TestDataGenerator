using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using TestDataGenerator.Common;
using TestDataGenerator.Services.Models;

namespace TestDataGenerator.Services
{
    /// <summary>
    /// Defines methods for sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// When implemented, this method creates an email according to the given EmailModel
        /// and returns a Boolean value indicating whether the email was successfully sent or not.
        /// </summary>               
        /// <param name="model">An EmailModel whose values are to be subtituted into its appropriate email template.</param>
        /// <returns>true if the email was successfully sent; otherwise false.</returns>
        bool CreateAndSend<TEmailModel>(TEmailModel model, Controller controller) where TEmailModel : EmailModel;
    }    

    public class EmailService : IEmailService
    {
        private string SubstitutePlaceholders(string template, Dictionary<string, string> keyValuePairs)
        {
            var sb = new StringBuilder(template);
            foreach (var item in keyValuePairs)
            {
                sb = sb.Replace($"%{item.Key}%", item.Value);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Creates an email message by the given EmailModel and returns the new entry's Id for future use.
        /// </summary>
        /// <param name="model">An EmailModel whose values are to be subtituted into its belonging email template.</param>
        /// <returns></returns>
        public bool CreateAndSend<TEmailModel>(TEmailModel model, Controller controller) where TEmailModel : EmailModel
        {
            try
            {
                // get HTML template file path by model's classname
                var modelName = model.GetType().Name;
                var className = modelName.Substring(0, modelName.Length - "EmailModel".Length);
                var htmlTemplateFile = Path.Combine(
                        HostingEnvironment.MapPath("~/Views/EmailTemplates"),
                        $"{className}EmailTemplate.html");

                // convert the content of the targeted HTML template to string
                var template = File.ReadAllText(htmlTemplateFile);
                var finalEmailBody = SubstitutePlaceholders(template, model.ToDictionary());

                // put content into the email layout
                var layoutTemplateFile = Path.Combine(HostingEnvironment.MapPath("~/Views/EmailTemplates"), "_EmailLayout.html");
                var layoutTemplate = File.ReadAllText(layoutTemplateFile);
                var layoutModel = new LayoutEmailModel() { EmailBody = finalEmailBody };
                finalEmailBody = SubstitutePlaceholders(layoutTemplate, layoutModel.ToDictionary());

                // send email
                using (var smtp = new SmtpClient())
                {
                    var mail = new MailMessage("noreply@mortoff.hu", model.To)
                    {
                        Subject = model.Subject,
                        Body = finalEmailBody,
                        IsBodyHtml = true
                    };

                    smtp.Send(mail);
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return false;
            }
        }
    }
}