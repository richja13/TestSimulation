using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    TMP_Text _hpText;

    [SerializeField]
    Slider _hpSlider;

    [SerializeField]
    TMP_Text _nameText;

    [SerializeField]
    Animator _animator;

    bool _opened;

    Transform _selectedObject;

    delegate void MouseClicked(RaycastHit hit);
    event MouseClicked mouseClicked;


    void Start()
    {
        _animator = GetComponent<Animator>();
        mouseClicked += OnAgentClicked;
    }

    void Update()
    {
        DetectMouseClick();    
    }

    void DetectMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                mouseClicked?.Invoke(hit);
        }
    }

    void OnAgentClicked(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Agent"))
            ToggleUI(hit.transform);
    }

    void ToggleUI(Transform objTransform)
    {
        if (_selectedObject is null)
            _selectedObject = objTransform;

        if (objTransform.name == _selectedObject.name)
        {
            _opened = (_opened) ? false : true;
            _animator.SetBool("toggle", _opened);
        }
        else
        {
            _selectedObject = objTransform;
            _animator.SetTrigger("reset");
        }
    }
}
