using System;
using UnityEngine;
using UnityEngine.UI;
using ValueType = Model.ValueType;

namespace ButtonScript
{
    public class ValueInputDialog : MonoBehaviour
    {
        private Button _cancelButton;
        private Button _okButton;

        private Dropdown _typedropdown;
        private Dropdown _unitedropdown;
        private Dropdown _booldropdown;
        private InputField _intinputField;
        private InputField _floatinputField;


        private ValueType _type;
        private Selectable _active;
        private void Awake()
        {
            _cancelButton = transform.Find("Buttons/CancelButton").GetComponent<Button>();
            _okButton = transform.Find("Buttons/OKButton").GetComponent<Button>();

            _typedropdown = transform.Find("TypeDropDown").GetComponent<Dropdown>();
        
            _typedropdown.onValueChanged.AddListener(delegate(int arg0) { DropDownValueChanged(); });


            _unitedropdown = transform.Find("UniteDropdown").GetComponent<Dropdown>();
            _booldropdown = transform.Find("BooleanDropdown").GetComponent<Dropdown>();

            _intinputField = transform.Find("IntInputField").GetComponent<InputField>();
            _floatinputField = transform.Find("FloatInputField").GetComponent<InputField>();
            _active = _intinputField;
        
            Hide();
        }

        private void DropDownValueChanged()
        {
            ValueType t = (ValueType)Enum.Parse(typeof(ValueType), _typedropdown.options[_typedropdown.value].text, true);
            this._type = t;
            SetActiveInput();
            _floatinputField.text = "";
            _intinputField.text = "";
        }
    
        public void Show(string value, Action onCancel, Action<string> onOk)
        {
            switch (_type)
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
        
            _okButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
        
            _okButton.onClick.AddListener(() =>
            {
                Hide();
                onOk(_active.GetType() == typeof(Dropdown)
                    ? ((Dropdown) _active).options[((Dropdown) _active).value].text
                    : ((InputField) _active).text);
            });
            _cancelButton.onClick.AddListener(() =>
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
            this._type = _type;
            SetActiveInput();
            _typedropdown.value = _typedropdown.options.FindIndex(option => option.text == this._type.ToString().ToLower());
        }

        public ValueType GetValueType()
        {
            return _type;
        }

        private void SetActiveInput()
        {
            switch (_type)
            {
                case ValueType.INT:
                {
                    _active.gameObject.SetActive(false);
                    _active = _intinputField;
                    _active.gameObject.SetActive(true);
                }
                    break;
            
                case ValueType.FLOAT:
                {
                    _active.gameObject.SetActive(false);
                    _active = _floatinputField;
                    _active.gameObject.SetActive(true);
                }
                    break;
            
                case ValueType.BOOL:
                {
                    _active.gameObject.SetActive(false);
                    _active = _booldropdown;
                    _active.gameObject.SetActive(true);
                }
                    break;
            
                case ValueType.UNITE:
                {
                    _active.gameObject.SetActive(false);
                    _active = _unitedropdown;
                    _active.gameObject.SetActive(true);
                }
                    break;
            }
        }
    
    }
}
