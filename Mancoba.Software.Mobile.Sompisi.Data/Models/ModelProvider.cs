namespace Mancoba.Sompisi.Data.Models
{
	public sealed class ModelProvider: ModelCommonBase, IModelCommon
	{
	    #region Properties
        public string Name { get; set; }

        public string ContactPerson { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }

		public string WebAddress { get; set; }		

	    public string StreetId { get; set; }

        public bool IsFavourite { get; set; }

        public string House { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string Town { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }

	    public string Address
	    {
	        get { return $"{House} {Street}, {Suburb}, {Town}, {Province}."; }
	    }
        #endregion

    }
}