using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TrelloWebServiceIntegration
{
    class Program
    {

        static void Main(string[] args)
        {
            //MyAPIKey = 04d8591e9052daac8cf038e268174f4a
            //MyToken = 4ecde57d2f7e5d1a9227532566439b93e333adef7ab131eccfca0d27e268cca0
            string secim;
            string apiKey;
            string token;

            Console.WriteLine("API Key'inizi girermisiniz?");
            Console.Write("API Key:");
            apiKey = Console.ReadLine();
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("API Key Boş Olamaz!");
                return;
            }

            Console.WriteLine("Token'ınızı girermisiniz?");
            Console.Write("Token:");
            token = Console.ReadLine();
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token Boş Olamaz!");
                return;
            }

            string boardId = CreateBoard(apiKey, token);
            string firstListId = CreateList(boardId, 1, apiKey, token);
            string secondListId = CreateList(boardId, 2, apiKey, token);

            Console.WriteLine("Hangi Listeye Kart Eklemek İstermisiniz?(1/2)");
            Console.Write("Seçim:");
            secim = Console.ReadLine();
            if (IsNumber(secim))
            {
                if (Convert.ToInt32(secim) == 1)
                {
                    AddAndUpdateAndDeleteCard(firstListId, apiKey, token);
                }
                else
                {
                    AddAndUpdateAndDeleteCard(secondListId, apiKey, token);
                }
            }
            else
            {
                Console.WriteLine("Sayısal Değer Giriniz!");
                return;
            }

            DeleteBoard(boardId, apiKey, token);
            Console.ReadKey();
        }

        #region Create Board
        private static string CreateBoard(string apiKey, string token)
        {
            try
            {
                string boardName;
                Console.WriteLine("Çalışma Alanının Adını Girer Misiniz?");
                Console.Write("Çalışma Alanı Adı:");
                boardName = Console.ReadLine();

                var request = (HttpWebRequest)WebRequest.Create("https://api.trello.com/1/boards/?");

                var postData = "name=" + Uri.EscapeDataString(boardName);
                postData += "&key=" + Uri.EscapeDataString(apiKey);
                postData += "&token=" + Uri.EscapeDataString(token);
                postData += "&defaultLists=false";

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var responseModel = JsonConvert.DeserializeObject<CreateBoardResponse>(responseString);
                return responseModel.id;
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
                return string.Empty;
            }
        }

        #endregion

        #region Create List
        private static string CreateList(string boardId, int order, string apiKey, string token)
        {
            try
            {
                string listName;
                Console.WriteLine(order + ". Liste Adını Girer Misiniz?");
                Console.Write(order + ". Liste Adı:");
                listName = Console.ReadLine();

                var request = (HttpWebRequest)WebRequest.Create("https://api.trello.com/1/lists?");

                var postData = "name=" + Uri.EscapeDataString(listName);
                postData += "&idBoard=" + Uri.EscapeDataString(boardId);
                postData += "&key=" + Uri.EscapeDataString(apiKey);
                postData += "&token=" + Uri.EscapeDataString(token);

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var responseModel = JsonConvert.DeserializeObject<CreateListResponse>(responseString);
                return responseModel.id;
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
                return string.Empty;
            }
        }
        #endregion

        #region Create Card
        private static string CreateCard(string cardId, int order, string apiKey, string token)
        {
            try
            {
                string cardName;
                Console.WriteLine(order + ". Kart Adını Girer Misiniz?");
                Console.Write(order + ". Kart Adı:");
                cardName = Console.ReadLine();

                var request = (HttpWebRequest)WebRequest.Create("https://api.trello.com/1/cards?");

                var postData = "name=" + Uri.EscapeDataString(cardName);
                postData += "&idList=" + Uri.EscapeDataString(cardId);
                postData += "&key=" + Uri.EscapeDataString(apiKey);
                postData += "&token=" + Uri.EscapeDataString(token);

                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var responseModel = JsonConvert.DeserializeObject<CreateListResponse>(responseString);
                return responseModel.id;
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
                return string.Empty;
            }
        }

        #endregion

        #region Add And Update And Delete Card
        private static void AddAndUpdateAndDeleteCard(string listId, string apiKey, string token)
        {
            string firstCardId = CreateCard(listId, 1, apiKey, token);
            string secondCardId = CreateCard(listId, 2, apiKey, token);
            string secim;
            Console.WriteLine("Hangi Kartı Güncellemek İstersiniz?(1/2)");
            Console.Write("Seçim:");
            secim = Console.ReadLine();
            if (IsNumber(secim))
            {
                if (Convert.ToInt32(secim) == 1)
                {
                    UpdateCard(firstCardId, apiKey, token);
                }
                else
                {
                    UpdateCard(secondCardId, apiKey, token);
                }
            }
            else
            {
                Console.WriteLine("Sayısal Değer Giriniz!");
                return;
            }

            DeleteCard(firstCardId, secondCardId, apiKey, token);
        }

        #endregion

        #region Update Card
        private static void UpdateCard(string cardId, string apiKey, string token)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://api.trello.com/1/cards/" + cardId + "?key=" + apiKey + "&token=" + token);

                string name;
                Console.WriteLine("Kartın Yeni Adını Giriniz:");
                Console.Write("Yeni Ad:");
                name = Console.ReadLine();

                string description;
                Console.WriteLine("Kartın Yeni Açıklamasını Giriniz:");
                Console.Write("Açıklama:");
                description = Console.ReadLine();

                var updateCardRequestObject = new UpdateCardRequest
                {
                    name = name,
                    desc = description
                };
                var postDataBytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(updateCardRequestObject));

                request.Method = "PUT";
                request.ContentType = "application/json; charset=UTF-8";
                request.ContentLength = postDataBytes.Length;
                request.Accept = "application/json";

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(postDataBytes, 0, postDataBytes.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
        }

        #endregion

        #region Delete Card
        private static void DeleteCard(string firstCardId, string secondCardId, string apiKey, string token)
        {
            try
            {
                string secim;
                Console.WriteLine("Kartları silmek ister misiniz?(E/H)");
                Console.Write("Seçim:");
                secim = Console.ReadLine();

                if (secim.ToUpper() == "E" || secim.ToUpper() == "H")
                {
                    if (secim.ToUpper().Equals("E"))
                    {
                        var request = (HttpWebRequest)WebRequest.Create("https://api.trello.com/1/cards/" + firstCardId + "?key=" + apiKey + "&token=" + token);
                        request.Method = "DELETE";
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        var request2 = (HttpWebRequest)WebRequest.Create("https://api.trello.com/1/cards/" + secondCardId + "?key=" + apiKey + "&token=" + token);
                        request2.Method = "DELETE";
                        HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                    }
                    else
                    {
                        Console.WriteLine("E yada H Harflerinden Birini Giriniz!");
                        return;
                    }
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
        }

        #endregion

        #region Delete Board
        private static void DeleteBoard(string boardId, string apiKey, string token)
        {
            try
            {
                string secim;
                Console.WriteLine("Çalışma Alanını silmek ister misiniz?(E/H)");
                Console.Write("Seçim:");
                secim = Console.ReadLine();

                if (secim.ToUpper() == "E" || secim.ToUpper() == "H")
                {
                    if (secim.ToUpper().Equals("E"))
                    {
                        var request = (HttpWebRequest)WebRequest.Create("https://api.trello.com/1/boards/" + boardId + "?key=" + apiKey + "&token=" + token);
                        request.Method = "DELETE";
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    }
                }
                else
                {
                    Console.WriteLine("E yada H Harflerinden Birini Giriniz!");
                    return;
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
        }
        #endregion

        public static bool IsNumber(string s)
        {
            return s.All(char.IsDigit);
        }
    }
}
