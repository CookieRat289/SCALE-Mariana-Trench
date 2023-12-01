using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyScript : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject[] enemyTypes = new GameObject[5];
    [SerializeField] Transform fishParent;
    [SerializeField] LogicScript logicScript;

    // Start is called before the first frame update
    void Start()
    {
        if(logicScript.kilometresTravelled > 0) {
            InvokeRepeating("SpawnEnemy", 5f, 2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy() {
        if(logicScript.gameRunning) {
            int chooseType = Random.Range(0, enemyTypes.Length);
            enemyPrefab = enemyTypes[chooseType];
            
            int xScreen = Random.Range(0, 2);
            float xPos = 0;

            if(xScreen == 0) {
                xPos = Random.Range(-14, -11);
            } else if(xScreen == 1) {
                xPos = Random.Range(11, 14);
            }
            float yPos = Random.Range(0, 7);

            Instantiate(enemyPrefab.transform, new Vector3(xPos, yPos, 3), Quaternion.identity, fishParent);
        }
        
    }
}
