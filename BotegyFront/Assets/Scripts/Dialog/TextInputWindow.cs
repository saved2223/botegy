using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class TextInputWindow : MonoBehaviour
    {
        private InputField _inputField;
        private Button _cancelButton;
        private Button _okButton;
    
        private void Awake()
        {
            _inputField = gameObject.GetComponentInChildren<InputField>();
            _cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
            _okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();

            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(string title, string text, Action onCancel, Action<string> onOk)
        {
            GetComponentInChildren<Text>().text = title;
        
            _inputField.text = text;
            gameObject.SetActive(true);
        
            _okButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
        
            _okButton.onClick.AddListener(() =>
            {
                Hide();
                onOk(_inputField.text);
            });
            _cancelButton.onClick.AddListener(() =>
            {
                Hide();
                onCancel();
            });

        }

    }
}
