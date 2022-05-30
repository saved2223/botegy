using System;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class DropDownWindow : MonoBehaviour
    {
        private Dropdown _dropdown;
        private Button _cancelButton;
        private Button _okButton;

        private void Awake()
        {
            _dropdown = gameObject.GetComponentInChildren<Dropdown>();
            _cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
            _okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();
        
            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(string selectedOption, Type t, Action onCancel, Action<string> onOk)
        {
            CreateOptionList(t);
        
            _dropdown.value = _dropdown.options.FindIndex(option => option.text == selectedOption);
            gameObject.SetActive(true);
        
        
            _okButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
        
            _okButton.onClick.AddListener(() =>
            {
                Hide();
                onOk(_dropdown.options[_dropdown.value].text);
            });
            _cancelButton.onClick.AddListener(() =>
            {
                Hide();
                onCancel();
            });

        }

        private void CreateOptionList(Type tType)
        {
            _dropdown.ClearOptions();
            if (tType == typeof(BinaryOperatorBlock))
            {
                foreach (BinaryOperator op in Enum.GetValues(typeof(BinaryOperator)))
                {
                    _dropdown.options.Add(new Dropdown.OptionData() {text = op.GetOperatorText()});
                }
            }
            else if (tType == typeof(FunctionBlock))
            {
                foreach (Functions op in Enum.GetValues(typeof(Functions)))
                {
                    _dropdown.options.Add(new Dropdown.OptionData() {text = op.ToString().ToLower()});
                }
            }
        }
    
    
    }
}
