using AppSceneManager;
using UnityEngine;

namespace ButtonScript
{
    public class RemoveButtonHandler : MonoBehaviour
    {
        public BotEditorSceneManager _manager;

        public void ButtonClicked()
        {
            _manager.DeleteBlock();
        }
    }
}
