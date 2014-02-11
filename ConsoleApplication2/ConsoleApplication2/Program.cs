using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        string fileName;
        public Program() { }
        public void run()
        {
            Console.WriteLine("Please enter a valid image name:");
            fileName = Console.ReadLine();
            try
            {
                Bitmap map = new Bitmap(fileName);
                Bitmap greyMap = new GreyAlgorithm("Gray").DoAlgorithm(map);
                greyMap.Save("grey_" + fileName);
                int[] density = GrayDensity.calcDensity(greyMap);
                saveCSV("gray", density);
                Bitmap equaMap = Equalization.Equalize(greyMap, density, greyMap.Width * greyMap.Height);
                equaMap.Save("equa_" + fileName);
                int[] compDensity = GrayDensity.compactDensity(greyMap);
                saveCSV("gray", compDensity);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went terribly wrong, I do apologize.");
            }
            Console.WriteLine("Please press a button to exit.");
            Console.ReadKey();
        }
        public void saveCSV(String type, int[] d)
        {
            StreamWriter fs = new StreamWriter(type + "Denity" + d.Length + "_" + fileName + ".csv", /*FileMode.Create*/ false);
            for (int i = 0; i < d.Length; i++)
            {
                fs.WriteLine(d[i].ToString());
            }
            fs.Flush();
            fs.Close();
        }
        public void saveCSVDouble(String type, double[] d)
        {
            StreamWriter fs = new StreamWriter(type + "Denity" + d.Length + "_" + fileName + ".csv", /*FileMode.Create*/ false);
            for (int i = 0; i < d.Length; i++)
            {
                fs.WriteLine(d[i].ToString());
            }
            fs.Flush();
            fs.Close();
        }
        static void Main(string[] args)
        {
            Program p = new Program();
            p.run();
        }
    }
}
