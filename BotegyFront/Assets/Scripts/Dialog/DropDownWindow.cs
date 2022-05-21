using System;
using System.Collections;
using System.Collections.Generic;
using Model;
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
