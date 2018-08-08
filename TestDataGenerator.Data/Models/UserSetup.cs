using System;
using System.Collections.Generic;

namespace TestDataGenerator.Data.Models
{
    public class UserSetup
    {
        /// <summary>
        /// A unique identifier for the setup.
        /// </summary>
        public int Id { get; set; }

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