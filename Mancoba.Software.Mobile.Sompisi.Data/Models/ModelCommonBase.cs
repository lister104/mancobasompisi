namespace Mancoba.Sompisi.Data.Models
{
	public abstract class ModelCommonBase
	{
		#region Constructors
		protected ModelCommonBase()
		{
			Intitialise();
		}

		private void Intitialise()
		{
			Id = null;
		}
		#endregion

		#region Properties

		public string Id { get; set; }

		public bool IsSuccessful => !string.IsNullOrWhiteSpace(Id);

		#endregion
	}
}