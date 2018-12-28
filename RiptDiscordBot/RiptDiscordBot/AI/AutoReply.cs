using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace RiptDiscordBot.AI
{
    class AutoReply
    {
        /// <summary>
        /// リプライ内容を取得する
        /// ワードに紐づく応答内容が複数存在する場合はランダムに選出
        /// </summary>
        /// <param name="word">ワード</param>
        /// <returns></returns>
        public static string GetReply(string word)
        {
            string ret = "";

            //検索
            using (SQLiteConnection connection = new SQLiteConnection(Common.SystemSetting.DBConnectionString))
            {
                connection.Open();

                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    //SQLの設定
                    cmd.CommandText = "SELECT * FROM AutoReply WHERE Word LIKE + '%" + "@Word" + "%'";

                    //パラメータの設定
                    cmd.Parameters.Add(new SQLiteParameter("@Word", word));

                    //準備
                    cmd.Prepare();

                    //検索
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        var list = new List<string>();
                        while (reader.Read())
                        {
                            string reply = reader["Reply"].ToString();
                            list.Add(reply);
                            break;
                        }

                        if (list.Count > 0)
                        {
                            var rnd = new System.Random();
                            ret = list[rnd.Next(list.Count)];
                        }

                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// 自動応答を追加する
        /// 既に同様の返答が存在する場合は上書き
        /// </summary>
        /// <param name="word">ワード</param>
        /// <param name="reply">返答内容</param>
        public static void AddReply(string word, string reply)
        {
            //検索
            using (SQLiteConnection connection = new SQLiteConnection(Common.SystemSetting.DBConnectionString))
            {
                connection.Open();

                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    string sql = "";
                    sql += " " + "UPDATE";
                    sql += " " + "AUTOREPLY";
                    sql += " " + "SET";
                    sql += " " + "Word = @Word";
                    sql += " " + "WHERE";
                    sql += " " + "Reply = " + "@Reply";

                    sql += " " + "IF @@ROWCOUNT = 0";

                    sql += " " + "INSERT INTO";
                    sql += " " + "AUTOREPLY";
                    sql += " " + "VALUES";
                    sql += " " + "(";
                    sql += " " + "@Word,";
                    sql += " " + "@Reply";
                    sql += " " + ")";

                    //SQLの設定
                    cmd.CommandText = sql;

                    //パラメータの設定
                    cmd.Parameters.Add(new SQLiteParameter("@Word", word));
                    cmd.Parameters.Add(new SQLiteParameter("@Reply", reply));

                    //準備
                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
