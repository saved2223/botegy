using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteConfirmDialog : MonoBehaviour
{
    private Button cancelButton;
    private Button okButton;

    private void Awake()
    {
        cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
        okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();

        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(Action onCancel, Action onOk)
    {
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        
        okButton.onClick.AddListener(() =>
        {
            Hide();
            onOk();
        });
        cancelButton.onClick.AddListener(() =>
        {
            Hide();
            onCancel();
        });

    }
}
