using BAMSS.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BAMSS.Diagnostic
{
    public class ApplicationDiagnostic
    {
        private static readonly string _programFilePath = Process.GetCurrentProcess().MainModule.FileName;
        private object _lockWrite = new object();
        private IApplicationDiagRecord _diagRec;
        private Action _logPurger = null;
        private string _logFileFullPath { get { return string.Format(this._logFileNameFormat, this._logFilePath, this._logFilePrefix, DateTime.Now, this._logName, this._logFileInstanceInfo, this._logFileExtension); } }
        private string _logFilePath = string.Format(@"{0}\DiagLogs", Path.GetDirectoryName(_programFilePath));
        private string _logFilePrefix = "DiagLog_";
        private string _logName = Path.GetFileNameWithoutExtension(_programFilePath);
        private string _logFileInstanceInfo = "";
        private string _logFileExtension = ".csv";
        private string _logFileNameFormat = @"{0}\{1}{3}_{4}_{2:yyyyMMdd}{5}";     //{0}=DirectoryPath / {1}=Prefix / {2}=DateTime /{3}=LogName /{4}=InstanceInfo /{5}=Extension
        private int _interval = 60;     // 秒指定
        private bool _enabled = false;
        private int _scanCountToPurge;
        /// <summary>
        /// パージ動作の周期：何回の書き込み毎にパージ処理を実行するかを指定する。デフォルトは15、0の指定でパージしない。
        /// </summary>
        public int PurgeCycle { set; get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="interval">情報出力周期[秒](null時：デフォルト=60)</param>
        /// <param name="diagRec">収集情報レコードクラス</param>
        public ApplicationDiagnostic(int interval, IApplicationDiagRecord diagRec)
        {
            this.PurgeCycle = 15;
            this._logPurger = this.DefaultPurger;
            if (interval != 0) this._interval = interval;
            if (diagRec != null) this._diagRec = diagRec;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="interval">情報出力周期[秒](null時：デフォルト=60)</param>
        /// <param name="diagRec">収集情報レコードクラス</param>
        /// <param name="diagnosticPurger">記録ファイルパージ処理 記録ファイルのパージを行う関数 (null時：デフォルト=3日間保持)</param>
        public ApplicationDiagnostic(int interval, IApplicationDiagRecord diagRec, Action diagnosticPurger)
            : this(interval, diagRec)
        {
            if (diagnosticPurger != null) this._logPurger = diagnosticPurger;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="interval">情報出力周期[秒](null時：デフォルト=60)</param>
        /// <param name="diagRec">収集情報レコードクラス</param>
        /// <param name="diagnosticPurger">記録ファイルパージ処理 記録ファイルのパージを行う関数 (null時：デフォルト=3日間保持)</param>
        /// <param name="logFilePath">情報出力先ディレクトリ パス末尾の\は付いていてもいなくてもかまわない (null時：デフォルト=プログラムパス\DiagLogs)</param>
        public ApplicationDiagnostic(int interval, IApplicationDiagRecord diagRec, Action diagnosticPurger, string logFilePath)
            : this(interval, diagRec, diagnosticPurger)
        {
            if (logFilePath != null)
            {
                if (logFilePath.EndsWith(@"\")) logFilePath.TrimEnd(new char[] { '\'' });
                this._logFilePath = logFilePath;
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="interval">情報出力周期[秒](null時：デフォルト=60)</param>
        /// <param name="diagRec">収集情報レコードクラス</param>
        /// <param name="diagnosticPurger">記録ファイルパージ処理 記録ファイルのパージを行う関数 (null時：デフォルト=3日間保持)</param>
        /// <param name="logFilePath">情報出力先ディレクトリ パス末尾の\は付いていてもいなくてもかまわない (null時：デフォルト=プログラムパス\DiagLogs)</param>
        /// <param name="logFilePrefix">記録ファイルの識別接頭辞文字列 (null時：デフォルト=DiagLog_)</param>
        /// <param name="logName">記録ファイルのプログラム識別名 (null時：デフォルト=プログラムファイル名)</param>
        /// <param name="logFileInstanceInfo">記録ファイルのプログラムインスタンス識別名 (null時：デフォルト=空文字)</param>
        /// <param name="logFileExtension">記録ファイルの拡張子 (null時：デフォルト=.csv)</param>
        public ApplicationDiagnostic(int interval, IApplicationDiagRecord diagRec, Action diagnosticPurger, string logFilePath, string logFilePrefix, string logName, string logFileInstanceInfo, string logFileExtension)
            : this(interval, diagRec, diagnosticPurger, logFilePath)
        {
            if (logFilePrefix != null) this._logFilePrefix = logFilePrefix;
            if (logName != null) this._logName = logName;
            if (logFileInstanceInfo != null) this._logFileInstanceInfo = logFileInstanceInfo;
            if (logFileExtension != null) this._logFileExtension = logFileExtension;
        }
        /// <summary>
        /// 記録動作開始処理 指定パラメータに基づき情報収集活動を開始する。
        /// </summary>
        public void Start()
        {
            if (this._enabled) return;
            this._enabled = true;
            this.DiagnosticAsync();
        }
        /// <summary>
        /// 記録動作停止処理 情報収集活動を停止する。
        /// </summary>
        public void Stop()
        {
            this._enabled = false;
        }
        /// <summary>
        /// 記録処理（非同期動作）
        /// </summary>
        private async void DiagnosticAsync()
        {
            await Task.Run(() =>
            {
                while (this._enabled)
                {
                    try
                    {
                        this.WriteLog();
                        if ((this.PurgeCycle > 0) && (++this._scanCountToPurge >= this.PurgeCycle)) this._logPurger();
                    }
                    catch (Exception ex) { ex.WriteLog(); }
                    finally { Thread.Sleep(this._interval * 1000); }
                }
            });
        }
        /// <summary>
        /// 書出処理
        /// </summary>
        public void WriteLog()
        {
            lock (this._lockWrite)    //定周期スレッドと、この関数の任意コールの競合回避の為のクリティカルセクション
            {
                var record = this._diagRec.Record();
                if (string.IsNullOrEmpty(record)) return;
                var logFile = this._logFileFullPath;
                var logDir = Path.GetDirectoryName(logFile);
                if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
                if (!File.Exists(logFile)) File.WriteAllText(logFile, this._diagRec.Header());
                using (var writer = new StreamWriter(logFile, append: true)) writer.Write(record);
            }
        }
        /// <summary>
        /// パージ処理
        /// </summary>
        public void PurgeLog()
        {
            lock (this._lockWrite)    //定周期スレッドと、この関数の任意コールの競合回避の為のクリティカルセクション
            {
                this._scanCountToPurge = 0;
                this._logPurger();
            }
        }
        /// <summary>
        /// デフォルトパージ処理
        /// </summary>
        private void DefaultPurger()
        {
            var keepDays = 31;
            var files = Directory.GetFiles(this._logFilePath, string.Format("{0}{1}{2}_*{3}", this._logFilePrefix, this._logName, this._logFileInstanceInfo, this._logFileExtension));
            foreach (var file in files.Where(c => string.Compare(Path.GetFileNameWithoutExtension(c).Substring(0, 8, reverse: true), string.Format("{0:yyyyMMdd}", DateTime.Now.AddDays(keepDays * (-1)))) < 0))
            {
                File.Delete(file);
            }
        }
    }
}
