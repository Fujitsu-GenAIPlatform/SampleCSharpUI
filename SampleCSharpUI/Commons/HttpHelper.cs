using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SampleCSharpUI.Commons
{
    internal class HttpHelper
    {
        private static HttpClient SendClient { get; set; } = null;

        // GET リクエスト送信 
        internal static async Task<string> GetRequestAsync(string apiPath, string idToken)
        {
            var answer = string.Empty;

            if (SendClient == null)
            {
                var ch = new HttpClientHandler();
                ch.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPlicyErrors) => true;
                SendClient = new HttpClient(ch);
            }
            if (SendClient != null)
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Get;
                    request.RequestUri = new Uri($"https://{Config.TenantName}.generative-ai-platform.cloud.global.fujitsu.com{apiPath}");
                    request.Headers.Add("Authorization", string.Format("Bearer {0}", idToken));
                    using (var response = await SendClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        answer = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return answer;
        }

        // DELETE リクエスト送信 
        internal static async Task<string> DeleteRequestAsync(string apiPath, string idToken)
        {
            var answer = string.Empty;

            if (SendClient == null)
            {
                var ch = new HttpClientHandler();
                ch.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPlicyErrors) => true;
                SendClient = new HttpClient(ch);
            }
            if (SendClient != null)
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Delete;
                    request.RequestUri = new Uri($"https://{Config.TenantName}.generative-ai-platform.cloud.global.fujitsu.com{apiPath}");
                    request.Headers.Add("Authorization", string.Format("Bearer {0}", idToken));
                    using (var response = await SendClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        answer = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return answer;
        }

        // POST リクエスト送信 
        internal static async Task<string> PostRequestAsync(string apiPath, string idToken, string requestBody)
        {
            // JSON 文字列を送信する場合は StringContent を作成して共通メソッドへ渡す
            var content = new StringContent(requestBody ?? string.Empty, Encoding.UTF8, "application/json");
            return await SendRequestAsync(HttpMethod.Post, apiPath, idToken, content);
        }
        internal static async Task<string> PostRequestAsync(string apiPath, string idToken, MultipartFormDataContent requestBody)
        {
            // MultipartFormDataContent はそのまま渡せる
            return await SendRequestAsync(HttpMethod.Post, apiPath, idToken, requestBody);
        }

        // PUT リクエスト送信
        internal static async Task<string> PutRequestAsync(string apiPath, string idToken, string requestBody)
        {
            // PUT も JSON 文字列をコンテンツ化して送信
            var content = new StringContent(requestBody ?? string.Empty, Encoding.UTF8, "application/json");
            return await SendRequestAsync(HttpMethod.Put, apiPath, idToken, content);
        }
        internal static async Task<string> PutRequestAsync(string apiPath, string idToken, MultipartFormDataContent requestBody)
        {
            // PUT も JSON 文字列をコンテンツ化して送信
            return await SendRequestAsync(HttpMethod.Put, apiPath, idToken, requestBody);
        }

        // 共通送信メソッド: HttpContent を受け取ることで文字列/マルチパート両対応
        private static async Task<string> SendRequestAsync(HttpMethod method, string apiPath, string idToken, System.Net.Http.HttpContent content = null)
        {
            var answer = string.Empty;

            // 送信用HttpClientは利用できるときは再利用する 
            if (SendClient == null)
            {
                //証明書エラーを無視(高セキュリティ環境では適切に検証すること)
                var ch = new HttpClientHandler();
                ch.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPlicyErrors) => true;
                SendClient = new HttpClient(ch);
            }

            // 送信用HttpClientが利用可能な場合に送信処理を実行
            if (SendClient != null)
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = method;
                    request.RequestUri = new Uri($"https://{Config.TenantName}.generative-ai-platform.cloud.global.fujitsu.com{apiPath}");
                    request.Headers.Add("Authorization", string.Format("Bearer {0}", idToken));

                    // content が null の場合は Content を設定しない
                    if (content != null)
                    {
                        request.Content = content;
                    }

                    // リクエスト送信とレスポンス受信
                    using (var response = await SendClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        answer = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return answer;
        }

    }
}
