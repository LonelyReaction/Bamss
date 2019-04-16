using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using BAMSS.Diagnostic;

namespace Tester
{
    public partial class Form1 : Form
    {
        private IApplicationDiagRecord _csvRec;
        private ApplicationDiagnostic _diag;
        public Form1()
        {
            InitializeComponent();
            //自プロセスの監視

            //採取処理を予め作っておいて指定する場合。
            bool forMySelf = true;
            if (forMySelf)
            {
                // 自プロセスを対象とする場合。
                this._csvRec = new ProcessDiagCSVRecP01() as IApplicationDiagRecord;
            }
            else
            {
                // プロセスID指定のプロセスを対象とする場合。
                this._csvRec = new ProcessDiagCSVRecP01(Process.GetProcessById(2314)) as IApplicationDiagRecord;
            }
            this._diag = new ApplicationDiagnostic(5, this._csvRec, null, null, null, "MyLog", "01", null);

            //採取処理をインラインで指定する場合。
            //this._diag = new ApplicationDiagnostic(5, new ApplicationDiagRecord("", () => ""));
            
            //採取開始！
            this._diag.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //プロセスを選択して監視
            Process.GetProcesses().Where(p => p.ProcessName.ToUpper().StartsWith("TEST")).ToList().ForEach(
                p => (
                    new ApplicationDiagnostic(
                        5, 
                        new ProcessDiagCSVRecP01(p),
                        null,
                        @"C:\Projects\Bamss\Common\BAMSS\Diagnostic\Gathering\TestLog",
                        "Multi_",
                        "JunYokoe",
                        null,
                        ".CSV"
                    )
                ).Start()
            );
            MessageBox.Show("Test");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this._diag.WriteLog();
        }
    }
}
