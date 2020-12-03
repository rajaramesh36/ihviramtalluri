namespace InstaHangouts.Model
{
    using System.ComponentModel.DataAnnotations;
    using System;
    using InstaHangouts.Model.Common;


    public class UserModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Display(Name = "UserId")]
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the Insta Hangouts Id identifier.
        /// </summary>
        /// <value>The Insta Hangouts Id.</value>
        [Display(Name = "InstaHangoutsId")]
        public string InstaHangoutsId { get; set; }

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
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>The role identifier.</value>
        [Display(Name = "RoleId")]
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [Display(Name = "LastName")]
        public string LastName { get; set; }

               /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        [Display(Name = "Role")]
        public string Role { get; set; }
                
        /// <summary>
        /// Gets or sets the profile BLOB path.
        /// </summary>
        /// <value>The profile BLOB path.</value>
        [Display(Name = "ProfileBlobPath")]
        public string ProfileBlobPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is first login.
        /// </summary>
        /// <value><c>true</c> if this instance is first login; otherwise, <c>false</c>.</value>
        [Display(Name = "IsFirstLogin")]
        public bool IsFirstLogin { get; set; }
      
        /// <summary>
        /// Gets the name of the Full Name.
        /// </summary>
        /// <value>The name of the Full Name.</value>
        [Display(Name = "FullName")]
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
    }
}
