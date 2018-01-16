namespace Mancoba.Sompisi.Data.LocalDb.Entities
{
	internal sealed class EntityProvince : EntityCommonBase, IEntityCommon
	{
	    public EntityProvince()
	    {
	        Country = "South Africa";
	    }
        public string Country { get; set; }
        public string Name { get; set; }		
	}
}