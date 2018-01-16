namespace Mancoba.Sompisi.Data.LocalDb.Entities
{
	internal sealed class EntitySuburb : EntityCommonBase, IEntityCommon
	{
        [SQLite.Indexed]
        public string TownId { get; set; }
		public string Name { get; set; }
	}
}