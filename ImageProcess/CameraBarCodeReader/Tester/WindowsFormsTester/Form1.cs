using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAMSS.Forms.Control;
using System.Drawing.Imaging;
namespace WindowsFormsTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.cameraBarCodeReader1.CameraNo = comboBox1.SelectedIndex;
            this.cameraBarCodeReader1.StartCapture();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.comboBox1.Items.Clear();
                //var tsmSelectCamera = new ToolStripMenuItem("Select Camera");
                //this.cmsMenu.Items.Add(tsmSelectCamera);
                this.cameraBarCodeReader1.GetCameraList().ToList().ForEach(c =>
                {
                    this.comboBox1.Items.Add(c.Value);
                    //tsmSelectCamera.DropDownItems.Add(c.Value).Tag = this.comboBox1.Items.Count - 1;
                });
                //tsmSelectCamera.DropDownItemClicked += tsmSelectCamera_DropDownItemClicked;
                this.cameraBarCodeReader1.OnCameraChanging += cameraBarCodeReader1_OnCameraChanging;
                this.cameraBarCodeReader1.OnCameraChanged += cameraBarCodeReader1_OnCameraChanged;
                this.cameraBarCodeReader1.OnBarcodeRead += cameraBarCodeReader1_OnBarcodeRead;
                this.chkDoubleRead.Checked = this.cameraBarCodeReader1.DoubleRead;
                this.nudScanTime.Value = (decimal)this.cameraBarCodeReader1.ReadScanTime;
                this.chkCorrection.Checked = this.cameraBarCodeReader1.Correction;
                this.comboBox1.SelectedIndex = this.cameraBarCodeReader1.CameraNo;
            }
        }

        void cameraBarCodeReader1_OnBarcodeRead(string code)
        {
            MessageBox.Show(code,"Event");
        }

        void cameraBarCodeReader1_OnCameraChanged(CameraChangArgs e)
        {
            comboBox1.SelectedIndex = e.ChangedNo;
            //throw new NotImplementedException();
        }

        void cameraBarCodeReader1_OnCameraChanging(ref CameraChangArgs e)
        {
            //e.Cancel = true;
            //throw new NotImplementedException();
        }

        //void tsmSelectCamera_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{
        //    comboBox1.SelectedIndex = int.Parse(e.ClickedItem.Tag.ToString());
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            this.cameraBarCodeReader1.StopCapture();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.cameraBarCodeReader1.TakeSnapShot();
            var dlg = new SaveFileDialog()
            {
                Title = "保存先のディレクトリとファイル名を指定してください",
                InitialDirectory = @"C:\Projects\TFC\Source\CameraBarCodeReader\Data",
                AddExtension = true,
                OverwritePrompt = true,
                Filter = "*.png|*.png",
                FilterIndex = 0,
                DefaultExt = ".png"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.cameraBarCodeReader1.SaveSnapShot(dlg.FileName, ImageFormat.Png);
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            try
            {
                this.button4.Enabled = false;
                var res = await this.cameraBarCodeReader1.ReadAsync();
                if (res == null)
                {
                    //MessageBox.Show("読み取り失敗");
                }
                else
                {
                    MessageBox.Show(string.Format("[{0}]({1})", res, res.Length), "Return");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.button4.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                this.button5.Enabled = false;
                var res = this.cameraBarCodeReader1.ReadAsync();
                if ((res == null) || (res.Result == null) || (res.Result.Length < 1))
                {
                    //MessageBox.Show("読み取り失敗");
                }
                else 
                {
                    MessageBox.Show(res.Result); 
                }
            }
            finally
            {
                this.button5.Enabled = true;
            }
        }

        private void chkCorrection_CheckedChanged(object sender, EventArgs e)
        {
            this.cameraBarCodeReader1.Correction = this.chkCorrection.Checked;
        }

        private void chkDoubleCheck_CheckedChanged(object sender, EventArgs e)
        {
            this.cameraBarCodeReader1.DoubleRead = this.chkDoubleRead.Checked;
        }

        private void nudScanTime_ValueChanged(object sender, EventArgs e)
        {
            this.cameraBarCodeReader1.ReadScanTime = (long)this.nudScanTime.Value;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
