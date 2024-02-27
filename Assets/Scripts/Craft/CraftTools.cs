using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;


public class CraftTools : MonoBehaviour , IInteractable
{
    public ToolData tool;

    public string GetInteractPrompt()
    {
        return string.Format(" 사용하기 {0}", tool.displayName);
    }

    public void OnInteract()
    {
        Debug.Log("작동!");
    }

    
}
