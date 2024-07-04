using TMPro;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField fieldToFill;
    public void TypeKey(string letter)
    {
        fieldToFill.text = $"{fieldToFill.text}{letter}";
    }
    public void Backspace()
    {
        if (fieldToFill.text.Length <= 0)
        {
            return;
        }
        fieldToFill.text = fieldToFill.text[..^1];
    }
}
