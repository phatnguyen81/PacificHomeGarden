using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace pCMS.Framework
{
	/// <summary>
	/// Summary description for CaptchaImage.
	/// </summary>
	public class CaptchaImage
	{
		// Public properties (all read-only).
		public string Text
		{
			get { return this.text; }
		}
		public Bitmap Image
		{
			get { return this.image; }
		}
		public int Width
		{
			get { return this.width; }
		}
		public int Height
		{
			get { return this.height; }
		}

		// Internal properties.
		private string text;
		private int width;
		private int height;
		private string familyName;
		private Bitmap image;

		// For generating random numbers.
		private Random random = new Random();

		// ====================================================================
		// Initializes a new instance of the CaptchaImage class using the
		// specified text, width and height.
		// ====================================================================
		public CaptchaImage(string s, int width, int height)
		{
			this.text = s;
			this.SetDimensions(width, height);
			this.GenerateImage();
		}

		// ====================================================================
		// Initializes a new instance of the CaptchaImage class using the
		// specified text, width, height and font family.
		// ====================================================================
		public CaptchaImage(string s, int width, int height, string familyName)
		{
			this.text = s;
			this.SetDimensions(width, height);
			this.SetFamilyName(familyName);
			this.GenerateImage();
		}

		// ====================================================================
		// This member overrides Object.Finalize.
		// ====================================================================
		~CaptchaImage()
		{
			Dispose(false);
		}

		// ====================================================================
		// Releases all resources used by this object.
		// ====================================================================
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.Dispose(true);
		}

		// ====================================================================
		// Custom Dispose method to clean up unmanaged resources.
		// ====================================================================
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				// Dispose of the bitmap.
				this.image.Dispose();
		}

		// ====================================================================
		// Sets the image width and height.
		// ====================================================================
		private void SetDimensions(int width, int height)
		{
			// Check the width and height.
			if (width <= 0)
				throw new ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.");
			if (height <= 0)
				throw new ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.");
			this.width = width;
			this.height = height;
		}

		// ====================================================================
		// Sets the font used for the image text.
		// ====================================================================
		private void SetFamilyName(string familyName)
		{
			// If the named font is not installed, default to a system font.
			try
			{
				var font = new Font(this.familyName, 12F);
				this.familyName = familyName;
				font.Dispose();
			}
			catch (Exception)
			{
				this.familyName = System.Drawing.FontFamily.GenericSerif.Name;
			}
		}

		// ====================================================================
		// Creates the bitmap image.
		// ====================================================================
		private void GenerateImage()
		{
			// Create a new 32-bit bitmap image.
            var bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);

			// Create a graphics object for drawing.
            var g = Graphics.FromImage(bitmap);
			g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, this.width, this.height);

			// Fill in the background.
            var hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
			g.FillRectangle(hatchBrush, rect);

			// Set up the text font.
			SizeF size;
			float fontSize = rect.Height + 1;
			Font font;
			// Adjust the font size until the text fits within the image.
			do
			{
				fontSize--;
				font = new Font(this.familyName, fontSize, FontStyle.Bold);
				size = g.MeasureString(this.text, font);
			} while (size.Width > rect.Width);

			// Set up the text format.
			var format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;

			// Create a path using the text and warp it randomly.
            var path = new GraphicsPath();
			path.AddString(this.text, font.FontFamily, (int) font.Style, font.Size, rect, format);
            var v = 4F;
			PointF[] points =
			{
				new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
				new PointF(this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v),
				new PointF(rect.Width - this.random.Next(rect.Width) / v, rect.Height - this.random.Next(rect.Height) / v)
			};
            var matrix = new Matrix();
			matrix.Translate(0F, 0F);
			path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

			// Draw the text.
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.Black, Color.Black);
			g.FillPath(hatchBrush, path);

			// Add some random noise.
            var m = Math.Max(rect.Width, rect.Height);
            for (var i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
			{
                var x = random.Next(rect.Width);
                var y = random.Next(rect.Height);
                var w = random.Next(m / 50);
                var h = random.Next(m / 50);
				g.FillEllipse(hatchBrush, x, y, w, h);
			}

			// Clean up.
			font.Dispose();
			hatchBrush.Dispose();
			g.Dispose();

			// Set the image.
			image = bitmap;
		}
	}
}
