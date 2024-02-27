using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private Transform Player_pos;

    CraftBtn craftBtn;

    public bool isPreviewActivated = false;

    private void Awake()
    {
        craftBtn = GetComponent<CraftBtn>();
    }
    public void SlotClick(int _slotNum)
    {
        go_Preview = Instantiate(craft_Building[_slotNum].preview_Prefab, Player_pos.position + Player_pos.forward, Quaternion.identity);
        isPreviewActivated = true;
        craftBtn.CraftPanel.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
