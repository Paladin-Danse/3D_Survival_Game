using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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

    public bool isPreviewActivated = false;

    private void Awake()
    {
        buildBtn = GetComponent<BuildBtn>();
        controller = FindObjectOfType<PlayerController>();
    }    

    public void SlotClick(int _slotNum)
    {
        go_Preview = Instantiate(craft_Building[_slotNum].Bdata.preview_Prefab, Player_pos.position + Player_pos.forward, Quaternion.identity);
        go_Prefab = craft_Building[_slotNum].Bdata.build_Prefab;
        isPreviewActivated = true;
        controller.ToggleCursor(false);
        buildBtn.CraftPanel.SetActive(false);
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
                go_Preview.transform.position = _location;
            }
        }
    }

    public void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().IsBuildable())
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }    
}
