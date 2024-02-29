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
    public GameObject build_Prefab;
    public GameObject preview_Prefab;
    public int buildingNumber;
    public List<IngredientItem> ingredientItem = new List<IngredientItem>();    
}

[System.Serializable]
public class IngredientItem
{
    public ItemData ingreItem;
    public int ingreItemCount;
}
