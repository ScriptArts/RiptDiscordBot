using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using RiptDiscordBot.Common;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestProgram
{
    class Program
    {
        public static DiscordSocketClient client;
        public static CommandService commands;
        public static IServiceProvider services;
        private const ulong channelId = 511477391730802692;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();


        public async Task MainAsync()
        {
            // DBがない場合再作成
            DBAccesser.CreateDB();

            // Discordへの接続
            client = new DiscordSocketClient();
            commands = new CommandService();
            services = new ServiceCollection().BuildServiceProvider();
            client.MessageReceived += CommandRecieved;
            client.UserJoined += UserJoined;
            client.UserLeft += UserLeft;
            client.Log += Log;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
            await client.LoginAsync(TokenType.Bot, RiptDiscordBot.Common.Connection.DiscordToken);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        /// <summary>
        /// 何かしらのメッセージの受信
        /// </summary>
        /// <param name="msgParam"></param>
        /// <returns></returns>
        private async Task CommandRecieved(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;

            Console.WriteLine("{0} {1}:{2}", message.Channel.Name, message.Author.Username, message);
            //メッセージがnullの場合
            if (message == null)
                return;

            //発言者がBotの場合無視する
            if (message.Author.IsBot)
                return;

            // 自動応答
            int argPos = 0;
            if (!message.HasCharPrefix('!', ref argPos))
            {
                await new RiptDiscordBot.Modules.EveryoneUser.Education.Reply().SendReply(message);
            }

            // コマンド実行
            argPos = 0;

            //コマンドかどうか判定
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos)))
                return;

            var context = new CommandContext(client, message);
            //コマンドを実行
            var result = await commands.ExecuteAsync(context, argPos, services);

            //実行できなかった場合
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }

        /// <summary>
        /// ユーザーが会議に参加したときの処理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task UserJoined(SocketGuildUser user)
        {
            var chatchannnel = client.GetChannel(channelId) as SocketTextChannel;
            string welcome = string.Format("{0}様、ようこそScriptArtsへ！", user.Username);
            await chatchannnel.SendMessageAsync(welcome);
        }

        /// <summary>
        /// ユーザーが会議から抜けたときの処理
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task UserLeft(SocketGuildUser user)
        {
            var chatchannnel = client.GetChannel(channelId) as SocketTextChannel;
            string bye = string.Format("{0}様さようなら、またのお越しをお待ちしております", user.Username);
            await chatchannnel.SendMessageAsync(bye);
        }

        /// <summary>
        /// Discordから切断されてしまった場合
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private Task Disconnected(Exception ex)
        {
            // ソフトウェア再起動で再接続
            Console.WriteLine("異常を検知したためTUSBちゃんを再起動します...");
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            Environment.Exit(0);
            return Task.CompletedTask;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}