using Mancoba.Sompisi.Utils.Enums;

namespace Mancoba.Sompisi.Data.Models
{
	public sealed class ModelProduct : ModelCommonBase, IModelCommon
	{
        public string Name { get; set; }

        public string Description { get; set; }

		public decimal Price { get; set; }

	}
}