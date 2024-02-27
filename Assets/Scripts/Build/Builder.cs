using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject build_Prefab;
    public GameObject preview_Prefab;
}
public class Builder : MonoBehaviour
{
    [SerializeField] private Craft[] craft_Building;

    [HideInInspector] public GameObject go_Preview;
    [HideInInspector] public GameObject go_Prefab;

    [SerializeField] private Transform Player_pos;

    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    CraftBtn craftBtn;
    PlayerController controller;

    public bool isPreviewActivated = false;

    private void Awake()
    {
        craftBtn = GetComponent<CraftBtn>();
        controller = FindObjectOfType<PlayerController>();
    }
    public void SlotClick(int _slotNum)
    {
        go_Preview = Instantiate(craft_Building[_slotNum].preview_Prefab, Player_pos.position + Player_pos.forward, Quaternion.identity);
        go_Prefab = craft_Building[_slotNum].build_Prefab;
        isPreviewActivated = true;
        controller.ToggleCursor(false);
        craftBtn.CraftPanel.SetActive(false);
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
