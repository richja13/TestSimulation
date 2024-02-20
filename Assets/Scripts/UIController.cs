using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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

    [SerializeField]
    Material _outlineMaterial;

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

        if (objTransform == _selectedObject)
        {
            _opened = (_opened) ? false : true;
            _animator.SetBool("toggle", _opened);
            RemoveOutline();
            AddOutline();
        }
        else
        {
            RemoveOutline();
            _selectedObject = objTransform;
            AddOutline();
        }

        if (!_opened || _selectedObject is null) return;
        GetAgentStatistics(_selectedObject.GetComponent<AgentController>());
    }

    void AddOutline()
    {
        if (_selectedObject is null || _opened is false) 
            return;
        var renderer = _selectedObject.GetComponent<MeshRenderer>();
        var material = renderer.material;
        List<Material> materialsList = new();
        materialsList.Add(material);
        materialsList.Add(_outlineMaterial);
        renderer.SetMaterials(materialsList);
    }

    void RemoveOutline()
    {
        if (_selectedObject is null)
            return;

        var renderer = _selectedObject.GetComponent<MeshRenderer>();
        var material = renderer.material;
        List<Material> materialsList = new();
        materialsList.Add(material);
        renderer.SetMaterials(materialsList);
    }

    void GetAgentStatistics(AgentController controller)
    {
        _nameText.text = controller.AgentName;
        _hpText.text = controller.CurrentHp.ToString();
        _hpSlider.value = controller.CurrentHp;
    }
}
