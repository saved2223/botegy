using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionWindow : MonoBehaviour
{
    private InputField _inputField;
    private Toggle checkBox;
    private Button cancelButton;
    private Button okButton;
    
    private void Awake()
    {
        _inputField = gameObject.GetComponentInChildren<InputField>();
        cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
        okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();
        checkBox = transform.GetComponentInChildren<Toggle>();
        
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(string text, bool hasElse, Action onCancel, Action<string, bool> onOk)
    {
        _inputField.text = text;
        checkBox.isOn = hasElse;
        gameObject.SetActive(true);
        
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        
        okButton.onClick.AddListener(() =>
        {
            Hide();
            onOk(_inputField.text, checkBox.isOn);
        });
        cancelButton.onClick.AddListener(() =>
        {
            Hide();
            onCancel();
        });

    }
}
