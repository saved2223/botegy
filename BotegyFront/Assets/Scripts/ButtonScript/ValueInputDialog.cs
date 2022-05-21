using System;
using UnityEngine;
using UnityEngine.UI;
using ValueType = Model.ValueType;

public class ValueInputDialog : MonoBehaviour
{
    private Button cancelButton;
    private Button okButton;

    private Dropdown _typedropdown;
    private Dropdown _unitedropdown;
    private Dropdown _booldropdown;
    private InputField _intinputField;
    private InputField _floatinputField;


    private ValueType type;
    private Selectable active;
    private void Awake()
    {
        cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
        okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();

        _typedropdown = transform.Find("TypeDropDown").GetComponent<Dropdown>();
        
        _typedropdown.onValueChanged.AddListener(delegate(int arg0) { DropDownValueChanged(); });


        _unitedropdown = transform.Find("UniteDropdown").GetComponent<Dropdown>();
        _booldropdown = transform.Find("BooleanDropdown").GetComponent<Dropdown>();

        _intinputField = transform.Find("IntInputField").GetComponent<InputField>();
        _floatinputField = transform.Find("FloatInputField").GetComponent<InputField>();
        active = _intinputField;
        
        Hide();
    }

    private void DropDownValueChanged()
    {
        ValueType t = (ValueType)Enum.Parse(typeof(ValueType), _typedropdown.options[_typedropdown.value].text, true);
        this.type = t;
        SetActiveInput();
        _floatinputField.text = "";
        _intinputField.text = "";
    }
    
    public void Show(string value, Action onCancel, Action<string> onOk)
    {
        switch (type)
        {
            case ValueType.INT:
                _intinputField.text = value;
                break;
            case ValueType.FLOAT:
                _floatinputField.text = value;
                break;
            case ValueType.BOOL:
                _booldropdown.value = _booldropdown.options.FindIndex(option => option.text == value);
                break;
            case ValueType.UNITE:
                _unitedropdown.value = _unitedropdown.options.FindIndex(option => option.text == value);
                break;
        }
        
        gameObject.SetActive(true);
        
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        
        okButton.onClick.AddListener(() =>
        {
            Hide();
            onOk(active.GetType() == typeof(Dropdown)
                ? ((Dropdown) active).options[((Dropdown) active).value].text
                : ((InputField) active).text);
        });
        cancelButton.onClick.AddListener(() =>
        {
            Hide();
            onCancel();
        });

    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }


    public void SetType(ValueType _type)
    {
        this.type = _type;
        SetActiveInput();
        _typedropdown.value = _typedropdown.options.FindIndex(option => option.text == type.ToString().ToLower());
    }

    public ValueType GetValueType()
    {
        return type;
    }

    private void SetActiveInput()
    {
        switch (type)
        {
            case ValueType.INT:
            {
                active.gameObject.SetActive(false);
                active = _intinputField;
                active.gameObject.SetActive(true);
            }
                break;
            
            case ValueType.FLOAT:
            {
                active.gameObject.SetActive(false);
                active = _floatinputField;
                active.gameObject.SetActive(true);
            }
                break;
            
            case ValueType.BOOL:
            {
                active.gameObject.SetActive(false);
                active = _booldropdown;
                active.gameObject.SetActive(true);
            }
                break;
            
            case ValueType.UNITE:
            {
                active.gameObject.SetActive(false);
                active = _unitedropdown;
                active.gameObject.SetActive(true);
            }
                break;
        }
    }
    
}
