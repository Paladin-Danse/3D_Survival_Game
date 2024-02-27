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
        return string.Format(" ����ϱ� {0}", tool.displayName);
    }

    public void OnInteract()
    {
        Debug.Log("�۵�!");
    }

    
}
