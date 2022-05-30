using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class DeleteConfirmDialog : MonoBehaviour
    {
        private Button _cancelButton;
        private Button _okButton;

        private void Awake()
        {
            _cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
            _okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();

            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(Action onCancel, Action onOk)
        {
            _okButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
        
            gameObject.SetActive(true);
        
            _okButton.onClick.AddListener(() =>
            {
                Hide();
                onOk();
            });
            _cancelButton.onClick.AddListener(() =>
            {
                Hide();
                onCancel();
            });

        }
    }
}
