using UnityEngine;

namespace ButtonScript
{
    public class CategoryScript : MonoBehaviour
    {
        private string _category;

        public string Category
        {
            get => _category;
            set => _category = value;
        }

    }
}