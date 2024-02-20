using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class UIMarcoPolo : MonoBehaviour
{
    [SerializeField]
    Button _openButton;

    Animator _animator;

    bool _open;

    [SerializeField]
    TMP_InputField _inputField;
    [SerializeField]
    TMP_Text _outputText;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _openButton.onClick.AddListener(TogglePanel);
        _inputField.onEndEdit.AddListener(ValuePassed);
    }

    private void ValuePassed(string value)
    {
        _outputText.text = MarcoPoloSystem.Calculate(Int32.Parse(value));
    }

    void TogglePanel()
    {
        _open = (_open) ? false : true;
        _animator.SetBool("opened", _open);
        _openButton.GetComponentInChildren<TMP_Text>().text = (!_open) ? "^OPEN PANEL^" : "CLOSE PANEL";
    }
}
