using AppSceneManager;
using UnityEngine;

namespace ButtonScript
{
    public class ClickEventHandler : MonoBehaviour
    {
        [SerializeField] private BotEditorSceneManager _manager;
    
        public void OnClick()
        {
            _manager.SetSelectedObject(null);
        }
    }
}
