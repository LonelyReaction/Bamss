using System.Windows.Forms;

namespace BAMSS.UI.SwapPage
{
    public partial class SwapPageChildTextInput2 : SwapPageBase
    {
        public SwapPageChildTextInput2()
        {
            InitializeComponent();
        }
        public override int OnNext()
        {
            return (MessageBox.Show(string.Format("入力された値は、[{0}]でした。", this.txtSettingValue1.Text), "OnNext", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.OK) ? 1 : 0;
        }

        public override int OnPrevious()
        {
            MessageBox.Show("戻ります！", "OnPrevious");
            return -1;
        }

        public override void Action(int stageNo, bool firstStage, bool finalStage, Control previousControl, Control nextControl)
        {
            base.Action(stageNo, firstStage, finalStage, previousControl, nextControl);
            this._nextControl.Text = "次ページ";
            this._previousControl.Text = "前ページ";
            this.txtSettingValue1.Focus();
        }

        public override void Close()
        {
            if (this._closed) return;
            this._closed = true;
        }
    }
}
