using System;
using SQLite;

namespace Mancoba.Sompisi.Data.LocalDb.Entities
{
	internal sealed class EntityProviderPayment : EntityCommonBase, IEntityCommon
    {       
        [Indexed]
        public string ProductId { get; set; }

        [Indexed]
        public string ProviderId { get; set; }

        public string ReceiptNumber { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime DatePaid { get; set; }

    }
}