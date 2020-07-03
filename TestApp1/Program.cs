﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace TestApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            double psnrValue;
            //Mat srcMat = Cv2.ImRead("C:\\Users\\zjsyzyt\\Desktop\\test\\Image_Original.jpg",ImreadModes.Grayscale);
            //Mat Mat = Cv2.ImRead("C:\\Users\\zjsyzyt\\Desktop\\test\\Images00000.jpg", ImreadModes.Grayscale);
            Mat srcMat = Cv2.ImRead("C:\\Users\\zjsyzyt\\OneDrive\\0工作文档\\2020论文征文\\图像退化论文\\质量评估\\质量评估1-offset\\MeasureDown1\\Image_Original.jpg", ImreadModes.Grayscale);
            Mat Mat = Cv2.ImRead("C:\\Users\\zjsyzyt\\OneDrive\\0工作文档\\2020论文征文\\图像退化论文\\质量评估\\质量评估1-offset\\MeasureDown1\\Images00000.jpg", ImreadModes.Grayscale);

            /*
            using (Mat tmp_m = new Mat(), tmp_sd = new Mat())
            {
                int x = Mat.Rows, y = Mat.Cols;
                Mat absDiff = new Mat();
                Cv2.Absdiff(srcMat, Mat, absDiff);
                //Cv2.Subtract(srcMat, Mat, Diff);
                absDiff.ConvertTo(absDiff, MatType.CV_64F);
                absDiff = absDiff.Mul(absDiff);
                Scalar s = Cv2.Sum(absDiff);
                double msee = s[0];
                psnrValue = 20 * Math.Log10(255 / Math.Sqrt(msee/(x*y)));
            }
            Console.WriteLine(psnrValue.ToString("F5"));
          


            
            //MeanStdDev是求一个矩阵内部元素的均值和方差；
            //我们要用的是两个矩阵的每个对应元素之间差值的平方和，完全是两回事情
            //using (Mat tmp_m = new Mat(), tmp_sd = new Mat(),absDiff = new Mat())
            //{
            //    Cv2.Absdiff(srcMat, Mat, absDiff);
            //    //Cv2.Subtract(srcMat, Mat, Diff);
            //    absDiff.MeanStdDev(tmp_m, tmp_sd);
            //    double mean = tmp_m.At<double>(0, 0);
            //    double sd = tmp_sd.At<double>(0, 0);
            //    psnrValue = 20 * Math.Log10(255 / sd);
            //}
            //Console.WriteLine(psnrValue.ToString("F5"));
            


            double psnrValue2;
            int m = Mat.Rows;
            int n = Mat.Cols;
            double mse = 0;
            for (int i=0;i<m;i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int X = Mat.At<byte>(i, j);
                    int Y = srcMat.At<byte>(i, j);
                    mse = mse + Math.Pow((Y - X), 2);
                    //mse = mse + Math.Pow((Mat.At<int>(i, j) - srcMat.At<int>(i, j)) , 2);
                }
            }
            psnrValue2=20* Math.Log10(255 / Math.Sqrt(mse/(m*n)));
            Console.WriteLine(psnrValue2.ToString("F5"));
            Console.ReadKey();


            double psnrValue3 = Cv2.PSNR(Mat, srcMat);
            Console.WriteLine(psnrValue3.ToString("F5"));
            Console.ReadKey();

            //int[,] A = new int[3, 3]{
            //    {0,1,2},
            //    {3,4,5},
            //    {1,2,1}
            //};
            //int[,] B = new int[3, 3]{
            //    {0,1,2},
            //    {3,-4,5},
            //    {1,2,1}
            //};
            //A.mean


            //
            //以下代码验证当矩阵某个元素正负相反时，平均值和标准差均发生了变化，
            //因此absDiff的计算会把相差的负值自动转变为对应的正值；

            Mat dstMat = new Mat();
            srcMat.ConvertTo(dstMat, MatType.CV_64F);
            using (Mat tmp_m = new Mat(), tmp_sd = new Mat())
            {
                dstMat.MeanStdDev(tmp_m, tmp_sd);
                double mean = tmp_m.At<double>(0, 0);
                double sd = tmp_sd.At<double>(0, 0);
                Console.WriteLine(mean.ToString("F10"));
                Console.WriteLine(sd.ToString("F10"));

                dstMat.At<double>(200, 300) = -dstMat.At<double>(200, 300);
                dstMat.MeanStdDev(tmp_m, tmp_sd);
                mean = tmp_m.At<double>(0, 0);
                sd = tmp_sd.At<double>(0, 0);

                Console.WriteLine(mean.ToString("F10"));
                Console.WriteLine(sd.ToString("F10"));
                Console.ReadKey();

            }
            */

            double ssimValue;
            using (Mat I1 = new Mat(), I2 = new Mat())
            {
                int winSize = 7;
                double k1 = 0.01d;
                double k2 = 0.03d;
                double L = 255d;
                //float k1 = 0.01f;
                //float k2 = 0.03f;
                //float L = 255f;
                srcMat.ConvertTo(I1, MatType.CV_32F);
                Mat.ConvertTo(I2, MatType.CV_32F);
                int ndim = I1.Dims;
                double NP = (double)Math.Pow(winSize, ndim);//滑动窗口覆盖的像素点个数
                double cov_norm = NP / (NP - 1);//NP个点上求平均值，无偏估计，需要乘以NP再除以NP-1
                double C1 = (k1 * L) * (k1 * L);
                double C2 = (k2 * L) * (k2 * L);

                Mat ux = new Mat();
                Mat uy = new Mat();
                Mat uxx = I1.Mul(I1);//元素为x^2
                Mat uyy = I2.Mul(I2);//元素为y^2
                Mat uxy = I1.Mul(I2);//元素为xy

                Size window = new Size(winSize, winSize);
                Point anchorPoint = new Point(-1, -1);
                Cv2.Blur(I1, ux, window, anchorPoint);//按窗口对图像均值滤波，计算x的期望
                Cv2.Blur(I2, uy, window, anchorPoint);

                Cv2.Blur(uxx, uxx, window, anchorPoint);//按窗口对uxx均值滤波，计算x^2的期望
                Cv2.Blur(uyy, uyy, window, anchorPoint);
                Cv2.Blur(uxy, uxy, window, anchorPoint);

                Mat ux_sq = ux.Mul(ux);//对均值矩阵进行元素平方，元素ux^2
                Mat uy_sq = ux.Mul(uy);
                Mat uxy_m = ux.Mul(uy);//对均值矩阵进行元素平方，元素ux*uy

                Mat vx = cov_norm * (uxx - ux_sq);//x方差，=E(x^2)-E^2(x)，无偏估计
                Mat vy = cov_norm * (uyy - uy_sq);
                Mat vxy = cov_norm * (uxy - uxy_m);//x,y协方差，=E(xy)-E(x)E(y)

                Mat A1 = 2 * uxy_m;
                Mat A2 = 2 * vxy;
                Mat B1 = ux_sq + uy_sq;
                Mat B2 = vx + vy;

                Mat ssim_map = (A1 + C1).Mul(A2 + C2) / (B1 + C1).Mul(B2 + C2);

                Scalar mssim = Cv2.Mean(ssim_map);
                ssim_map.ConvertTo(ssim_map, MatType.CV_8UC1, 255, 0);

                //imshow("ssim", ssim_map);
                ssimValue = mssim[0];

                Console.WriteLine(ssimValue.ToString("F10"));
                Console.ReadKey();
            }


        }
    }
}
