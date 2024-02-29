using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;


public class CraftKit : MonoBehaviour , IInteractable
{
    public KitData kit;
    public CraftMenu ItemCraft; 

    public string GetInteractPrompt()
    {
        return string.Format("<color=yellow>" + " 사용하기 {0}" + "</color>"  , kit.displayName);
    }

    public void OnInteract()
    {
        if (ItemCraft != null)
        {
            ItemCraft.OnCraftWindow();
        }
    }
}
