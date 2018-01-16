namespace Mancoba.Sompisi.Data.LocalDb.Entities
{
	internal sealed class EntityTown : EntityCommonBase, IEntityCommon
	{
        [SQLite.Indexed]
        public string ProvinceId { get; set; }
		public string Name { get; set; }
	}
}