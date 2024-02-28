using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.InputSystem.InputAction;

[System.Serializable]
public class Building
{
    public BuildingData Bdata;
}
public class Builder : MonoBehaviour
{
    [SerializeField] public Building[] craft_Building;

    [HideInInspector] public GameObject go_Preview;
    [HideInInspector] public GameObject go_Prefab;

    [SerializeField] private Transform Player_pos;    

    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;    

    BuildBtn buildBtn;
    PlayerController controller;
    Inventory inven;

    public bool isPreviewActivated = false;

    private void Awake()
    {
        buildBtn = GetComponent<BuildBtn>();
        controller = FindObjectOfType<PlayerController>();
        inven = Inventory.instance;
    }    

    public void SlotClick(int _slotNum)
    {
        go_Preview = Instantiate(craft_Building[_slotNum].Bdata.preview_Prefab, Player_pos.position + Player_pos.forward, Quaternion.identity);
        go_Prefab = craft_Building[_slotNum].Bdata.build_Prefab;
        isPreviewActivated = true;
        controller.ToggleCursor(false);
        buildBtn.CraftPanel.SetActive(false);

        //if (craft_Building[0].Bdata.ingredientItem[0].ingreItem.displayName == inven.slots[0].item.displayName)
        //{

        //}
    }
        
    void Update()
    {
        if(isPreviewActivated)
        {
            PreviewPosUpdate();
        }
    }

    private void PreviewPosUpdate()
    {
        if(Physics.Raycast(Player_pos.position, Player_pos.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.localEulerAngles = buildBtn.Brotation;
                _location.Set(Mathf.Round(_location.x), Mathf.Round(_location.y / 0.1f) * 0.1f, Mathf.Round(_location.z));
                go_Preview.transform.position = _location;
            }
        }
    }

    public void Build()
    {        
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {
            Instantiate(go_Prefab, go_Preview.transform.position, go_Preview.transform.rotation);
            Destroy(go_Preview);
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }    
}
