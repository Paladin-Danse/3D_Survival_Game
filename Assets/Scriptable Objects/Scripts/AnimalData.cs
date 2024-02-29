using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Animal", menuName = "Animal Data")]
public class AnimalData : ScriptableObject
{
    public int AnimalID;
    [Header("Stats")]
    public string name;
    public int maxHealth;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;
    
    [Header("AI")]
    public bool isHostile;
    public float detectDistance;
    public float safeDistance;
    [Range(0f, 360f)] public float fieldOfView = 120f;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;
    
    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;
    public float deathToTime;
}
