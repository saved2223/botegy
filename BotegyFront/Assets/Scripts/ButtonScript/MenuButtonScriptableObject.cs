using UnityEngine;

namespace ButtonScript
{
    [CreateAssetMenu(fileName = "BlockButton")]
    public class MenuButtonScriptableObject : ScriptableObject
    {
        public string text;
        public string tClass;
        public string category;
    }
}
