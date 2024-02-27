using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Building", menuName = "New Building")]
public class BuildingData : ScriptableObject
{
    [Header("Info")]
    public string BuildingName;
    public string description;    
    public Sprite icon;    
}
