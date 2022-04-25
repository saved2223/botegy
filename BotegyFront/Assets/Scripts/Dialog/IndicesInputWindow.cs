using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicesInputWindow : MonoBehaviour
{
    private InputField _inputField;
    private Button cancelButton;
    private Button okButton;
    
    private void Awake()
    {
        _inputField = gameObject.GetComponentInChildren<InputField>();
        cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
        okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();

        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(string text, Action onCancel, Action<string> onOk)
    {
        _inputField.text = text;
        gameObject.SetActive(true);
        
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        
        okButton.onClick.AddListener(() =>
        {
            Hide();
            onOk(_inputField.text);
        });
        cancelButton.onClick.AddListener(() =>
        {
            Hide();
            onCancel();
        });

    }
}
