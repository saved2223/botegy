using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class ConditionWindow : MonoBehaviour
    {
        private Toggle _checkBox;
        private Button _cancelButton;
        private Button _okButton;
    
        private void Awake()
        {
            _cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
            _okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();
            _checkBox = transform.GetComponentInChildren<Toggle>();
        
            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(bool hasElse, Action onCancel, Action<bool> onOk)
        {
            _checkBox.isOn = hasElse;
            gameObject.SetActive(true);
        
            _okButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
        
            _okButton.onClick.AddListener(() =>
            {
                Hide();
                onOk(_checkBox.isOn);
            });
            _cancelButton.onClick.AddListener(() =>
            {
                Hide();
                onCancel();
            });

        }
    }
}
