using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeedIngredientList : MonoBehaviour
{
    [Header("Need Ingredient")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject IngredientTxt;

    Builder builder;

    public int buildIndex;


    private void Awake()
    {
        builder = FindObjectOfType<Builder>();                
    }
    private void Start()
    {        
        NeedIngretList();
        
    }

    public void NeedIngretList()
    {
        NeedIngredient needIngredient = Instantiate(IngredientTxt, content).GetComponent<NeedIngredient>();
        needIngredient.InitNeedIngredient(builder.craft_Building[buildIndex]);
    }
}
