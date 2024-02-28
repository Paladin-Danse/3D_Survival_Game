using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


public class ItemCraft : MonoBehaviour
{


    public GameObject craftWindow;
    [SerializeField] public Transform createPosition;

    [Header("Selected Craft")]
    public string craftName;
    public Sprite craftImage;
    public string craftDescription;
    public TextMeshProUGUI craftNeedItem;
    public TextMeshProUGUI craftNeedItemCount;

    public GameObject craftBtn;
    public GameObject closeBtn;


    private void Start()
    {
        craftWindow.SetActive(false);
    }


}

