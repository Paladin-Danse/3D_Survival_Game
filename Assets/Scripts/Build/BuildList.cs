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
        foreach(Building building in builder.craft_Building)
        {
            ConstructionBuilding constructionBuilding = Instantiate(buildingPanel, content).GetComponent<ConstructionBuilding>();
            constructionBuilding.InitBuilding(building);
        }
    }

}
