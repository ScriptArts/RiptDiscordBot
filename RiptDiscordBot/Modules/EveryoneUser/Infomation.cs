using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace RiptDiscordBot.Modules.EveryoneUser
{
    public class Infomation : ModuleBase
    {
        private CommandService _service;

        public Infomation(CommandService service)
        {
            _service = service;
        }

        [Command("help")]
        [Summary("使えるコマンドの一覧を表示します")]
        public async Task Help()
        {
            var sb = new StringBuilder();
            sb.Append("```");
            foreach (var command in _service.Commands)
            {
                sb.AppendFormat("{0}：{1}\n", command.Name, command.Summary);
            }
            sb.Append("```");
            await ReplyAsync(sb.ToString());

        }

        [Command("ping")]
        [Summary("指定したアドレスにPingを送信します")]
        public async Task Ping([Remainder, Summary("アドレス")] string ip = null)
        {
            if (ip == null)
            {
                await ReplyAsync("pingコマンドの後にアドレスを続けて入力してください\n" +
                                 "例:「!ping skyblock.jp」");
            }
            else
            {
                //Pingオブジェクトの作成
                Ping p = new Ping();
                //"www.yahoo.com"にPingを送信する
                PingReply reply = p.Send(ip);

                //結果を取得
                if (reply.Status == IPStatus.Success)
                {
                    string rep = string.Format("{0} からの応答:bytes={1} time={2}ms",
                        reply.Address, reply.Buffer.Length,
                        reply.RoundtripTime);
                    await ReplyAsync(rep);
                }
                else
                {
                    await ReplyAsync(string.Format("Ping送信に失敗。({0})", reply.Status));
                }
                p.Dispose();
            }
        }

        [Command("chat")]
        [Summary("りぷとちゃんと会話します")]
        public async Task Test(string word)
        {
            string reply = API.Talk.GetChat(word);
            await ReplyAsync(reply);
        }

        [Command("reg")]
        [Summary("")]
        public async Task Reg()
        {
            string reply = API.Docomo.NaturalChatting.RegistrationUser();
            await ReplyAsync(reply);
        }
    }
}
