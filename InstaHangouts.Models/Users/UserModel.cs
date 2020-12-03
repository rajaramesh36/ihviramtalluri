namespace InstaHangouts.Models
{
    using System.ComponentModel.DataAnnotations;
    using System;    
    using InstaHangouts.Models.Common;

    public class UserModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Display(Name = "UserId")]        
        public int UserId { get; set; }
        
        /// <summary>
        /// Gets or sets the email identifier.
        /// </summary>
        /// <value>The email identifier.</value>
        [Display(Name = "EmailId")]        
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Display(Name = "Password")]        
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The ConfirmPassword.</value>
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }       
        
        /// <summary>
        /// Gets or sets a value indicating whether this instance is first login.
        /// </summary>
        /// <value><c>true</c> if this instance is first login; otherwise, <c>false</c>.</value>
        [Display(Name = "IsFirstLogin")]
        public bool? IsFirstLogin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is first login.
        /// </summary>
        /// <value><c>true</c> if this instance is first login; otherwise, <c>false</c>.</value>
        [Display(Name = "IsEmailValidated")]
        public bool? IsEmailValidated { get; set; }

        /// <summary>
        /// Gets or sets the email identifier.
        /// </summary>
        /// <value>The email identifier.</value>
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        public bool EventAttender { get; set; }

        /// <summary>
        /// Gets the name of the Full Name.
        /// </summary>
        /// <value>The name of the Full Name.</value>
        [Display(Name = "FullName")]
        public string FullName
        { get; set; }

        public string UserErrorMessage { get; set; }
        public string Message { get; set; }

        public bool isAssocationDeleted { get; set; }

        public string OldEmailId { get; set; }

        public string ZipCode { get; set; }

        public string OldFullName { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Display(Name = "EmailId")]
        public string EmailId { get; set; }
    }

    /// <summary>
    /// Class Email Template Model.
    /// </summary>
    public class EmailTemplateModel
    {
        /// <summary>
        /// Gets or sets the mail identifier.
        /// </summary>
        /// <value>The mail identifier.</value>
        public int? MailID { get; set; }

        /// <summary>
        /// Gets or sets the mail code.
        /// </summary>
        /// <value>The mail code.</value>
        public string MailCode { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the mail subject.
        /// </summary>
        /// <value>The mail subject.</value>
        public string MailSubject { get; set; }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        public string Template { get; set; }

        /// <summary>
        /// Gets or sets to value.
        /// </summary>
        /// <value>To value.</value>
        public string ToWhome { get; set; }

        /// <summary>
        /// Gets or sets the cc value.
        /// </summary>
        /// <value>The cc value.</value>
        public string CcWhome { get; set; }

        /// <summary>
        /// Gets or sets the BCC value.
        /// </summary>
        /// <value>The BCC value.</value>
        public string BccWhome { get; set; }
    }
}
