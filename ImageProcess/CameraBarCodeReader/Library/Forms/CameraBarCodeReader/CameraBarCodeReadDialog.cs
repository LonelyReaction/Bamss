using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAMSS.Forms.Control
{
    public partial class CameraBarCodeReadDialogSimple : Form
    {
        private string _barcodeData;
        public CameraBarCodeReadDialogSimple(string titleText = "Barcode Reader")
        {
            InitializeComponent();
            this.Text = titleText;
        }
        private async void Read()
        {
            this._barcodeData = await this.cbrBarCode.ReadAsync();
        }
        private void CameraBarCodeReadDialog_Shown(object sender, EventArgs e)
        {
            this.Start();
        }
        private void Start()
        {
            try
            {
                this.cbrBarCode.StartCapture();
                this.Read();
            }
            finally
            {
                this.cbrBarCode.StopCapture();
            }
        }
    }
}
