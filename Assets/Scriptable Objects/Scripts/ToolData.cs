using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ToolType
{
    Table,
    Alchemy,
    Casting,
    Cooking
}

[CreateAssetMenu(fileName = "Tool", menuName = "New Tool")]
public class ToolData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ToolType type;
}
