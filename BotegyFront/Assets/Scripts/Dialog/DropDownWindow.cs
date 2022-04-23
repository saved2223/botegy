using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownWindow : MonoBehaviour
{
    private Dropdown _dropdown;
    private Button cancelButton;
    private Button okButton;

    private void Awake()
    {
        _dropdown = gameObject.GetComponentInChildren<Dropdown>();
        cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
        okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();
        
        // CreateOptionList();
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
        
        
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        
        okButton.onClick.AddListener(() =>
        {
            Hide();
            // onOk(_dropdown.value.ToString());
            onOk(_dropdown.options[_dropdown.value].text);
        });
        cancelButton.onClick.AddListener(() =>
        {
            Hide();
            onCancel();
        });

    }
    
    private void CreateOptionList(Type tType)
    {
        _dropdown.ClearOptions();
        // _dropdown.value = 0;
        if (tType == typeof(BinaryOperatorBlock))
        {
            foreach (BinaryOperator op in Enum.GetValues(typeof(BinaryOperator)))
            {
                _dropdown.options.Add(new Dropdown.OptionData() {text = op.GetOperatorText()});
            }
        }

        else if (tType == typeof(LoopBlock))
        {
            foreach (LoopBlock.LoopType op in Enum.GetValues(typeof(LoopBlock.LoopType)))
            {
                _dropdown.options.Add(new Dropdown.OptionData() {text = op.ToString().ToLower()});
            }
        }
        
        else if (tType == typeof(FunctionBlock))
        {
            foreach (FunctionBlock.Functions op in Enum.GetValues(typeof(FunctionBlock.Functions)))
            {
                _dropdown.options.Add(new Dropdown.OptionData() {text = op.ToString().ToLower()});
            }
        }
    }
    
    
}
