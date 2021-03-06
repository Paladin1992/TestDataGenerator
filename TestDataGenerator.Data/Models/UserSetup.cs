﻿using System;
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
        /// The email to identify the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// A user-defined name to identify the setup.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The fields related to the current setup.
        /// </summary>
        public List<FieldModel> Fields { get; set; } = new List<FieldModel>();

        /// <summary>
        /// The date the setup was created on.
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}