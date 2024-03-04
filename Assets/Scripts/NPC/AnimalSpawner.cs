using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public class AnimalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Enviroment;
    private GameObject[] animals;
    private int SpawnAnimalCount;
    private List<GameObject> animalList;

    float AnimalSpawnTime = 0f;

    private void Awake()
    {
        if (Enviroment == null) Enviroment = GameObject.Find("_Enviroments");
        animals = Resources.LoadAll<GameObject>("Prefabs/Animal").ToArray();
    }

    void Start()
    {
        for (int i = 0; i < SpawnAnimalCount; i++)
        {
            StartCoroutine(AnimalSpawnCoroutine());
        }

        AnimalSpawnTime = 10f;
    }

    public void AnimalRespawn()
    {
        StartCoroutine(AnimalSpawnCoroutine());
    }

    IEnumerator AnimalSpawnCoroutine()
    {
        NavMeshHit hit;

        float RandomPosX = Enviroment.transform.position.x + Enviroment.transform.localScale.x * 0.5f;
        float RandomPosZ = Enviroment.transform.position.z + Enviroment.transform.localScale.z * 0.5f;

        yield return new WaitForSeconds(AnimalSpawnTime);

        if (NavMesh.SamplePosition(new Vector3(Random.Range(-RandomPosX, RandomPosX), 0, Random.Range(-RandomPosZ, RandomPosZ)), out hit, 10f, NavMesh.AllAreas))
        {
            GameObject obj = animalList.Find(animal => animal.activeSelf == false);
            if (obj != null)
            {
                obj.transform.position = hit.position;
                obj.SetActive(true);
            }
            else
            {
                GameObject RandomAnimal = animals[Random.Range(0, animals.Length)];

                animalList.Add(Instantiate(RandomAnimal, hit.position, RandomAnimal.transform.rotation));
            }
        }
    }
}
