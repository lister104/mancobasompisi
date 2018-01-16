using System;

namespace Mancoba.Sompisi.Data.LocalDb.Entities
{
	internal abstract class EntityCommonBase
	{
		#region Constructors

		protected EntityCommonBase()
		{
			Intitialise();
		}

		private void Intitialise()
		{
			Id = null;
			DateCreated  = DateTime.Now;

			CreatorUserId = null;
			DateUpdated = DateTime.Now;

			UpdatorUserId = null;
			DateCreated = DateTime.Now;

			IsSynced = false;
			DateSynced = new DateTime(1900,01,01);
		}

		#endregion

		#region Properties

		[SQLite.PrimaryKey]
		public string Id { get; set; }
		
		public DateTime DateCreated { get; set; }

		public string CreatorUserId { get; set; }

		public DateTime DateUpdated { get; set; }

		public string UpdatorUserId { get; set; }

        [SQLite.Indexed]
        public bool IsSynced { get; set; }

		public DateTime DateSynced { get; set; }

		#endregion
	}
}