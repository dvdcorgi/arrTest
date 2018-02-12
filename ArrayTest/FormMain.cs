using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alea;
using Alea.Parallel;
using NUnit.Framework;

namespace ArrayTest
{
    public partial class FormMain : Form
    {
        private int CompareImageWidth = 32;
        private int CompareImageHeight = 32;

        public FormMain()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var arg1 = Enumerable.Range(0, 5).ToArray();
            var arg2 = Enumerable.Range(0, 5).ToArray();
            var result = new int[5];

            Gpu.Default.For(0, result.Length, i => result[i] = arg1[i] + arg2[i]);

            var expected = arg1.Zip(arg2, (x, y) => x + y);

            Assert.That(result, Is.EqualTo(expected));

            TestFunc(5);
        }

        private static void TestFunc(int myCount)
        {
            try
            {
                List<int[]> myList = new List<int[]>();

                int[] array1 = { myCount, 1, 2, 3, 4 };

                int[] array2 = { 1, myCount, 3, 4, 5 };
                int[] array3 = { 1, 2, 3, 4, myCount };
                int[] array4 = { 1, 2, myCount, 4, 5 };

                myList.Add(array2);
                myList.Add(array3);
                myList.Add(array4);

                for (int i = 0; i < myList.Count; i++)
                {
                    for (int v = 0; v < array1.Length; v++)
                    {
                        var a = myCount;
                        var b = myList[i][v];
                        Console.WriteLine("current num:" + b);
                        if (array1[i] == myList[i][v])
                        {
                            Console.WriteLine("match");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var img1 = (byte[])new ImageConverter().ConvertTo(Image.FromFile(@"C:\Users\Dasumi\Source\Repos\arrTest\ArrayTest\bin\Debug\testImage\img1.jpg"), typeof(byte[]));
            var img2 = (byte[])new ImageConverter().ConvertTo(Image.FromFile(@"C:\Users\Dasumi\Source\Repos\arrTest\ArrayTest\bin\Debug\testImage\img2.jpg"), typeof(byte[]));
            Single sim = Similarity(img1, img2);
            Console.WriteLine(sim);
        }

        private Single Similarity(byte[] img1, byte[] img2)
        {
            return 100 * Distance(img1, img2);
        }

        private Single Distance(byte[] img1, byte[] img2)
        {
            try
            {
                Single finalDistance = 0;
                int macroBlockSize = 32;
                byte[,] subFirst;
                byte[,] subSecond;

                for (int y = 0; (y <= (CompareImageWidth - 1)); y = (y + macroBlockSize))
                {
                    for (int x = 0; (x <= (CompareImageHeight - 1)); x = (x + macroBlockSize))
                    {
                        byte[] macroblock1 = ConstructMacroblockAt(x, y, macroBlockSize, img1);
                        byte[] macroblock2 = ConstructMacroblockAt(x, y, macroBlockSize, img2);

                        finalDistance += _ssim.Distance(macroblock1, macroblock2);
                    }
                }

                return Convert.ToSingle(finalDistance) / ((CompareImageWidth / macroBlockSize) * (CompareImageHeight / macroBlockSize));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public byte[] ConstructMacroblockAt(int x, int y, int macrosize, byte[] source)
        {
            try
            {
                byte[] ret = new byte[(int)macrosize * macrosize - 1];
                int retPos = 0;
                //  our position in the return array
                int sourcePos = (x + (y * CompareImageWidth));

                Parallel.For(0, macrosize - 1, i =>
                {
                    Array.Copy(source, sourcePos, ret, retPos, macrosize);
                    retPos += macrosize;
                    sourcePos += CompareImageWidth;
                });

                //start gpu conversion

                var gpu = Gpu.Default;
                Console.WriteLine(gpu);

                //gpu.For(0, macrosize - 1, i =>
                //{
                //    Array.Copy(source, sourcePos, ret, retPos, macrosize);
                //    retPos += macrosize;
                //    sourcePos += CompareImageWidth;
                //});

                var expected = ret;

                Assert.That(ret, Is.EqualTo(expected));

                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ConstructMacroblockAt Error: " + ex.Message);
                return null;
            }
        }
    }
}
