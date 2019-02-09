using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiptDiscordBot.API
{
    class Talk
    {
        public static string GetChat(string word)
        {
            // クライアント＆リクエストの作成
            var client = new RestClient();
            var request = new RestRequest();

            // URLの設定
            client.BaseUrl = new Uri("https://api.a3rt.recruit-tech.co.jp/talk/v1/smalltalk");

            // メソッド、パラメータの指定
            request.Method = Method.POST;
            request.AddParameter("apikey", "");
            request.AddParameter("query", word);

            // リクエスト送信
            var response = client.Execute(request);

            var res = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(response.Content);

            if (res.ContainsKey("results"))
            {
                var results = (Newtonsoft.Json.Linq.JArray)res.GetValue("results");
                var result = (Newtonsoft.Json.Linq.JObject)results[0];
                string reply = result.GetValue("reply").ToString();

                return reply;
            }
            else
            {
                return "";
            }
        }
    }
}
