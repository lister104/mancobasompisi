namespace Mancoba.Sompisi.Data.LocalDb.Entities
{
    internal sealed class EntityProduct : EntityCommonBase, IEntityCommon
    {                     
        public string Name { get; set; }

        public string Description { get; set; }

		public decimal Price { get; set; }
	}
}