using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net.Config;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using System.Drawing.Imaging;

namespace Festo_R2U_Package_YJKP
{
    public partial class Form_Customized : Form
    {
        public Form_Customized()
        {
            InitializeComponent();
            InitLog4Net();
        }
        int MaxCurves = 10;//堆叠显示最多可以显示多少条
        string bmpName;//chart的背景bmp的文件名

        Color FestoBlue_Light = Color.FromArgb(200, 200, 230, 250);//第1个参数为透明度(alpha)参数,其后为红,绿和蓝.
        Color FestoBlue = Color.FromArgb(200, 0, 145, 220);//第1个参数为透明度(alpha)参数,其后为红,绿和蓝.
        Color FestoBlue_Dark = Color.FromArgb(200, 114, 196, 239);//第1个参数为透明度(alpha)参数,其后为红,绿和蓝.
        double X_Min = 100000;
        double X_Max = 0;
        double Y_Min = 100000;
        double Y_Max = 0;

        double X_Min2 = 100000;     //历史曲线的查看
        double X_Max2 = 0;          //历史曲线的查看
        double Y_Min2 = 100000;     //历史曲线的查看
        double Y_Max2 = 0;          //历史曲线的查看

        FileInfo[] arrFi_CurPath;
        YJKP_Log CurLog = new YJKP_Log();

        #region Log结构
        public class YJKP_Log
        {
            public int PartNo;
            public string CurProgramName = "";
            public string CurResult = "";
            public double Position_Max;
            public double Force_Max;
            public Curve[] Curves = new Curve[5];
            public VariablesHost[] VariablesHosts = new VariablesHost[100];
            public Recipes Recipes_Cur = new Recipes();
        }

        public class Window
        {
            public bool b_Active;

            public bool b_Config;

            public bool b_Config_MinPosition;
            public double MinPosition;
            public int MinPosition_Index;

            public bool b_Config_MaxPosition;
            public double MaxPosition;
            public int MaxPosition_Index;

            public bool b_Config_MinForce;
            public double MinForce;
            public int MinForce_Index;

            public bool b_Config_MaxForce;
            public double MaxForce;
            public int MaxForce_Index;

            public EdgeStatus_Window EdgeStatus_D;
            public EdgeStatus_Window EdgeStatus_U;
            public EdgeStatus_Window EdgeStatus_L;
            public EdgeStatus_Window EdgeStatus_R;
        }

        public enum EdgeStatus_Window { NotCare, Forbidden, In, Out }

        public class Threshold
        {
            public bool b_Active;

            public bool b_Config;

            public bool b_Mode;

            public bool b_Config_Position;
            public double Position;
            public int Position_Index;

            public bool b_Config_MinPosition;
            public double MinPosition;
            public int MinPosition_Index;

            public bool b_Config_MaxPosition;
            public double MaxPosition;
            public int MaxPosition_Index;

            public bool b_Config_Force;
            public double Force;
            public int Force_Index;

            public bool b_Config_MinForce;
            public double MinForce;
            public int MinForce_Index;

            public bool b_Config_MaxForce;
            public double MaxForce;
            public int MaxForce_Index;

            public int EdgeStatus_Threshold;
        }

        public class Envelope
        {
            public bool b_Active;

            public bool b_Config;

            public int Count_Up;

            public EnvelopePoint[] EnvelopePoints_U = new EnvelopePoint[5];

            public int Count_Down;

            public EnvelopePoint[] EnvelopePoints_D = new EnvelopePoint[5];
        }

        public struct EnvelopePoint
        {
            public bool b_Config_Position;
            public double Position;
            public int Position_Index;
            public bool b_Config_Force;
            public double Force;
            public int Force_Index;
        }

        public struct Curve
        {
            public double Position_Max;
            public double Force_Max;
            public double Position_Min;
            public double Force_Min;
            public double Position_Start;
        }

        public struct VariablesHost
        {
            public int No;
            public double Value;
        }

        public class Windowing
        {
            public bool b_Active;
            public Window[] Windows = new Window[5];
        }

        public class Thresholding
        {
            public bool b_Active;
            public Threshold[] Thresholds = new Threshold[5];
        }

        public class Envelopeing
        {
            public bool b_Active;
            public Envelope[] Envelopes = new Envelope[5];
        }

        public class Recipes
        {
            public int No;
            public Windowing mWindow = new Windowing();
            public Thresholding mThreshold = new Thresholding();
            public Envelopeing mEnvelope = new Envelopeing();
        }
        #endregion

        int Count_OK = 0;
        int Count_NOK = 0;
        int Count_Total = 0;

        public class mPoint
        {
            public int Index;
            public double Position;
            public double Force;
            public mPoint()
            {
                Index = 0;
                Position = 0;
                Force = 0;
            }
            public mPoint(int mIndex, double mPosition, double mForce)
            {
                Index = mIndex;
                Position = mPosition;
                Force = mForce;
            }
            public mPoint(string mIndex, string mPosition, string mForce)
            {
                Index = Convert.ToInt32(mIndex);
                Position = Convert.ToDouble(mPosition);
                Force = Convert.ToDouble(mForce);
            }
        }
        int ConcernedRecordIndex = 1;//Record1 or Record2
        List<mPoint> mPoints1 = new List<mPoint>();
        List<mPoint> mPoints2 = new List<mPoint>();
        List<mPoint> mPoints3 = new List<mPoint>();
        List<mPoint> mPoints4 = new List<mPoint>();
        List<mPoint> mPoints5 = new List<mPoint>();

        Bitmap chart1BackImage;
        Bitmap chart2BackImage;

        #region 配置文件中读取的参数
        public string WatchPath
        {
            get
            {
                if (File.Exists("config.ini"))
                {
                    IniParser ini = new IniParser("config.ini");
                    if (!ini.KeyExists("WatchPath"))
                    {
                        return "";
                    }
                    string s_Path = ini.GetSetting("WatchPath");
                    try
                    {
                        return s_Path;
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public string FileControl
        {
            get
            {
                if (File.Exists("config.ini"))
                {
                    IniParser ini = new IniParser("config.ini");
                    if (!ini.KeyExists("FileControl"))
                    {
                        return "";
                    }
                    string s_Path = ini.GetSetting("FileControl");
                    try
                    {
                        return s_Path;
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
        } 
        #endregion

        private static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        private void Form_Customized_Load(object sender, EventArgs e)
        {
            DirectoryInfo folder = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
            //获取文件夹下所有的文件
            FileInfo[] fileList = folder.GetFiles();
            foreach (FileInfo file in fileList)
            {
                //判断文件的扩展名是否为 .gif
                if (file.Extension == ".bmp")
                {
                    file.Delete();  // 删除
                }
            }
            this.Text = this.Text + "   V" + Assembly.GetExecutingAssembly().GetName().Version + "";
            btn_CurrentCurve_Click(sender, e);
            fileSystemWatcher1.IncludeSubdirectories = false;
            fileSystemWatcher1.Created += new FileSystemEventHandler(fileSystemWatcher1_Created);
            fileSystemWatcher1.Changed += new FileSystemEventHandler(fileSystemWatcher1_Changed);
            if (WatchPath == "")
            {
                MessageBox.Show("未设置监控路径，请设置");
                new Form_ProcessViewConfig1(this, CurLog.CurProgramName).Show();
            }
            else if (Directory.Exists(WatchPath))
            {
                fileSystemWatcher1.Path = WatchPath;
                fileSystemWatcher1.EnableRaisingEvents = true;
            }
            else
            {
                MessageBox.Show("不存在的监控路径,请确认");
                new Form_ProcessViewConfig1(this, CurLog.CurProgramName).Show();
            }
            chart1.ChartAreas[0].BackImageWrapMode = ChartImageWrapMode.Scaled;
            chart1.ChartAreas[0].AxisX.Title = "Position[mm]";
            chart1.ChartAreas[0].AxisY.Title = "Force[N]";
            chart2.ChartAreas[0].BackImageWrapMode = ChartImageWrapMode.Scaled;
            chart2.ChartAreas[0].AxisX.Title = "Position[mm]";
            chart2.ChartAreas[0].AxisY.Title = "Force[N]";
            //chart1.ChartAreas[0].AxisX.Minimum = 10;
            //chart1.ChartAreas[0].AxisX.Maximum = 22;
            //chart1.ChartAreas[0].AxisY.Minimum = -5;
            //chart1.ChartAreas[0].AxisY.Maximum = 350;
            #region.......chart缩放功能.........
            //会导致界面单击后出现不明红线
            ////// Enable range selection and zooming end user interface
            ////chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            ////chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            ////chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            ////chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            ////chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            ////chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

            //将滚动内嵌到坐标轴中
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            // 设置滚动条的大小
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 15;

            // 设置滚动条的按钮的风格
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;

            // 设置自动放大与缩小的最小量
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
            #endregion
            btn_AutoZoom_Click(sender, e);
            btn_CaptureDIsplay_Click(sender, e);
            btn_Lock_Click(sender, e);
            chart2.Click += new System.EventHandler(this.chart2_Click);
            chart2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart2_MouseMove);
            chart2.MouseWheel += new MouseEventHandler(chart2_MouseWheel);
        }

        void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            if (System.IO.Path.GetExtension(e.Name) == ".log" || System.IO.Path.GetExtension(e.Name) == ".LOG" || System.IO.Path.GetExtension(e.Name) == ".Log")
            {
                txt_CurRecordName.Text = e.Name;
                #region 解析Log文件
                CurLog = new YJKP_Log();

                //文件可能被重命名，无法提取信息
                //CurLog.CurProgramName = e.Name.Split('_')[0];
                //CurLog.CurResult = e.Name.Split('_')[e.Name.Split('_').Length - 1].Substring(0, e.Name.Split('_')[e.Name.Split('_').Length - 1].Length - 4);

                lbl_Value.Text = "";//当前位置值清空
                get_Points(e.FullPath);//解析Log日志

                lbl_Result.Text = CurLog.CurResult;
                if (CurLog.CurResult == "OK" || CurLog.CurResult == "Ok" || CurLog.CurResult == "ok")
                {
                    lbl_Result.BackColor = Color.ForestGreen;
                    Count_OK = Count_OK + 1;
                    Count_Total = Count_Total + 1;
                }
                else
                {
                    lbl_Result.BackColor = Color.Red;
                    Count_NOK = Count_NOK + 1;
                    Count_Total = Count_Total + 1;
                }
                lbl_CountNG.Text = Count_NOK.ToString();
                lbl_CountOK.Text = Count_OK.ToString();
                lbl_CountTotal.Text = Count_Total.ToString();

                if (btn_BundlePlot.BackColor != FestoBlue_Light)//仅显示最近一条记录
                {
                    //清除历史曲线
                    chart1.Series.Clear();
                    //清除之前的最值
                    X_Min = 100000;
                    X_Max = 0;
                    Y_Min = 100000;
                    Y_Max = 0;
                }

                //有时记录了多条曲线，绘制部分曲线（例如下压过程中的曲线）
                switch (ConcernedRecordIndex)
                {
                    case 1:
                        DrawCurve(mPoints1, chart1);
                        break;
                    case 2:
                        DrawCurve(mPoints2, chart1);
                        break;
                    case 3:
                        DrawCurve(mPoints3, chart1);
                        break;
                    case 4:
                        DrawCurve(mPoints4, chart1);
                        break;
                    case 5:
                        DrawCurve(mPoints5, chart1);
                        break;
                    default:
                        break;
                }
                chart1.ChartAreas[0].AxisX.Minimum = Math.Floor(X_Min);
                chart1.ChartAreas[0].AxisX.Maximum = Math.Ceiling(X_Max);
                chart1.ChartAreas[0].AxisY.Minimum = Math.Floor(Y_Min) < 0 ? Math.Floor(Y_Min) : 0;
                chart1.ChartAreas[0].AxisY.Maximum = Math.Ceiling(Y_Max);
                DrawCaptures(chart1);
                #endregion
                #region 整理文件夹
                FileInfo fif = new FileInfo(e.FullPath);
                switch (FileControl)
                {
                    case "Day":
                        if (!Directory.Exists(fif.Directory + "//" + fif.CreationTime.ToString("yyyyMMdd")))
                        {
                            Directory.CreateDirectory(fif.Directory + "//" + fif.CreationTime.ToString("yyyyMMdd"));
                        }
                        File.Move(e.FullPath, System.IO.Path.GetDirectoryName(e.FullPath) + "//" + fif.CreationTime.ToString("yyyyMMdd") + "//" + e.Name);
                        break;
                    case "Week":
                        int weekNum = ((fif.CreationTime.DayOfYear - new DateTime(fif.CreationTime.Year, 1, 1).DayOfWeek - fif.CreationTime.DayOfWeek) / 7 + 2);
                        if (!Directory.Exists(fif.Directory + "//" + fif.CreationTime.ToString("yyyy") + " W" + weekNum.ToString()))
                        {
                            Directory.CreateDirectory(fif.Directory + "//" + fif.CreationTime.ToString("yyyy") + " W" + weekNum.ToString());
                        }
                        File.Move(e.FullPath, System.IO.Path.GetDirectoryName(e.FullPath) + "//" + fif.CreationTime.ToString("yyyy") + " W" + weekNum.ToString() + "//" + e.Name);
                        break;                    
                    case "Month":
                        if (!Directory.Exists(fif.Directory + "//" + fif.CreationTime.ToString("yyyyMM")))
                        {
                            Directory.CreateDirectory(fif.Directory + "//" + fif.CreationTime.ToString("yyyyMM"));
                        }
                        File.Move(e.FullPath, System.IO.Path.GetDirectoryName(e.FullPath) + "//" + fif.CreationTime.ToString("yyyyMM") + "//" + e.Name);
                        break;
                    case "Program":
                        if (!Directory.Exists(fif.Directory + "\\" + CurLog.CurProgramName))
                        {
                            Directory.CreateDirectory(fif.Directory + "\\" + CurLog.CurProgramName);
                        }
                        File.Move(e.FullPath, System.IO.Path.GetDirectoryName(e.FullPath) + "\\" + CurLog.CurProgramName + "\\" + e.Name);
                        break;
                    default:
                        try
                        {
                            //if (FileControl.Split(' ')[1] == "Days")
                            //{
                            //    DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetDirectoryName(e.FullPath));

                            //    FileInfo[] arrFi = di.GetFiles("*.log");
                            //    SortAsFileCreationTime(ref arrFi);

                            //    DateTime OldFileCreateTime = arrFi[0].CreationTime;
                            //}
                            if (FileControl.Split(' ')[1] == "Files")
                            {
                                if (!Directory.Exists(fif.Directory + "\\log"))
                                {
                                    Directory.CreateDirectory(fif.Directory + "\\log");
                                }
                                DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetDirectoryName(e.FullPath));
                                DirectoryInfo di_log = new DirectoryInfo(System.IO.Path.GetDirectoryName(e.FullPath) + "\\log");

                                FileInfo[] arrFi = di.GetFiles("*.log");
                                SortAsFileCreationTime(ref arrFi);

                                for (int i = 0; i < arrFi.Length; i++)
                                {
                                    di_log = new DirectoryInfo(System.IO.Path.GetDirectoryName(e.FullPath) + "\\log");
                                    if (di_log.GetFiles("*.log").Length < Convert.ToInt32(FileControl.Split(' ')[0]))
                                    {
                                        //move
                                        try
                                        {
                                            File.Move(arrFi[i].FullName, System.IO.Path.GetDirectoryName(e.FullPath) + "\\log\\" + arrFi[i].Name);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.ToString());
                                        }
                                    }
                                    else if (di_log.GetFiles("*.log").Length == Convert.ToInt32(FileControl.Split(' ')[0]))
                                    {
                                        //rename & new
                                        try
                                        {
                                            FileInfo[] arrFi_log = di_log.GetFiles("*.log");
                                            SortAsFileCreationTime(ref arrFi_log);
                                            di_log.MoveTo(System.IO.Path.GetDirectoryName(e.FullPath) + "\\log_" + arrFi_log[arrFi_log.Length - 1].CreationTime.ToString("yyyy-MM-dd-HH_mm_ss"));
                                            Directory.CreateDirectory(fif.Directory + "\\log");
                                            File.Move(arrFi[i].FullName, System.IO.Path.GetDirectoryName(e.FullPath) + "\\log\\" + arrFi[i].Name);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.ToString());
                                        }
                                    }

                                }
                                //DateTime OldFileCreateTime = arrFi[0].CreationTime;

                            }
                        }
                        catch (Exception)
                        {

                        }
                        break;
                }
                #endregion
            }
        }

        /// <summary>
        /// C#按创建时间排序
        /// </summary>
        /// <param name="arrFi">待排序数组</param>
        private void SortAsFileCreationTime(ref FileInfo[] arrFi)
        {
            //Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return y.CreationTime.CompareTo(x.CreationTime); });//（倒序）
            Array.Sort(arrFi, delegate(FileInfo x, FileInfo y) { return x.CreationTime.CompareTo(y.CreationTime); });//（顺序）
        }

        /// <summary>
        /// C#按文件夹夹创建时间排序（顺序）
        /// </summary>
        /// <param name="dirs">待排序文件夹数组</param>
        private void SortAsFolderCreationTime(ref DirectoryInfo[] dirs)
        {
            Array.Sort(dirs, delegate(DirectoryInfo x, DirectoryInfo y) { return x.CreationTime.CompareTo(y.CreationTime); });
        }

        //加载log日志解析
        public void get_Points(string FileName)
        {
            #region 等文件写入完成
            while (true)
            {
                try
                {
                    using (StreamReader sReader = new StreamReader(FileName))
                    {
                        //if (stream != null)
                        break;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Output file {0} not yet ready ({1})", FileName, ex.Message));
                }
                System.Threading.Thread.Sleep(500);
            }
            #endregion
            try
            {
                using (StreamReader sReader = new StreamReader(FileName))
                {
                    mPoints1 = new List<mPoint>();
                    mPoints2 = new List<mPoint>();
                    mPoints3 = new List<mPoint>();
                    mPoints4 = new List<mPoint>();
                    mPoints5 = new List<mPoint>();
                    chart1BackImage = new Bitmap(chart1.Width, chart1.Height);
                    chart2BackImage = new Bitmap(chart2.Width, chart2.Height);
                    while (sReader.Peek() >= 0)
                    {
                        string mStr = sReader.ReadLine();
                        if (mStr.StartsWith("[Part no.];"))
                        {
                            mStr = sReader.ReadLine();
                            string[] Array_mStr = mStr.Split(';');
                            CurLog.PartNo = Convert.ToInt32(Array_mStr[0]);
                            CurLog.CurProgramName = Array_mStr[1];
                            if (Array_mStr[4] == "FALSE" || Array_mStr[4] == "False" || Array_mStr[4] == "false")
                            {
                                CurLog.CurResult = "NOK";
                            }
                            else
                            {
                                CurLog.CurResult = "OK";
                            }
                            CurLog.Position_Max = Convert.ToDouble(Array_mStr[5]);
                            CurLog.Force_Max = Convert.ToDouble(Array_mStr[6]);
                        }
                        else if (mStr.StartsWith("[Variables]"))
                        {
                            #region Variables
                            for (int i = 0; i < 100; i++)
                            {
                                mStr = sReader.ReadLine();
                                string[] Array_mStr = mStr.Split(';');
                                CurLog.VariablesHosts[i].No = Convert.ToInt16(Array_mStr[0]);
                                try
                                {
                                    CurLog.VariablesHosts[i].Value = Convert.ToDouble(Array_mStr[1]);
                                }
                                catch (Exception)
                                {
                                }
                            }
                            #endregion
                        }
                        else if (mStr.StartsWith("[Recipes]"))
                        {
                            mStr = sReader.ReadLine();
                            CurLog.Recipes_Cur.No = Convert.ToInt16(mStr.Split(';')[1]);
                        }
                        else if (mStr.StartsWith("[Curves]"))
                        {
                            mStr = sReader.ReadLine();//No.
                            mStr = sReader.ReadLine();//[Curve 1]
                            ConcernedRecordIndex = Convert.ToInt16(mStr.Split(' ')[1].Substring(0, 1));
                            mStr = sReader.ReadLine();//[Max. position]
                            mStr = sReader.ReadLine();
                            string[] Array_mStr = mStr.Split(';');
                            CurLog.Curves[ConcernedRecordIndex].Position_Max = Convert.ToDouble(mStr.Split(';')[0]);
                            CurLog.Curves[ConcernedRecordIndex].Force_Max = Convert.ToDouble(mStr.Split(';')[1]);
                            CurLog.Curves[ConcernedRecordIndex].Position_Min = Convert.ToDouble(mStr.Split(';')[2]);
                            CurLog.Curves[ConcernedRecordIndex].Force_Min = Convert.ToDouble(mStr.Split(';')[3]);
                            CurLog.Curves[ConcernedRecordIndex].Position_Start = Convert.ToDouble(mStr.Split(';')[4]);
                        }
                        else if (mStr.StartsWith("[Windowing]"))
                        {
                            #region Windowing
                            mStr = sReader.ReadLine();
                            if (CurLog.Recipes_Cur.mWindow.b_Active = Convert.ToBoolean(mStr.Split(';')[1]))
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    CurLog.Recipes_Cur.mWindow.Windows[i] = new Window();
                                    mStr = sReader.ReadLine();//[Window i]
                                    mStr = sReader.ReadLine();//Active:;FALSE
                                    if (CurLog.Recipes_Cur.mWindow.Windows[i].b_Active = Convert.ToBoolean(mStr.Split(';')[1]))
                                    {
                                        mStr = sReader.ReadLine();//[Config.]....
                                        mStr = sReader.ReadLine();
                                        string[] Array_mStr = mStr.Split(';');
                                        CurLog.Recipes_Cur.mWindow.Windows[i].b_Config = Convert.ToBoolean(Array_mStr[0]);

                                        CurLog.Recipes_Cur.mWindow.Windows[i].b_Config_MinPosition = Convert.ToBoolean(Array_mStr[1]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].MinPosition = Convert.ToDouble(Array_mStr[2]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].MinPosition_Index = Convert.ToInt16(Array_mStr[3]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].b_Config_MaxPosition = Convert.ToBoolean(Array_mStr[4]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].MaxPosition = Convert.ToDouble(Array_mStr[5]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].MaxPosition_Index = Convert.ToInt16(Array_mStr[6]);

                                        CurLog.Recipes_Cur.mWindow.Windows[i].b_Config_MinForce = Convert.ToBoolean(Array_mStr[7]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].MinForce = Convert.ToDouble(Array_mStr[8]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].MinForce_Index = Convert.ToInt16(Array_mStr[9]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].b_Config_MaxForce = Convert.ToBoolean(Array_mStr[10]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].MaxForce = Convert.ToDouble(Array_mStr[11]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].MaxForce_Index = Convert.ToInt16(Array_mStr[12]);

                                        mStr = sReader.ReadLine();//[Config.]....
                                        mStr = sReader.ReadLine();
                                        Array_mStr = mStr.Split(';');
                                        CurLog.Recipes_Cur.mWindow.Windows[i].EdgeStatus_D = (EdgeStatus_Window)Convert.ToInt16(Array_mStr[0]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].EdgeStatus_U = (EdgeStatus_Window)Convert.ToInt16(Array_mStr[1]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].EdgeStatus_L = (EdgeStatus_Window)Convert.ToInt16(Array_mStr[2]);
                                        CurLog.Recipes_Cur.mWindow.Windows[i].EdgeStatus_R = (EdgeStatus_Window)Convert.ToInt16(Array_mStr[3]);
                                    }
                                    else
                                    {
                                        mStr = sReader.ReadLine();//[Config.]....
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();//[Config.]....
                                        mStr = sReader.ReadLine();
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (mStr.StartsWith("[Threshold]"))
                        {
                            #region Threshold
                            mStr = sReader.ReadLine();
                            if (CurLog.Recipes_Cur.mThreshold.b_Active = Convert.ToBoolean(mStr.Split(';')[1]))
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    CurLog.Recipes_Cur.mThreshold.Thresholds[i] = new Threshold();
                                    mStr = sReader.ReadLine();//[Threshold i]
                                    mStr = sReader.ReadLine();//Active:;FALSE
                                    if (CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Active = Convert.ToBoolean(mStr.Split(';')[1]))
                                    {
                                        mStr = sReader.ReadLine();//[Config.]....
                                        mStr = sReader.ReadLine();
                                        string[] Array_mStr = mStr.Split(';');
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Config = Convert.ToBoolean(Array_mStr[0]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Mode = Convert.ToBoolean(Array_mStr[1]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Config_Position = Convert.ToBoolean(Array_mStr[2]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].Position = Convert.ToDouble(Array_mStr[3]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].Position_Index = Convert.ToInt16(Array_mStr[4]);

                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Config_MinPosition = Convert.ToBoolean(Array_mStr[5]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].MinPosition = Convert.ToDouble(Array_mStr[6]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].MinPosition_Index = Convert.ToInt16(Array_mStr[7]);

                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Config_MaxPosition = Convert.ToBoolean(Array_mStr[8]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].MaxPosition = Convert.ToDouble(Array_mStr[9]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].MaxPosition_Index = Convert.ToInt16(Array_mStr[10]);

                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Config_Force = Convert.ToBoolean(Array_mStr[11]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].Force = Convert.ToDouble(Array_mStr[12]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].Force_Index = Convert.ToInt16(Array_mStr[13]);

                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Config_MinForce = Convert.ToBoolean(Array_mStr[14]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].MinForce = Convert.ToDouble(Array_mStr[15]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].MinForce_Index = Convert.ToInt16(Array_mStr[16]);

                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Config_MaxForce = Convert.ToBoolean(Array_mStr[17]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].MaxForce = Convert.ToDouble(Array_mStr[18]);
                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].MaxForce_Index = Convert.ToInt16(Array_mStr[19]);

                                        CurLog.Recipes_Cur.mThreshold.Thresholds[i].EdgeStatus_Threshold = Convert.ToInt16(Array_mStr[20]);
                                    }
                                    else
                                    {
                                        mStr = sReader.ReadLine();//[Config.]....
                                        mStr = sReader.ReadLine();
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (mStr.StartsWith("[Envelope]"))
                        {
                            #region Envelope
                            mStr = sReader.ReadLine();
                            if (CurLog.Recipes_Cur.mEnvelope.b_Active = Convert.ToBoolean(mStr.Split(';')[1]))
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    CurLog.Recipes_Cur.mEnvelope.Envelopes[i] = new Envelope();
                                    mStr = sReader.ReadLine();//[Envelope i]
                                    mStr = sReader.ReadLine();//Active:;FALSE
                                    if (CurLog.Recipes_Cur.mEnvelope.Envelopes[i].b_Active = Convert.ToBoolean(mStr.Split(';')[1]))
                                    {
                                        mStr = sReader.ReadLine();//[Config.];FALSE
                                        CurLog.Recipes_Cur.mEnvelope.Envelopes[i].b_Config = Convert.ToBoolean(mStr.Split(';')[1]);
                                        mStr = sReader.ReadLine();//Points up side:;2
                                        CurLog.Recipes_Cur.mEnvelope.Envelopes[i].Count_Up = Convert.ToInt16(mStr.Split(';')[1]);
                                        for (int j = 0; j < 5; j++)
                                        {
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j] = new EnvelopePoint();
                                            mStr = sReader.ReadLine();//j;FALSE;0.0;1;FALSE;0.0;1
                                            string[] Array_mStr = mStr.Split(';');
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j].b_Config_Position = Convert.ToBoolean(Array_mStr[1]);
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j].Position = Convert.ToDouble(Array_mStr[2]);
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j].Position_Index = Convert.ToInt16(Array_mStr[3]);
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j].b_Config_Force = Convert.ToBoolean(Array_mStr[4]);
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j].Force = Convert.ToDouble(Array_mStr[5]);
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j].Force_Index = Convert.ToInt16(Array_mStr[6]);
                                        }
                                        mStr = sReader.ReadLine();//Points down side:;2
                                        CurLog.Recipes_Cur.mEnvelope.Envelopes[i].Count_Down = Convert.ToInt16(mStr.Split(';')[1]);
                                        for (int j = 0; j < 5; j++)
                                        {
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j] = new EnvelopePoint();
                                            mStr = sReader.ReadLine();//j;FALSE;0.0;1;FALSE;0.0;1
                                            string[] Array_mStr = mStr.Split(';');
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j].b_Config_Position = Convert.ToBoolean(Array_mStr[1]);
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j].Position = Convert.ToDouble(Array_mStr[2]);
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j].Position_Index = Convert.ToInt16(Array_mStr[3]);
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j].b_Config_Force = Convert.ToBoolean(Array_mStr[4]);
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j].Force = Convert.ToDouble(Array_mStr[5]);
                                            CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j].Force_Index = Convert.ToInt16(Array_mStr[6]);
                                        }
                                    }
                                    else
                                    {
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                        mStr = sReader.ReadLine();
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (mStr.StartsWith("[Record "))
                        {
                            int recordIndex = Convert.ToInt32(mStr.Split(' ')[1].Substring(0, mStr.Split(' ')[1].Length - 1));
                            mStr = sReader.ReadLine();//No. points:;1923
                            int recordLength = Convert.ToInt32(mStr.Split(';')[1]);
                            mStr = sReader.ReadLine();//[Point];[Position];[Force]
                            #region 将所有的点录入List
                            for (int i = 0; i < recordLength; i++)
                            {
                                mStr = sReader.ReadLine();
                                switch (recordIndex)
                                {
                                    case 1:
                                        mPoints1.Add(new mPoint(mStr.Split(';')[0], mStr.Split(';')[1], mStr.Split(';')[2]));
                                        break;
                                    case 2:
                                        mPoints2.Add(new mPoint(mStr.Split(';')[0], mStr.Split(';')[1], mStr.Split(';')[2]));
                                        break;
                                    case 3:
                                        mPoints3.Add(new mPoint(mStr.Split(';')[0], mStr.Split(';')[1], mStr.Split(';')[2]));
                                        break;
                                    case 4:
                                        mPoints4.Add(new mPoint(mStr.Split(';')[0], mStr.Split(';')[1], mStr.Split(';')[2]));
                                        break;
                                    case 5:
                                        mPoints5.Add(new mPoint(mStr.Split(';')[0], mStr.Split(';')[1], mStr.Split(';')[2]));
                                        break;
                                    default:
                                        break;
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DrawCurve(List<mPoint> PointsList, Chart mChart)
        {
            System.Windows.Forms.DataVisualization.Charting.Series mSeries = new System.Windows.Forms.DataVisualization.Charting.Series();
            mSeries.ChartArea = "ChartArea1";
            mSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            mSeries.IsVisibleInLegend = false;
            mSeries.Legend = "Legend1";
            mSeries.Name = "Series" + (mChart.Series.Count + 1).ToString();

            if (mChart==chart1)
            {
                for (int i = 0; i < PointsList.Count; i++)
                {
                    mSeries.Points.AddXY(PointsList[i].Position, PointsList[i].Force);
                    if (PointsList[i].Position > X_Max)
                    {
                        X_Max = PointsList[i].Position;
                    }
                    if (PointsList[i].Position < X_Min)
                    {
                        X_Min = PointsList[i].Position;
                    }
                    if (PointsList[i].Force > Y_Max)
                    {
                        Y_Max = PointsList[i].Force;
                    }
                    if (PointsList[i].Force < Y_Min)
                    {
                        Y_Min = PointsList[i].Force;
                    }
                }
                if (mChart.Series.Count > MaxCurves)
                {
                    mChart.Series[mChart.Series.Count % MaxCurves] = mSeries;
                }
                else
                {
                    mChart.Series.Add(mSeries);
                }
            }
            else
            {
                for (int i = 0; i < PointsList.Count; i++)
                {
                    mSeries.Points.AddXY(PointsList[i].Position, PointsList[i].Force);
                    if (PointsList[i].Position > X_Max2)
                    {
                        X_Max2 = PointsList[i].Position;
                    }
                    if (PointsList[i].Position < X_Min2)
                    {
                        X_Min2 = PointsList[i].Position;
                    }
                    if (PointsList[i].Force > Y_Max2)
                    {
                        Y_Max2 = PointsList[i].Force;
                    }
                    if (PointsList[i].Force < Y_Min2)
                    {
                        Y_Min2 = PointsList[i].Force;
                    }
                }
                mChart.Series.Add(mSeries);
            }
           
        }

        #region DrawCaptures
        private void DrawThresholds(Chart mChart)
        {
            if (CurLog.Recipes_Cur.mThreshold.b_Active)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Active)
                    {
                        DrawThreshold(i,mChart);
                    }
                }
            }
        }

        private void DrawThreshold(int i, Chart mChart)
        {
            Graphics g = Graphics.FromImage(chart1BackImage);
            if (mChart == chart2)
            {
                g = Graphics.FromImage(chart2BackImage);
            }
            if (CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Mode)
            {
                //force
                float StartX, StartY, EndX, EndY;//

                EndY = StartY = (float)mChart.ChartAreas[0].AxisY.ValueToPixelPosition(CurLog.Recipes_Cur.mThreshold.Thresholds[i].Force);
                if (CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Config)
                {
                    //相对
                    StartX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mThreshold.Thresholds[i].MinPosition + CurLog.Curves[ConcernedRecordIndex].Position_Start);
                    EndX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mThreshold.Thresholds[i].MaxPosition + CurLog.Curves[ConcernedRecordIndex].Position_Start);
                }
                else
                {
                    //绝对
                    StartX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mThreshold.Thresholds[i].MinPosition);
                    EndX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mThreshold.Thresholds[i].MaxPosition);
                }

                g.DrawLine(new Pen(Color.Blue, 1), StartX, StartY, EndX, EndY);
                g.DrawLine(new Pen(Color.Blue, 1), StartX, StartY - 3, StartX, StartY + 3);
                g.DrawLine(new Pen(Color.Blue, 1), EndX, EndY - 3, EndX, EndY + 3);
                switch (CurLog.Recipes_Cur.mThreshold.Thresholds[i].EdgeStatus_Threshold)
                {
                    case 1://Down
                        g.DrawLine(new Pen(Color.Blue, 1), (StartX + EndX) / 2 - 3, StartY, (StartX + EndX) / 2, StartY + 5);
                        g.DrawLine(new Pen(Color.Blue, 1), (StartX + EndX) / 2 + 3, StartY, (StartX + EndX) / 2, StartY + 5);
                        break;
                    case 0://up
                        g.DrawLine(new Pen(Color.Blue, 1), (StartX + EndX) / 2 - 3, StartY, (StartX + EndX) / 2, StartY - 5);
                        g.DrawLine(new Pen(Color.Blue, 1), (StartX + EndX) / 2 + 3, StartY, (StartX + EndX) / 2, StartY - 5);     
                        break;                  
                    default:
                        break;
                }
            }
            else
            {
                //position
                float StartX, StartY, EndX, EndY;
                if (CurLog.Recipes_Cur.mThreshold.Thresholds[i].b_Config)
                {
                    //相对
                    EndX = StartX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mThreshold.Thresholds[i].Position + CurLog.Curves[ConcernedRecordIndex].Position_Start);
                }
                else
                {
                    //绝对
                    EndX = StartX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mThreshold.Thresholds[i].Position);
                }

                StartY = (float)mChart.ChartAreas[0].AxisY.ValueToPixelPosition(CurLog.Recipes_Cur.mThreshold.Thresholds[i].MinForce);
                EndY = (float)mChart.ChartAreas[0].AxisY.ValueToPixelPosition(CurLog.Recipes_Cur.mThreshold.Thresholds[i].MaxForce);
                g.DrawLine(new Pen(Color.Blue, 1), StartX, StartY, EndX, EndY);
                g.DrawLine(new Pen(Color.Blue, 1), StartX - 3, StartY, StartX + 3, StartY);
                g.DrawLine(new Pen(Color.Blue, 1), EndX - 3, EndY, EndX + 3, EndY);
                switch (CurLog.Recipes_Cur.mThreshold.Thresholds[i].EdgeStatus_Threshold)
                {
                    case 3://Right

                        g.DrawLine(new Pen(Color.Blue, 1), StartX, (StartY + EndY) / 2 - 3, StartX + 5, (StartY + EndY) / 2);
                        g.DrawLine(new Pen(Color.Blue, 1), StartX, (StartY + EndY) / 2 + 3, StartX + 5, (StartY + EndY) / 2);
                        break;
                    case 2://Left
                        g.DrawLine(new Pen(Color.Blue, 1), StartX, (StartY + EndY) / 2 - 3, StartX - 5, (StartY + EndY) / 2);
                        g.DrawLine(new Pen(Color.Blue, 1), StartX, (StartY + EndY) / 2 + 3, StartX - 5, (StartY + EndY) / 2);

                        break;
                    default:
                        break;
                }
            }
            g.Dispose();
        }

        private void DrawEnvelopes(Chart mChart)
        {
            if (CurLog.Recipes_Cur.mEnvelope.b_Active)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (CurLog.Recipes_Cur.mEnvelope.Envelopes[i].b_Active)
                    {
                        DrawEnvelope(i, mChart);
                    }
                }
            }
        }

        private void DrawEnvelope(int i, Chart mChart)
        {
            Graphics g = Graphics.FromImage(chart1BackImage);
            if (mChart == chart2)
            {
                g = Graphics.FromImage(chart2BackImage);
            }
            for (int j = 1; j < CurLog.Recipes_Cur.mEnvelope.Envelopes[i].Count_Up; j++)
            {
                float StartX, StartY, EndX, EndY;

                StartX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j - 1].Position);
                StartY = (float)mChart.ChartAreas[0].AxisY.ValueToPixelPosition(CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j - 1].Force);
                EndX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j].Position);
                EndY = (float)mChart.ChartAreas[0].AxisY.ValueToPixelPosition(CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_U[j].Force);

                g.DrawLine(new Pen(Color.Blue, 1), StartX, StartY, EndX, EndY);
            }
            for (int j = 1; j < CurLog.Recipes_Cur.mEnvelope.Envelopes[i].Count_Down; j++)
            {
                float StartX, StartY, EndX, EndY;
                StartX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j - 1].Position);
                StartY = (float)mChart.ChartAreas[0].AxisY.ValueToPixelPosition(CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j - 1].Force);
                EndX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j].Position);
                EndY = (float)mChart.ChartAreas[0].AxisY.ValueToPixelPosition(CurLog.Recipes_Cur.mEnvelope.Envelopes[i].EnvelopePoints_D[j].Force);
                g.DrawLine(new Pen(Color.Blue, 1), StartX, StartY, EndX, EndY);
            }
            g.Dispose();
        }

        private void DrawWindows(Chart mChart)
        {
            if (CurLog.Recipes_Cur.mWindow.b_Active)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (CurLog.Recipes_Cur.mWindow.Windows[i].b_Active)
                    {
                        DrawWindow(i, mChart);
                    }
                }
            }
        }

        private void DrawWindow(int i, Chart mChart)
        {
            //Methods a
            //Graphics dc = mChart.CreateGraphics();
            //Show();
            //Pen bluePen = new Pen(Color.Blue, 3);
            //dc.DrawRectangle(bluePen, 100, 100, 50, 50);

            //Methods b area.AxisX.PixelPositionToValue(e.X);
            Graphics g = Graphics.FromImage(chart1BackImage);
            if (mChart == chart2)
            {
                g = Graphics.FromImage(chart2BackImage);
            }
            float RectangularX, RectangularY, RectangularWidth, RectangularHeight;

            if (CurLog.Recipes_Cur.mWindow.Windows[i].b_Config)
            {
                //相对
                RectangularX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mWindow.Windows[i].MinPosition + CurLog.Curves[ConcernedRecordIndex].Position_Start);
                RectangularWidth = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mWindow.Windows[i].MaxPosition + CurLog.Curves[ConcernedRecordIndex].Position_Start) - RectangularX;
            }
            else
            {
                //绝对
                RectangularX = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mWindow.Windows[i].MinPosition);
                RectangularWidth = (float)mChart.ChartAreas[0].AxisX.ValueToPixelPosition(CurLog.Recipes_Cur.mWindow.Windows[i].MaxPosition) - RectangularX;
            }
            RectangularY = (float)mChart.ChartAreas[0].AxisY.ValueToPixelPosition(CurLog.Recipes_Cur.mWindow.Windows[i].MaxForce);
            RectangularHeight = (float)mChart.ChartAreas[0].AxisY.ValueToPixelPosition(CurLog.Recipes_Cur.mWindow.Windows[i].MinForce) - RectangularY;

            g.DrawRectangle(new Pen(Color.Blue, 1), RectangularX, RectangularY, RectangularWidth, RectangularHeight);
            switch (CurLog.Recipes_Cur.mWindow.Windows[i].EdgeStatus_L)
            {
                case EdgeStatus_Window.NotCare:
                    break;
                case EdgeStatus_Window.Forbidden:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX - 3, RectangularY + RectangularHeight / 2 - 3, RectangularX + 3, RectangularY + RectangularHeight / 2 + 3);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX - 3, RectangularY + RectangularHeight / 2 + 3, RectangularX + 3, RectangularY + RectangularHeight / 2 - 3);
                    break;
                case EdgeStatus_Window.In:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX, RectangularY + RectangularHeight / 2 - 3, RectangularX + 5, RectangularY + RectangularHeight / 2);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX, RectangularY + RectangularHeight / 2 + 3, RectangularX + 5, RectangularY + RectangularHeight / 2);
                    break;
                case EdgeStatus_Window.Out:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX, RectangularY + RectangularHeight / 2 - 3, RectangularX - 5, RectangularY + RectangularHeight / 2);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX, RectangularY + RectangularHeight / 2 + 3, RectangularX - 5, RectangularY + RectangularHeight / 2);
                    break;
                default:
                    break;
            }
            switch (CurLog.Recipes_Cur.mWindow.Windows[i].EdgeStatus_U)
            {
                case EdgeStatus_Window.NotCare:
                    break;
                case EdgeStatus_Window.Forbidden:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 - 3, RectangularY - 3, RectangularX + RectangularWidth / 2 + 3, RectangularY + 3);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 - 3, RectangularY + 3, RectangularX + RectangularWidth / 2 + 3, RectangularY - 3);
                    break;
                case EdgeStatus_Window.In:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 - 3, RectangularY, RectangularX + RectangularWidth / 2, RectangularY + 5);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 + 3, RectangularY, RectangularX + RectangularWidth / 2, RectangularY + 5);
                    break;
                case EdgeStatus_Window.Out:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 - 3, RectangularY, RectangularX + RectangularWidth / 2, RectangularY - 5);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 + 3, RectangularY, RectangularX + RectangularWidth / 2, RectangularY - 5);
                    break;
                default:
                    break;
            }
            switch (CurLog.Recipes_Cur.mWindow.Windows[i].EdgeStatus_R)
            {
                case EdgeStatus_Window.NotCare:
                    break;
                case EdgeStatus_Window.Forbidden:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth - 3, RectangularY + RectangularHeight / 2 - 3, RectangularX + RectangularWidth + 3, RectangularY + RectangularHeight / 2 + 3);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth - 3, RectangularY + RectangularHeight / 2 + 3, RectangularX + RectangularWidth + 3, RectangularY + RectangularHeight / 2 - 3);
                    break;
                case EdgeStatus_Window.In:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth, RectangularY + RectangularHeight / 2 - 3, RectangularX + RectangularWidth - 5, RectangularY + RectangularHeight / 2);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth, RectangularY + RectangularHeight / 2 + 3, RectangularX + RectangularWidth - 5, RectangularY + RectangularHeight / 2);
                    break;
                case EdgeStatus_Window.Out:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth, RectangularY + RectangularHeight / 2 - 3, RectangularX + RectangularWidth + 5, RectangularY + RectangularHeight / 2);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth, RectangularY + RectangularHeight / 2 + 3, RectangularX + RectangularWidth + 5, RectangularY + RectangularHeight / 2);
                    break;
                default:
                    break;
            }
            switch (CurLog.Recipes_Cur.mWindow.Windows[i].EdgeStatus_D)
            {
                case EdgeStatus_Window.NotCare:
                    break;
                case EdgeStatus_Window.Forbidden:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 - 3, RectangularY + RectangularHeight - 3, RectangularX + RectangularWidth / 2 + 3, RectangularY + RectangularHeight + 3);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 - 3, RectangularY + RectangularHeight + 3, RectangularX + RectangularWidth / 2 + 3, RectangularY + RectangularHeight - 3);
                    break;
                case EdgeStatus_Window.In:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 - 3, RectangularY + RectangularHeight, RectangularX + RectangularWidth / 2, RectangularY + RectangularHeight - 5);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 + 3, RectangularY + RectangularHeight, RectangularX + RectangularWidth / 2, RectangularY + RectangularHeight - 5);
                    break;
                case EdgeStatus_Window.Out:
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 - 3, RectangularY + RectangularHeight, RectangularX + RectangularWidth / 2, RectangularY + RectangularHeight + 5);
                    g.DrawLine(new Pen(Color.Blue, 1), RectangularX + RectangularWidth / 2 + 3, RectangularY + RectangularHeight, RectangularX + RectangularWidth / 2, RectangularY + RectangularHeight + 5);
                    break;
                default:
                    break;
            }
            g.Dispose();

        }

        private void DrawCaptures(Chart mChart)
        {
            try
            {
                DrawWindows(mChart);
                DrawThresholds(mChart);
                DrawEnvelopes(mChart);
                mChart.ChartAreas[0].BackImage = "";
                if (mChart == chart2)
                {
                    bmpName = "Capture2_" + DateTime.Now.ToString("HHmmss") + DateTime.Now.Millisecond.ToString() + ".bmp";
                    chart1BackImage.Save(bmpName);
                }
                else
                {
                    bmpName = "Capture1_" + DateTime.Now.ToString("HHmmss") + DateTime.Now.Millisecond.ToString() + ".bmp";
                    chart1BackImage.Save(bmpName);
                }

                if (btn_CaptureDIsplay.BackColor == FestoBlue_Light)
                {
                    mChart.ChartAreas[0].BackImage = bmpName;
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void DrawRectangular(double x1, double y1, double x2, double y2, Chart mChart)
        {
            //Methods a
            //Graphics dc = chart1.CreateGraphics();
            //Show();
            //Pen bluePen = new Pen(Color.Blue, 3);
            //dc.DrawRectangle(bluePen, 100, 100, 50, 50);

            //Methods b area.AxisX.PixelPositionToValue(e.X);
            Bitmap b;
            if (mChart == chart2)
            {
                b = new Bitmap(chart2.Width, chart2.Height);
            }
            else
            {
                b = new Bitmap(chart1.Width, chart1.Height);
            }
            Graphics g = Graphics.FromImage(b);
            float RectangularX, RectangularY, RectangularWidth, RectangularHeight;
            if ((float)chart1.ChartAreas[0].AxisX.ValueToPixelPosition(x1) < (float)chart1.ChartAreas[0].AxisX.ValueToPixelPosition(x2))
            {
                RectangularX = (float)chart1.ChartAreas[0].AxisX.ValueToPixelPosition(x1);
                RectangularWidth = (float)chart1.ChartAreas[0].AxisX.ValueToPixelPosition(x2) - (float)chart1.ChartAreas[0].AxisX.ValueToPixelPosition(x1);
            }
            else
            {
                RectangularX = (float)chart1.ChartAreas[0].AxisX.ValueToPixelPosition(x2);
                RectangularWidth = (float)chart1.ChartAreas[0].AxisX.ValueToPixelPosition(x1) - (float)chart1.ChartAreas[0].AxisX.ValueToPixelPosition(x2);
            }
            if ((float)chart1.ChartAreas[0].AxisY.ValueToPixelPosition(y1) < (float)chart1.ChartAreas[0].AxisY.ValueToPixelPosition(y2))
            {
                RectangularY = (float)chart1.ChartAreas[0].AxisY.ValueToPixelPosition(y1);
                RectangularHeight = (float)chart1.ChartAreas[0].AxisY.ValueToPixelPosition(y2) - RectangularY;
            }
            else
            {
                RectangularY = (float)chart1.ChartAreas[0].AxisY.ValueToPixelPosition(y2);
                RectangularHeight = (float)chart1.ChartAreas[0].AxisY.ValueToPixelPosition(y1) - RectangularY;
            }
            g.DrawRectangle(new Pen(Color.Blue, 2), RectangularX, RectangularY, RectangularWidth, RectangularHeight);
            g.Dispose();
            chart1.ChartAreas[0].BackImage = "";
            bmpName = "Capture" + DateTime.Now.ToString("HHmmss") + ".bmp";
            b.Save(bmpName);
            if (btn_CaptureDIsplay.BackColor == FestoBlue_Light)
            {
                chart1.ChartAreas[0].BackImage = bmpName;
            }

        }
        
        #endregion

        //显示Festo网页界面
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Form_Web(new Form_Init().WebLink).Show();
        }

        #region 页面切换
        private void btn_CurrentCurve_Click(object sender, EventArgs e)
        {
            btn_CurrentCurve.BackColor = FestoBlue_Light;
            //btn_CurrentCurve.FlatAppearance.BorderColor = FestoBlue;
            btn_HIstoricalCurves.BackColor = System.Drawing.SystemColors.Control;
            //btn_HIstoricalCurves.FlatAppearance.BorderColor = Color.White;
            panel_CUR.Location = panel1.Location;
            panel_CUR.Size = panel1.Size;
            panel_CUR.BringToFront();
        }

        private void btn_HIstoricalCurves_Click(object sender, EventArgs e)
        {
            btn_HIstoricalCurves.BackColor = FestoBlue_Light;
            //btn_HIstoricalCurves.FlatAppearance.BorderColor = FestoBlue;
            btn_CurrentCurve.BackColor = System.Drawing.SystemColors.Control;
            //btn_CurrentCurve.FlatAppearance.BorderColor = Color.White;
            panel_HIST.Location = panel1.Location;
            panel_HIST.Size = panel1.Size;
            panel_HIST.BringToFront();
        }
        #endregion

        //弹出当前曲线设置窗口
        private void btn_CurrentCurve_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 's')
            {
                //弹出当前曲线设置窗口
                Form_ProcessViewConfig1 mForm_ProcessViewConfig = new Form_ProcessViewConfig1(this, CurLog.CurProgramName);
                mForm_ProcessViewConfig.Show();
                mForm_ProcessViewConfig.BringToFront();
            }
        }

        #region Button BackColor Setting
        private void btn_BundlePlot_Click(object sender, EventArgs e)
        {
            if (btn_BundlePlot.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_BundlePlot.BackColor = FestoBlue_Light;
            }
            else
            {
                btn_BundlePlot.BackColor = System.Drawing.SystemColors.Control;
            }
            btn_CurrentCurve.Focus();
        }

        //【互斥】自动缩放模式
        private void btn_AutoZoom_Click(object sender, EventArgs e)
        {
            if (btn_AutoZoom.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_AutoZoom.BackColor = FestoBlue_Light;
                btn_Enlarge.BackColor = System.Drawing.SystemColors.Control;
                btn_Reduce.BackColor = System.Drawing.SystemColors.Control;
                btn_Move.BackColor = System.Drawing.SystemColors.Control;
            }
            else
            {
                //已经是自动缩放模式，显示手动上下限设置的框
                try
                {
                    txt_MinX.Text = chart1.ChartAreas[0].AxisX.Minimum.ToString();
                    txt_MaxX.Text = chart1.ChartAreas[0].AxisX.Maximum.ToString();
                    txt_MinY.Text = chart1.ChartAreas[0].AxisY.Minimum.ToString();
                    txt_MaxY.Text = chart1.ChartAreas[0].AxisY.Maximum.ToString();
                }
                catch (Exception)
                {

                }
                txt_MinX.Location = new Point(txt_MinX.Location.X, chart1.Size.Height - txt_MinX.Size.Height - 5);
                txt_MaxX.Location = new Point(txt_MaxX.Location.X, chart1.Size.Height - txt_MinX.Size.Height - 5);
                txt_MinX.Visible = true;
                txt_MaxX.Visible = true;
                txt_MinY.Visible = true;
                txt_MaxY.Visible = true;
            }
            chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset();
            //if (txt_MinX.Text==""||txt_MaxX.Text==""||txt_MinY.Text==""||txt_MaxY.Text=="")
            //{
            //    chart1.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            //    chart1.ChartAreas[0].AxisY.ScaleView.ZoomReset();
            //}
            //else
            //{
            //    chart1.ChartAreas[0].AxisX.ScaleView.Zoom(Convert.ToDouble(txt_MinX.Text), Convert.ToDouble(txt_MaxX.Text));
            //    chart1.ChartAreas[0].AxisY.ScaleView.Zoom(Convert.ToDouble(txt_MinY.Text), Convert.ToDouble(txt_MaxY.Text));
            //}
            btn_CurrentCurve.Focus();
        }

        //【互斥】放大模式
        private void btn_Enlarge_Click(object sender, EventArgs e)
        {
            if (btn_Enlarge.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_Enlarge.BackColor = FestoBlue_Light;
                btn_AutoZoom.BackColor = System.Drawing.SystemColors.Control;
                btn_Reduce.BackColor = System.Drawing.SystemColors.Control;
                btn_Move.BackColor = System.Drawing.SystemColors.Control;
                txt_MinX.Visible = false;
                txt_MaxX.Visible = false;
                txt_MinY.Visible = false;
                txt_MaxY.Visible = false;
            }
            else
            {
                //btn_Enlarge.BackColor = System.Drawing.SystemColors.Control;
            }
            btn_CurrentCurve.Focus();
        }

        //【互斥】缩小模式
        private void btn_Reduce_Click(object sender, EventArgs e)
        {
            if (btn_Reduce.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_Reduce.BackColor = FestoBlue_Light;
                btn_Enlarge.BackColor = System.Drawing.SystemColors.Control;
                btn_AutoZoom.BackColor = System.Drawing.SystemColors.Control;
                btn_Move.BackColor = System.Drawing.SystemColors.Control;
                txt_MinX.Visible = false;
                txt_MaxX.Visible = false;
                txt_MinY.Visible = false;
                txt_MaxY.Visible = false;
            }
            else
            {
                //btn_Reduce.BackColor = System.Drawing.SystemColors.Control;
            }
            btn_CurrentCurve.Focus();
        }

        //【互斥】移动模式
        private void btn_Move_Click(object sender, EventArgs e)
        {
            if (btn_Move.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_Move.BackColor = FestoBlue_Light;
                btn_Enlarge.BackColor = System.Drawing.SystemColors.Control;
                btn_Reduce.BackColor = System.Drawing.SystemColors.Control;
                btn_AutoZoom.BackColor = System.Drawing.SystemColors.Control;
                txt_MinX.Visible = false;
                txt_MaxX.Visible = false;
                txt_MinY.Visible = false;
                txt_MaxY.Visible = false;
            }
            else
            {
                //btn_Move.BackColor = System.Drawing.SystemColors.Control;
            }
            btn_CurrentCurve.Focus();
        }

        private void btn_CaptureDIsplay_Click(object sender, EventArgs e)
        {
            if (btn_CaptureDIsplay.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_CaptureDIsplay.BackColor = FestoBlue_Light;
                if (chart1.Series.Count > 0)
                {
                    DrawCaptures(chart1);
                }
                //chart1.ChartAreas[0].BackImage = bmpName;
            }
            else
            {
                btn_CaptureDIsplay.BackColor = System.Drawing.SystemColors.Control;
                chart1.ChartAreas[0].BackImage = "";
            }
            btn_CurrentCurve.Focus();
        }
        #endregion

        #region chart事件
        private void chart1_Click(object sender, EventArgs e)
        {
            if (btn_Enlarge.BackColor == FestoBlue_Light)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.Zoom(Math.Round(chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum - 0.9 * (chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum - chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum), 3),
                                                      Math.Round(chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum + 0.9 * (chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum - chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum), 3));
            }
            else if (btn_Reduce.BackColor == FestoBlue_Light)
            {
                chart1.ChartAreas[0].AxisX.ScaleView.Size += 0.1;
            }

        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                HitTestResult hit = chart1.HitTest(e.X, e.Y);
                if (hit.Series != null)
                {
                    var xValue = hit.Series.Points[hit.PointIndex].XValue;
                    var yValue = hit.Series.Points[hit.PointIndex].YValues.First();
                    lbl_Value.ForeColor = Color.Orange;
                    lbl_Value.Text = string.Format("{0:F0}{1:F0}", "Position:" + xValue, "(mm),Force:" + yValue + "(N)");
                }
                else
                {
                    var area = chart1.ChartAreas[0];
                    double xValue = area.AxisX.PixelPositionToValue(e.X);
                    double yValue = area.AxisY.PixelPositionToValue(e.Y);
                    lbl_Value.ForeColor = Color.Black;
                    lbl_Value.Text = string.Format("{0:F0}{1:F0}", "Position:" + Math.Round(xValue, 2), "(mm),Force:" + Math.Round(yValue, 3) + "(N)");
                }
            }
            catch (Exception)
            {

            }
        }

        void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            //按住Ctrl，缩放
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {

                if (chart1.ChartAreas[0].AxisX.ScaleView.Size.ToString() == "NaN")
                {
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = 1;
                }
                else
                {
                    if (e.Delta < 0)
                        chart1.ChartAreas[0].AxisX.ScaleView.Size += 4;
                    else
                    {
                        try
                        {
                            if (chart1.ChartAreas[0].AxisX.ScaleView.Size > 4)
                            {
                                chart1.ChartAreas[0].AxisX.ScaleView.Size -= 4;
                            }
                            else
                            {
                                //MessageBox.Show("MIN");
                            }
                        }
                        catch (Exception)
                        {
                            chart1.ChartAreas[0].AxisX.ScaleView.Size = 0;
                        }
                    }
                }

            }
            //不按Ctrl，滚动
            else
            {
                if (e.Delta < 0)
                {
                    if (chart1.ChartAreas[0].AxisX.ScaleView.Position + chart1.ChartAreas[0].AxisX.ScaleView.Size < X_Max)
                    {
                        chart1.ChartAreas[0].AxisX.ScaleView.Position += 2;
                    }
                }
                else
                {
                    if (chart1.ChartAreas[0].AxisX.ScaleView.Position > X_Min)
                    {
                        chart1.ChartAreas[0].AxisX.ScaleView.Position -= 2;
                    }
                }
            }
        }

        private void chart2_Click(object sender, EventArgs e)
        {
            if (btn_Enlarge.BackColor == FestoBlue_Light)
            {
                chart2.ChartAreas[0].AxisX.ScaleView.Zoom(Math.Round(chart2.ChartAreas[0].AxisX.ScaleView.ViewMaximum - 0.9 * (chart2.ChartAreas[0].AxisX.ScaleView.ViewMaximum - chart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum), 3),
                                                      Math.Round(chart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum + 0.9 * (chart2.ChartAreas[0].AxisX.ScaleView.ViewMaximum - chart2.ChartAreas[0].AxisX.ScaleView.ViewMinimum), 3));
            }
            else if (btn_Reduce.BackColor == FestoBlue_Light)
            {
                chart2.ChartAreas[0].AxisX.ScaleView.Size += 0.1;
            }

        }

        private void chart2_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                HitTestResult hit = chart2.HitTest(e.X, e.Y);
                if (hit.Series != null)
                {
                    var xValue = hit.Series.Points[hit.PointIndex].XValue;
                    var yValue = hit.Series.Points[hit.PointIndex].YValues.First();
                    lbl_Value2.ForeColor = Color.Orange;
                    lbl_Value2.Text = string.Format("{0:F0}{1:F0}", "Position:" + xValue, "(mm),Force:" + yValue + "(N)");
                }
                else
                {
                    var area = chart2.ChartAreas[0];
                    double xValue = area.AxisX.PixelPositionToValue(e.X);
                    double yValue = area.AxisY.PixelPositionToValue(e.Y);
                    lbl_Value2.ForeColor = Color.Black;
                    lbl_Value2.Text = string.Format("{0:F0}{1:F0}", "Position:" + Math.Round(xValue, 2), "(mm),Force:" + Math.Round(yValue, 3) + "(N)");
                }
            }
            catch (Exception)
            {

            }
        }

        void chart2_MouseWheel(object sender, MouseEventArgs e)
        {
            //按住Ctrl，缩放
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {

                if (chart2.ChartAreas[0].AxisX.ScaleView.Size.ToString() == "NaN")
                {
                    chart2.ChartAreas[0].AxisX.ScaleView.Size = 1;
                }
                else
                {
                    if (e.Delta < 0)
                        chart2.ChartAreas[0].AxisX.ScaleView.Size += 4;
                    else
                    {
                        try
                        {
                            if (chart2.ChartAreas[0].AxisX.ScaleView.Size > 4)
                            {
                                chart2.ChartAreas[0].AxisX.ScaleView.Size -= 4;
                            }
                            else
                            {
                                //MessageBox.Show("MIN");
                            }
                        }
                        catch (Exception)
                        {
                            chart2.ChartAreas[0].AxisX.ScaleView.Size = 0;
                        }
                    }
                }

            }
            //不按Ctrl，滚动
            else
            {
                if (e.Delta < 0)
                {
                    if (chart2.ChartAreas[0].AxisX.ScaleView.Position + chart2.ChartAreas[0].AxisX.ScaleView.Size < X_Max)
                    {
                        chart2.ChartAreas[0].AxisX.ScaleView.Position += 2;
                    }
                }
                else
                {
                    if (chart2.ChartAreas[0].AxisX.ScaleView.Position > X_Min)
                    {
                        chart2.ChartAreas[0].AxisX.ScaleView.Position -= 2;
                    }
                }
            }
        }

        #endregion

        private void Form_Customized_Resize(object sender, EventArgs e)
        {
            if (btn_CurrentCurve.BackColor != System.Drawing.SystemColors.Control)
            {
                btn_CurrentCurve_Click(sender, e);
            }
            else
            {
                btn_HIstoricalCurves_Click(sender, e);
            }
        }

        #region 指定上下限
        private void txt_MaxY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                chart1.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(txt_MaxY.Text);
                //chart1.ChartAreas[0].AxisY.ScaleView.Zoom(Convert.ToDouble(txt_MinY.Text),Convert.ToDouble(txt_MaxY.Text));
            }
        }

        private void txt_MinY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                chart1.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(txt_MinY.Text);
                //chart1.ChartAreas[0].AxisY.ScaleView.Zoom(Convert.ToDouble(txt_MinY.Text), Convert.ToDouble(txt_MaxY.Text));
            }
        }

        private void txt_MinX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                chart1.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(txt_MinX.Text);
                //chart1.ChartAreas[0].AxisX.ScaleView.Zoom(Convert.ToDouble(txt_MinX.Text), Convert.ToDouble(txt_MaxX.Text));
            }
        }

        private void txt_MaxX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                chart1.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(txt_MaxX.Text);
                //chart1.ChartAreas[0].AxisX.ScaleView.Zoom(Convert.ToDouble(txt_MinX.Text), Convert.ToDouble(txt_MaxX.Text));
            }
        }
        #endregion

        private void btn_Lock_Click(object sender, EventArgs e)
        {
            if (btn_Lock.ImageIndex == 0)
            {
                btn_Lock.ImageIndex = 1;//解锁
                btn_AutoZoom.Enabled = true;
                btn_BundlePlot.Enabled = true;
                btn_CaptureDIsplay.Enabled = true;
                btn_Enlarge.Enabled = true;
                btn_Move.Enabled = true;
                btn_Reduce.Enabled = true;
                chart1.Click += new System.EventHandler(this.chart1_Click);
                chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
                chart1.MouseWheel += new MouseEventHandler(chart1_MouseWheel);
            }
            else
            {
                btn_Lock.ImageIndex = 0;//锁定
                btn_AutoZoom.Enabled = false;
                btn_BundlePlot.Enabled = false;
                btn_CaptureDIsplay.Enabled = false;
                btn_Enlarge.Enabled = false;
                btn_Move.Enabled = false;
                btn_Reduce.Enabled = false;
                chart1.Click -= new System.EventHandler(this.chart1_Click);
                chart1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
                chart1.MouseWheel -= new MouseEventHandler(chart1_MouseWheel);
                lbl_Value.Text = "";
            }
        }

        private void Form_Customized_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void txt_LogPath_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_LogPath.Text = folderBrowserDialog1.SelectedPath;                
            }
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            if (txt_LogPath.Text.Replace(" ","")=="")
            {
                MessageBox.Show("请先选择文件路径！");
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(txt_LogPath.Text);
                arrFi_CurPath = di.GetFiles("*.log");
                trackBar1.Minimum = 1;
                trackBar1.Maximum = arrFi_CurPath.Length;
                trackBar1.Value = 1;
                int RecordCount;
                //清除历史曲线
                chart2.Series.Clear();
                //清除之前的最值
                X_Min2 = 100000;
                X_Max2 = 0;
                Y_Min2 = 100000;
                Y_Max2 = 0;
                if (btn_BundlePlot2.BackColor != FestoBlue_Light)//仅显示最近一条记录
                {
                    RecordCount = 1;
                    lbl_CurRecordName.Text = arrFi_CurPath[0].Name;
                    lbl_Last.Text = "";
                    lbl_Next.Text = arrFi_CurPath[1].Name;
                }
                else
                {
                    RecordCount = arrFi_CurPath.Length;
                    lbl_CurRecordName.Text = "";
                    lbl_Last.Text = "";
                    lbl_Next.Text = "";
                }
                for (int i = 0; i < RecordCount; i++)
                {
                    #region 解析Log文件
                    CurLog = new YJKP_Log();

                    //文件可能被重命名，无法提取信息
                    //CurLog.CurProgramName = e.Name.Split('_')[0];
                    //CurLog.CurResult = e.Name.Split('_')[e.Name.Split('_').Length - 1].Substring(0, e.Name.Split('_')[e.Name.Split('_').Length - 1].Length - 4);

                    lbl_Value2.Text = "";//当前位置值清空
                    get_Points(arrFi_CurPath[i].FullName);//解析Log日志

                    lbl_Result.Text = CurLog.CurResult;
                    if (CurLog.CurResult == "OK" || CurLog.CurResult == "Ok" || CurLog.CurResult == "ok")
                    {
                        lbl_Result.BackColor = Color.ForestGreen;

                    }
                    else
                    {
                        lbl_Result.BackColor = Color.Red;

                    }

                    //有时记录了多条曲线，绘制部分曲线（例如下压过程中的曲线）
                    switch (ConcernedRecordIndex)
                    {
                        case 1:
                            DrawCurve(mPoints1, chart2);
                            break;
                        case 2:
                            DrawCurve(mPoints2, chart2);
                            break;
                        case 3:
                            DrawCurve(mPoints3, chart2);
                            break;
                        case 4:
                            DrawCurve(mPoints4, chart2);
                            break;
                        case 5:
                            DrawCurve(mPoints5, chart2);
                            break;
                        default:
                            break;
                    }
                    chart2.ChartAreas[0].AxisX.Minimum = Math.Floor(X_Min2);
                    chart2.ChartAreas[0].AxisX.Maximum = Math.Ceiling(X_Max2);
                    chart2.ChartAreas[0].AxisY.Minimum = Math.Floor(Y_Min2) < 0 ? Math.Floor(Y_Min2) : 0;
                    chart2.ChartAreas[0].AxisY.Maximum = Math.Ceiling(Y_Max2);
                    DrawCaptures(chart2);
                    #endregion
                }
            }
        }

        #region 简单控件
        private void btn_AutoZoom2_Click(object sender, EventArgs e)
        {
            if (btn_AutoZoom2.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_AutoZoom2.BackColor = FestoBlue_Light;
                btn_Enlarge2.BackColor = System.Drawing.SystemColors.Control;
                btn_Reduce2.BackColor = System.Drawing.SystemColors.Control;
                btn_Move2.BackColor = System.Drawing.SystemColors.Control;
            }
            else
            {
                //已经是自动缩放模式，显示手动上下限设置的框
                try
                {
                    txt_MinX_HIst.Text = chart2.ChartAreas[0].AxisX.Minimum.ToString();
                    txt_MaxX_HIst.Text = chart2.ChartAreas[0].AxisX.Maximum.ToString();
                    txt_MinY_Hist.Text = chart2.ChartAreas[0].AxisY.Minimum.ToString();
                    txt_MaxY_Hist.Text = chart2.ChartAreas[0].AxisY.Maximum.ToString();
                }
                catch (Exception)
                {

                }
                txt_MinX_HIst.Location = new Point(txt_MinX_HIst.Location.X, chart2.Size.Height - txt_MinX_HIst.Size.Height - 5);
                txt_MaxX_HIst.Location = new Point(txt_MaxX_HIst.Location.X, chart2.Size.Height - txt_MinX_HIst.Size.Height - 5);
                txt_MinX_HIst.Visible = true;
                txt_MaxX_HIst.Visible = true;
                txt_MinY_Hist.Visible = true;
                txt_MaxY_Hist.Visible = true;
            }
            chart2.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chart2.ChartAreas[0].AxisY.ScaleView.ZoomReset();
            btn_HIstoricalCurves.Focus();
        }

        private void btn_Enlarge2_Click(object sender, EventArgs e)
        {
            if (btn_Reduce2.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_Reduce2.BackColor = FestoBlue_Light;
                btn_Enlarge2.BackColor = System.Drawing.SystemColors.Control;
                btn_AutoZoom2.BackColor = System.Drawing.SystemColors.Control;
                btn_Move2.BackColor = System.Drawing.SystemColors.Control;
                txt_MinX_HIst.Visible = false;
                txt_MaxX_HIst.Visible = false;
                txt_MinY_Hist.Visible = false;
                txt_MaxY_Hist.Visible = false;
            }
            else
            {
                //btn_Reduce2.BackColor = System.Drawing.SystemColors.Control;
            }
            btn_HIstoricalCurves.Focus();
        }

        private void btn_Reduce2_Click(object sender, EventArgs e)
        {
            if (btn_Reduce2.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_Reduce2.BackColor = FestoBlue_Light;
                btn_Enlarge2.BackColor = System.Drawing.SystemColors.Control;
                btn_AutoZoom2.BackColor = System.Drawing.SystemColors.Control;
                btn_Move2.BackColor = System.Drawing.SystemColors.Control;
                txt_MinX_HIst.Visible = false;
                txt_MaxX_HIst.Visible = false;
                txt_MinY_Hist.Visible = false;
                txt_MaxY_Hist.Visible = false;
            }
            else
            {
                //btn_Reduce2.BackColor = System.Drawing.SystemColors.Control;
            }
            btn_HIstoricalCurves.Focus();
        }

        private void btn_Move2_Click(object sender, EventArgs e)
        {
            if (btn_Reduce2.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_Reduce2.BackColor = FestoBlue_Light;
                btn_Enlarge2.BackColor = System.Drawing.SystemColors.Control;
                btn_AutoZoom2.BackColor = System.Drawing.SystemColors.Control;
                btn_Move2.BackColor = System.Drawing.SystemColors.Control;
                txt_MinX_HIst.Visible = false;
                txt_MaxX_HIst.Visible = false;
                txt_MinY_Hist.Visible = false;
                txt_MaxY_Hist.Visible = false;
            }
            else
            {
                //btn_Reduce2.BackColor = System.Drawing.SystemColors.Control;
            }
            btn_HIstoricalCurves.Focus();
        }

        private void btn_BundlePlot2_Click(object sender, EventArgs e)
        {
            if (btn_BundlePlot2.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_BundlePlot2.BackColor = FestoBlue_Light;
            }
            else
            {
                btn_BundlePlot2.BackColor = System.Drawing.SystemColors.Control;
            }
            btn_HIstoricalCurves.Focus();
        }

        private void btn_CaptureDIsplay2_Click(object sender, EventArgs e)
        {
            if (btn_CaptureDIsplay2.BackColor == System.Drawing.SystemColors.Control)
            {
                btn_CaptureDIsplay2.BackColor = FestoBlue_Light;
                if (chart2.Series.Count > 0)
                {
                    DrawCaptures(chart2);
                }
                //chart1.ChartAreas[0].BackImage = bmpName;
            }
            else
            {
                btn_CaptureDIsplay2.BackColor = System.Drawing.SystemColors.Control;
                chart2.ChartAreas[0].BackImage = "";
            }
            btn_HIstoricalCurves.Focus();
        }

        private void btn_Lock2_Click(object sender, EventArgs e)
        {
            //if (btn_Lock2.ImageIndex == 0)
            //{
            //    btn_Lock2.ImageIndex = 1;//解锁
            //    btn_AutoZoom2.Enabled = true;
            //    btn_BundlePlot2.Enabled = true;
            //    btn_CaptureDIsplay2.Enabled = true;
            //    btn_Enlarge2.Enabled = true;
            //    btn_Move2.Enabled = true;
            //    btn_Reduce2.Enabled = true;
            //    chart2.Click += new System.EventHandler(this.chart2_Click);
            //    chart2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart2_MouseMove);
            //    chart2.MouseWheel += new MouseEventHandler(chart2_MouseWheel);
            //}
            //else
            //{
            //    btn_Lock.ImageIndex = 0;//锁定
            //    btn_AutoZoom.Enabled = false;
            //    btn_BundlePlot.Enabled = false;
            //    btn_CaptureDIsplay.Enabled = false;
            //    btn_Enlarge.Enabled = false;
            //    btn_Move.Enabled = false;
            //    btn_Reduce.Enabled = false;
            //    chart1.Click -= new System.EventHandler(this.chart1_Click);
            //    chart1.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            //    chart1.MouseWheel -= new MouseEventHandler(chart1_MouseWheel);
            //    lbl_Value.Text = "";
            //}
        }

        private void txt_MaxX_HIst_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                chart2.ChartAreas[0].AxisX.Maximum = Convert.ToDouble(txt_MaxX_HIst.Text);
            }
        }

        private void txt_MaxY_Hist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                chart2.ChartAreas[0].AxisY.Maximum = Convert.ToDouble(txt_MaxY_Hist.Text);
            }
        }

        private void txt_MinX_HIst_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                chart2.ChartAreas[0].AxisX.Minimum = Convert.ToDouble(txt_MinX_HIst.Text);
            }
        }

        private void txt_MinY_Hist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                chart2.ChartAreas[0].AxisY.Minimum = Convert.ToDouble(txt_MinY_Hist.Text);
            }
        } 
        #endregion

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (btn_BundlePlot2.BackColor == FestoBlue_Light)
            {
                //醒目当前选中的曲线，更新lbl_next&lbl_last
            }
            else
            {
                //删除当前的曲线和背景评价框，显示新的一条
                //清除历史曲线
                chart2.Series.Clear();
                //清除之前的最值
                X_Min2 = 100000;
                X_Max2 = 0;
                Y_Min2 = 100000;
                Y_Max2 = 0;

                CurLog = new YJKP_Log();

                //文件可能被重命名，无法提取信息
                //CurLog.CurProgramName = e.Name.Split('_')[0];
                //CurLog.CurResult = e.Name.Split('_')[e.Name.Split('_').Length - 1].Substring(0, e.Name.Split('_')[e.Name.Split('_').Length - 1].Length - 4);

                lbl_Value2.Text = "";//当前位置值清空
                get_Points(arrFi_CurPath[trackBar1.Value -1].FullName);//解析Log日志

                lbl_Result.Text = CurLog.CurResult;
                if (CurLog.CurResult == "OK" || CurLog.CurResult == "Ok" || CurLog.CurResult == "ok")
                {
                    lbl_Result.BackColor = Color.ForestGreen;

                }
                else
                {
                    lbl_Result.BackColor = Color.Red;

                }

                //有时记录了多条曲线，绘制部分曲线（例如下压过程中的曲线）
                switch (ConcernedRecordIndex)
                {
                    case 1:
                        DrawCurve(mPoints1, chart2);
                        break;
                    case 2:
                        DrawCurve(mPoints2, chart2);
                        break;
                    case 3:
                        DrawCurve(mPoints3, chart2);
                        break;
                    case 4:
                        DrawCurve(mPoints4, chart2);
                        break;
                    case 5:
                        DrawCurve(mPoints5, chart2);
                        break;
                    default:
                        break;
                }
                chart2.ChartAreas[0].AxisX.Minimum = Math.Floor(X_Min2);
                chart2.ChartAreas[0].AxisX.Maximum = Math.Ceiling(X_Max2);
                chart2.ChartAreas[0].AxisY.Minimum = Math.Floor(Y_Min2) < 0 ? Math.Floor(Y_Min2) : 0;
                chart2.ChartAreas[0].AxisY.Maximum = Math.Ceiling(Y_Max2);
                DrawCaptures(chart2);
            }
            if (trackBar1.Value == trackBar1.Minimum)
            {
                lbl_Last.Text = "";
            }
            else
            {
                lbl_Last.Text = arrFi_CurPath[trackBar1.Value - 2].Name;
            }
            lbl_CurRecordName.Text = arrFi_CurPath[trackBar1.Value - 1].Name;
            if (trackBar1.Value==trackBar1.Maximum)
            {
                lbl_Next.Text = "";
            }
            else
            {
                lbl_Next.Text = arrFi_CurPath[trackBar1.Value].Name;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    

            }
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value <= trackBar1.Maximum - 1)
            {
                trackBar1.Value++;
            }
            if (trackBar1.Value == trackBar1.Maximum)
            {
                btn_Next.Enabled = false;
            }
            else
            {
                btn_Next.Enabled = true;
            }
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            if (trackBar1.Value >= trackBar1.Minimum + 1)
            {
                trackBar1.Value--;
            }
            if (trackBar1.Value == trackBar1.Minimum)
            {
                btn_Back.Enabled = false;
            }
            else
            {
                btn_Back.Enabled = true;
            }
        }
    }
}
