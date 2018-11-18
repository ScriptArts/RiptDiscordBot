using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiptDiscordBot.Common
{
    public class SystemSetting
    {
        /// <summary>
        /// DiscordBotのトークン
        /// </summary>
        public static string DiscordToken { private set; get; }

        /// <summary>
        /// 設定ファイルアクセサ
        /// </summary>
        private static IniFile ini = new IniFile(System.IO.Directory.GetCurrentDirectory() + @"\setting.ini");

        /// <summary>
        /// iniファイルから設定をロードする
        /// </summary>
        public static void Load()
        {
            DiscordToken = ini.GetValue("Discord", "Token", "");
        }

        /// <summary>
        /// iniファイルに設定を保存する
        /// </summary>
        public static void Save()
        {
            ini["Discord", "Token"] = DiscordToken;
        }
    }
}
