using Entity;
using UnityEngine;

namespace AppSceneManager
{
    public static class SceneInfo
    {
        public static User CurrUser = null;
        public static Bot EditedBot = null;
        public static Match Match = null;

        public static string PrevScene = null;
        public static string SearchPrevScene = null;
    }
}