using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public CraftSlot craftSlot;

    public int index;


    public void Menual(CraftSlot cSlot)
    {
        craftSlot = cSlot;
        icon.gameObject.SetActive(true);
        icon.sprite = cSlot.itemData.icon;
    }

    public void Clear()
    {
        craftSlot = null;
        icon.gameObject.SetActive(false);
       
    }

    public void OnButtonClick()
    {
        CraftMenu.instance.SelectCraft(index);
    }
}
