using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class EditConfirmDialog : MonoBehaviour
    {
        private Button _cancelButton;
        private Button _changeButton;
        private Button _duplicateButton;

        private void Awake()
        {
            _cancelButton = transform.Find("Buttons/Image/CancelButton").GetComponent<Button>();
            _changeButton = transform.Find("Buttons/Image/ChangeButton").GetComponent<Button>();
            _duplicateButton = transform.Find("Buttons/Image/DuplicateButton").GetComponent<Button>();
        
            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(Action onCancel, Action onChange, Action onDuplicate)
        {
            _cancelButton.onClick.RemoveAllListeners();
            _changeButton.onClick.RemoveAllListeners();
            _duplicateButton.onClick.RemoveAllListeners();
        
            gameObject.SetActive(true);
        
            _cancelButton.onClick.AddListener(() =>
            {
                Hide();
                onCancel();
            });
        
            _changeButton.onClick.AddListener(() =>
            {
                Hide();
                onChange();
            });
        
            _duplicateButton.onClick.AddListener(() =>
            {
                Hide();
                onDuplicate();
            });

        }
    }
}
