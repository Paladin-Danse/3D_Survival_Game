using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonDamge : MonoBehaviour
{

    [SerializeField]
    void DamageObjectsWithTargetTag()
    {
        // ������ ��� GameObject�� ã�ƿɴϴ�.
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        //GameObject[] allObjects = GameObject.CompareTag<GameObject>();

        // ã�� ��� GameObject�� ��ȸ�ϸ� �±װ� ��ġ�ϴ� ��� �����մϴ�.
        //foreach (GameObject obj in allObjects)
        //{

        //    if (obj.CompareTag(targetTag))
        //    {
        //        Destroy(obj);
        //    }
        //}
    }


}
