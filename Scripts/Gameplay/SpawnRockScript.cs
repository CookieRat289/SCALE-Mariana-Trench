using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnRockScript : MonoBehaviour
{
    [SerializeField] GameObject rockPrefab;
    [SerializeField] GameObject resourcePrefab;
    [SerializeField] Transform rockParent;
    [SerializeField] AutoScrollScript autoScrollScript;
    VarManager data;
    GameObject spawnedResource;

    public GameObject spawnedRock;
    public float rockTimer;

    int rockSide;
    int rockLimit;

    // Start is called before the first frame update
    void Start()
    {
        string json = File.ReadAllText(Application.dataPath + "SaveData.json");
        VarManager data = JsonUtility.FromJson<VarManager>(json);

        if(data.kilometresTravelled < 3) {
            rockTimer = 5;

        } else if(data.kilometresTravelled < 7) {
            rockTimer = 3;

        } else if(data.kilometresTravelled >= 7) {
            rockTimer = 2;
        }

        rockSide = Random.Range(0, 2);
        InvokeRepeating("SpawnRock", autoScrollScript.startMoveTimer, rockTimer);
    }

    // Update is called once per frame
    void Update()
    {
        string json = File.ReadAllText(Application.dataPath + "SaveData.json");
        VarManager data = JsonUtility.FromJson<VarManager>(json);
    }

    public void SpawnRock() {
        if(autoScrollScript.startMoveTimer <= 0) {
            if(autoScrollScript.moveSpeed > 0) {
                if(rockSide == 0) {
                    spawnedRock = Instantiate(rockPrefab, new Vector3(Random.Range(4f, 9.1f), 8, 1), Quaternion.identity, rockParent);
                    spawnedRock.transform.Rotate(0, 0, 180);
                    rockSide = 1;

                    int resourceChance = Random.Range(0, 10);
                    if(resourceChance <= 7) {
                        spawnedResource = Instantiate(resourcePrefab, new Vector3(7.15f, spawnedRock.transform.position.y - 1.25f, -5), Quaternion.identity, spawnedRock.transform);
                        spawnedResource.transform.Rotate(0, 180, 0);
                    }

                } else if(rockSide == 1) {
                    spawnedRock = Instantiate(rockPrefab, new Vector3(Random.Range(-9, -4.1f), 8, 1), Quaternion.identity, rockParent);
                    rockSide = 0;

                    int resourceChance = Random.Range(0, 10);
                    if(resourceChance <= 7) {
                        spawnedResource = Instantiate(resourcePrefab, new Vector3(-7.15f, spawnedRock.transform.position.y - 1.25f, -5), Quaternion.identity, spawnedRock.transform);
                    }
                }
            }
        }
    }
}
