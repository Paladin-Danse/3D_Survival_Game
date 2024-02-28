using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;


public class CraftTools : MonoBehaviour , IInteractable
{
    public ToolData tool;
    public ItemCraft ItemCraft; 

    public string GetInteractPrompt()
    {
        return string.Format(" 사용하기 {0}"+ "<color=yellow>" + "(E)" + "</color>", tool.displayName);
    }

    public void OnInteract()
    {
        ItemCraft.craftWindow.SetActive(true);
    }
}
