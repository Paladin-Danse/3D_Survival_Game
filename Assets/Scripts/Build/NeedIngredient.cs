using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeedIngredient : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ingredientTxt;    

    Building building;    

    public void InitNeedIngredient(Building building)
    {
        this.building = building;
        string ingreTxt = "";

        for (int i = 0; i < building.Bdata.ingredientItem.Count; i++)
        {
            string ingredientName = building.Bdata.ingredientItem[i].ingreItem.displayName;
            int ingredientCount = building.Bdata.ingredientItem[i].ingreItemCount;
            
            ingreTxt += $"{ingredientName} X {ingredientCount}\n";
        }
        
        ingredientTxt.text = ingreTxt;
    }
}
