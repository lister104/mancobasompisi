namespace Mancoba.Sompisi.Data.LocalDb.Entities
{
	internal sealed class EntityStreet : EntityCommonBase, IEntityCommon
	{
        [SQLite.Indexed]
        public string SuburbId { get; set; }
		public string Name { get; set; }
	}
}