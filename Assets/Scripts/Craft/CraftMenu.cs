using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;


public class CraftSlot
{
    public KitData kit;
    public CraftData craftData;
    public ItemData itemData;
}

public class CraftMenu : MonoBehaviour
{
    public CraftSlot[] cSlots;
    public CraftData craftData;
    public KitData kitData;
    public Inventory _Inventory;
    public GameObject craftWindow;
    public Transform craftPostioin;


    [Header("Selected Craft")]
    private CraftSlot selectedCraft;
    private int selectedCraftIndex;
    public TextMeshProUGUI kitName;
    public TextMeshProUGUI kitDescription;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI craftNeedItem;
    public TextMeshProUGUI craftNeedItemCount;

    public GameObject craftBtn;
    public GameObject craftPrefab;

    PlayerController controller;

    [Header("Events")]
    public UnityEvent onOpenCraft;
    public UnityEvent onCloseCraft;


    public static CraftMenu instance;
    
    private void Awake()
    {
        instance = this;
        controller = FindObjectOfType<PlayerController>();
    }


    private void Start()
    {
       
        cSlots = new CraftSlot[3];

        for (int i = 0; i < cSlots.Length; i++)
        {
            cSlots[i] = new CraftSlot();
        }
        ClearSelectCraftWindow();
    }

    public void OnCraftWindow()
    {


        if (craftWindow.activeInHierarchy)
        {
            craftWindow.SetActive(false);
            onCloseCraft?.Invoke();
            controller.ToggleCursor(false);

        }
        else
        {
            craftWindow.SetActive(true);
            onOpenCraft?.Invoke();
            controller.ToggleCursor(true);
        }
    }

    public bool IsOpen()
    {
        return craftWindow.activeInHierarchy;
    }

    public void SelectCraft(int index)
    {
        
    }

    public void OnCraftBtn()
    {
        CraftItem(selectedCraft.itemData);

    }

    private void CraftItem(ItemData item )
    {
        Instantiate(item.dropPrefab, craftPostioin);
        RemoveNeedItem();
    }

    private void RemoveNeedItem()
    {
 
        
    }

    void ClearSelectCraftWindow()
    {
        selectedCraft = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;

        craftBtn.SetActive(false);
    }
}
