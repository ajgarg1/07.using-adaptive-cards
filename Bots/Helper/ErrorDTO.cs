using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdaptiveCardsBot.Bots.Helper
{
 //   public class ErrorDTO
 //   {
	//}


		[Serializable]
		public class ErrorDTO
	{

			private String _ErrorCode = "";
			private String _HelpId = "";
			private String message;
			public ErrorDTO()
			{
				this.message = String.Empty;
			}

			public String ErrorCode
			{
				get
				{
					return (this._ErrorCode);
				}
				set
				{
					this._ErrorCode = value;
				}
			}
			public String HelpId
			{
				get
				{
					return (this._HelpId);
				}
				set
				{
					this._HelpId = value;
				}
			}
			public String Message
			{
				set
				{
					this.message = value;
				}
				get
				{
					return this.message;
				}
			}
		}
	

}
