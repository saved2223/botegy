using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class MatchResultDialog : MonoBehaviour
    {
        private Button _showMatchButton;
        private Button _closeButton;
    
        private void Awake()
        {
            _showMatchButton = transform.Find("Buttons/Image/PlayBackButton").GetComponent<Button>();
            _closeButton = transform.Find("Buttons/Image/CloseButton").GetComponent<Button>();

            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(string text, Action onCancel, Action onOk)
        {
            transform.Find("Text").GetComponent<Text>().text = text;
            gameObject.SetActive(true);
        
            _showMatchButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
        
            _showMatchButton.onClick.AddListener(() =>
            {
                Hide();
                onOk();
            });
            _closeButton.onClick.AddListener(() =>
            {
                Hide();
                onCancel();
            });

        }
    }
}
