namespace Mancoba.Sompisi.Data.LocalDb.Entities
{
	internal sealed class EntityInstaller : EntityCommonBase, IEntityCommon
	{
        #region Properties
        public string Name { get; set; }

        public string ContactPerson { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }

		public string WebAddress { get; set; }

		[SQLite.Indexed]
        public string StreetId { get; set; }

        public string House { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string Town { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }

        public bool IsFavourite { get; set; } = false;

	    #endregion
	}
}