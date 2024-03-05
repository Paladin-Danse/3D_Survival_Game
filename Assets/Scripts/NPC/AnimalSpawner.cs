using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public class AnimalSpawner : MonoBehaviour
{
    private static AnimalSpawner m_Instance;
    public static AnimalSpawner Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = GameObject.FindObjectOfType<AnimalSpawner>();
            return m_Instance;
        }
    }

    [SerializeField] private GameObject environment;
    [SerializeField] private int SpawnAnimalCount;
    private GameObject[] animals;
    private List<GameObject> animalList;

    float AnimalSpawnTime = 0f;

    private void Awake()
    {
        if (environment == null) environment = GameObject.Find("_Environments");
        animals = Resources.LoadAll<GameObject>("Prefabs/Animal").ToArray();
    }

    void Start()
    {
        animalList = new List<GameObject>();

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
        Vector3 boundsSize = environment.GetComponentInChildren<MeshFilter>().mesh.bounds.size;

        float RandomPosX = environment.transform.position.x + boundsSize.x * 0.5f;
        float RandomPosZ = environment.transform.position.z + boundsSize.z * 0.5f;

        yield return new WaitForSeconds(AnimalSpawnTime);

        if (NavMesh.SamplePosition(new Vector3(Random.Range(-RandomPosX, RandomPosX), 0, Random.Range(-RandomPosZ, RandomPosZ)), out hit, 10f, NavMesh.AllAreas))
        {
            GameObject RandomAnimal = animals[Random.Range(0, animals.Length)];
            GameObject obj = animalList.Find(animal => animal.activeSelf == false);
            
            if (obj != null && obj.transform.GetChild(0).GetComponent<Animal>().data.AnimalID == RandomAnimal.transform.GetChild(0).GetComponent<Animal>().data.AnimalID)
            {
                obj.transform.position = hit.position;
                obj.SetActive(true);
            }
            else
            {
                animalList.Add(Instantiate(RandomAnimal, hit.position, RandomAnimal.transform.rotation));
            }
        }
    }
}
