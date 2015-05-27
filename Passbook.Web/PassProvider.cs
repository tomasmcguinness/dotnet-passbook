using System;
using System.Collections.Generic;

namespace Passbook.Web
{
	public abstract class PassProvider : IDisposable
	{
		public abstract bool SupportsPassType(string passTypeIdentifier);

		public virtual void RegisterDevicePass(string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber, string pushToken)
		{
			throw new NotImplementedException();
		}

		public virtual List<string> GetDevicePasses(string deviceLibraryIdentifier, string passTypeIdentifier, DateTime updatedSince)
		{
			throw new NotImplementedException();
		}

		public virtual byte[] GetPass(string passTypeIdentifier, string serialNumber)
		{
			throw new NotImplementedException();
		}

		public virtual void UnregisterDevicePass(string deviceLibraryIdentifier, string passTypeIdentifier, string serialNumber)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~PassProvider() 
		{
			// Finalizer calls Dispose(false)
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}
}

