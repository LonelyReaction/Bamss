using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BAMSS.Logging.Message;

namespace BAMSS.Extensions
{
    public static class ExceptionExtensions
    {
        private static readonly string _programFilePath = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string _programFileName = Path.GetFileNameWithoutExtension(_programFilePath.Split('.')[0]);
        /// <summary>
        /// 例外ロギング処理
        /// </summary>
        /// <param name="ex">例外オブジェクト（拡張メソッドインスタンスパラメータ）</param>
        /// <param name="information">追加のユーザー情報</param>
        /// <param name="appendAction">追加の処理（ログファイルへの出力等アプリケーション固有の処理）Action&lt;Exception, int&gt; Exception:例外インスタンス int:例外階層レベル番号
        /// </param>
        public static void WriteLog(this Exception ex, string information, IMessageLogger logger, Action<Exception, int> appendAction = null)
        {
            using (EventLog eLog = new EventLog(logName: _programFileName, machineName: ".", source: _programFileName))
            {
                if (!EventLog.SourceExists(eLog.Source, eLog.MachineName))
                {
                    EventSourceCreationData escd = new EventSourceCreationData(eLog.Source, eLog.Log);
                    escd.MachineName = eLog.MachineName;
                    EventLog.CreateEventSource(escd);
                }
                WriteLog(ex, eLog, information, logger, appendAction);
            }
        }
        /// <summary>
        /// 例外ロギング処理
        /// </summary>
        /// <param name="ex">例外オブジェクト（拡張メソッドインスタンスパラメータ）</param>
        /// <param name="information">追加情報</param>
        /// <param name="appendAction">追加Action</param>
        public static void WriteLog(this Exception ex, string information, Action<Exception, int> appendAction = null)
        {
            using (EventLog eLog = new EventLog(logName: _programFileName, machineName: ".", source: _programFileName))
            {
                if (!EventLog.SourceExists(eLog.Source, eLog.MachineName))
                {
                    EventSourceCreationData escd = new EventSourceCreationData(eLog.Source, eLog.Log);
                    escd.MachineName = eLog.MachineName;
                    EventLog.CreateEventSource(escd);
                }
                WriteLog(ex, eLog, information, null, appendAction);
            }
        }
        /// <summary>
        /// 例外ロギング処理
        /// </summary>
        /// <param name="ex">例外オブジェクト（拡張メソッドインスタンスパラメータ）</param>
        /// <param name="appendAction">追加の処理（ログファイルへの出力等アプリケーション固有の処理）Action&lt;Exception, int&gt; Exception:例外インスタンス int:例外階層レベル番号
        /// </param>
        public static void WriteLog(this Exception ex, Action<Exception, int> appendAction = null)
        {
            WriteLog(ex, null, appendAction);
        }
        /// <summary>
        /// 例外ロギング処理
        /// </summary>
        /// <param name="ex">例外オブジェクト（拡張メソッドインスタンスパラメータ）</param>
        /// <param name="logger">ロガー</param>
        public static void WriteLog(this Exception ex, IMessageLogger logger)
        {
            WriteLog(ex, null, logger, null);
        }
        private static void WriteLog(Exception ex, EventLog eLog, string information, IMessageLogger logger, Action<Exception, int> appendAction)
        {
            WriteLog(ex, eLog, information, logger, appendAction, 0);
        }
        private static void WriteLog(Exception ex, EventLog eLog, string information, IMessageLogger logger, Action<Exception, int> appendAction, int innerLevel = 0)
        {
            if (ex.InnerException != null) WriteLog(ex.InnerException, eLog, null, logger, appendAction, (innerLevel + 1));
            eLog.WriteEntry((string.IsNullOrEmpty(information) ? "" : string.Format("{0}\n\n", information)) + BuildLogData(ex, innerLevel), EventLogEntryType.Error);
            if (appendAction != null) appendAction(ex, innerLevel);
            if (logger != null) logger.Write(MessageLevel.Error, information, null, BuildLogData(ex, innerLevel), null);
        }
        /// <summary>
        /// エラーレポート用情報文字列生成
        /// </summary>
        /// <param name="ex">例外オブジェクト（拡張メソッドインスタンスパラメータ）</param>
        /// <param name="innerLevel">例外階層レベル番号</param>
        /// <returns></returns>
        public static string BuildLogData(this Exception ex, int innerLevel = 0)
        {
            return string.Format(
                "[Message]\n{0}\n" + 
                "[StackTrace]\n{1}\n" +
                "[Source]\n{2}\n" + 
                "[Program File]\n{3}\n" + 
                "[Innner Level]\n{4}\n" + 
                "[Process Information]\n{5}", 
                ex.Message, 
                ex.StackTrace, 
                ex.Source, 
                _programFilePath,
                innerLevel,
                BuildProcessInformations()
            );
        }
        private static string BuildProcessInformations()
        {
            try
            {
                var p = Process.GetCurrentProcess();
                p.Refresh();
                return string.Format(
                    "Id={0}\n" +
                    "SessionId={1}\n" +
                    "BasePriority={2}\n" +
                    "StartTime={3:yyyy/MM/dd hh:mm:ss.fff}\n" +
                    "CommandLine={4}\n" +
                    "HandleCount={5}\n" +
                    "ThreadCount={6}\n" +
                    "TotalProcessorTime={7}\n" +
                    "UserProcessorTime={8}\n" +
                    "PrivateMemorySize={9}\n" +
                    "WorkingSet={10}\n" +
                    "MaxWorkingSet={11}\n" +
                    "PeakWorkingSet={12}\n" +
                    "VirtualMemorySize={13}\n" +
                    "PeakVirtualMemorySize={14}\n",
                    p.Id,
                    p.SessionId,
                    p.BasePriority,
                    p.StartTime,
                    string.Join(" ", Environment.GetCommandLineArgs()),
                    p.HandleCount,
                    p.Threads.Count,
                    p.TotalProcessorTime,
                    p.UserProcessorTime,
                    p.PrivateMemorySize64,
                    p.WorkingSet64,
                    p.MaxWorkingSet,
                    p.PeakWorkingSet64,
                    p.VirtualMemorySize64,
                    p.PeakVirtualMemorySize64
                );
            }
            catch { return "***プロセス情報は取得に失敗しました。***"; }
        }
        /// <summary>
        /// 全エラーリスト生成（発生している例外をLinq処理する為のリストを発生順に生成）
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static IEnumerable<Exception> AllExceptions(this Exception ex)
        {
            var list = new List<Exception>();
            AddToListRecursive(ex, list);
            return list;
        }
        private static void AddToListRecursive(Exception ex, List<Exception> list)
        {
            if (ex.InnerException != null) AddToListRecursive(ex.InnerException, list);
            list.Add(ex);
        }
        /// <summary>
        /// 最初に発生した例外を取得
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception FirstException(this Exception ex)
        {
            return ex.AllExceptions().FirstOrDefault();
        }
        /// <summary>
        /// 最後に発生した例外を取得
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception LastException(this Exception ex)
        {
            return ex.AllExceptions().LastOrDefault();
        }
        public static IEnumerable<Exception> Where(this Exception ex, Func<Exception, bool> predicate)
        {
            return ex.AllExceptions().Where(predicate);
        }
    }
}
