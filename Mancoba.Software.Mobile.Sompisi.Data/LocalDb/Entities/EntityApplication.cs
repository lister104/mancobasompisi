using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mancoba.Sompisi.Data.LocalDb.Entities
{
    internal sealed class EntityApplication : EntityCommonBase, IEntityCommon
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }

        public string EmailAddress { get; set; }

        public string Street { get; set; }
                
        public string Suburb { get; set; }

        public string Town { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }
        
        public string PostalCode { get; set; }
    }
}
