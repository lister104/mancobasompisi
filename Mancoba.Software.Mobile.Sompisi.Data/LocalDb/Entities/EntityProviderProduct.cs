namespace Mancoba.Sompisi.Data.LocalDb.Entities
{
    internal sealed class EntityProviderProduct : EntityCommonBase, IEntityCommon
    {
        [SQLite.Indexed]
        public string ProviderId { get; set; }

        [SQLite.Indexed]
        public string ProductId { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }       
    }
}