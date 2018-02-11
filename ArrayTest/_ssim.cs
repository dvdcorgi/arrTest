using System;

namespace ArrayTest
{
    internal class _ssim
    {
        private static Single C1 = Convert.ToSingle(Math.Pow(0.01 * 255, 2));
        private static Single C2 = Convert.ToSingle(Math.Pow(0.03 * 255, 2));

        internal static Single Distance(byte[] macroblock1, byte[] macroblock2)
        {
            Single muOfFirst = Mu(macroblock1);
            Single muOfSecond = Mu(macroblock2);
            Single sigmaOfFirst = sigmaSingle(macroblock1, muOfFirst);
            Single sigmaOfSecond = sigmaSingle(macroblock2, muOfSecond);
            Single sigmaOfBoth = sigmaDouble(macroblock1, macroblock2, muOfFirst, muOfSecond);
            //return (((2 * (muOfFirst * muOfSecond)) + C1) * (((2 * sigmaOfBoth) + C2) / (Convert.ToSingle(Math.Pow(muOfFirst, 2)) + (Convert.ToSingle(Math.Pow(muOfSecond, 2)) + C1)) * (Convert.ToSingle(Math.Pow(sigmaOfFirst, 2)) + (Convert.ToSingle(Math.Pow(sigmaOfSecond, 2)) + C2))));
            //Return(2 * muOfFirst * muOfSecond + C1) * (2 * sigmaOfBoth + C2) / ((CSng(Math.Pow(muOfFirst, 2)) + CSng(Math.Pow(muOfSecond, 2)) + C1) * (CSng(Math.Pow(sigmaOfFirst, 2)) + CSng(Math.Pow(sigmaOfSecond, 2)) + C2))
            return (2 * muOfFirst * muOfSecond + C1) * (2 * sigmaOfBoth + C2) / ((Convert.ToSingle(Math.Pow(muOfFirst, 2)) + Convert.ToSingle(Math.Pow(muOfSecond, 2)) + C1) * (Convert.ToSingle(Math.Pow(sigmaOfFirst, 2)) + Convert.ToSingle(Math.Pow(sigmaOfSecond, 2)) + C2));
        }

        private static float sigmaDouble(byte[] first, byte[] second, Single muOfFirst, Single muOfSecond)
        {
            Single sum = 0;
            for (int i = 0; (i
                        <= (first.Length - 1)); i++)
            {
                sum = (sum + ((first[i] - muOfFirst) * (second[i] - muOfSecond)));
            }

            return (sum / (first.Length) - 1);
        }

        public static Single Mu(byte[] image)
        {
            Single sum = 0;
            foreach (var item in image)
            {
                sum += item;
            }
            return (sum / Convert.ToSingle(image.Length));
        }

        private static float sigmaSingle(byte[] image, Single myMu)
        {
            Single sum = 0;
            foreach (byte item in image)
            {
                sum += Convert.ToSingle(Math.Pow((item - myMu), 2));
            }
            //Return CSng(Math.Pow(sum / (CSng(image.Length) - 1), 0.5))
            return Convert.ToSingle(Math.Pow(sum / (Convert.ToSingle(image.Length) - 1), 0.5));
        }

        //Public Function sigmaSingle(ByVal image() As Byte, ByVal mu As Single) As Single
        //    Dim sum As Single = 0
        //    For Each value As Byte In image
        //        sum += CSng(Math.Pow(value - mu, 2))
        //    Next value
        //    Return CSng(Math.Pow(sum / (CSng(image.Length) - 1), 0.5))
        //End Function


    }
}