using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class GreyAlgorithm : VisionAlgorithm
    {
        public GreyAlgorithm(String name) : base(name) { }
        public override Bitmap DoAlgorithm(Bitmap sourceImage)
        {
            Bitmap newBitmap = new Bitmap(sourceImage.Width, sourceImage.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
      {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
      });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(sourceImage, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
               0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
    class GrayDensity
    {
        public static int[] calcDensity(Bitmap source)
        {
            int [] density = new int [256];

            int alpha = 4;

            BitmapData bmpData = source.LockBits(new Rectangle(0,0, source.Width, source.Height), 
            ImageLockMode.ReadOnly, source.PixelFormat);

            if (source.PixelFormat == PixelFormat.Format24bppRgb) alpha = 3;
 
            IntPtr ptr = bmpData.Scan0; 
 
            int bytes = Math.Abs(bmpData.Stride) * source.Height; 
            byte[] rgbValues = new byte[bytes]; 
 
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, 
            bytes);
            for (int counter = 0; counter < rgbValues.Length; counter += alpha) 
            {
                density[rgbValues[counter]]++;

            } 
 
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, 
            bytes);
            source.UnlockBits(bmpData);
                    
            return density;
        }
        public static int[] compactDensity(Bitmap source)
        {
            int[] compactdensity = new int[10];

            int alpha = 4;

            BitmapData bmpData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height),
            ImageLockMode.ReadOnly, source.PixelFormat);

            if (source.PixelFormat == PixelFormat.Format24bppRgb) alpha = 3;

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * source.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0,
            bytes);
            for (int counter = 0; counter < rgbValues.Length; counter += alpha)
            {
                compactdensity[(int)((rgbValues[counter])/25.6)]++;

            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr,
            bytes);
            source.UnlockBits(bmpData);

            return compactdensity;
        }
    }
    class Equalization
    {
        public static Bitmap Equalize(Bitmap source, int [] density, int total) {

            int alpha = 4;
            Bitmap returnImage = new Bitmap(source);
            BitmapData bmpData = returnImage.LockBits(new Rectangle(0,0,returnImage.Width, returnImage.Height),
ImageLockMode.ReadWrite, source.PixelFormat);

            if (source.PixelFormat == PixelFormat.Format24bppRgb) alpha = 3;


            for (int i = 1; i < density.Length; i++)
            {
                density[i] += density[i - 1];
            }

            double intensity = density.Length / (double)total; 

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * returnImage.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            Boolean bla = false;
            for (int counter = 0; counter < rgbValues.Length; counter += alpha)
            {
                if (rgbValues[counter] != 0) bla = true;
                rgbValues[counter] = (byte)(int)(density[rgbValues[counter]] * intensity);
                rgbValues[counter + 1] = (byte)(int)(density[rgbValues[counter + 1]] * intensity); 
                rgbValues[counter + 2] = (byte)(int)(density[rgbValues[counter + 2]] * intensity);
                

            }

            Console.WriteLine(bla);
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr,
            bytes);

            returnImage.UnlockBits(bmpData);

            return returnImage;
        }
    }
}

