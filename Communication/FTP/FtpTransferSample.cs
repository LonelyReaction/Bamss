
//using Microsoft.VisualBasic;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using TMD.Utility;
//using System.Threading;
//using System.IO;
//using ProcessStatusMemory;

//public class FtpTransferSample
//{
//    private FTPTransferServerConf _conf;
//    private MessageLogger _log;
//    private DateTime _bootTime;

//    private ProcessStatusInfo _processStatus = null;
//    public event BeginScanEventHandler BeginScan;
//    public delegate void BeginScanEventHandler();
//    public event EndScanEventHandler EndScan;
//    public delegate void EndScanEventHandler();
//    public event BeginBackupEventHandler BeginBackup;
//    public delegate void BeginBackupEventHandler(string sourceFilePath, string destFilePath);
//    public event EndBackupEventHandler EndBackup;
//    public delegate void EndBackupEventHandler(string sourceFilePath, string destFilePath);
//    public event BeginSendEventHandler BeginSend;
//    public delegate void BeginSendEventHandler(string filePath, FTPTransferConf ftpConf);
//    public event EndSendEventHandler EndSend;
//    public delegate void EndSendEventHandler(string filePath, FTPTransferConf ftpConf);
//    public event BeginTransferEventHandler BeginTransfer;
//    public delegate void BeginTransferEventHandler(string filePath, FTPTransferConf ftpConf);
//    public event FileSentEventHandler FileSent;
//    public delegate void FileSentEventHandler(string filePath, string fileData, FTPTransferConf ftpConf);
//    public event EndTransferEventHandler EndTransfer;
//    public delegate void EndTransferEventHandler(string filePath, FTPTransferConf ftpConf);
//    public event RebootRequestEventHandler RebootRequest;
//    public delegate void RebootRequestEventHandler(string requestTime);

//    public FtpTransferSample(MessageLogger log)
//    {
//        this._log = log;
//        this._bootTime = Now;
//    }

//    public void Initialize(FTPTransferServerConf conf)
//    {
//        this._conf = conf;
//        this.SetupAndStartHeartBeat(this._conf.ProcessStatusID, this._conf.ProcessStatusConf);
//    }

//    protected virtual void SetupAndStartHeartBeat(string processID, string confPath)
//    {
//        if ((processID.Length > 0))
//        {
//            this._processStatus = (new ProcessStatusAccessor(confPath)).Process(processID);
//            this._processStatus.LastAlivingTime = DateTime.Now;
//        }
//    }

//    public FTPTransferServerConf FTPTransferServerConf
//    {
//        get { return this._conf; }
//    }

//    /// <summary>
//    /// 自動再起動が必要なタイミングか否かを取得する関数
//    /// </summary>
//    /// <returns></returns>
//    /// <remarks></remarks>
//    private bool NowIsTimeForReboot()
//    {
//        return (this._conf.RebootTime.Length > 0) && (string.Format("{0:yyyyMMdd}", this._bootTime) < string.Format("{0:yyyyMMdd}", Now)) && (this._conf.RebootTime.PadLeft(4, "0").Substring(0, 4) <= string.Format("{0:HHmm}", Now));
//    }

//    public bool Transfer()
//    {
//        bool needReboot = false;
//        bool rapidMode = false;
//        try {
//            if (BeginScan != null) {
//                BeginScan();
//            }
//            try {
//                if (((this._processStatus != null))) {
//                    this._processStatus.LastAlivingTime = DateTime.Now;
//                    this._processStatus.StatusInfo = "";
//                }
//            } catch (Exception ex) {
//            }

//            foreach (FTPTransferConf ftc in this._conf.FTPTransferConfs) {
//                if ((File.Exists(string.Format("{0}\\{1}", System.Environment.CurrentDirectory, ftc.TransferPauseRequestFile)))) {
//                    //転送休止要求ファイルの検知時は、その設定は転送を休止する（スキップ）
//                    continue;
//                }
//                List<string> allDirs = new List<string>();
//                List<string> targetDirs = new List<string>();
//                List<string> targetFiles = new List<string>();
//                try {
//                    switch (ftc.SourceDirSearchType.ToLower) {
//                        case "root":
//                            //targetFiles = Directory.GetFiles(ftc.SourceFileKeeper.RootDirectoryName).ToList
//                            this._log.DebugWithoutEventLog("送信対象ファイルリスト取得開始[RootMode]");
//                            targetFiles = Directory.EnumerateFiles(ftc.SourceFileKeeper.RootDirectoryName).ToList;
//                            this._log.DebugWithoutEventLog("送信対象ファイルリスト取得完了[RootMode]({0}ファイル)", targetFiles.Count);
//                            targetFiles.Sort();
//                            this._log.DebugWithoutEventLog("送信対象ファイル絞り込み開始[RootMode]");
//                            targetFiles = targetFiles.Take(this._conf.MaxTransferCount).ToList;
//                            this._log.DebugWithoutEventLog("送信対象ファイル絞り込み完了[RootMode]({0}ファイル)", targetFiles.Count);
//                            break;
//                        case "sub":
//                            //MOD 2012.08.07(Tue) BAMSS Jun.Yokoe　実績転送が遅くなっていた不具合の対応
//                            //targetFiles = ftc.SourceFileKeeper.GetAllFilesInAllSubDirectory(Me._conf.MaxTransferCount)
//                            //'targetDirs = (From d In Directory.GetDirectories(ftc.SourceFileKeeper.RootDirectoryName, "*", SearchOption.TopDirectoryOnly) Order By d).ToList.Take(1).ToList
//                            //'If (targetDirs.Count > 0) Then
//                            //'    targetFiles = Directory.GetFiles(targetDirs(0), "*", SearchOption.TopDirectoryOnly).ToList.Take(Me._conf.MaxTransferCount).ToList
//                            //'End If
//                            //allDirs = (From d In Directory.GetDirectories(ftc.SourceFileKeeper.RootDirectoryName, "*", SearchOption.TopDirectoryOnly) Order By d).ToList
//                            this._log.DebugWithoutEventLog("送信対象ファイルリスト取得開始[SubMode]");
//                            allDirs = (from d in Directory.EnumerateDirectories(ftc.SourceFileKeeper.RootDirectoryName, "*", SearchOption.TopDirectoryOnly)orderby d).ToList;
//                            if ((allDirs.Count > 0)) {
//                                for (int i = 0; i <= (allDirs.Count - 1); i += 1) {
//                                    //targetFiles.AddRange(Directory.GetFiles(allDirs(i), "*", SearchOption.TopDirectoryOnly).ToList.Take(Me._conf.MaxTransferCount - targetFiles.Count).ToList)
//                                    targetFiles.AddRange(Directory.EnumerateFiles(allDirs(i), "*", SearchOption.TopDirectoryOnly).Take(this._conf.MaxTransferCount - targetFiles.Count));
//                                    targetDirs.Add(allDirs(i));
//                                    if ((targetFiles.Count >= this._conf.MaxTransferCount)) {
//                                        break; // TODO: might not be correct. Was : Exit For
//                                    }
//                                }
//                            }
//                            this._log.DebugWithoutEventLog("送信対象ファイルリスト取得完了[SubMode]({0}ファイル)", targetFiles.Count);
//                            break;
//                        default:
//                            this._log.Alarm(string.Format("{0}の転送元ディレクトリタイプの指定が認識できない設定になっている為、処理できません。", ftc.TransferName));
//                            return (!rapidMode);
//                        //targetFiles = New List(Of String)
//                    }
//                    if ((targetFiles.Count >= this._conf.MaxTransferCount)) {
//                        rapidMode = true;
//                    }
//                    if ((targetFiles.Count > 0)) {
//                        Threading.Thread.Sleep(1000);
//                        //ファイルアクセス干渉回避（あくまで低減策：干渉した場合は、例外処理にて該当ファイル処理を後回し）
//                    }
//                    foreach (string sourceFilePath in targetFiles) {
//                        if ((this.NowIsTimeForReboot())) {
//                            needReboot = true;
//                            break; // TODO: might not be correct. Was : Exit For
//                        }

//                        //送信処理開始イベント
//                        this._log.DebugWithoutEventLog("送信処理開始");
//                        if (BeginSend != null) {
//                            BeginSend(sourceFilePath, ftc);
//                        }

//                        //転送元と転送先の情報を生成
//                        string fileName = Path.GetFileName(sourceFilePath);
//                        string destFileName = null;
//                        if ((ftc.NeedTrimingDateTime)) {
//                            destFileName = (fileName.Length > 18 ? fileName.Substring(18) : fileName);
//                        } else {
//                            destFileName = fileName;
//                        }
//                        string destFilePath = ftc.PutDirName + "\\" + destFileName;
//                        bool existDataFile = false;
//                        bool existFlagFile = true;
//                        string targetFileNameWithoutExtension = Path.GetFileNameWithoutExtension(destFileName);

//                        try {
//                            if (((this._processStatus != null))) {
//                                this._processStatus.StatusInfo = string.Format("{0}→{1}", sourceFilePath, destFilePath);
//                            }
//                        } catch (Exception ex) {
//                        }

//                        //転送先の状態確認
//                        try {
//                            this._log.DebugWithoutEventLog("送信先データファイル存在確認開始({0})", targetFileNameWithoutExtension);
//                            existDataFile = IsExistsInDestDirFiles(destFileName, ftc);
//                            this._log.DebugWithoutEventLog("送信先データファイル存在確認完了({0})", targetFileNameWithoutExtension);
//                            //existDataFile = False
//                            if ((ftc.NeedGoFile)) {
//                                this._log.DebugWithoutEventLog("送信先フラグファイル存在確認開始({0})", targetFileNameWithoutExtension);
//                                existFlagFile = IsExistsInDestDirFiles(string.Format("{0}{1}", targetFileNameWithoutExtension, ftc.ExtensionOfGoFile), ftc);
//                                this._log.DebugWithoutEventLog("送信先フラグファイル存在確認完了({0})", targetFileNameWithoutExtension);
//                                //existFlagFile = False
//                            }
//                        } catch (Exception ex) {
//                            //転送先ファイル存在確認でエラーが発生しているので、転送も不可能な状態であると判断し、次の送信設定処理に移行する。
//                            this._log.Alarm(ex, string.Format("{0}の転送先ファイル存在確認に失敗しました。", ftc.TransferName));
//                            break; // TODO: might not be correct. Was : Exit For
//                        }

//                        //ファイルの存在状況による処理
//                        if ((ftc.NeedGoFile)) {
//                            if ((existDataFile) & (existFlagFile)) {
//                                //フラグファイルが必要で転送先に転送しようとしたファイルと同一名のファイルとフラグファイルが存在するので、未処理と判断しスキップして次のファイルを処理する。
//                                //もうすぐ消えるハズ！単なる未処理なら問題無いが、問題かもしれないので、警告出力（SONAR仕様）
//                                this._log.Warning(string.Format("{0}の転送先ファイルがデータ／フラグともに存在していました、次回処理にリトライします。[{1}]", ftc.TransferName, targetFileNameWithoutExtension));
//                                continue;
//                            }
//                            if ((!existDataFile) & (existFlagFile)) {
//                                //フラグファイルが必要な通信で、フラグファイルだけが存在する場合、警告出力で後回し。【もうすぐ消えるハズ！（SONAR仕様）】
//                                this._log.Warning(string.Format("{0}の転送先ファイルがフラグファイルのみ存在していました、次回処理にリトライします。[{1}]", ftc.TransferName, targetFileNameWithoutExtension));
//                                continue;
//                            }
//                            if ((existDataFile) & (!existFlagFile)) {
//                                //フラグファイルが必要な通信で、データファイルだけがある場合、データファイルを削除して、通常フロー。（SONAR仕様）
//                                try {
//                                    this._log.DebugWithoutEventLog("転送先ファイル削除開始({0})", destFileName);
//                                    this.DeleteFile(destFileName, ftc);
//                                    this._log.DebugWithoutEventLog("転送先ファイル削除完了({0})", destFileName);
//                                } catch (Exception ex) {
//                                    //リカバリ削除処理に失敗：そのファイルは後回しにして次のファイルの処理に移行する。
//                                    this._log.Alarm(ex, string.Format("{0}のファイル削除処理で異常が発生しました、フラグファイルが存在しなかった為リカバリ動作を行いましたが失敗しました。削除しようとしたファイル[{1}]", ftc.TransferName, destFileName));
//                                    continue;
//                                }
//                            }
//                        } else {
//                            if ((existDataFile)) {
//                                //フラグファイルが不要で、転送先に転送しようとしたファイルと同一名のファイルが存在するので、未処理と判断しスキップして次のファイルを処理する。
//                                this._log.Warning(string.Format("{0}の転送先ファイルが存在していました、次回処理にリトライします。[{1}]", ftc.TransferName, destFileName));
//                                continue;
//                            }
//                        }

//                        //ファイル転送処理
//                        try {
//                            //転送
//                            this._log.DebugWithoutEventLog("ファイル転送開始({0})", targetFileNameWithoutExtension);
//                            this.PutFile(destFileName, ftc, sourceFilePath);
//                            this._log.DebugWithoutEventLog("ファイル転送完了({0})", targetFileNameWithoutExtension);
//                            //ファイル転送完了
//                            //Me._log.Info(String.Format("{0}ファイル送信完了[{1}→{2}:{3}]", ftc.TransferName, sourceFilePath, ftc.FTPServerName, destFileName))
//                        } catch (FileNotFoundException ex) {
//                            //削除済みファイルの残像を操作しようとした場合の処理→次のファイルへ
//                            this._log.Warning(ex, string.Format("{0}の転送処理で転送元ファイルが存在しませんでした。（ファイルの残像を検出したか送信元にアクセスできなくなった可能性があります、このファイルの処理はスキップして処理は継続します。）", ftc.TransferName));
//                            continue;
//                        } catch (UnauthorizedAccessException ex) {
//                            //転送元ファイルへのアクセス拒否
//                            this._log.Warning(ex, string.Format("{0}の転送元ファイルへのアクセスが拒否されました、データ生成側にて書込中の可能性があるので、このファイルの処理はスキップして処理は継続します。[{1}]→[{2}]（転送元ファイルへのアクセス権が無い可能性もあります）", ftc.TransferName, ftc, sourceFilePath, destFileName));
//                            continue;
//                        } catch (IOException ex) {
//                            //ファイルIO異常
//                            this._log.Warning(ex, string.Format("{0}の転送元ファイルへのアクセスエラーが発生しました、送信元にアクセスできなくなったか、データ生成側にて書込中の可能性があるので、このファイルの処理はスキップして処理は継続します。[{1}]→[{2}]", ftc.TransferName, ftc, sourceFilePath, destFileName));
//                            continue;
//                        } catch (Exception ex) {
//                            //予期しないエラーが発生、予期していないエラーなので、とりあえずスキップ次の送信を行う。
//                            this._log.Alarm(ex, string.Format("{0}の転送処理で異常が発生しました、このファイルの処理はスキップして処理は継続します。", ftc.TransferName));
//                            continue;
//                            //次の参照先処理へのジャンプは行わない→もしかしたら他のファイルは転送できるかもしれないし、エラー発生ファイルで次回もエラーが発生し続けたら転送できるファイルも転送できなくなるから。
//                            //Exit For
//                        }

//                        if ((ftc.BackupEnabled)) {
//                            //バックアップ開始イベント
//                            if (BeginBackup != null) {
//                                BeginBackup(sourceFilePath, string.Format("{0}\\{1}", ftc.BackupFileKeeper.RootDirectoryName, fileName));
//                            }

//                            //設定ファイルで有効化されている場合のみ、バックアップを行う。
//                            //転送が完了したので、バックアップを行なう（ファイル移動）
//                            bool backupSuccess = false;
//                            try {
//                                this._log.DebugWithoutEventLog("転送ファイルバックアップ開始({0})", sourceFilePath);
//                                ftc.BackupFileKeeper.MoveIn(sourceFilePath, fileName);
//                                this._log.DebugWithoutEventLog("転送ファイルバックアップ完了({0})", sourceFilePath);
//                                backupSuccess = true;
//                            } catch (Exception ex) {
//                                this._log.Alarm(ex, string.Format("{0}のバックアップ処理で異常が発生しました。[{1}]→[{2}]", ftc.TransferName, sourceFilePath, fileName));
//                            }

//                            //バックアップ失敗時のリカバリ（二重送信回避の為、バックアップを諦めて削除を試みる）
//                            if ((backupSuccess)) {
//                                //バックアップ完了イベント
//                                if (EndBackup != null) {
//                                    EndBackup(sourceFilePath, string.Format("{0}\\{1}", ftc.BackupFileKeeper.RootDirectoryName, fileName));
//                                }
//                            } else {
//                                this._log.Alarm(string.Format("{0}のバックアップに失敗したので、バックアップを諦めて転送元ファイルを削除します。[{1}]", ftc.TransferName, sourceFilePath));
//                                try {
//                                    this._log.DebugWithoutEventLog("転送ファイル削除開始({0})", sourceFilePath);
//                                    File.Delete(sourceFilePath);
//                                    //この時点でファイルが存在しない場合、例外は発生せず、削除正常完了扱い。
//                                    this._log.DebugWithoutEventLog("転送ファイル削除完了({0})", sourceFilePath);
//                                } catch (Exception ex) {
//                                    this._log.Alarm(ex, string.Format("{0}のバックアップ失敗による転送元ファイルの削除にも失敗しました。[{1}]、このファイルは転送が完了していますが、送信元フォルダにファイルが残っている為、再度実績を送信してしまう可能性があります。", ftc.TransferName, sourceFilePath));
//                                }
//                            }
//                        } else {
//                            //設定ファイルで無効化されている場合は、バックアップは行わず削除。
//                            try {
//                                this._log.DebugWithoutEventLog("転送ファイル削除開始({0})", sourceFilePath);
//                                File.Delete(sourceFilePath);
//                                this._log.DebugWithoutEventLog("転送ファイル削除完了({0})", sourceFilePath);
//                            } catch (Exception ex) {
//                                this._log.Alarm(ex, string.Format("{0}の削除処理で異常が発生しました。[{1}]", ftc.TransferName, sourceFilePath));
//                            }
//                        }
//                        //送信処理終了イベント
//                        if (EndSend != null) {
//                            EndSend(sourceFilePath, ftc);
//                        }
//                        try {
//                            if (((this._processStatus != null))) {
//                                this._processStatus.StatusInfo = "";
//                            }
//                        } catch (Exception ex) {
//                        }
//                    }
//                } catch (Exception ex) {
//                } finally {
//                    //FTP切断
//                    this._log.DebugWithoutEventLog("FTP切断開始");
//                    this.DisconnectFtp(ftc);
//                    this._log.DebugWithoutEventLog("FTP切断完了");
//                }
//                //ファイルがなくなったディレクトリを削除する。
//                //MOD 2012.08.07(Tue) BAMSS Jun.Yokoe　実績転送が遅くなっていた不具合の対応
//                //処理対象フォルダが存在して、それが最後のディレクトリでなく、そのフォルダ内にファイルが無ければ削除する
//                if ((targetDirs.Count > 0)) {
//                    for (int i = 0; i <= (targetDirs.Count - 1); i += 1) {
//                        if ((allDirs(allDirs.Count - 1) != targetDirs(i)) && (Directory.GetFiles(targetDirs(i)).Count < 1)) {
//                            this._log.DebugWithoutEventLog("ディレクトリ削除開始({0})", targetDirs(i));
//                            Directory.Delete(targetDirs(i));
//                            this._log.DebugWithoutEventLog("ディレクトリ削除完了({0})", targetDirs(i));
//                        }
//                    }
//                }
//                //ftc.SourceFileKeeper.DeleteSubDirectoryThatHaveNoFile()
//                if ((needReboot) || (this.NowIsTimeForReboot())) {
//                    needReboot = true;
//                    break; // TODO: might not be correct. Was : Exit For
//                }
//            }
//            if ((needReboot) || (this.NowIsTimeForReboot())) {
//                this._log.DebugWithoutEventLog("日次再起動時間検出：本FTP通信プロセスを再起動します。");
//                needReboot = true;
//            }
//        } catch (Exception ex) {
//            this._log.Alarm(ex, "ファイル移送に失敗しました。");
//        } finally {
//            if (EndScan != null) {
//                EndScan();
//            }
//        }
//        if ((needReboot)) {
//            if (RebootRequest != null) {
//                RebootRequest(this._conf.RebootTime);
//            }
//        }
//        return (!rapidMode);
//    }
//    /// <summary>
//    /// ＦＴＰファイル送信処理
//    /// </summary>
//    /// <param name="destFileName">送信先ファイル名</param>
//    /// <param name="ftpConf">FTP送信設定情報</param>
//    /// <param name="target">送信元ファイル名</param>
//    /// <returns></returns>
//    /// <remarks></remarks>
//    private bool PutFile(string destFileName, FTPTransferConf ftpConf, string target)
//    {
//        try
//        {
//            //転送開始イベント
//            if (BeginTransfer != null)
//            {
//                BeginTransfer(target, ftpConf);
//            }

//            Uri targetUri = default(Uri);
//            System.Net.FtpWebRequest ftpReq = default(System.Net.FtpWebRequest);
//            //アップロードするファイルを開く（ファイル生成側アクセス中のアクセスを禁止する為、読込のみの処理だが、ReadWrite-LockでOpenしている）
//            using (System.IO.FileStream fs = new System.IO.FileStream(target, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.None))
//            {
//                if ((ftpConf.PutDirName.Length == 0))
//                {
//                    targetUri = new Uri(string.Format("{0}{1}", ftpConf.FTPServerName, destFileName));
//                    //送信するファイルのURI
//                }
//                else
//                {
//                    targetUri = new Uri(string.Format("{0}{1}\\{2}", ftpConf.FTPServerName, ftpConf.PutDirName, destFileName));
//                    //送信するファイルのURI
//                }
//                ftpReq = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(targetUri);
//                //FtpWebRequestの作成
//                ftpReq.UseBinary = true;
//                //バイナリモード指定
//                ftpReq.Credentials = new System.Net.NetworkCredential(ftpConf.UserName, ftpConf.Password);
//                //ログインユーザー名とパスワードを設定
//                ftpReq.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
//                //Methodにアップロードメソッドを指定
//                ftpReq.KeepAlive = true;
//                //要求の完了後に接続を閉じない
//                ftpReq.UsePassive = false;
//                //PASSIVEモードを無効にする
//                using (System.IO.Stream reqStrm = ftpReq.GetRequestStream())
//                {
//                    //★この時点で転送先にファイルが生成される★
//                    //アップロードStreamに書き込む
//                    byte[] buffer = new byte[8192];
//                    while (true)
//                    {
//                        int readSize = fs.Read(buffer, 0, buffer.Length);
//                        if (readSize == 0)
//                        {
//                            break; // TODO: might not be correct. Was : Exit While
//                        }
//                        reqStrm.Write(buffer, 0, readSize);
//                    }
//                }
//            }

//            if ((ftpConf.NeedGoFile))
//            {
//                if ((ftpConf.PutDirName.Length == 0))
//                {
//                    targetUri = new Uri(string.Format("{0}{1}", ftpConf.FTPServerName, string.Format("{0}{1}", Path.GetFileNameWithoutExtension(destFileName), ftpConf.ExtensionOfGoFile)));
//                    //送信するファイルのURI
//                }
//                else
//                {
//                    targetUri = new Uri(string.Format("{0}{1}\\{2}", ftpConf.FTPServerName, ftpConf.PutDirName, string.Format("{0}{1}", Path.GetFileNameWithoutExtension(destFileName), ftpConf.ExtensionOfGoFile)));
//                    //送信するファイルのURI
//                }
//                ftpReq = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(targetUri);
//                //FtpWebRequestの作成
//                ftpReq.UseBinary = true;
//                //バイナリモード指定
//                ftpReq.Credentials = new System.Net.NetworkCredential(ftpConf.UserName, ftpConf.Password);
//                //ログインユーザー名とパスワードを設定
//                ftpReq.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
//                //Methodにアップロードメソッドを指定
//                ftpReq.KeepAlive = true;
//                //要求の完了後に接続を閉じない
//                ftpReq.UsePassive = false;
//                //PASSIVEモードを無効にする
//                using (System.IO.Stream reqStrm = ftpReq.GetRequestStream())
//                {
//                    //フラグファイルの書込み
//                    byte[] buffer = new byte[1];
//                    reqStrm.Write(buffer, 0, 0);
//                }
//            }
//            //ファイル転送完了イベント
//            if (FileSent != null)
//            {
//                FileSent(target, File.ReadAllText(target), ftpConf);
//            }
//        }
//        catch (Exception ex)
//        {
//            throw ex;
//        }
//        finally
//        {
//            //転送処理終了イベント
//            if (EndTransfer != null)
//            {
//                EndTransfer(target, ftpConf);
//            }
//        }
//        //Using ftpRes As System.Net.FtpWebResponse = CType(ftpReq.GetResponse(), System.Net.FtpWebResponse)
//        //    Console.WriteLine("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription)
//        //End Using
//        return true;
//    }
//    /// <summary>
//    /// ＦＴＰファイル削除処理
//    /// </summary>
//    /// <param name="targetFileName">送信先ファイル名</param>
//    /// <param name="ftpConf">FTP送信設定情報</param>
//    /// <returns></returns>
//    /// <remarks></remarks>
//    private bool DeleteFile(string targetFileName, FTPTransferConf ftpConf)
//    {
//        Uri targetUri = default(Uri);
//        if ((ftpConf.PutDirName.Length == 0))
//        {
//            targetUri = new Uri(string.Format("{0}{1}", ftpConf.FTPServerName, targetFileName));
//            //送信するファイルのURI
//        }
//        else
//        {
//            targetUri = new Uri(string.Format("{0}{1}\\{2}", ftpConf.FTPServerName, ftpConf.PutDirName, targetFileName));
//            //送信するファイルのURI
//        }
//        System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(targetUri);
//        //FtpWebRequestの作成
//        ftpReq.UseBinary = true;
//        //バイナリモード指定
//        ftpReq.Credentials = new System.Net.NetworkCredential(ftpConf.UserName, ftpConf.Password);
//        //ログインユーザー名とパスワードを設定
//        ftpReq.Method = System.Net.WebRequestMethods.Ftp.DeleteFile;
//        //Methodにファイル削除メソッドを指定
//        ftpReq.KeepAlive = true;
//        //要求の完了後に接続を閉じない
//        ftpReq.UsePassive = false;
//        //PASSIVEモードを無効にする

//        //Using ftpRes As System.Net.FtpWebResponse = CType(ftpReq.GetResponse(), System.Net.FtpWebResponse)
//        //    Console.WriteLine("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription)
//        //End Using
//        return true;
//    }
//    /// <summary> 
//    /// FTPサーバーから切断する 
//    /// </summary> 
//    /// <param name="ftpConf">FTP送信設定情報</param>
//    private void DisconnectFtp(FTPTransferConf ftpConf)
//    {
//        Uri targetUri = new Uri(ftpConf.FTPServerName);
//        //切断するサーバー名
//        System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(targetUri);
//        ftpReq.Credentials = new System.Net.NetworkCredential(ftpConf.UserName, ftpConf.Password);
//        //ログインユーザー名とパスワードを設定
//        ftpReq.Method = System.Net.WebRequestMethods.Ftp.PrintWorkingDirectory;
//        ftpReq.KeepAlive = false;

//        //Using ftpRes As System.Net.FtpWebResponse = DirectCast(ftpReq.GetResponse(), System.Net.FtpWebResponse)
//        //    Console.WriteLine("{0}: {1}", ftpRes.StatusCode, ftpRes.StatusDescription)
//        //End Using
//    }
//    /// <summary>
//    /// FTP送信先ファイル存在確認
//    /// </summary>
//    /// <param name="fileName">確認対象ファイル名</param>
//    /// <param name="ftpConf">FTP送信設定情報</param>
//    /// <returns></returns>
//    /// <remarks></remarks>
//    private bool IsExistsInDestDirFiles(string fileName, FTPTransferConf ftpConf)
//    {
//        try
//        {
//            Uri targetUri = new Uri(string.Format("{0}{1}", ftpConf.FTPServerName, ftpConf.PutDirName));
//            //ファイル一覧を取得するディレクトリのURI
//            System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(targetUri);
//            //FtpWebRequestの作成
//            ftpReq.UseBinary = true;
//            //バイナリモード指定
//            ftpReq.Credentials = new System.Net.NetworkCredential(ftpConf.UserName, ftpConf.Password);
//            //ログインユーザー名とパスワードを設定
//            ftpReq.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
//            //MethodにFTP サーバー上のファイルの短い一覧を取得する FTP NLIST プロトコル メソッドを指定
//            ftpReq.KeepAlive = true;
//            //要求の完了後に接続を閉じない
//            ftpReq.UsePassive = false;
//            //PASSIVEモードを無効にする
//            using (System.Net.FtpWebResponse ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse())
//            {
//                using (System.IO.StreamReader sr = new System.IO.StreamReader(ftpRes.GetResponseStream()))
//                {
//                    char[] splitChar = { Constants.vbLf };
//                    //区切り文字指定
//                    string stringRes = sr.ReadToEnd().Replace(Constants.vbCr, "");
//                    //Crが含まれる場合は消去
//                    List<string> listRes = stringRes.Split(splitChar).ToList;
//                    //区切り文字でリスト生成
//                    //ファイルが存在しない場合は空文字の配列が一つできるので、２つ以上でファイルが一つ以上存在する事になる。
//                    if ((listRes.Count > 1))
//                    {
//                        return listRes.GetRange(0, listRes.Count - 1).Contains(fileName);
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//            }

//            return true;
//        }
//        catch (System.Net.WebException ex)
//        {
//            //送信先FTPサーバーOSによってLSコマンドの結果が異なる事による対応
//            //ファイルリスト取得先フォルダにファイルが一つも存在しない場合に、
//            //滴下サーバーでは、ファイルは無かったという正常応答、
//            //工程サーバーでは、550 No such file or directoryでエラー応答した。
//            //上記結果は、枚葉管理FTP送信端末からのFTPコマンドで確認、
//            //対応前プログラムの工程サーバー送信時の例外メッセージでも同様のメッセージを確認した。
//            //よって、この例外は正常（ファイル無し）として処理するように本プログラムで対応した。
//            System.Net.FtpWebResponse res = (System.Net.FtpWebResponse)ex.Response;
//            if ((res.StatusDescription.ToUpper.Replace(" ", "").Contains("NOSUCHFILEORDIRECTORY") && (res.StatusCode == 550)))
//            {
//                //通常はファイルが無い場合、滴下サーバーはココを通るハズ！
//                //このパターンは日常的に発生するので、ログは採取しない。
//                return false;
//            }
//            else
//            {
//                //通常は無いハズだが、何かあった時のログ取りの為、この場合はメッセージを残す。
//                this._log.Debug(ex, string.Format("{0}にて、転送先ファイル存在確認に失敗しました。{4}[Destination]{4}{1}{4}[Description]{4}{2}{4}[ErrorCode]{4}{3}", ftpConf.TransferName, string.Format("Server:[{0}] Dir:[{1}] FileName:[{2}]", ftpConf.FTPServerName, ftpConf.PutDirName, fileName), res.StatusDescription, res.StatusCode, Constants.vbCrLf));
//                return false;
//                //Throw ex
//            }
//        }
//        catch (Exception ex)
//        {
//            //通常は無いハズだが、何かあった時のログ取りの為、この場合はメッセージを残す。
//            this._log.Debug(ex, string.Format("{0}にて、転送先ファイル存在確認に失敗しました。{4}[Destination]{4}{1}{4}[Description]{4}{2}{4}[Source]{4}{3}", ftpConf.TransferName, string.Format("Server:[{0}] Dir:[{1}] FileName:[{2}]", ftpConf.FTPServerName, ftpConf.PutDirName, fileName), ex.Message, ex.Source, Constants.vbCrLf));
//            return false;
//        }
//    }
//}

////=======================================================
////Service provided by Telerik (www.telerik.com)
////Conversion powered by NRefactory.
////Twitter: @telerik
////Facebook: facebook.com/telerik
////=======================================================
