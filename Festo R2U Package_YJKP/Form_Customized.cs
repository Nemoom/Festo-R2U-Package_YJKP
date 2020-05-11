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

namespace Festo_R2U_Package_YJKP
{
    public partial class Form_Customized : Form
    {
        public Form_Customized()
        {
            InitializeComponent();
            InitLog4Net();
        }
        int MaxCurves = 10;
        private string CurProgramName = "";
        private string CurResult = "";
        Color FestoBlue_Light = Color.FromArgb(200, 200, 230, 250);//第1个参数为透明度(alpha)参数,其后为红,绿和蓝.
        Color FestoBlue = Color.FromArgb(200, 0, 145, 220);//第1个参数为透明度(alpha)参数,其后为红,绿和蓝.
        Color FestoBlue_Dark = Color.FromArgb(200, 114, 196, 239);//第1个参数为透明度(alpha)参数,其后为红,绿和蓝.
        double X_Min = 100000;
        double X_Max = 0;
        double Y_Min = 100000;
        double Y_Max = 0;

        double Position_Max;
        double Force_Max;
        double Position_Min;
        double Force_Min;

        public class Window 
        {
            public double Intersection;

            public double Position_U;
            public double Force_U;
            public double Position_D;
            public double Force_D;
            public double Position_L;
            public double Force_L;
            public double Position_R;
            public double Force_R;

            public double Position_Max;
            public double Position_Min;
            public double Force_Max;
            public double Force_Min;
        }

        public class Threshold 
        {
            public double Intersection;
            public double Position;
            public double Force;
        }

        public class Envelope 
        {
            public double Intersection;

            public double Position_U;
            public double Force_U;
            public double Position_D;
            public double Force_D;            
        }

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

        private static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(logCfg);
        }

        private void Form_Customized_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + "   V" + Assembly.GetExecutingAssembly().GetName().Version + "";
            btn_CurrentCurve_Click(sender, e);
            fileSystemWatcher1.IncludeSubdirectories = false;
            fileSystemWatcher1.Created += new FileSystemEventHandler(fileSystemWatcher1_Created);
            fileSystemWatcher1.Changed += new FileSystemEventHandler(fileSystemWatcher1_Changed);
            if (WatchPath=="")
            {
                 MessageBox.Show("未设置监控路径，请设置");
                 new Form_ProcessViewConfig1(this,CurProgramName).Show();
            }
            else if (Directory.Exists(WatchPath))
            {
                fileSystemWatcher1.Path = WatchPath;                
                fileSystemWatcher1.EnableRaisingEvents = true;                
            }
            else
            {
                MessageBox.Show("不存在的监控路径,请确认");
                new Form_ProcessViewConfig1(this,CurProgramName).Show();
            }

            chart1.ChartAreas[0].AxisX.Title = "Position[mm]";
            chart1.ChartAreas[0].AxisY.Title = "Force[N]";
            #region.......chart缩放功能.........

            // Enable range selection and zooming end user interface
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;            
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

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
            btn_Lock_Click(sender, e);        

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
                    if (chart1.ChartAreas[0].AxisX.ScaleView.Position + chart1.ChartAreas[0].AxisX.ScaleView.Size< X_Max)
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

        void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            if (System.IO.Path.GetExtension(e.Name) == ".log" || System.IO.Path.GetExtension(e.Name) == ".LOG" || System.IO.Path.GetExtension(e.Name) == ".Log")
            {
                txt_CurRecordName.Text = e.Name;
                CurProgramName = e.Name.Split('_')[0];
                CurResult = e.Name.Split('_')[e.Name.Split('_').Length - 1].Substring(0, e.Name.Split('_')[e.Name.Split('_').Length - 1].Length - 4);
                lbl_Result.Text = CurResult;
                if (CurResult == "OK" || CurResult == "Ok" || CurResult == "ok")
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
                lbl_Value.Text = "";
                get_Points(e.FullPath);
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
                        DrawCurve(mPoints1);
                        break;
                    case 2:
                        DrawCurve(mPoints2);
                        break;
                    default:
                        break;
                }
            }            
        }

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
                    while (sReader.Peek() >= 0)
                    {
                        string mStr = sReader.ReadLine();
                        if (mStr.Length > 8)
                        {
                            if (mStr.Substring(0, 8) == "[Record ")
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
                                        default:
                                            break;
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void DrawCurve(List<mPoint> PointsList)
        {
            System.Windows.Forms.DataVisualization.Charting.Series mSeries = new System.Windows.Forms.DataVisualization.Charting.Series();
            mSeries.ChartArea = "ChartArea1";
            mSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            mSeries.IsVisibleInLegend = false;
            mSeries.Legend = "Legend1";
            mSeries.Name = "Series" + (chart1.Series.Count + 1).ToString();
            
            for (int i = 0; i < PointsList.Count; i++)
            {
                mSeries.Points.AddXY(PointsList[i].Position, PointsList[i].Force);
                if (PointsList[i].Position>X_Max)
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
            if (chart1.Series.Count > MaxCurves)
            {
                chart1.Series[chart1.Series.Count % MaxCurves] = mSeries;
            }
            else
            {
                chart1.Series.Add(mSeries);
            }            
        }

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
            if (e.KeyChar=='s')
            {
                //弹出当前曲线设置窗口
                Form_ProcessViewConfig1 mForm_ProcessViewConfig = new Form_ProcessViewConfig1(this,CurProgramName);
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
            }
            else
            {
                btn_CaptureDIsplay.BackColor = System.Drawing.SystemColors.Control;
            }
            btn_CurrentCurve.Focus();
        }   
        #endregion     

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
                lbl_Value.Text = string.Format("{0:F0}{1:F0}", "Position:" + Math.Round(xValue,2), "(mm),Force:" + Math.Round(yValue,3) + "(N)");

            }
        }

        private void Form_Customized_Resize(object sender, EventArgs e)
        {
            if (btn_CurrentCurve.BackColor!=System.Drawing.SystemColors.Control)
            {
                btn_CurrentCurve_Click(sender, e);
            }
            else
            {
                btn_HIstoricalCurves_Click(sender, e);
            }
        }

        private void txt_MaxY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar=='\r')
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
    }
}
