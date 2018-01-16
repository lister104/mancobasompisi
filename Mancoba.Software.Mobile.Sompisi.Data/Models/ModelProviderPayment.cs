using System;

namespace Mancoba.Sompisi.Data.Models
{
	public sealed class ModelProviderPayment : ModelCommonBase, IModelCommon
	{		
		public string ProductId { get; set; }
		
		public string ProviderId { get; set; }

        public string ReceiptNumber { get; set; }

        public decimal AmountPaid { get; set; }

		public DateTime DatePaid { get; set; }

	}
}