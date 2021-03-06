﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;

namespace ImageNation
{
    public partial class MainForm : Form
    {
        //1.声明自适应类实例  
        AutoSizeFormClass asc = new AutoSizeFormClass();
        public MainForm()
        {
            InitializeComponent();//构造函数
            ComboBox_GrayScale.Enabled = false;
            ComboBox_PepperNoise.Enabled = false;
            ComboBox_PyrDown.Enabled = false;
            //CheckBoxPyrDown.Enabled = false;
            bgWorker_pBarImg.WorkerReportsProgress = true;  //设置报告进度更新
            bgWorker_pBarImg.WorkerSupportsCancellation = true;  //设置支持异步取消

            //测试用
            //OpenImgFileDialog.FileName = "C:\\Users\\zjsyzyt\\Pictures\\image1.jpg";
            //this.pictureBox1.Load(OpenImgFileDialog.FileName);

            //ImgStorageFolder.SelectedPath = "C:\\Users\\zjsyzyt\\Desktop\\test3";
        }
        //2. 为窗体添加Load事件，并在其方法MainForm_Load中，调用类的初始化方法，记录窗体和其控件的初始位置和大小  
        private void MainForm_Load(object sender, EventArgs e)
        {
            asc.controllInitializeSize(this);
        }
        //3.为窗体添加SizeChanged事件，并在其方法MainForm_SizeChanged中，调用类的自适应方法，完成自适应  
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            asc.controlAutoSize(this);
        }

        public PreviewForm previewForm { get; set;}


        //将私有的勾选状态转成可以用公有的方法获取;通过属性传递
        //mainForm和previewForm之间状态同步
        public CheckBox State_CheckBoxAlgo1Gauss()
        {
            //this.CheckBoxAlgo1Gauss.CheckState = previewForm.State_CheckBoxAlgo1Gauss().CheckState;
            return this.CheckBoxAlgo1Gauss;
        }

        public CheckBox State_CheckBoxAlgo2GrayScale()
        {
            //this.CheckBoxAlgo2GrayScale.CheckState = previewForm.State_CheckBoxAlgo2GrayScale().CheckState;
            return this.CheckBoxAlgo2GrayScale;
        }

        public CheckBox State_CheckBoxAlgo3GaussianBlur()
        {
            //this.CheckBoxAlgo3GaussianBlur.CheckState = previewForm.State_CheckBoxAlgo3GaussianBlur().CheckState;
            return this.CheckBoxAlgo3GaussianBlur;
        }

        public CheckBox State_CheckBoxAlgo4PepperNoise()
        {
            //this.CheckBoxAlgo4PepperNoise.CheckState = previewForm.State_CheckBoxAlgo4PepperNoise().CheckState;
            return this.CheckBoxAlgo4PepperNoise;
        }

        public ComboBox State_ComboBox_PepperNoise()
        {
            return this.ComboBox_PepperNoise;
        }

        public ComboBox State_ComboBox_GrayScale()
        {
            return this.ComboBox_GrayScale;
        }

        public CheckBox State_CheckBoxOffsetX()
        {
            return this.CheckBoxOffsetX;
        }

        public CheckBox State_CheckBoxOffsetY()
        {
            return this.CheckBoxOffsetY;
        }

        public CheckBox State_CheckBoxRotate()
        {
            return this.CheckBoxRotate;
        }

        public CheckBox State_CheckBoxPyrDown()
        {
            return this.CheckBoxPyrDown;
        }

        public ComboBox State_ComboBoxPyrDown()
        {
            return this.ComboBox_PyrDown;
        }

        public Mat imgOrigin;//定义初始图像
        public Mat imgResult;//定义结果图像
        public bool imgLoadFlag = false;

        //Mat(OpenImgFileDialog.FileName, ImreadModes.Grayscale);
        public Mat ImgOriginPublic
        {
            get
            {
                imgOrigin = new Mat(OpenImgFileDialog.FileName, ImreadModes.Grayscale);
                imgLoadFlag = true;
                return imgOrigin;
            }

        }

        
        ParaProcess ParaList= new ParaProcess();//动态类，需要实例化；工具类一般用静态类，函数前加static即可直接调用
        ImgProcess img = new ImgProcess();

        public int img_num;//图像数量总数
        public int imgCurrentValue;//当前处理图像序号

        private void Button_StartDegradation_Click(object sender, EventArgs e)
        {
            ImgStorageFolderPath = ImgStorageFolder.SelectedPath;
            bgWorker_pBarImg.RunWorkerAsync();  //运行backgroundWorker组件
            ProgressForm progressForm = new ProgressForm(bgWorker_pBarImg);//新建一个显示进度窗口

            progressForm.Show();
            progressForm.Location = new System.Drawing.Point(this.Location.X + 450, this.Location.Y + 350);
        }


        ////线程开始的时候调用的委托
        //private delegate void maxValueDelegate(int maxValue);
        ////线程执行中调用的委托
        //private delegate void nowValueDelegate(int nowValue);


        ///// <summary>
        ///// 线程开始事件,设置进度条最大值
        ///// 但是我不能直接操作进度条,需要一个委托来替我完成
        ///// </summary>
        ///// <param name="sender">ThreadMethod函数中传过来的最大值</param>
        ///// <param name="e"></param>
        //void method_threadStartEvent(object sender, EventArgs e)
        //{
        //    int maxValue = Convert.ToInt32(sender);
        //    maxValueDelegate max = new maxValueDelegate(setMax);
        //    this.Invoke(max, maxValue);
        //}

        ///// <summary>
        ///// 线程执行中的事件,设置进度条当前进度
        ///// 但是我不能直接操作进度条,需要一个委托来替我完成
        ///// </summary>
        ///// <param name="sender">ThreadMethod函数中传过来的当前值</param>
        ///// <param name="e"></param>
        //void method_threadEvent(object sender, EventArgs e)
        //{
        //    int nowValue = Convert.ToInt32(sender);
        //    nowValueDelegate now = new nowValueDelegate(setNow);
        //    this.Invoke(now, nowValue);
        //}

        ///// <summary>
        ///// 线程完成事件
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void method_threadEndEvent(object sender, EventArgs e)
        //{
        //    MessageBox.Show("执行已经完成!");
        //}

        ///// <summary>
        ///// 我被委托调用,专门设置进度条最大值的
        ///// </summary>
        ///// <param name="maxValue"></param>
        //private void setMax(int maxValue)
        //{
        //    this.pBarImg.Maximum = maxValue;
        //}

        ///// <summary>
        ///// 我被委托调用,专门设置进度条当前值的
        ///// </summary>
        ///// <param name="nowValue"></param>
        //private void setNow(int nowValue)
        //{
        //    this.pBarImg.Value = nowValue;
        //}



        public string originalPath = "C:\\Users\\zjsyzyt\\Pictures";

        private void Button_ImageScan_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();     //显示选择文件对话框
            //string originalPath = "C:\\Users\\zjsyzyt\\Pictures";
            OpenImgFileDialog.InitialDirectory = originalPath;//初始加载路径为C盘；
            OpenImgFileDialog.Filter = "All Image Files|*.bmp;*.jpg";//过滤你想设置的文本文件类型（这是txt型）
            //OpenImgFileDialog.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|";//过滤你想设置的文本文件类型（这是txt型）
            //OpenImgFileDialog.Filter ="图像文件(*.jpg)|*.jpg|图像文件(*.bmp)|*.bmp"; ;//过滤你想设置的文本文件类型（这是txt型）
                                                            // openFileDialog1.Filter = "文本文件 (*.txt)|*.txt|All files (*.*)|*.*";（这是全部类型文件）
            if (this.OpenImgFileDialog.ShowDialog() == DialogResult.OK)
            {
                TextBox_ImgPath.Text = Path.GetFileName(OpenImgFileDialog.FileName);//显示文件的名字
                originalPath = Path.GetFullPath(OpenImgFileDialog.FileName);//将本次图像路径作为下次的初始路径
                this.pictureBox1.Load(OpenImgFileDialog.FileName);
                imgLoadFlag = true;
            }

        }


        public string ImgStorageFolderPath;
        public string originalStoragePath = "C:\\Users\\zjsyzyt\\Pictures";

        private void Button_FilePathScan_Click(object sender, EventArgs e)
        {
            ImgStorageFolder.SelectedPath = originalStoragePath;
            if (this.ImgStorageFolder.ShowDialog() == DialogResult.OK)
            {
                if (this.ImgStorageFolder.SelectedPath.Trim() != "")
                    this.TextBox_ImgStoragePath.Text = this.ImgStorageFolder.SelectedPath.Trim();
                originalStoragePath = this.ImgStorageFolder.SelectedPath;//将本次文件路径作为下次的初始路径

            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void OpenImgFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void ImgNum_ValueChanged(object sender, EventArgs e)
        {

        }
        
        private void Value_ParaMiuMin_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Value_ParaMiuMax_ValueChanged(object sender, EventArgs e)
        {

        }

        private void CheckBoxAlgo1Gauss_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CheckBoxAlgo2GrayScale_CheckedChanged(object sender, EventArgs e)
        {
            switch (CheckBoxAlgo2GrayScale.Checked)
            {
                case true:
                    ComboBox_GrayScale.Enabled = true;
                    ComboBox_GrayScale.SelectedIndex = 0;
                    break;
                case false:
                    ComboBox_GrayScale.Enabled = false;
                    break;
            }
        }

        private void Value_ParaSigmaMin_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Value_ParaSigmaMax_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FileNamePrefix_TextChanged(object sender, EventArgs e)
        {

        }

        private void CheckBoxAlgo3GaussianBlur_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Value_ParaSigma2Max_ValueChanged(object sender, EventArgs e)
        {

        }

        private void CheckBoxOffsetX_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CheckBoxOffsetY_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CheckBoxRotate_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CheckBoxPyrDown_CheckedChanged(object sender, EventArgs e)
        {
            switch (CheckBoxPyrDown.Checked)
            {
                case true:
                    ComboBox_PyrDown.Enabled = true;
                    ComboBox_PyrDown.SelectedIndex = 0;//默认设置还原尺寸
                    break;
                case false:
                    ComboBox_PyrDown.Enabled = false;
                    break;
            }
        }

        private void CheckBoxIQAThreshold_CheckedChanged(object sender, EventArgs e)
        {
            switch (CheckBoxIQAThreshold.Checked)
            {
                case true:
                    if (CheckBoxPyrDown.Checked && ComboBox_PyrDown.SelectedIndex != 0)
                    {
                        ComboBox_PyrDown.SelectedIndex = 0;//质量评估状态下，设置必须选择上采样复原
                        MessageBox.Show("使用质量评估时，必须选择上采样复原图像尺寸");
                    }
                    break;
                case false:
                    CheckBoxPyrDown.Enabled = true;
                    ComboBox_PyrDown.Enabled = true;
                    break;
            }
        }

        private void ComboBox_PyrDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (CheckBoxIQAThreshold.Checked)
            {
                case true:
                    if (ComboBox_PyrDown.SelectedIndex == 1)
                    {
                        ComboBox_PyrDown.SelectedIndex = 0;//质量评估状态下，设置必须选择上采样复原
                        MessageBox.Show("使用质量评估时，必须选择上采样复原图像尺寸");
                    }
                    break;
                case false:
                    break;
            }
        }

        private void Label_Para3Max_Click(object sender, EventArgs e)
        {

        }

        private void Label_Para3Min_Click(object sender, EventArgs e)
        {

        }

        private void Label_ParaSigma2_Click(object sender, EventArgs e)
        {

        }

        private void TrackBar_ParaSigma2_Scroll(object sender, EventArgs e)
        {

        }

        private void Value_ParaSigma2Min_ValueChanged(object sender, EventArgs e)
        {

        }


        private void Button_PreviewForm_Click(object sender, EventArgs e)
        {
            if (imgLoadFlag == false)
            {
                MessageBox.Show("请加载图像");
                return;
            }
            PreviewForm previewForm = new PreviewForm(this);//引用Preview(MainForm)

            //用这个方法无法传输图像和数据，仅实例化Preview()
            //PreviewForm previewForm = new PreviewForm();//实例化，引用Preview()
            //previewForm.mainForm = this;//建立父子关系,此时this指向mainform窗口

            //添加委托实例定义
            previewForm.SendParaInterceptMin += new PreviewForm.SendPara(RecvParaInterceptMin);
            previewForm.SendParaInterceptMax += new PreviewForm.SendPara(RecvParaInterceptMax);
            previewForm.SendParaSlopeMin += new PreviewForm.SendPara(RecvParaSlopeMin);
            previewForm.SendParaSlopeMax += new PreviewForm.SendPara(RecvParaSlopeMax);

            previewForm.SendParaMiuMin += new PreviewForm.SendPara(RecvParaMiuMin);
            previewForm.SendParaMiuMax += new PreviewForm.SendPara(RecvParaMiuMax);
            previewForm.SendParaSigmaMin += new PreviewForm.SendPara(RecvParaSigmaMin);
            previewForm.SendParaSigmaMax += new PreviewForm.SendPara(RecvParaSigmaMax);

            previewForm.SendParaNoiseCoeffMin += new PreviewForm.SendPara(RecvParaNoiseCoeffMin);
            previewForm.SendParaNoiseCoeffMax += new PreviewForm.SendPara(RecvParaNoiseCoeffMax);

            previewForm.SendParaSigma2Min += new PreviewForm.SendPara(RecvParaSigma2Min);
            previewForm.SendParaSigma2Max += new PreviewForm.SendPara(RecvParaSigma2Max);

            previewForm.SendParaOffsetXMin += new PreviewForm.SendPara(RecvParaOffsetXMin);
            previewForm.SendParaOffsetXMax += new PreviewForm.SendPara(RecvParaOffsetXMax);

            previewForm.SendParaOffsetYMin += new PreviewForm.SendPara(RecvParaOffsetYMin);
            previewForm.SendParaOffsetYMax += new PreviewForm.SendPara(RecvParaOffsetYMax);

            previewForm.SendParaAngleMin += new PreviewForm.SendPara(RecvParaAngleMin);
            previewForm.SendParaAngleMax += new PreviewForm.SendPara(RecvParaAngleMax);

            previewForm.SendParaPyrDownCoeff += new PreviewForm.SendPara(RecvParaPyrDownCoeff);

            previewForm.Show();


        }

        private void CheckBoxAlgo4PepperNoise_CheckedChanged(object sender, EventArgs e)
        {
            switch (CheckBoxAlgo4PepperNoise.Checked)
            {
                case true:
                    ComboBox_PepperNoise.Enabled = true;
                    ComboBox_PepperNoise.SelectedIndex = 2;
                    break;
                case false:
                    ComboBox_PepperNoise.Enabled = false;
                    break;
            }
        }

        private void ComboBox_PepperNoise_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ComboBox_GrayScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            Value_ParaInterceptMin.Minimum = -255;
            Value_ParaInterceptMin.Maximum = 255;
            Value_ParaInterceptMax.Minimum = -255;
            Value_ParaInterceptMax.Maximum = 255;
            switch (ComboBox_GrayScale.SelectedIndex)
            {
                case 0://增大对比度
                    Value_ParaSlopeMin.Minimum = 1;
                    Value_ParaSlopeMin.Maximum = 2.5M;
                    Value_ParaSlopeMax.Minimum = 1;
                    Value_ParaSlopeMax.Maximum = 2.5M;
                    Value_ParaSlopeMin.DecimalPlaces = 1;
                    Value_ParaSlopeMin.Increment = 0.1M;
                    Value_ParaSlopeMax.DecimalPlaces = 1;
                    Value_ParaSlopeMax.Increment = 0.1M;
                    Value_ParaSlopeMin.Value = 1.8M;
                    Value_ParaSlopeMax.Value = 1.8M;
                    Value_ParaInterceptMin.Value = 30;
                    Value_ParaInterceptMax.Value = 30;
                    break;
                case 1://减小对比度
                    Value_ParaSlopeMin.Minimum = 0;
                    Value_ParaSlopeMin.Maximum = 1;
                    Value_ParaSlopeMax.Minimum = 0;
                    Value_ParaSlopeMax.Maximum = 1;
                    Value_ParaSlopeMin.DecimalPlaces = 1;
                    Value_ParaSlopeMin.Increment = 0.1M;
                    Value_ParaSlopeMax.DecimalPlaces = 1;
                    Value_ParaSlopeMax.Increment = 0.1M;
                    Value_ParaSlopeMin.Value = 0.7M;
                    Value_ParaSlopeMax.Value = 0.7M;
                    Value_ParaInterceptMin.Value = -30;
                    Value_ParaInterceptMax.Value = -30;
                    break;
                case 2://倒置
                    Value_ParaSlopeMin.Minimum = -1;
                    Value_ParaSlopeMin.Maximum = -1;
                    Value_ParaSlopeMax.Minimum = -1;
                    Value_ParaSlopeMax.Maximum = -1;
                    Value_ParaSlopeMin.Value = -1;
                    Value_ParaSlopeMax.Value = -1;
                    Value_ParaInterceptMin.Minimum = 255;
                    Value_ParaInterceptMin.Maximum = 255;
                    Value_ParaInterceptMax.Minimum = 255;
                    Value_ParaInterceptMax.Maximum = 255;
                    Value_ParaInterceptMin.Value = 255;
                    Value_ParaInterceptMax.Value = 255;
                    break;
                default:
                    break;
            }
        }


        //委托方法接收PreView传来的参数
        private void RecvParaInterceptMin(SendValueEventArgs e)
        {
            Value_ParaInterceptMin.Value = e.Value;
        }

        private void RecvParaInterceptMax(SendValueEventArgs e)
        {
            Value_ParaInterceptMax.Value = e.Value;
        }

        private void RecvParaSlopeMin(SendValueEventArgs e)
        {
            Value_ParaSlopeMin.Value = e.Value;
        }

        private void RecvParaSlopeMax(SendValueEventArgs e)
        {
            Value_ParaSlopeMax.Value = e.Value;
        }


        private void RecvParaMiuMin(SendValueEventArgs e)
        {
            Value_ParaMiuMin.Value = e.Value;
        }

        private void RecvParaMiuMax(SendValueEventArgs e)
        {
            Value_ParaMiuMax.Value = e.Value;
        }

        private void RecvParaSigmaMin(SendValueEventArgs e)
        {
            Value_ParaSigmaMin.Value = e.Value;
        }

        private void RecvParaSigmaMax(SendValueEventArgs e)
        {
            Value_ParaSigmaMax.Value = e.Value;
        }

        private void RecvParaNoiseCoeffMin(SendValueEventArgs e)
        {
            Value_ParaNoiseCoeffMin.Value = e.Value;
        }

        private void RecvParaNoiseCoeffMax(SendValueEventArgs e)
        {
            Value_ParaNoiseCoeffMax.Value = e.Value;
        }

        private void RecvParaSigma2Min(SendValueEventArgs e)
        {
            Value_ParaSigma2Min.Value = e.Value;
        }

        private void RecvParaSigma2Max(SendValueEventArgs e)
        {
            Value_ParaSigma2Max.Value = e.Value;
        }

        private void RecvParaOffsetXMin(SendValueEventArgs e)
        {
            Value_ParaOffsetXMin.Value = e.Value;
        }

        private void RecvParaOffsetXMax(SendValueEventArgs e)
        {
            Value_ParaOffsetXMax.Value = e.Value;
        }

        private void RecvParaOffsetYMin(SendValueEventArgs e)
        {
            Value_ParaOffsetYMin.Value = e.Value;
        }

        private void RecvParaOffsetYMax(SendValueEventArgs e)
        {
            Value_ParaOffsetYMax.Value = e.Value;
        }

        private void RecvParaAngleMin(SendValueEventArgs e)
        {
            Value_ParaAngleMin.Value = e.Value;
        }

        private void RecvParaAngleMax(SendValueEventArgs e)
        {
            Value_ParaAngleMax.Value = e.Value;
        }

        private void RecvParaPyrDownCoeff(SendValueEventArgs e)
        {
            Value_ParaPyrDownCoeff.Value = e.Value;
        }



        private void Value_ParaOffsetXMin_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Value_ParaOffsetXMax_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Value_ParaOffsetYMin_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Value_ParaOffsetYMax_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Value_ParaAngleMin_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Value_ParaAngleMax_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Value_ParaPyrDownCoeff_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_MainForm_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_MainForm_Click(object sender, EventArgs e)
        {

        }

        private void Label_Para21Min_Click(object sender, EventArgs e)
        {

        }

        //建立委托，将ComboBox_PepperNoise.SelectedIndex传递给新线程
        private delegate int returnIndexDelegate();

        //返回控件索引值
        private int returnIndex()
        {
            return ComboBox_PepperNoise.SelectedIndex;
        }

        //判断一下是不是该用Invoke，不是就直接返回
        private int returnCB(returnIndexDelegate myDelegate)
        {
            if (ComboBox_PepperNoise.InvokeRequired)
            {
                return (int)ComboBox_PepperNoise.Invoke(myDelegate);
            }
            else
            {
                return myDelegate();
            }
        }

        //建立委托，将ComboBox_PyrDown.SelectedIndex传递给新线程
        private delegate int returnPyrDownIndexDelegate();

        //返回控件索引值
        private int returnPyrDownIndex()
        {
            return ComboBox_PyrDown.SelectedIndex;
        }

        //判断一下是不是该用Invoke，不是就直接返回
        private int returnPyrDownIndexCB(returnIndexDelegate myDelegate)
        {
            if (ComboBox_PyrDown.InvokeRequired)
            {
                return (int)ComboBox_PyrDown.Invoke(myDelegate);
            }
            else
            {
                return myDelegate();
            }
        }


        //在另一个线程上开始运行(处理进度条)
        private void bgWorker_pBarImg_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            //AllocConsole();
            img_num = Convert.ToInt32(ImgNum.Value); //获取输入框中的数字,目标图片总数
            //pBarImg.Maximum = img_num;//设置最大长度值
            //pBarImg.Value = 0;//设置当前值
            //imgCurrentValue = 0;

            ////多线程开启进度条更新
            //ThreadMethod method = new ThreadMethod();
            ////先订阅一下事件
            //method.threadStartEvent += new EventHandler(method_threadStartEvent);
            //method.threadEvent += new EventHandler(method_threadEvent);
            //method.threadEndEvent += new EventHandler(method_threadEndEvent);

            ////ParameterizedThreadStart ts = new ParameterizedThreadStart(method.runMethod);
            //method.count = img_num;
            //method.value = imgCurrentValue;
            //Thread thread = new Thread(new ThreadStart(method.MethodStart));
            ////Thread thread = new Thread(ts);
            //thread.IsBackground = true;
            //thread.Start();

            //imgOrigin = new Mat(OpenImgFileDialog.FileName,ImreadModes.Grayscale);
            Mat imgOrigin = this.ImgOriginPublic;

            String[] path_Ori = { ImgStorageFolder.SelectedPath, string.Concat("Image_Original", ".jpg") };
            string fullpath_Ori = Path.Combine(path_Ori);
            imgOrigin.SaveImage(fullpath_Ori);

            //double[,] ParaVal = new double[6, img_num];
            double[] MiuVal = new double[img_num];
            double[] SigmaVal = new double[img_num];
            double[] SlopeVal = new double[img_num];
            double[] InterceptVal = new double[img_num];
            double[] Sigma2Val = new double[img_num];
            double[] CoeffVal = new double[img_num];

            double[] OffsetXVal = new double[img_num];
            double[] OffsetYVal = new double[img_num];
            double[] AngleVal = new double[img_num];
            //double[] PyrDownCoeffVal = new double[img_num];
            double[] PSNRVal = new double[img_num];
            double[] SSIMVal = new double[img_num];
            double[] DHashVal = new double[img_num];

            double miuMin = 0, miuMax = 0;
            double sigmaMin = 0, sigmaMax = 0;
            double slopeMin = 0, slopeMax = 0;
            double interceptMin = 0, interceptMax = 0;
            double sigma2Min = 0, sigma2Max = 0;
            double coeffMin = 0, coeffMax = 0;
            double offsetXMin = 0, offsetXMax = 0;
            double offsetYMin = 0, offsetYMax = 0;
            double angleMin = 0, angleMax = 0;
            int pyrDownNum = 0;

            //判断一共有几种算法，统计有多少变量，做成一整个数组

            if (CheckBoxAlgo1Gauss.Checked)
            {
                double miuScale = 1;//设为-128~128
                double sigmaScale = 1;//设为0~5

                miuMin = Convert.ToDouble(Value_ParaMiuMin.Value) / miuScale;
                miuMax = Convert.ToDouble(Value_ParaMiuMax.Value) / miuScale;
                sigmaMin = Convert.ToDouble(Value_ParaSigmaMin.Value) / sigmaScale;
                sigmaMax = Convert.ToDouble(Value_ParaSigmaMax.Value) / sigmaScale;

                MiuVal = ParaList.GetRandomList(miuMin, miuMax, img_num);
                SigmaVal = ParaList.GetRandomList(sigmaMin, sigmaMax, img_num);
            }

            if (CheckBoxAlgo2GrayScale.Checked)
            {

                slopeMin = Convert.ToDouble(Value_ParaSlopeMin.Value);
                slopeMax = Convert.ToDouble(Value_ParaSlopeMax.Value);
                interceptMin = Convert.ToDouble(Value_ParaInterceptMin.Value);
                interceptMax = Convert.ToDouble(Value_ParaInterceptMax.Value);

                SlopeVal = ParaList.GetRandomList(slopeMin, slopeMax, img_num);
                InterceptVal = ParaList.GetRandomList(interceptMin, interceptMax, img_num);
            }

            if (CheckBoxAlgo3GaussianBlur.Checked)
            {
                double sigma2Scale = 1;//参数值设为0~100，值越小越模糊，故需反转

                sigma2Min = Convert.ToDouble(Value_ParaSigma2Min.Value) / sigma2Scale;
                sigma2Max = Convert.ToDouble(Value_ParaSigma2Max.Value) / sigma2Scale;

                Sigma2Val = ParaList.GetRandomList(sigma2Min, sigma2Max, img_num);
            }

            if (CheckBoxAlgo4PepperNoise.Checked)
            {
                //通过 Invoke 方法对控件进行调用
                Action action = () =>
                {
                    ComboBox_PepperNoise.Visible = true;
                };
                Invoke(action);

                //ComboBox_PepperNoise.Visible = true;

                coeffMin = Convert.ToDouble(Value_ParaNoiseCoeffMin.Value);
                coeffMax = Convert.ToDouble(Value_ParaNoiseCoeffMax.Value);

                CoeffVal = ParaList.GetRandomList(coeffMin, coeffMax, img_num);

            }

            if (CheckBoxOffsetX.Checked)
            {
                offsetXMin = Convert.ToDouble(Value_ParaOffsetXMin.Value);
                offsetXMax = Convert.ToDouble(Value_ParaOffsetXMax.Value);

                OffsetXVal = ParaList.GetRandomList(offsetXMin, offsetXMax, img_num);
            }

            if (CheckBoxOffsetY.Checked)
            {
                offsetYMin = Convert.ToDouble(Value_ParaOffsetYMin.Value);
                offsetYMax = Convert.ToDouble(Value_ParaOffsetYMax.Value);

                OffsetYVal = ParaList.GetRandomList(offsetYMin, offsetYMax, img_num);
            }

            if (CheckBoxRotate.Checked)
            {
                angleMin = Convert.ToDouble(Value_ParaAngleMin.Value);
                angleMax = Convert.ToDouble(Value_ParaAngleMax.Value);

                AngleVal = ParaList.GetRandomList(angleMin, angleMax, img_num);
            }

            if (CheckBoxPyrDown.Checked)
            {
                pyrDownNum = (int)Value_ParaPyrDownCoeff.Value;
            }

            bool PyrUpStateFlag;//降采样是否复原状态

            if (returnPyrDownIndexCB(returnPyrDownIndex) == 0)
            {
                PyrUpStateFlag = true;
            }
            else
            {
                PyrUpStateFlag = false;
            }


            int IQAStateFlag;//质量评估勾选状态

            if (CheckBoxIQAThreshold.Checked)
            {
                IQAStateFlag = 1;
            }
            else
            {
                IQAStateFlag = 0;
            }

            //存储数组
            string newTxtPath = ImgStorageFolder.SelectedPath + string.Concat("/", "Para", ".csv");//创建txt文件的具体路径
            StreamWriter sw = new StreamWriter(newTxtPath, false, Encoding.Default);//实例化StreamWriter

            //存储参数表头
            sw.WriteLine("高斯方差" + "\t"
                    + "高斯均值" + "\t"
                    + "灰度变换斜率" + "\t"
                    + "灰度变换截距" + "\t"
                    + "模糊程度" + "\t"
                    + "椒盐噪声比例" + "\t"
                    + "X像素偏移" + "\t"
                    + "Y像素偏移" + "\t"
                    + "旋转角度偏移" + "\t"
                    + "降采样次数" + "\t"
                    + "PSNR" + "\t"
                    + "SSIM" + "\t"
                    + "DHash" + "\n"
                    + sigmaMin.ToString("F2") + "\t"
                    + miuMin.ToString("F2") + "\t"
                    + slopeMin.ToString("F2") + "\t"
                    + interceptMin.ToString("F2") + "\t"
                    + sigma2Min.ToString("F2") + "\t"
                    + coeffMin.ToString("F2") + "\t"
                    + offsetXMin.ToString("F2") + "\t"
                    + offsetYMin.ToString("F2") + "\t"
                    + angleMin.ToString("F2") + "\t"
                    + pyrDownNum.ToString("F0")
                    + "\t"
                    + "\t"
                    + "\n"
                    + sigmaMax.ToString("F2") + "\t"
                    + miuMax.ToString("F2") + "\t"
                    + slopeMax.ToString("F2") + "\t"
                    + interceptMax.ToString("F2") + "\t"
                    + sigma2Max.ToString("F2") + "\t"
                    + coeffMax.ToString("F2") + "\t"
                    + offsetXMax.ToString("F2") + "\t"
                    + offsetYMax.ToString("F2") + "\t"
                    + angleMax.ToString("F2") + "\t"
                    + pyrDownNum.ToString("F0")
                    + "\t"
                    + "\t"
                    + "\n"
                    );


            for (int i = 0; i < img_num; i++)
            {
                Mat transImg = imgOrigin.Clone();
                if (CheckBoxAlgo1Gauss.Checked)
                {
                    transImg = img.GaussianNoise(transImg, MiuVal[i], SigmaVal[i]);
                }

                if (CheckBoxAlgo2GrayScale.Checked)
                {
                    transImg = img.GrayScale(transImg, SlopeVal[i], InterceptVal[i]);
                }

                if (CheckBoxAlgo3GaussianBlur.Checked)
                {
                    transImg = img.LowPassFreq(transImg, Sigma2Val[i]);
                }

                if (CheckBoxAlgo4PepperNoise.Checked)
                {
                    int PepperNoiseIndex = returnCB(returnIndex);

                    transImg = img.PepperNoise(transImg, CoeffVal[i], PepperNoiseIndex);
                }

                if (CheckBoxOffsetX.Checked || CheckBoxOffsetY.Checked)
                {
                    double offsetx, offsety;
                    if (CheckBoxOffsetX.Checked)
                    {
                        offsetx = OffsetXVal[i];
                    }
                    else
                    {
                        offsetx = 0;
                    }

                    if (CheckBoxOffsetY.Checked)
                    {
                        offsety = OffsetYVal[i];
                    }
                    else
                    {
                        offsety = 0;
                    }

                    transImg = img.ImgAffine_Offset(transImg, offsetx, offsety);
                }

                if (CheckBoxRotate.Checked)
                {
                    transImg = img.ImgAffine_Rotate(transImg, AngleVal[i]);
                }

                if (CheckBoxPyrDown.Checked)
                {
                    pyrDownNum = (int)Value_ParaPyrDownCoeff.Value;
                    transImg = img.ImgPyrDown(transImg, pyrDownNum, PyrUpStateFlag);
                }

                //img.Dispose();
                //GC.Collect();
                imgResult = transImg.Clone();
                transImg.Release();

                int OffsetStateFlag;//位移退化状态
                if (CheckBoxOffsetX.Checked && CheckBoxOffsetY.Checked && CheckBoxRotate.Checked)
                {
                    OffsetStateFlag = 1;
                }
                else
                {
                    OffsetStateFlag = 0;
                }


                string NameResult = "";
                if (IQAStateFlag == 1)
                {
                    //质量评估
                    using (ImgQE imgQuality = new ImgQE())
                    {
                        PSNRVal[i] = imgQuality.ValuePSNR(imgOrigin, imgResult);
                        SSIMVal[i] = imgQuality.ValueSSIM(imgOrigin, imgResult);
                        DHashVal[i] = imgQuality.ValueDHash(imgOrigin, imgResult);
                    }
                    double SSIMThreshold;
                    double PSNRThreshold = (double)Value_PSNRThreshold.Value;
                    double HashThreshold = (double)Value_HashThreshold.Value;
                    if (OffsetStateFlag == 0)
                    {
                        SSIMThreshold = (double)Value_SSIMThreshold.Value;
                    }
                    else
                    {
                        SSIMThreshold = (double)Value_SSIMOffsetThreshold.Value;
                    }

                    //判断
                    if (PSNRVal[i] > PSNRThreshold && SSIMVal[i] > SSIMThreshold && DHashVal[i] > HashThreshold)
                    {
                        NameResult = "OK";
                    }
                    else
                    {
                        NameResult = "NG";
                    }
                }


                //存储图片
                string imgNamePrefix = TextBox_FileNamePrefix.Text;
                //String[] path = {ImgStorageFolder.SelectedPath, string.Concat("Images", i.ToString("D5"),".jpg") };
                String[] path = { ImgStorageFolder.SelectedPath, string.Concat(imgNamePrefix, i.ToString("D5"), NameResult, ".jpg") };
                string fullpath = Path.Combine(path);
                imgResult.SaveImage(fullpath);

                //存储参数数据
                sw.WriteLine(SigmaVal[i].ToString("F5") + "\t"
                        + MiuVal[i].ToString("F5") + "\t"
                        + SlopeVal[i].ToString("F5") + "\t"
                        + InterceptVal[i].ToString("F5") + "\t"
                        + Sigma2Val[i].ToString("F5") + "\t"
                        + CoeffVal[i].ToString("F5") + "\t"
                        + OffsetXVal[i].ToString("F5") + "\t"
                        + OffsetYVal[i].ToString("F5") + "\t"
                        + AngleVal[i].ToString("F5") + "\t"
                        + pyrDownNum.ToString("F0") + "\t"
                        + PSNRVal[i].ToString("F5") + "\t"
                        + SSIMVal[i].ToString("F5") + "\t"
                        + DHashVal[i].ToString("F5") + "\t"
                        );

                imgCurrentValue = i + 1;
                //method.MethodEvent();
                int imgCurrentPercent = (int)(100 * imgCurrentValue / img_num);

                worker.ReportProgress(imgCurrentPercent);

                if (worker.CancellationPending) //获取程序是否已请求取消后台操作
                {
                    e.Cancel = true;
                    break;
                }
            }

            sw.Flush();
            sw.Close();

        }

        private void bgWorker_pBarImg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("已取消");
            }
            else
            {
                //MessageBox.Show("完成");

                timer_ComfirmForm.Enabled = true;
            }
        }

        private void timer_ComfirmForm_Tick(object sender, EventArgs e)
        {
            ConfirmForm confirmForm = new ConfirmForm(this);//新建一个确认窗口
            confirmForm.Show();
            confirmForm.Location = new System.Drawing.Point(this.Location.X + 450, this.Location.Y + 350);
            timer_ComfirmForm.Enabled = false;
        }
    }

    //public class ThreadMethod
    //{
    //    /// <summary>
    //    /// 线程开始事件
    //    /// </summary>
    //    public event EventHandler threadStartEvent;
    //    /// <summary>
    //    /// 线程执行时事件
    //    /// </summary>
    //    public event EventHandler threadEvent;
    //    /// <summary>
    //    /// 线程结束事件
    //    /// </summary>
    //    public event EventHandler threadEndEvent;

    //    public int count, value;

    //    //public void runMethod()
    //    //{
    //    //    threadStartEvent.Invoke(count, new EventArgs());//通知主界面,我开始了,count用来设置进度条的最大值
    //    //    threadEvent.Invoke(value, new EventArgs());//通知主界面我正在执行,value表示进度条当前进度
    //    //    threadEndEvent.Invoke(new object(), new EventArgs());//通知主界面我已经完成了
    //    //}

    //    public void MethodStart()
    //    {
    //        threadStartEvent.Invoke(count, new EventArgs());//通知主界面,我开始了,count用来设置进度条的最大值
    //    }
    //    public void MethodEvent()
    //    {
    //        threadEvent.Invoke(value, new EventArgs());//通知主界面我正在执行,value表示进度条当前进度
    //    }
    //    public void MethodEnd()
    //    {
    //        threadEndEvent.Invoke(new object(), new EventArgs());//通知主界面我已经完成了
    //    }


    //}
}
