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
    public ItemData item;
}

public class CraftMenu : MonoBehaviour
{

    
    public KitData kitData;
    public Inventory _Inventory;
    public GameObject craftWindow; // 제조메뉴
    [SerializeField] public Transform createPosition; //아이템 생성 위치

    [Header("Selected Craft")]
    public string itemName;
    public Sprite itemImage;
    public string itemDescription;
    public TextMeshProUGUI kitName;
    public TextMeshProUGUI kitDescription;
    public TextMeshProUGUI craftNeedItem;
    public TextMeshProUGUI craftNeedItemCount;

    public GameObject craftBtn;
    public GameObject closeBtn;
    
    PlayerController controller;

    [Header("Events")]
    public UnityEvent onOpenCraft;
    public UnityEvent onCloseCraft;

    void Awake()
    {
        controller = FindObjectOfType<PlayerController>();
    }


    private void Start()
    {
        craftWindow.SetActive(false);
    }

    public void OnCraftWindow()
    {
            bool isOpen = craftWindow.activeSelf;
            craftWindow.SetActive(!isOpen);              
    }


    public void Toggle()
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
}
