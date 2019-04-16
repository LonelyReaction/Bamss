using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelGpaph
{
    public partial class Form1 : Form
    {
        protected Object[,] _readWorkArray;
        public bool OnSheetOpenError { get; protected set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var excelApplication = new Excel.Application();
            try
            {
                excelApplication.DisplayAlerts = true;
                Excel.Workbooks workbooks = excelApplication.Workbooks;
                try
                {
                    Excel.Workbook workbook = workbooks.Open(@"C:\Projects\Bamss\Common\BAMSS\Practice\ExcelGpaph\ExcelGpaph\bin\Debug\Test.xlsx");
                    excelApplication.Visible = true;
                    try
                    {
                        workbook.ForceFullCalculation = true;
                        Excel.Sheets worksheets = workbook.Sheets;
                        try
                        {
                            this.OnSheetOpenError = true;
                            Excel.Worksheet worksheetGRAPH = worksheets["GRAPH"];
                            Excel.Worksheet worksheetDATA = worksheets["DATA"];
                            worksheetGRAPH.Copy(Type.Missing, worksheetGRAPH);
                            Excel.Worksheet appendSheet = worksheets["GRAPH (2)"];
                            appendSheet.Name = string.Format("GRAPH({0})", worksheets.Count - 2);
                            this.OnSheetOpenError = false;

                            /*
                                Sheets("GRAPH-BASE").Select
                                Sheets("GRAPH-BASE").Copy After:=Sheets(3)
                                Sheets("GRAPH-BASE (2)").Select
                                Sheets("GRAPH-BASE (2)").Name = "GRAPH-01"
                                ActiveSheet.ChartObjects("グラフ 1").Activate
                                ActiveSheet.ChartObjects("グラフ 1").Activate
                                ActiveChart.SeriesCollection.NewSeries
                                ActiveChart.SeriesCollection(1).Name = "=DATA!$B$4"
                                ActiveChart.SeriesCollection(1).XValues = "=DATA!$A$5:$A$125"
                                ActiveChart.SeriesCollection(1).Values = "=DATA!$B$5:$B$125"
                                ActiveChart.SeriesCollection.NewSeries
                                ActiveChart.SeriesCollection(2).Name = "=DATA!$C$4"
                                ActiveChart.SeriesCollection(2).XValues = "=DATA!$A$5:$A$125"
                                ActiveChart.SeriesCollection(2).Values = "=DATA!$C$5:$C$125"
                                ActiveChart.SeriesCollection.NewSeries
                                ActiveChart.SeriesCollection(3).Name = "=DATA!$E$4"
                                ActiveChart.SeriesCollection(3).XValues = "=DATA!$A$5:$A$125"
                                ActiveChart.SeriesCollection(3).Values = "=DATA!$E$5:$E$125"
                             */

                            try
                            {
                                Excel.ChartObject chartObj = (Excel.ChartObject)appendSheet.ChartObjects(1);
                                Excel.Chart chart = chartObj.Chart;
                                Excel.Range chartRange = worksheetDATA.Range[worksheetDATA.Cells[4, 1], worksheetDATA.Cells[245, 2]];
                                try
                                {

                                    chart.SetSourceData(chartRange);
                                }
                                catch { }
                                finally
                                {
                                    Marshal.ReleaseComObject(chartRange);
                                    Marshal.ReleaseComObject(chart);
                                    Marshal.ReleaseComObject(chartObj);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                //Marshal.ReleaseComObject(worksheetGRAPH); 
                                //Marshal.ReleaseComObject(worksheetDATA); 
                            }
                        }
                        finally
                        {
                            //Marshal.ReleaseComObject(worksheets); 
                        }
                    }
                    finally
                    {
                        //if (workbook != null) { workbook.Close(false); }
                        //Marshal.ReleaseComObject(workbook);
                    }
                }
                finally
                {
                    //Marshal.ReleaseComObject(workbooks); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //if (excelApplication != null)
                //{
                //    excelApplication.DisplayAlerts = true;
                //    excelApplication.Quit();
                //}
                //Marshal.ReleaseComObject(excelApplication);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var excelApplication = new Excel.Application();
            try
            {
                excelApplication.DisplayAlerts = true;
                Excel.Workbooks workbooks = excelApplication.Workbooks;
                try
                {
                    Excel.Workbook workbook = workbooks.Open(@"C:\Projects\Bamss\Common\BAMSS\Practice\ExcelGpaph\ExcelGpaph\bin\Debug\Test2.xlsx");
                    excelApplication.Visible = true;
                    try
                    {
                        workbook.ForceFullCalculation = true;
                        Excel.Sheets worksheets = workbook.Sheets;
                        try
                        {
                            /*  ≪Excelマクロ記録によるコード≫
                                ActiveSheet.ChartObjects("グラフ 17""""").Activate
                                ActiveChart.SeriesCollection(1).Select
                                Selection.Formula = "=SERIES(ﾃﾞｰﾀｰ!R5C2,ﾃﾞｰﾀｰ!R6C1:R67C1,ﾃﾞｰﾀｰ!R6C2:R67C2,1)"
                                Selection.Formula = "=SERIES(ﾃﾞｰﾀｰ!$B$5,ﾃﾞｰﾀｰ!$A$6:$A$67,ﾃﾞｰﾀｰ!$B$6:$B$67,1)"
                            */
                            this.OnSheetOpenError = true;
                            Excel.Worksheet worksheetGRAPH = worksheets["GRAPH"];
                            Excel.Worksheet worksheetDATA = worksheets["DATA"];
                            worksheetGRAPH.Copy(Type.Missing, worksheetGRAPH);
                            Excel.Worksheet appendSheet = worksheets["GRAPH (2)"];
                            appendSheet.Name = string.Format("GRAPH({0})", worksheets.Count - 2);
                            this.OnSheetOpenError = false;

                            try
                            {
                                Excel.ChartObject chartObj = (Excel.ChartObject)appendSheet.ChartObjects(1);
                                Excel.Chart chart = chartObj.Chart;
                                try
                                {
                                    //SERIES(名前,　横軸(ロット)範囲,　対象値セル範囲,　グラフの表示順[折れ線なので関係ないね])
                                    //例)   SERIES(ﾃﾞｰﾀｰ!R5C2,ﾃﾞｰﾀｰ!R6C1:R67C1,ﾃﾞｰﾀｰ!R6C2:R67C2,1)
                                    //      SERIES(ﾃﾞｰﾀｰ!$B$5,ﾃﾞｰﾀｰ!$A$6:$A$67,ﾃﾞｰﾀｰ!$B$6:$B$67,1)
                                    string targetItemName = "";
                                    foreach (Excel.Series series in chart.SeriesCollection())
                                    {
                                        if (series.Name == targetItemName)
                                        {
                                            series.Formula = "=SERIES(ﾃﾞｰﾀｰ!R5C2,ﾃﾞｰﾀｰ!R6C1:R67C1,ﾃﾞｰﾀｰ!R6C2:R67C2,1)";
                                        }
                                        else
                                        {
                                            switch (series.Name)
                                            {   //この名前も参照してるけど、でとれるかな？
                                                case "+3σ":
                                                    series.Formula = "=SERIES(ﾃﾞｰﾀｰ!R5C2,ﾃﾞｰﾀｰ!R6C1:R67C1,ﾃﾞｰﾀｰ!R6C2:R67C2,1)";
                                                    break;
                                                case "-3σ":
                                                    series.Formula = "=SERIES(ﾃﾞｰﾀｰ!R5C2,ﾃﾞｰﾀｰ!R6C1:R67C1,ﾃﾞｰﾀｰ!R6C2:R67C2,1)";
                                                    break;
                                                case "Max":
                                                    series.Formula = "=SERIES(ﾃﾞｰﾀｰ!R5C2,ﾃﾞｰﾀｰ!R6C1:R67C1,ﾃﾞｰﾀｰ!R6C2:R67C2,1)";
                                                    break;
                                                case "Min":
                                                    series.Formula = "=SERIES(ﾃﾞｰﾀｰ!R5C2,ﾃﾞｰﾀｰ!R6C1:R67C1,ﾃﾞｰﾀｰ!R6C2:R67C2,1)";
                                                    break;
                                                default:
                                                    series.Formula = "=SERIES(ﾃﾞｰﾀｰ!R5C2,ﾃﾞｰﾀｰ!R6C1:R67C1,ﾃﾞｰﾀｰ!R6C2:R67C2,1)";
                                                    break;
                                            }
                                        }
                                    }
                                }
                                catch { }
                                finally
                                {
                                    Marshal.ReleaseComObject(chart);
                                    Marshal.ReleaseComObject(chartObj);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                //Marshal.ReleaseComObject(worksheetGRAPH); 
                                //Marshal.ReleaseComObject(worksheetDATA); 
                            }
                        }
                        finally
                        {
                            //Marshal.ReleaseComObject(worksheets); 
                        }
                    }
                    finally
                    {
                        //if (workbook != null) { workbook.Close(false); }
                        //Marshal.ReleaseComObject(workbook);
                    }
                }
                finally
                {
                    //Marshal.ReleaseComObject(workbooks); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //if (excelApplication != null)
                //{
                //    excelApplication.DisplayAlerts = true;
                //    excelApplication.Quit();
                //}
                //Marshal.ReleaseComObject(excelApplication);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
