using AppSceneManager;
using UnityEngine;
using UnityEngine.UI;

public class BlockButtonHandler : MonoBehaviour
{
    private string category;
    private string targetClass;
    private Button _button;
    private CodePanelManager _manager;


    public string Category
    {
        get => category;
        set => category = value;
    }

    public string TargetClass
    {
        get => targetClass;
        set => targetClass = value;
    }

    public CodePanelManager Manager
    {
        get => _manager;
        set => _manager = value;
    }

    private void Awake()
    {
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked()
    {
        _manager.AddBlock(category, targetClass);
       
    }
}
