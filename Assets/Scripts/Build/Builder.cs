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

    public bool isPreviewActivated = false;
    int buildIndex;

    private void Awake()
    {
        buildBtn = GetComponent<BuildBtn>();
        controller = FindObjectOfType<PlayerController>();        
    }    

    public void SlotClick(int _slotNum)
    {        
        for(int i = 0;  i < craft_Building[_slotNum].Bdata.ingredientItem.Count; i++)
        {
            for(int j = 0; j < Inventory.instance.slots.Length; j++)
            {                
                if (Inventory.instance.slots[j].item != null && craft_Building[_slotNum].Bdata.ingredientItem[i].ingreItem.displayName == Inventory.instance.slots[j].item.displayName)
                {
                    if(Inventory.instance.slots[j].quantity >= craft_Building[_slotNum].Bdata.ingredientItem[i].ingreItemCount)
                    {
                        go_Preview = Instantiate(craft_Building[_slotNum].Bdata.preview_Prefab, Player_pos.position + Player_pos.forward, Quaternion.identity);
                        go_Prefab = craft_Building[_slotNum].Bdata.build_Prefab;
                        isPreviewActivated = true;
                        controller.ToggleCursor(false);
                        buildBtn.CraftPanel.SetActive(false);
                    }
                    else if (Inventory.instance.slots[j].quantity < craft_Building[_slotNum].Bdata.ingredientItem[i].ingreItemCount)
                    {
                        //재료가 부족합니다 판넬                        
                    }
                }
            }
        }
        buildIndex = craft_Building[_slotNum].Bdata.buildingNumber;
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

            for (int i = 0; i < craft_Building[buildIndex].Bdata.ingredientItem.Count; i++)
            {
                for (int j = 0; j < Inventory.instance.slots.Length; j++)
                {
                    if(Inventory.instance.slots[j].item != null && craft_Building[buildIndex].Bdata.ingredientItem[i].ingreItem.displayName == Inventory.instance.slots[j].item.displayName)
                    {
                        Inventory.instance.slots[j].quantity -= craft_Building[buildIndex].Bdata.ingredientItem[i].ingreItemCount;
                        if (Inventory.instance.slots[j].quantity <= 0)
                        {
                            if (Inventory.instance.uiSlots[j].equipped)
                            {
                                Inventory.instance.UnEquip(j);
                            }

                            Inventory.instance.slots[j].item = null;                            
                        }
                    }                    
                    
                }
            }

            Inventory.instance.UpdateUI();
            
        }
    }        
}
