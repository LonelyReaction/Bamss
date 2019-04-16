using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BAMSS.Extensions;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                throw new FormatException("This is test message(11).");
            }
            catch (Exception ex)
            {
                ex.WriteLog();
            }
            try
            {
                sub1();
            }
            catch (Exception ex)
            {
                //ex.WriteEventLog((exp, l) =>
                //{
                //    File.WriteAllText(string.Format("Output{0:yyyyMMddhhmmssfff}.txt", DateTime.Now), exp.BuildLogData(l));
                //});
                ex.WriteLog(appendAction: WriteLogFile);
            }
        }
        private void WriteLogFile(Exception ex, int innerLevel)
        {
            File.WriteAllText(string.Format("Output{0:yyyyMMddhhmmssfff}.txt", DateTime.Now), ex.BuildLogData(innerLevel));
        }
        private void sub1()
        {
            try
            {
                try
                {
                    throw new FormatException("This is test message(21).");
                }
                catch (Exception ex)
                {
                    throw new OverflowException("This is test message(22).", ex);
                }
            }
            catch (Exception ex)
            {
                throw new OverflowException("This is test message(23).", ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                sub1();
            }
            catch (Exception ex)
            {
                if (ex.AllExceptions().Where(c => c.GetType() == typeof(FormatException)).Count() > 0)
                {
                    MessageBox.Show("FormatException");
                }
                if (ex.AllExceptions().Where(c => c.GetType() == typeof(FieldAccessException)).Count() > 0)
                {
                    MessageBox.Show("FieldAccessException");
                }
            }
        }
    }
}
