namespace Mancoba.Sompisi.Utils.Helpers
{
	public class DropDownListItem
	{
        public DropDownListItem(string caption):this(caption, caption, caption)
        {
           
        }
        public DropDownListItem(string id, string caption, string parentId)
		{
            Id = id;
            Caption = caption;
            ParentId = parentId;
        }

		public string Id { get; private set; }

		public string Caption { get;private set;  }

        public string ParentId { get;private set;  }

        public override string ToString()
		{
            return string.Format("{0}{1}", Caption, Id);		  
		}

		public override bool Equals(object obj)
		{
			var rhs = obj as DropDownListItem;
			if (rhs == null)
				return false;
			return rhs.Caption == Caption;
		}

		public override int GetHashCode()
		{
			if (Caption == null)
				return 0;
			return Caption.GetHashCode();
		}        
    }
}

