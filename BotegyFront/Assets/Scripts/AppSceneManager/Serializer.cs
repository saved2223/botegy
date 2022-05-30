using System;
using System.Collections.Generic;
using UnityEngine;

namespace AppSceneManager
{
    public static class Serializer
    {
        public static T DeserializeObject<T>(string jsonString)
        {
            return JsonUtility.FromJson<T>(jsonString);
        }

        public static List<T> DeserializeObjectList<T>(string jsonString)
        {
            jsonString = "{\"wrapper\":" + jsonString + "}";
            
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonString);
            return wrapper.wrapper;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public List<T> wrapper;
        }
    }
}