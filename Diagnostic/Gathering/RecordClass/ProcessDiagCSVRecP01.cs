using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BAMSS.Diagnostic
{
    public class ProcessDiagCSVRecP01 : IApplicationDiagRecord
    {
        private Process _process;
        private static readonly string _header = "LogTime,Id,SessionId,BasePriority,StartTime,CommandLine,HandleCount,ThreadCount,TotalProcessorTime,UserProcessorTime,PrivateMemorySize,WorkingSet,MaxWorkingSet,PeakWorkingSet,VirtualMemorySize,PeakVirtualMemorySize" + Environment.NewLine;
        private static readonly string FailedRec = "***プロセス情報は取得に失敗しました。***";
        public ProcessDiagCSVRecP01(Process process = null)
        {
            this._process = process ?? Process.GetCurrentProcess();
        }

        #region "IApplicationDiagRecord"
        public string Header()
        {
            return ProcessDiagCSVRecP01._header;
        }
        public string Record()
        {
            try
            {
                this._process.Refresh();
                return string.Format(
                    "{0:yyyy/MM/dd HH:mm:ss.fff},{1},{2},{3},{4:yyyy/MM/dd HH:mm:ss},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
                    DateTime.Now,
                    this._process.Id,
                    this._process.SessionId,
                    this._process.BasePriority,
                    this._process.StartTime,
                    string.Join(" ", Environment.GetCommandLineArgs()),
                    this._process.HandleCount,
                    this._process.Threads.Count,
                    this._process.TotalProcessorTime,
                    this._process.UserProcessorTime,
                    this._process.PrivateMemorySize64,
                    this._process.WorkingSet64,
                    this._process.MaxWorkingSet,
                    this._process.PeakWorkingSet64,
                    this._process.VirtualMemorySize64,
                    this._process.PeakVirtualMemorySize64
                ) + Environment.NewLine;
            }
            catch { return ProcessDiagCSVRecP01.FailedRec + Environment.NewLine; }
        }
        #endregion
    }
}
