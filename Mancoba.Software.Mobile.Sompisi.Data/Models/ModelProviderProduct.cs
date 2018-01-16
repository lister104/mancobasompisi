namespace Mancoba.Sompisi.Data.Models
{
	public sealed class ModelProviderProduct : ModelCommonBase, IModelCommon
	{       
        public string ProviderId { get; set; }
       
        public ModelProduct Product { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

    }
}