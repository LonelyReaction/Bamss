using System;
using System.Linq;
namespace BAMSS.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// このインスタンスから部分文字列を取得します。reverse: trueにて反転モードとなります、reverse: false時は通常のSubstring
        /// </summary>
        /// <param name="str">インスタンス</param>
        /// <param name="startIndex">true時、反転モードとなり、末尾からのインデックス番号となります。</param>
        /// <param name="length">true時、反転モードとなり、取得する末尾からの文字数となります。</param>
        /// <param name="reverse">true時、反転モードとなり、startIndexは文字列の末尾からのインデックス番号となります。</param>
        /// <returns></returns>
        public static string Substring(this string str, int startIndex, int length, bool reverse)
        {
            if (!reverse) return str.Substring(startIndex, length);
            return str.Substring(str.Length - length - startIndex, length);
        }
        /// <summary>
        /// このインスタンスから部分文字列を取得します。reverse: trueにて反転モードとなります、reverse: false時は通常のSubstring
        /// </summary>
        /// <param name="str">インスタンス</param>
        /// <param name="startIndex">true時、反転モードとなり、末尾からのインデックス番号となります。この位置から前部分全てを返します。</param>
        /// <param name="reverse">true時、反転モードとなり、startIndexは文字列の末尾からのインデックス番号となります。</param>
        /// <returns></returns>
        public static string Substring(this string str, int startIndex, bool reverse)
        {
            if (!reverse) return str.Substring(startIndex);
            return str.Substring(0, str.Length - startIndex);
        }
    }
}
