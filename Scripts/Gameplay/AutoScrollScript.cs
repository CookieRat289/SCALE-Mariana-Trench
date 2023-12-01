using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AutoScrollScript : MonoBehaviour
{
    [SerializeField] GameObject autoScroller;
    [SerializeField] SubmarineMoveScript submarineMoveScript;
    [SerializeField] SpawnRockScript spawnRockScript;
    public AudioSource dingSource;
    public AudioClip dingSound;
    public RectTransform fader;
    VarManager data;

    public GameObject winPrefab;
    public GameObject winLine;
    public GameObject foundRock;
    public Camera cam;

    public float moveSpeed;
    public float subMoveSpeed;
    public float startMoveTimer;
    public float distanceTimer;
    public float metresTravelled;
    public bool canMove;
    public bool canRock;
    public bool isDingNoise;

    // Start is called before the first frame update
    void Start()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1, 0);
        LeanTween.alpha(fader, 0, 0.5f).setOnComplete(() => {
            fader.gameObject.SetActive(false);
        });

        string json = File.ReadAllText(Application.dataPath + "SaveData.json");
        VarManager data = JsonUtility.FromJson<VarManager>(json);

        distanceTimer = 45f;
        startMoveTimer = 5f;

        canRock = true;

        if(data.kilometresTravelled < 3) {
            moveSpeed = 1f;
            subMoveSpeed = 1f;
            spawnRockScript.rockTimer = 5;
            cam.backgroundColor = new Color32(50, 77, 121, 1);

        } else if(data.kilometresTravelled < 5) {
            moveSpeed = 3f;
            subMoveSpeed = 3f;
            spawnRockScript.rockTimer = 3f;
            cam.backgroundColor = new Color32(50, 91, 121, 1);

        } else if(data.kilometresTravelled < 7) {
            moveSpeed = 3f;
            subMoveSpeed = 3f;
            spawnRockScript.rockTimer = 3f;
            cam.backgroundColor = new Color32(50, 104, 150, 1);

        } else if(data.kilometresTravelled >= 7) {
            moveSpeed = 4f;
            subMoveSpeed = 4f;
            spawnRockScript.rockTimer = 2f;
            cam.backgroundColor = new Color32(50, 130, 180, 1);
        }

        subMoveSpeed = moveSpeed;

        InvokeRepeating("IncreaseMetres", (startMoveTimer + distanceTimer / 10f), (distanceTimer / 10f));
    }

    // Update is called once per frame
    void Update()
    {
        string json = File.ReadAllText(Application.dataPath + "SaveData.json");
        VarManager data = JsonUtility.FromJson<VarManager>(json);

        if(startMoveTimer > 0) {
            startMoveTimer -= Time.deltaTime;
        } else {
            canMove = true;
        }

        if(isDingNoise) {
            dingSource.PlayOneShot(dingSound, 1f);
            isDingNoise = false;
        }

        subMoveSpeed = moveSpeed;
    }

    void FixedUpdate() {
        if(startMoveTimer <= 0) {
            if(distanceTimer > 0) {
                distanceTimer -= Time.deltaTime;
            } else if(distanceTimer <= 0) {
                if(winLine == null) {
                    winLine = Instantiate(winPrefab, new Vector3(0, 15, 0), Quaternion.identity, autoScroller.transform);
                }

                canRock = false;
            }
                
            int children = transform.childCount;
            for(int x = 0; x < children; x++) {
                Transform child = this.gameObject.transform.GetChild(x);
                Vector3 childPos = child.position;

                if(child.name != "Submarine") {
                    childPos.y -= moveSpeed * Time.fixedDeltaTime;

                    if(child.name == "Walls") {
                        int wallChilren = child.childCount;
                        for(int y = 0; y < wallChilren; y++) {
                            Transform wallChild = child.GetChild(y);
                            if(wallChild.position.y <= -9.5f) {
                                wallChild.position = new Vector3(wallChild.position.x, 10.5f, wallChild.position.z);
                            }
                        }
                    } else if(child.name == "Rocks") {
                        int rockChildren = child.childCount;
                        for(int z = 0; z < rockChildren; z++) {
                            Transform rockChild = child.GetChild(z);
                            if(rockChild.position.y <= -6.5f) {
                                Destroy(rockChild.gameObject);
                            }
                        }
                    } else if(child.name == "Resource(Clone)") {
                        if(childPos.y <= -6.5f) {
                            Destroy(child.gameObject);
                        }
                    } else if(child.name == "WinLine(Clone)") {
                        if(childPos.y <= -12) {
                            Destroy(child.gameObject);
                            winLine = Instantiate(winPrefab, new Vector3(0, 15, 0), Quaternion.identity, autoScroller.transform);
                        }
                    }

                } else {
                    if(childPos.y > -6.5f) {
                        childPos.y -= subMoveSpeed * Time.fixedDeltaTime;
                    } else {
                        canMove = false;

                        childPos = new Vector3(7.5f, 0, 0);
                        submarineMoveScript.OnHit(25f);

                        canMove = true;
                    }
                }

                child.position = childPos;
            }
        }
    }

    void IncreaseMetres() {
        if(canMove) {
            metresTravelled += 100f;
        }
    }

    public void SetData(float kmIncrease, string itemKey, float itemIncrease) {
        string json = File.ReadAllText(Application.dataPath + "SaveData.json");
        VarManager data = JsonUtility.FromJson<VarManager>(json);

        data.kilometresTravelled += kmIncrease;

        switch(itemKey) {
            case "Seaweed":
                data.seaweedAmount += itemIncrease;
                break;
            case "Rocks":
                data.rocksAmount += itemIncrease;
                break;
            case "Shells":
                data.shellsAmount += itemIncrease;
                break;
            case "Algae":
                data.algaeAmount += itemIncrease;
                break;
            case "Metal":
                data.metalAmount += itemIncrease;
                break;
            case "Coins":
                data.coinsAmount += itemIncrease;
                break;
            case "Jewels":
                data.jewelsAmount += itemIncrease;
                break;
            case "Pearls":
                data.pearlsAmount += itemIncrease;
                break;

            default:
                break;
        }

        json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.dataPath + "SaveData.json", json);
    }
}
