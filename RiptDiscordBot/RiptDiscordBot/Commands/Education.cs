using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiptDiscordBot.Commands
{
    public class Education : ModuleBase
    {
        private CommandService _service;

        public Education(CommandService service)
        {
            _service = service;
        }

        [Command("addeducation")]
        [Summary("自動応答を教育します")]
        public async Task AddEducation([Remainder, Summary("ワード")] string word, [Remainder, Summary("応答内容")] string reply)
        {
            AI.AutoReply.AddReply(word, reply);

            string ret = string.Format("ワード:{0} 応答内容:{1}を覚えました", word, reply);

            await ReplyAsync(ret);
        }
    }
}
