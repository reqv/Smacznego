using System;
using Android.Graphics;
using System.IO;
using Android.Content;

namespace Smacznego
{
	public class ImageManipulation
	{
		public static Bitmap decodeBitmapFromStream(ContentResolver resolver,Android.Net.Uri data, int requestedWidth,int requestedHeight)
		{
			Stream stream = resolver.OpenInputStream (data);
			BitmapFactory.Options options = new BitmapFactory.Options ();
			options.InJustDecodeBounds = true;
			BitmapFactory.DecodeStream (stream);

			options.InSampleSize = calculateSampleSize (options, requestedWidth, requestedHeight);
			stream = resolver.OpenInputStream (data);
			options.InJustDecodeBounds = false;

			Bitmap bitmap = BitmapFactory.DecodeStream (stream, null, options);
			return bitmap;
		}

		private static int calculateSampleSize(BitmapFactory.Options options, int requestedWidth,int requestedHeight)
		{
			int actualWidth = options.OutWidth;
			int actualHeight = options.OutHeight;

			int inSampleSize = 1;

			if (actualWidth > requestedWidth || actualHeight > requestedHeight) {
				int halfWidth = actualWidth / 2;
				int halfHeight = actualHeight / 2;
				while ((halfWidth / inSampleSize) > requestedWidth && (halfHeight / inSampleSize) > requestedHeight) {
					inSampleSize *= 2;
				}
			}
			return inSampleSize;
		}
	}
}

