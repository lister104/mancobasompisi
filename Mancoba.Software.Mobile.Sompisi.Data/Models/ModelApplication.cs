namespace Mancoba.Sompisi.Data.Models
{
    public sealed class ModelApplication : ModelCommonBase, IModelCommon
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

        public string Address
        {
            get { return $"{Street}, {Suburb}, {Town}, {Province}, {Country}, {PostalCode}."; }
        }
    }
}
