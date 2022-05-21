using UnityEngine;
using UnityEngine.UI;

namespace AppSceneManager
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] private CodePanelManager manager;
    
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject prefab;

        private MenuButtonScriptableObject[] objects;
    
        public void AddButtonsByCategory(string categoryName)
        {
            if (panel.activeSelf && menu.transform.Find("Header").GetComponent<Text>().text == categoryName.ToUpper())
                panel.SetActive(false);

            else
            {
                panel.SetActive(true);

                if (objects == null)
                {
                    GetAllObjects();
                }

                menu.transform.Find("Header").GetComponent<Text>().text = categoryName.ToUpper();

                Clear();

                foreach (MenuButtonScriptableObject obj in objects)
                {
                    if (obj.category == categoryName)
                    {
                        var inst = Instantiate(prefab, menu.transform);
                        inst.SetActive(true);
                        inst.GetComponentInChildren<Text>().text = obj.text.Replace("\\n", "\r\n");

                        var script = ((GameObject) inst).GetComponent<BlockButtonHandler>();
                        script.TargetClass = obj.tClass;
                        script.Category = categoryName;

                        script.Manager = manager;
                    }
                }
            }
        }

        private void GetAllObjects()
        {
            objects = Resources.LoadAll<MenuButtonScriptableObject>("Scriptables");
        }
    
        private void Clear()
        {
            foreach (Transform child in menu.transform)
            {
                if (child.gameObject.activeSelf && child.name != "Header" && child.name != "CloseButton")
                    Destroy(child.gameObject);
            }
        }
    }
}
