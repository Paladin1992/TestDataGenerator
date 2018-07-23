using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDataGenerator.Data.Enums;

namespace TestDataGenerator.Data.Models
{
    public class UserSetup
    {
        // GUID kötőjelek nélkül
        public string SetupId { get; set; }

        public string Email { get; set; }

        public List<IFieldModel> Fields { get; set; } = new List<IFieldModel>();

        /// <summary>
        /// The date the setup was created on.
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
