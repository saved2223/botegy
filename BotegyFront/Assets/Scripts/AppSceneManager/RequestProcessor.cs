using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace AppSceneManager
{
    public class RequestProcessor : MonoBehaviour
    {
        private const string URL = "https://botegy.herokuapp.com/";

        public void GenerateGetRequest<T>(string request, Dictionary<string, string> args, Action<string> action)
        {
            StartCoroutine(ProcessGetRequest<T>(URL + request + "?" + CreateRequestString(args), action));
        }        
        
        public void GenerateGetRequest(string request, Action action)
        {
            StartCoroutine(ProcessGetRequest(URL + request, action));
        }
        
        public void GeneratePostRequest<T>(string request, Dictionary<string, string> args, Action<string> action)
        {
            StartCoroutine(ProcessPostRequest<T>(URL + request, args, action));
        }        
        
        public void GeneratePutRequest<T>(string request, Dictionary<string, string> args, Action<string> action)
        {
            StartCoroutine(ProcessPutRequest<T>(URL + request, "?" + CreateRequestString(args), action));
        }        
        
        public void GeneratePutRequest(string request, Dictionary<string, string> args, Action action)
        {
            StartCoroutine(ProcessPutRequest(URL + request, "?" + CreateRequestString(args), action));
        }
        
        public void GenerateDeleteRequest(string request, Dictionary<string, string> args, Action action)
        {
            StartCoroutine(ProcessDeleteRequest(URL + request + "?" + CreateRequestString(args), action));
        }
        private static IEnumerator ProcessPostRequest<T>(string uri, Dictionary<string, string> dict, Action<string> onSuccess)
        {
            using (UnityWebRequest request = UnityWebRequest.Post(uri, dict))
            {
                yield return request.SendWebRequest();
                if (request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    while (!request.isDone)
                        yield return null;
                    byte[] result = request.downloadHandler.data;
                    
                    onSuccess(System.Text.Encoding.Default.GetString(result));
                }
            }
        }
        
        
        public static IEnumerator ProcessGetRequest<T>(string uri, Action<string> onSuccess)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    while (!request.isDone)
                        yield return null;
                    byte[] result = request.downloadHandler.data;

                    onSuccess(System.Text.Encoding.Default.GetString(result));
                }
            }
        }
        
        public static IEnumerator ProcessGetRequest(string uri, Action onSuccess)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    while (!request.isDone)
                        yield return null;
                    byte[] result = request.downloadHandler.data;

                    onSuccess();
                }
            }
        }


        public static IEnumerator ProcessPutRequest<T>(string uri, string bodyData, Action<string> onSuccess)
        {
            using (UnityWebRequest request = UnityWebRequest.Put(uri + bodyData, "data"))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    while (!request.isDone)
                        yield return null;
                    byte[] result = request.downloadHandler.data;

                    onSuccess(System.Text.Encoding.Default.GetString(result));
                }
            }
        }
        public static IEnumerator ProcessPutRequest(string uri, string bodyData, Action onSuccess)
        {
            using (UnityWebRequest request = UnityWebRequest.Put(uri + bodyData, "data"))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    while (!request.isDone)
                        yield return null;
                    byte[] result = request.downloadHandler.data;

                    onSuccess();
                }
            }
        }

        public static IEnumerator ProcessDeleteRequest(string uri, Action onSuccess)
        {
            using (UnityWebRequest request = UnityWebRequest.Delete(uri))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    while (!request.isDone)
                        yield return null;

                    onSuccess();
                }
            }
        }

        private static string CreateRequestString(Dictionary<string, string> dict)
        {
            string str = "";
            
            if (dict.Count >  0)
            {
                str = dict.Aggregate(str, (current, entry) => current + (entry.Key + "=" + entry.Value + "&"));

                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }        
        
        public static string PrepareURL(string data)
        {
            Dictionary<string, string> replacementDict = new Dictionary<string, string>()
                {
                    {" ", "%20" },
                    {"!", "%21" },
                    {"'", "%27"},
                    {"(", "%28"},
                    {")", "%29"},
                    {"*", "%2A"},
                    {"+", "%2B"},
                    {",", "%2C"},
                    {"-", "%2D"},
                    {".", "%2E"},
                    {"/", "%2F"},
                    {";", "%3B"},
                    {">", "%3E"},
                    {"<", "%3C"},
                    {"?", "%3F"},
                    {"{", "%7B"},
                    {"}", "%7D"},
                    {"=", "%3D"},
                    {"_", "%5F"},
                    {"\n", "%0A"},
                    {"\t", "%09"}
                };

            foreach (KeyValuePair<string, string> kv in replacementDict)
            {
                data = data.Replace(kv.Key, kv.Value);
            }

            return data;
        } 
        
        public static string PrepareName(string data)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9_]");
            data = rgx.Replace(data, "");
            return data;
        }        
    }
}