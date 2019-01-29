using Discord.Commands;
using RiptDiscordBot.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace RiptDiscordBot.Modules.EveryoneUser.Education
{
    public class Education : ModuleBase
    {
        [Command("listeducation")]
        [Alias("listedu")]
        [Summary("自動応答のリストを表示します")]
        public async Task ListEducation()
        {
            var serverId = Context.Guild.Id;
            var sql = string.Format("SELECT * FROM 自動応答 WHERE サーバーID = '{0}' ORDER BY 言葉",
                serverId);

            var table = DBAccesser.RunSQLGetResult(sql);
            foreach (DataRow row in table.Rows)
            {
                var word = row["言葉"].ToString();
                var reply = row["返事"].ToString();

                string format = string.Format("言葉:{0} 返事:{1}", word, reply);
                await ReplyAsync(format);
            }
        }

        [Command("addeducation")]
        [Alias("addedu")]
        [Summary("自動応答を追加します")]
        public async Task AddEducation(string word, string reply)
        {
            var serverId = Context.Guild.Id;
            var sql = string.Format("SELECT * FROM 自動応答 WHERE サーバーID = '{0}' AND 言葉 = '{1}'",
                serverId,
                word);

            var table = DBAccesser.RunSQLGetResult(sql);

            sql = string.Format("INSERT INTO 自動応答 VALUES ('{0}', '{1}', '{2}')",
                serverId,
                word,
                reply);

            if (DBAccesser.RunSQL(sql))
            {
                await ReplyAsync(string.Format("言葉:「{0}」 返事:「{1}」を覚えました",
                    word,
                    reply));
            }
            else
            {
                await ReplyAsync("うまく覚えられませんでした・・・");
            }
        }

        [Command("removeeducation")]
        [Alias("removeedu")]
        [Summary("自動応答を削除します")]
        public async Task RemoveEducation(string word)
        {
            var serverId = Context.Guild.Id;
            var sql = string.Format("DELETE FROM 自動応答 WHERE サーバーID = '{0}' AND 言葉 = '{1}'",
                serverId,
                word);

            if (DBAccesser.RunSQL(sql))
            {
                await ReplyAsync(string.Format("言葉:「{0}」を忘れました",
                    word));
            }
            else
            {
                await ReplyAsync("うまく忘れられませんでした・・・");
            }
        }
    }
}
