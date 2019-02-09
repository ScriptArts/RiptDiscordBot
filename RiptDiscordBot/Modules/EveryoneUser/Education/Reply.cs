using Discord.WebSocket;
using RiptDiscordBot.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiptDiscordBot.Modules.EveryoneUser.Education
{
    class Reply
    {
        public async Task SendReply(SocketUserMessage message)
        {
            var channel = message.Channel as SocketGuildChannel;
            var serverId = channel.Guild.Id;
            var content = message.Content;
            var sql = string.Format("SELECT * FROM 自動応答 WHERE サーバーID = '{0}'",
                serverId);

            using (var table = DBAccesser.RunSQLGetResult(sql))
            {
                var list = new List<string>();

                foreach (DataRow row in table.Rows)
                {
                    string word = row["言葉"].ToString();
                    string reply = row["返事"].ToString();

                    if (content.Contains(word))
                    {
                        list.Add(reply);
                    }
                }

                if (list.Count > 0)
                {
                    var rnd = new Random();
                    var reply = list[rnd.Next(list.Count)];

                    if (reply == "$")
                    {
                        await message.Channel.SendMessageAsync(API.Talk.GetChat(content));
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync(reply);
                    }
                    
                }
            }

            await Task.Delay(1);
        }
    }
}
