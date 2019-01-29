using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace RiptDiscordBot.Common
{
    class DBAccesser
    {
        private const string dbName = "discord.db";

        /// <summary>
        /// DB接続先設定を取得
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return "Data Source=" + dbName + ";Version=3;";
        }

        public static void CreateDB()
        {
        }

        /// <summary>
        /// 渡したSQLを実行し、変更を与えた行があればtrueを返す
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns></returns>
        public static bool RunSQL(string sql)
        {
            var ret = false;

            try
            {
                sql = new string(sql.Where(c => !char.IsControl(c)).ToArray());

                // DB接続
                using (var connection = new SQLiteConnection(GetConnectionString()))
                {
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        try
                        {
                            command.Connection.Open();
                            if (command.ExecuteNonQuery() > 0)
                            {
                                ret = true;
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            ret = false;
                        }
                        finally
                        {
                            command.Connection.Close();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ret;
        }

        /// <summary>
        /// 渡したSQLを実行し、取得したデータをすべて返す
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns></returns>
        public static DataTable RunSQLGetResult(string sql)
        {
            var ret = new DataTable();

            try
            {
                sql = new string(sql.Where(c => !char.IsControl(c)).ToArray());

                // DB接続
                using (var connection = new SQLiteConnection(GetConnectionString()))
                {
                    using (var adapter = new SQLiteDataAdapter(sql, connection))
                    {
                        try
                        {
                            adapter.Fill(ret);
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            adapter.Dispose();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ret;
        }
    }
}
