using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Net;
using BAMSS.Extensions;

namespace BAMSS.Communication.FTP
{
    public class FTPClient
    {
        private string _serverName;
        private string _directoryName;
        public string ServerName
        {
            get { return this._serverName; }
            private set { this._serverName = this.CutPathDelimiter(value); }
        }
        public string DirectoryName
        {
            get { return this._directoryName; }
            private set { this._directoryName = this.CutPathDelimiter(value); }
        }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public int BufferSize { get; set; } 
        public bool UseBinary { get; set; }
        public bool KeepAlive { get; set; }
        public bool UsePassive { get; set; }
        public FTPClient(string serverName, string dirName, string userName, string password, int bufferSize = 8192, bool useBinary = true, bool keepAlive = true, bool usePassive = false)
        {
            this.ServerName = serverName;
            this.DirectoryName = dirName;
            this.UserName = userName;
            this.Password = password;
            this.BufferSize = bufferSize;
            this.UseBinary = useBinary;
            this.KeepAlive = keepAlive;
            this.UsePassive = usePassive;
        }
        private string CutPathDelimiter(string value)
        {
            var tail = value.Substring(0, 1, reverse: true);
            if ((tail == @"\") || (tail == @"/")) return value.Substring(0, (value.Length - 1));
            return value;
        }
        public void UploadFile(string targetFile, string destFileName, string destDirPath = null)
        {
            using (var fs = new System.IO.FileStream(targetFile, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, FileShare.None))
            {
                try
                {
                    var ftpReq = this.GetFtpWebRequest(destFileName, destDirPath);
                    ftpReq.Method = WebRequestMethods.Ftp.UploadFile;
                    using (var reqStrm = ftpReq.GetRequestStream())    //★この時点で転送先にファイルが生成される★
                    {
                        byte[] buffer = new byte[this.BufferSize];
                        while (true)
                        {
                            int readSize = fs.Read(buffer, 0, buffer.Length);
                            if (readSize == 0) break;
                            reqStrm.Write(buffer, 0, readSize);
                        }
                    }
                }
                catch (Exception ex) 
                {
                    ex.WriteLog(string.Format("≪処理中ファイル情報≫\n送信元ファイル：[{0}]\n送信先ファイル：[{1}]\n送信先ディレクトリ：[{2}]", targetFile, destFileName, destDirPath));
                    throw;
                }
            }
        }
        private FtpWebRequest GetFtpWebRequest(string targetFileName, string destDirPath = null)
        {
            Uri targetUri;
            var directory = destDirPath ?? this._directoryName;
            if ((string.IsNullOrEmpty(destDirPath))) targetUri = new Uri(string.Format(@"{0}/{1}", this.ServerName, targetFileName));
            else targetUri = new Uri(string.Format(@"{0}/{1}\{2}", this.ServerName, directory, targetFileName));
            var ftpReq = (FtpWebRequest)WebRequest.Create(targetUri);
            ftpReq.Credentials = new NetworkCredential(this.UserName, this.Password);
            ftpReq.UseBinary = this.UseBinary;
            ftpReq.KeepAlive = this.KeepAlive;
            ftpReq.UsePassive = this.UsePassive;
            return ftpReq;
        }
        private void DeleteFile(string targetFileName, string destDirPath = null)
        {
            try
            {
                var ftpReq = this.GetFtpWebRequest(targetFileName, destDirPath);
                ftpReq.Method = WebRequestMethods.Ftp.DeleteFile;
                using (FtpWebResponse ftpRes = (FtpWebResponse)ftpReq.GetResponse()) { }
            }
            catch (Exception ex)
            {
                ex.WriteLog(string.Format("≪処理中ファイル情報≫\n削除ファイル：[{0}]\nディレクトリ：[{1}]", targetFileName, destDirPath));
                throw;
            }
        }

        private bool IsExistsInDestDirFiles(string fileName)
        {
            try
            {
            //    Uri targetUri = new Uri(string.Format("{0}{1}", ftpConf.FTPServerName, ftpConf.PutDirName));
            //    FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create(targetUri);
            //    ftpReq.UseBinary = true;
            //    ftpReq.Credentials = new NetworkCredential(ftpConf.UserName, ftpConf.Password);
            //    ftpReq.Method = WebRequestMethods.Ftp.ListDirectory;
            //    ftpReq.KeepAlive = true;
            //    ftpReq.UsePassive = false;
            //    using (FtpWebResponse ftpRes = (FtpWebResponse)ftpReq.GetResponse())
            //    {
            //        using (System.IO.StreamReader sr = new System.IO.StreamReader(ftpRes.GetResponseStream()))
            //        {
            //            char[] splitChar = { Constants.vbLf };
            //            //区切り文字指定
            //            string stringRes = sr.ReadToEnd().Replace(Constants.vbCr, "");
            //            //Crが含まれる場合は消去
            //            List<string> listRes = stringRes.Split(splitChar).ToList;
            //            //区切り文字でリスト生成
            //            //ファイルが存在しない場合は空文字の配列が一つできるので、２つ以上でファイルが一つ以上存在する事になる。
            //            if ((listRes.Count > 1))
            //            {
            //                return listRes.GetRange(0, listRes.Count - 1).Contains(fileName);
            //            }
            //            else
            //            {
            //                return false;
            //            }
            //        }
            //    }

                return true;
            }
            catch (WebException ex)
            {
                //送信先FTPサーバーOSによってLSコマンドの結果が異なる事による対応
                //ファイルリスト取得先フォルダにファイルが一つも存在しない場合に、
                //滴下サーバーでは、ファイルは無かったという正常応答、
                //工程サーバーでは、550 No such file or directoryでエラー応答した。
                //上記結果は、枚葉管理FTP送信端末からのFTPコマンドで確認、
                //対応前プログラムの工程サーバー送信時の例外メッセージでも同様のメッセージを確認した。
                //よって、この例外は正常（ファイル無し）として処理するように本プログラムで対応した。
                FtpWebResponse res = (FtpWebResponse)ex.Response;
                if ((res.StatusDescription.ToUpper().Replace(" ", "").Contains("NOSUCHFILEORDIRECTORY") && (res.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)))
                {
                    //通常はファイルが無い場合、滴下サーバーはココを通るハズ！
                    //このパターンは日常的に発生するので、ログは採取しない。
                    return false;
                }
                else
                {
                    //通常は無いハズだが、何かあった時のログ取りの為、この場合はメッセージを残す。
                    //this._log.Debug(ex, string.Format("{0}にて、転送先ファイル存在確認に失敗しました。{4}[Destination]{4}{1}{4}[Description]{4}{2}{4}[ErrorCode]{4}{3}", ftpConf.TransferName, string.Format("Server:[{0}] Dir:[{1}] FileName:[{2}]", ftpConf.FTPServerName, ftpConf.PutDirName, fileName), res.StatusDescription, res.StatusCode, Constants.vbCrLf));
                    return false;
                    //Throw ex
                }
            }
            catch
            {
                //通常は無いハズだが、何かあった時のログ取りの為、この場合はメッセージを残す。
                //this._log.Debug(ex, string.Format("{0}にて、転送先ファイル存在確認に失敗しました。{4}[Destination]{4}{1}{4}[Description]{4}{2}{4}[Source]{4}{3}", ftpConf.TransferName, string.Format("Server:[{0}] Dir:[{1}] FileName:[{2}]", ftpConf.FTPServerName, ftpConf.PutDirName, fileName), ex.Message, ex.Source, Constants.vbCrLf));
                return false;
            }
        }
        public void Disconnect()
        {
            try
            {
                Uri targetUri = new Uri(this.ServerName);
                var ftpReq = (FtpWebRequest)WebRequest.Create(targetUri);
                ftpReq.Credentials = new NetworkCredential(this.UserName, this.Password);
                ftpReq.Method = WebRequestMethods.Ftp.PrintWorkingDirectory; //ダミーコマンド
                ftpReq.KeepAlive = false;
                using (FtpWebResponse ftpRes = (FtpWebResponse)ftpReq.GetResponse()) { }
            }
            catch (Exception ex)
            {
                ex.WriteLog();
            }
        }
    }
}
