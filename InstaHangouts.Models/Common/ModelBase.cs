
namespace InstaHangouts.Models.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class Model Base.
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>The created on.</value>
        [DataType(DataType.Date)]
        [Display(Name = "Createdon")]
        public DateTime? Createdon { get; set; }

        /// <summary>
        /// Gets or sets the Modified On.
        /// </summary>
        /// <value>The modified on.</value>
        [DataType(DataType.Date)]
        [Display(Name = "Modifiedon")]
        public DateTime? Modifiedon { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        [Display(Name = "CreatedBy")]        
        public int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>The modified by.</value>
        [Display(Name = "ModifiedBy")]        
        public int ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        [Display(Name = "IsActive")]
        public bool? IsActive { get; set; }        
    }
}
