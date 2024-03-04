using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildList : MonoBehaviour
{
    [Header("Construction Building")]
    [SerializeField] private Transform content;    
    [SerializeField] private GameObject buildingPanel;    

    Builder builder;
    private void Awake()
    {
        builder = GetComponent<Builder>();
    }
    private void Start()
    {
        ConstructionList();
    }

    void ConstructionList()
    {
        for(int i = 0; i < builder.craft_Building.Length; i++)
        {
            ConstructionBuilding constructionBuilding = Instantiate(buildingPanel, content).GetComponent<ConstructionBuilding>();
            constructionBuilding.InitBuilding(builder.craft_Building[i]);
            constructionBuilding.GetComponent<NeedIngredientList>().buildIndex = i;            
        }
        
    }

}
