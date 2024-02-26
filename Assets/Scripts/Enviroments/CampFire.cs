using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    private HashSet<IDamagable> thingsToDamage = new HashSet<IDamagable>();

    private void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    void DealDamage()
    {
        foreach(IDamagable damageable in thingsToDamage)
        {
            damageable.TakePhysicalDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IDamagable damageable))
        {
            thingsToDamage.Add(damageable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable damageable))
        {
            thingsToDamage.Remove(damageable);
        }
    }
}
