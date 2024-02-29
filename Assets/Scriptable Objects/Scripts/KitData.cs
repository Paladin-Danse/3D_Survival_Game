using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum KitType
{
    Table,
    Alchemy,
    Casting,
    CookPot
}

[CreateAssetMenu(fileName = "Kit", menuName = "New Kit")]
public class KitData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public KitType type;
}
