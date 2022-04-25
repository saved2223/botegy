using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditConfirmDialog : MonoBehaviour
{
    private Button cancelButton;
    private Button changeButton;
    private Button duplicateButton;

    private void Awake()
    {
        cancelButton = transform.Find("Buttons/Image/CancelButton").GetComponent<Button>();
        changeButton = transform.Find("Buttons/Image/ChangeButton").GetComponent<Button>();
        duplicateButton = transform.Find("Buttons/Image/DuplicateButton").GetComponent<Button>();
        
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(Action onCancel, Action onChange, Action onDuplicate)
    {
        cancelButton.onClick.RemoveAllListeners();
        changeButton.onClick.RemoveAllListeners();
        duplicateButton.onClick.RemoveAllListeners();
        
        cancelButton.onClick.AddListener(() =>
        {
            Hide();
            onCancel();
        });
        
        changeButton.onClick.AddListener(() =>
        {
            Hide();
            onChange();
        });
        
        duplicateButton.onClick.AddListener(() =>
        {
            Hide();
            onDuplicate();
        });

    }
}
