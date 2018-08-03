using System;
using System.Collections.Generic;

namespace TestDataGenerator.Data.Models
{
    public class UserSetup
    {
        /// <summary>
        /// A GUID to uniquely identify the setup.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The email address of the user as a foreign key.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A user-defined name to identify the setup.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The fields related to the current setup.
        /// </summary>
        public List<IFieldModel> Fields { get; set; } = new List<IFieldModel>();

        /// <summary>
        /// The date the setup was created on.
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}