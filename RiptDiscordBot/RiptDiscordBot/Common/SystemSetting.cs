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

        public static readonly string DBConnectionString = @"Data Source=" + dbFilePath;

        private static readonly string dbFilePath = System.IO.Directory.GetCurrentDirectory() + @"\system.db";

        private static readonly string iniFilePath = System.IO.Directory.GetCurrentDirectory() + @"\setting.ini";

        /// <summary>
        /// 設定ファイルアクセサ
        /// </summary>
        private static IniFile ini = new IniFile(iniFilePath);

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
