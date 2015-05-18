using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Passbook.Generator.Configuration
{
	public enum FieldAttribute
	{
		Label,
		Value,
		ChangeMessage,
		AttributedValue,
		CurrencyCode
	}

	public class TemplateModel
	{
		private Dictionary<string, Object> mData;

		private static String FieldName(String key, FieldAttribute fieldAttribute)
		{
			return String.Format("Field.{0}.{1}", key, fieldAttribute.ToString());
		}

		internal static String MapPath(String filePath)
		{
			if (!String.IsNullOrEmpty(filePath))
			{
				if (File.Exists(filePath))
					return Path.GetFullPath(filePath);

				if (HostingEnvironment.IsHosted)
					// Map the filename for web applications
					return HostingEnvironment.MapPath(filePath);
				else
				{
					// Remove any web specific prefixes
					if (filePath.StartsWith("~/"))
						filePath = filePath.Substring(2);

					// Map the filename for non-web applications
					String path = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
					return Path.GetFullPath(Path.Combine(path, filePath));
				}
			}

			return String.Empty;
		}

		public TemplateModel()
		{
			mData = new Dictionary<string, Object>(StringComparer.OrdinalIgnoreCase);
		}

		public void AddField(String key, FieldAttribute fieldAttribute, Object value)
		{
			mData[FieldName(key, fieldAttribute)] = value;
		}

		public String GetField(String key, FieldAttribute fieldAttribute, String defaultValue)
		{
			return GetField<String>(key, fieldAttribute, defaultValue);
		}

		public T GetField<T>(String key, FieldAttribute fieldAttribute, T defaultValue)
		{
			Object value;

			if (mData.TryGetValue(FieldName(key, fieldAttribute), out value))
				return (T)Convert.ChangeType(value, typeof(T));

			return defaultValue;
		}

		public void AddImage(PassbookImage passbookImage, string fileName)
		{
			mData[passbookImage.ToString()] = fileName;
		}

		public void AddImage(PassbookImage passbookImage, byte[] fileData)
		{
			mData[passbookImage.ToString()] = fileData;
		}

		public byte[] GetImage(PassbookImage passbookImage)
		{
			Object value;

			if (mData.TryGetValue (passbookImage.ToString (), out value)) 
			{
				if (value is byte[])
					return (byte[])value;

				String filePath = MapPath(value as String);

				if (File.Exists(filePath))
					return File.ReadAllBytes (filePath);
			}

			return null;
		}
	}
}

