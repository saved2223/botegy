using AppSceneManager;
using Dialog;
using UnityEngine;
using UnityEngine.UI;

namespace ButtonScript
{
    public class HeaderManager : MonoBehaviour
    {
        [SerializeField] private TextInputWindow inputTextDialog;
        [SerializeField] public Text header;
    
        private void Awake()
        {
            header.text = SceneInfo.EditedBot.name != null ? SceneInfo.EditedBot.name : "New_bot";
        }

        public void ChangeBotName()
        {
            ShowTextInputWindow("ВВЕДИТЕ НАЗВАНИЕ БОТА");
        }
    
        private void ShowTextInputWindow(string title)
        {
            inputTextDialog.Show(title, header.text, () => { }, (s) =>
            {
                if (s.Length > 0)
                    header.text = s;
            });
        }    
    }
}
