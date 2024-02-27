using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonDamge : MonoBehaviour
{

    [SerializeField]
    void DamageObjectsWithTargetTag()
    {
        // 씬에서 모든 GameObject를 찾아옵니다.
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        //GameObject[] allObjects = GameObject.CompareTag<GameObject>();

        // 찾은 모든 GameObject를 순회하며 태그가 일치하는 경우 삭제합니다.
        //foreach (GameObject obj in allObjects)
        //{

        //    if (obj.CompareTag(targetTag))
        //    {
        //        Destroy(obj);
        //    }
        //}
    }


}
