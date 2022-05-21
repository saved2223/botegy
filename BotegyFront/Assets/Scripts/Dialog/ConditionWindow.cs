using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionWindow : MonoBehaviour
{
    private Toggle checkBox;
    private Button cancelButton;
    private Button okButton;
    
    private void Awake()
    {
        cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
        okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();
        checkBox = transform.GetComponentInChildren<Toggle>();
        
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(bool hasElse, Action onCancel, Action<bool> onOk)
    {
        checkBox.isOn = hasElse;
        gameObject.SetActive(true);
        
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        
        okButton.onClick.AddListener(() =>
        {
            Hide();
            onOk(checkBox.isOn);
        });
        cancelButton.onClick.AddListener(() =>
        {
            Hide();
            onCancel();
        });

    }
}
