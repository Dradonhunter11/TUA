using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUA.Discord
{
	public class DRPMessage
	{
		public readonly string Header;
		public readonly string Message;
		private readonly Func<bool> CanCallFunc;

		public DRPMessage(string header, string message, Func<bool> canCallFunc = null)
		{
			Header = header;
			Message = message;
			CanCallFunc = canCallFunc;
		}

		public bool CanCall()
		{
			if (CanCallFunc == null)
				return true;

			return CanCallFunc();
		}
	}
}
