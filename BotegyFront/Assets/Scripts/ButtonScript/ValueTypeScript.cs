using Model;
using UnityEngine;

public class ValueTypeScript : MonoBehaviour
{
    private ValueType _type;

    public ValueType ValueType
    {
        get => _type;
        set => _type = value;
    }
}
