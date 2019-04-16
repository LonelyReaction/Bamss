using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAMSS.UI.SwapPage;
using System.Reflection;

namespace BAMSS.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.SetupSwapPages();
            this.nudPageNo.Minimum = 0;
            this.nudPageNo.Maximum = this.sscTheater.Pages.Count - 1;
        }
        private void SetupSwapPages()
        {
            var assemblyName = "BAMSS.UI.SwapPageChildTester.dll";
            var spb = new SwapPageScenarioBuilder<SwapPageBase>();
            spb.AddClass(assemblyName, "BAMSS.UI.SwapPage.SwapPageChildTextInput1", null);
            spb.AddClass(assemblyName, "BAMSS.UI.SwapPage.SwapPageChildTextInput2", null);
            spb.AddClass(assemblyName, "BAMSS.UI.SwapPage", "SwapPageChildTextInput1", null);
            spb.AddClass(assemblyName, "BAMSS.UI.SwapPage", "SwapPageChildTextInput2", null);
            this.sscTheater.Pages.AddRange(spb.SwapPageList());
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.sscTheater.Start((int)this.nudPageNo.Value);
        }
        private void btnExit_Click(object sender, EventArgs e) { Application.Exit(); }
    }
}
