using Discord.WebSocket;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiptDiscordBot.API.Docomo
{
    class NaturalChatting
    {
        public static string RegistrationUser()
        {
            // クライアント＆リクエストの作成
            var client = new RestClient();
            var request = new RestRequest();

            // URLの設定
            client.BaseUrl = new Uri("https://api.a3rt.recruit-tech.co.jp/talk/v1/smalltalk");

            // メソッド、パラメータの指定
            request.Method = Method.POST;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("APIKEY", "");
            request.AddParameter("botId", "Chatting", ParameterType.RequestBody);
            request.AddParameter("appKind", "Discord Bot", ParameterType.RequestBody);

            // リクエスト送信
            var response = client.Execute(request);

            var res = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(response.Content);

            if (res.ContainsKey("appId"))
            {
                var appId = (Newtonsoft.Json.Linq.JValue)res.GetValue("appId");
                string reply = appId.ToString();

                return reply;
            }
            else
            {
                return "";
            }
        }
    }
}
