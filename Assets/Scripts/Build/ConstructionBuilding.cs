using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionBuilding : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buildingNameUITxt;
    [SerializeField] private TextMeshProUGUI buildingExTxt;
    [SerializeField] private Image buildingImageUI;
    [SerializeField] private Button buildButton;

    Building building;
    Builder builder;
    private void Awake()
    {
        builder = FindObjectOfType<Builder>();
    }
    public void InitBuilding(Building building)
    {
        this.building = building;

        buildingNameUITxt.text = building.Bdata.BuildingName;
        buildingImageUI.sprite = building.Bdata.icon;
        buildingExTxt.text = building.Bdata.description;
        buildButton.onClick.AddListener(() => builder.SlotClick(building.Bdata.buildingNumber));
    }
}
