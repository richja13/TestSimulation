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
    static Animator _animator;

    static bool _opened;

    public static Transform SelectedObject;

    [SerializeField]
    Material _outlineMaterial;

    [SerializeField]
    Transform _previewCamera;

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

        if (_opened && SelectedObject != null)
        {
            _previewCamera.gameObject.SetActive(true);
            CameraFollow(SelectedObject.position);
        }
        else
            _previewCamera.gameObject.SetActive(false);
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
        if (SelectedObject is null)
            SelectedObject = objTransform;

        if (objTransform == SelectedObject)
        {
            _opened = (_opened) ? false : true;
            _animator.SetBool("toggle", _opened);
            RemoveOutline();
            AddOutline();
        }
        else
        {
            RemoveOutline();
            SelectedObject = objTransform;
            AddOutline();
        }

        if (!_opened || SelectedObject is null) return;
        GetAgentStatistics(SelectedObject.GetComponent<AgentController>());
    }

    void AddOutline()
    {
        if (SelectedObject is null || _opened is false) 
            return;
        var renderer = SelectedObject.GetComponent<MeshRenderer>();
        var material = renderer.material;
        List<Material> materialsList = new();
        materialsList.Add(material);
        materialsList.Add(_outlineMaterial);
        renderer.SetMaterials(materialsList);
    }

    static void RemoveOutline()
    {
        if (SelectedObject is null)
            return;

        var renderer = SelectedObject.GetComponent<MeshRenderer>();
        var material = renderer.material;
        List<Material> materialsList = new();
        materialsList.Add(material);
        renderer.SetMaterials(materialsList);
    }

    void GetAgentStatistics(AgentController controller)
    {
        _nameText.text = controller.AgentName;
        _hpText.text = $"{controller.CurrentHp}/3";
        _hpSlider.value = controller.CurrentHp;
    }

    public static void CheckIfSelectedObjectDisabled(Transform agentTransform)
    {
        if (SelectedObject is null) return;

        if (agentTransform == SelectedObject.transform)
        {
            _opened = false;
            _animator.SetBool("toggle", _opened);
            RemoveOutline();
            SelectedObject = null;
        }
    }

    void CameraFollow(Vector3 pos)
    {
        _previewCamera.transform.position = pos;
    }
}
