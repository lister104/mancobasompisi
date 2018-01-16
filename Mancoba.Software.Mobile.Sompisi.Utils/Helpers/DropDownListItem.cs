namespace Mancoba.Sompisi.Utils.Helpers
{
	public class DropDownListItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownListItem" /> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        public DropDownListItem(string caption) : this(caption, caption, caption)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownListItem"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="parentId">The parent identifier.</param>
        public DropDownListItem(string id, string caption, string parentId)
		{
            Id = id;
            Caption = caption;
            ParentId = parentId;
        }

		public string Id { get; private set; }

		public string Caption { get;private set;  }

        public string ParentId { get;private set;  }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
		{
            return string.Format("{0}{1}", Caption, Id);		  
		}

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
		{
			var rhs = obj as DropDownListItem;
			if (rhs == null)
				return false;
			return rhs.Caption == Caption;
		}

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
		{
			if (Caption == null)
				return 0;
			return Caption.GetHashCode();
		}        
    }
}

