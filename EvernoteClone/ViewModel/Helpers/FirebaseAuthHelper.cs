using EvernoteClone.Model;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EvernoteClone.ViewModel.Helpers
{
    public class FirebaseAuthHelper
    {
        private static string api_key = "AIzaSyAVRIqmjrfLQRXx4naIqBf_26-1ehDcNGo";

        public async static Task<bool> RegisterAsync(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    email = user.Username,
                    password = user.Password,
                    returnSecureToken = true
                };

                string bodyJson = JsonSerializer.Serialize(body);
                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={api_key}", data);
                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<FirebaseResult>(resultJson);
                    App.UserId = result.localId;
                    return true;
                }
                else
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonSerializer.Deserialize<Error>(errorJson);
                    MessageBox.Show(error.error.message);
                    return false;
                }
            }
        }

        public async static Task<bool> LoginAsync(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    email = user.Username,
                    password = user.Password,
                    returnSecureToken = true
                };

                string bodyJson = JsonSerializer.Serialize(body);
                var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={api_key}", data);
                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<FirebaseResult>(resultJson);
                    App.UserId = result.localId;
                    return true;
                }
                else
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var error = JsonSerializer.Deserialize<Error>(errorJson);
                    MessageBox.Show(error.error.message);
                    return false;
                }
            }
        }
    }

    public class FirebaseResult
    {
        public string kind { get; set; }
        public string idTOken { get; set; }
        public string refreshToken { get; set; }
        public string expiresIn { get; set; }
        public string localId { get; set; }
    }

    public class ErrorDetails
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class Error
    {
        public ErrorDetails error { get; set; }
    }
}
