namespace FaceLock.WebAPI.Models.HelpreModels
{
	/// <summary>
	/// The base64 encoded data type of the image.
	/// </summary>
	public class Base64Image
	{
		/// <summary>
		/// The encoded data type of the photo.
		/// </summary>
		/// <example>image/png</example>
		public string Data { get; set; }
		/// <summary>
		/// The MIME type of the photo.
		/// </summary>
		/// <example>image/png</example>
		public string ImageMimeType { get; set; }

		public Base64Image(string data, string imageMimeType)
		{
			Data = data;
			ImageMimeType = imageMimeType;
		}
	}
}
