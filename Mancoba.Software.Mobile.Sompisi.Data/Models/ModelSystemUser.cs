using System;

namespace Mancoba.Sompisi.Data.Models
{
    public class ModelSystemUser : ModelCommonBase, IModelCommon
    {
        #region Properties		       

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string AccessCode { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }

		public string StreetId { get; set; }
		public string House { get; set; }
		public string Street { get; set; }
		public string Suburb { get; set; }
		public string Town { get; set; }
		public string Province { get; set; }
		public string Country { get; set; }

		#endregion

	}
}