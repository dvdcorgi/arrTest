using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArrayTest
{
    public partial class Form1 : Form
    {
        private int CompareImageWidth = 32;
        private int CompareImageHeight = 32;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            testFunc(5);
        }

        private static void testFunc(int myCount)
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

        private void button2_Click(object sender, EventArgs e)
        {
            var img1 = (byte[])new ImageConverter().ConvertTo(Image.FromFile(@"C: \Users\David\Desktop\TestAlea\img1.png"), typeof(byte[]));
            var img2 = (byte[])new ImageConverter().ConvertTo(Image.FromFile(@"C: \Users\David\Desktop\TestAlea\img2.png"), typeof(byte[]));
            Single sim = similarity(img1, img2);
            Console.WriteLine(sim);
        }

        private Single similarity(byte[] img1, byte[] img2)
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
                        // iterate vertically over all macroblocks
                        byte[] macroblock1 = ConstructMacroblockAt(x, y, macroBlockSize, img1);
                        byte[] macroblock2 = ConstructMacroblockAt(x, y, macroBlockSize, img2);
                        finalDistance += _ssim.Distance(macroblock1, macroblock2);
                    }
                }

                //for (int x = 0; x < CompareImageWidth - 1; x++)
                //{
                //    for (int y = 0; y < CompareImageHeight - 1; y++)
                //    {
                //        byte[] macroblock1 = ConstructMacroblockAt(x, y, macroBlockSize, img1);
                //        byte[] macroblock2 = ConstructMacroblockAt(x, y, macroBlockSize, img2);
                //        finalDistance = (finalDistance + _ssim.Distance(macroblock1, macroblock2));
                //    }
                //}

                //For y As Integer = 0 To CompareImageWidth -1 Step macroBlockSize 'iterate horizontally over all macroblocks
                //For x As Integer = 0 To CompareImageHeight -1 Step macroBlockSize 'iterate vertically over all macroblocks
                //    Dim macroblock1() As Byte = ConstructMacroblockAt(x, y, macroBlockSize, first)
                //    Dim macroblock2() As Byte = ConstructMacroblockAt(x, y, macroBlockSize, second)
                //    finalDistance += _ssim.Distance(macroblock1, macroblock2)
                //Next x


                //Next y

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

            return ret;
        }
    }
}
