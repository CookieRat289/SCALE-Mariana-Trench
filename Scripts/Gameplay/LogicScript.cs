using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LogicScript : MonoBehaviour
{
    [SerializeField] SaveDataScript saveDataScript;
    [SerializeField] SubmarineScript bodySubmarineScript;
    [SerializeField] SubmarineScript windowSubmarineScript;
    [SerializeField] SubmarineScript legSubmarineScript;
    [SerializeField] SpriteRenderer leftLegSpriteRenderer;
    [SerializeField] SpriteRenderer rightLegSpriteRenderer;
    [SerializeField] Sprite outLegs;
    [SerializeField] GameObject submarine;
    [SerializeField] GameObject subLeftLeg;
    [SerializeField] GameObject subRightLeg;
    [SerializeField] GameObject submarineBody;
    [SerializeField] GameObject submarineWindow;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject tutorialUI;
    [SerializeField] Image tutorialBG;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] Animator leftLegJumpAnim;
    [SerializeField] Animator rightLegJumpAnim;
    [SerializeField] RectTransform fader;
    VarManager data;
    Vector3 bodyLerpPos;
    Vector3 windowLerpPos;

    public Image healthBar;
    public AudioSource subAudioSource;
    public AudioSource soundSource;
    public AudioClip subMetalScrape1;
    public AudioClip subMetalScrape2;
    public AudioClip subMetalThump;
    public AudioClip flatlineSound;
    public AudioClip sceneWoosh;
    public AudioClip buttonClick;
    public Camera cam;

    public float submarineHealth;
    public float damageDealt;
    public float maxHealth;
    public float moveSpeed;
    public float experiencePoints;
    public float upgradePoints;
    public float upgradeCap;
    public float kilometresTravelled;
    public int tutorialTextID;
    public bool isHoldPearl;
    public bool isHoldCoin;
    public bool isHoldJewel;
    public bool newLoad;
    public bool animPlaying;
    public bool gameRunning;
    public bool legAnimPlaying;
    public bool legStretched;
    public bool canEnemy;
    public bool isEndless;
    string json;
    bool isTransTween;
    bool isDeathTween;
    bool isWinTween;
    bool canWooshSound;
    bool canScrapeSound;

    public Dictionary<string, float> collectedItems = new Dictionary<string, float>() {
        {"Seaweed", 0},
        {"Rocks", 0},
        {"Shells", 0},
        {"Algae", 0},
        {"Metal", 0},
        {"Pearls", 0},
        {"Coins", 0},
        {"Jewels", 0}
    };
    public string[] itemsArray = new string[] {
        "Seaweed",
        "Rocks",
        "Shells",
        "Algae",
        "Metal",
        "Pearls",
        "Coins",
        "Jewels"
    };
    public string[] tutorialTextList = new string[0];
    public Sprite[] legSpriteList = new Sprite[3];

    void Awake() {
        gameRunning = true;

        json = File.ReadAllText(Application.dataPath + "SaveData.json");
        data = JsonUtility.FromJson<VarManager>(json);

        saveDataScript.LoadData();


        bodySubmarineScript.allItemsZero = true;
        windowSubmarineScript.allItemsZero = true;
        legSubmarineScript.allItemsZero = true;

        foreach(int item in bodySubmarineScript.upgradeResources.Values) {
            if(item > 0) {
                bodySubmarineScript.allItemsZero = false;
                return;
            }
        }

        if(bodySubmarineScript.allItemsZero == true) {
            bodySubmarineScript.DetermineUpgrade();
        }
        foreach(int item in windowSubmarineScript.upgradeResources.Values) {
            if(item > 0) {
                windowSubmarineScript.allItemsZero = false;
                return;
            }
        }

        if(windowSubmarineScript.allItemsZero == true) {
            windowSubmarineScript.DetermineUpgrade();
        }
        foreach(int item in legSubmarineScript.upgradeResources.Values) {
            if(item > 0) {
                legSubmarineScript.allItemsZero = false;
                return;
            }
        }

        if(legSubmarineScript.allItemsZero == true) {
            legSubmarineScript.DetermineUpgrade();
        }


        if(data.kilometresTravelled >= 11f) {
            if(!isEndless) {
                data.isWinCutscene = true;
                SceneManager.LoadScene("WinScene");
            } else {
                return;
            }
        }
    }

    void Start() 
    {
        if(data.kilometresTravelled == 0) {
            gameRunning = false;
            legStretched = true;
            canEnemy = false;
            submarine.transform.position = new Vector3(submarine.transform.position.x, -0.58f, submarine.transform.position.z);
            submarineBody.transform.position = new Vector3(submarineBody.transform.position.x, submarineBody.transform.position.y - 2.9f, submarineBody.transform.position.z);
            submarineWindow.transform.position = new Vector3(submarineWindow.transform.position.x, submarineWindow.transform.position.y - 2.9f, submarineWindow.transform.position.z);

            bodyLerpPos = new Vector3(submarineBody.transform.position.x, submarineBody.transform.position.y + 2.9f, submarineBody.transform.position.z);
            windowLerpPos = new Vector3(submarineWindow.transform.position.x, submarineWindow.transform.position.y + 2.9f, submarineWindow.transform.position.z);

            Invoke("LegAnim", 2f);
        } else {
            canEnemy = true;
        }

        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1, 0);
        LeanTween.alpha(fader, 0, 0.5f).setOnComplete(() => {
            fader.gameObject.SetActive(false);
        });

        if(kilometresTravelled < 3) {
            cam.backgroundColor = new Color32(50, 77, 121, 1);
        } else if(kilometresTravelled < 5) {
            cam.backgroundColor = new Color32(50, 91, 121, 1);
        } else if(kilometresTravelled < 7) {
            cam.backgroundColor = new Color32(50, 104, 150, 1);
        } else if(kilometresTravelled >= 7) {
            cam.backgroundColor = new Color32(50, 130, 180, 1);
        }

        canEnemy = true;
        canWooshSound = true;
    }

    void Update() 
    {
        leftLegJumpAnim = subLeftLeg.GetComponent<Animator>();
        rightLegJumpAnim = subRightLeg.GetComponent<Animator>();

        json = File.ReadAllText(Application.dataPath + "SaveData.json");
        data = JsonUtility.FromJson<VarManager>(json);

        if(animPlaying) {
            submarine.transform.position = Vector2.Lerp(submarine.transform.position, new Vector2(0, 10), Time.deltaTime);
            if(canWooshSound) {
                soundSource.Play();
                canWooshSound = false;
            }
        }

        if(submarine.transform.position.y >= 9) {
            if(!isTransTween) {
                subAudioSource.PlayOneShot(sceneWoosh, 1f);

                fader.gameObject.SetActive(true);
                LeanTween.alpha(fader, 0, 0);
                LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() => {
                    fader.gameObject.SetActive(false);
                    SceneManager.LoadScene("Stage2Game");
                });
                isTransTween = true;
            }
        }

        if(kilometresTravelled >= 11) {
            if(!isEndless) {
                data.isWinCutscene = true;
                string json = JsonUtility.ToJson(data);
                File.WriteAllText(Application.dataPath + "SaveData.json", json);

                if(!isWinTween) {
                    fader.gameObject.SetActive(true);
                    LeanTween.alpha(fader, 0, 0);
                    LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() => {
                        fader.gameObject.SetActive(false);
                        SceneManager.LoadScene("Cutscene");
                    });

                    isWinTween = true;
                }
            }
        }

        if(submarineHealth <= 0) {


            saveDataScript.SaveData();

            if(!isDeathTween) {
                subAudioSource.PlayOneShot(flatlineSound, 1f);

                fader.gameObject.SetActive(true);
                LeanTween.color(fader, Color.red, 1f).setOnComplete(() => {
                    LeanTween.color(fader, Color.black, 0.5f).setOnComplete(() => {
                        fader.gameObject.SetActive(false);
                        SceneManager.LoadScene("TitleScene");
                    });
                });
                isDeathTween = true;
            }
        }

        float healthDivider = 100f + maxHealth;
        healthBar.fillAmount = submarineHealth / healthDivider;

        if(legAnimPlaying) {
            if(!canScrapeSound) {
                subAudioSource.PlayOneShot(subMetalScrape1, 0.50f);
                subAudioSource.PlayOneShot(subMetalScrape2, 0.45f);
                canScrapeSound = true;
            }

            leftLegSpriteRenderer.sprite = outLegs;
            rightLegSpriteRenderer.sprite = outLegs;
            submarineBody.transform.position = Vector3.Lerp(submarineBody.transform.position, bodyLerpPos, 2 * Time.deltaTime);
            submarineWindow.transform.position = Vector3.Lerp(submarineWindow.transform.position, windowLerpPos, 2 * Time.deltaTime);

            if(System.Math.Round(submarineBody.transform.position.y, 2) == System.Math.Round(bodyLerpPos.y, 2)) {
                subAudioSource.Stop();
                subAudioSource.PlayOneShot(subMetalThump, 1f);

                legStretched = false;
                legAnimPlaying = false;
                submarine.transform.position = new Vector3(submarine.transform.position.x, submarine.transform.position.y - 1, submarine.transform.position.z);
                tutorialTextID = -1;
                Invoke("Tutorial", 2f);
            }
        }
    }

    public void LevelUpgrade() {
        experiencePoints -= upgradePoints;
        upgradePoints = Mathf.Round(upgradePoints * 1.5f);
        upgradeCap += 1;
    }

    public void ChangeScene() {
        if(gameRunning) {
            saveDataScript.SaveData();

            subAudioSource.PlayOneShot(buttonClick, 1f);

            PlayJumpAnim();
        }
    }

    void PlayJumpAnim() {
        leftLegJumpAnim.SetBool("isMove", true);
        rightLegJumpAnim.SetBool("isMove", true);

        animPlaying = true;
    }

    public void OpenPauseMenu() {
        if(gameRunning) {
            subAudioSource.PlayOneShot(buttonClick, 1f);
            gameRunning = false;
            pauseMenu.SetActive(true);
        }
    }

    public void ClosePauseMenu() {
        gameRunning = true;
        subAudioSource.PlayOneShot(buttonClick, 1f);
        pauseMenu.SetActive(false);
    }

    public void TitleSceneChange() {
        gameRunning = false;
        subAudioSource.PlayOneShot(buttonClick, 1f);
        subAudioSource.PlayOneShot(sceneWoosh, 1f);
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() => {
            fader.gameObject.SetActive(false);
            SceneManager.LoadScene("TitleScene");
        });
    }

    void LegAnim() {
        gameRunning = false;
        legAnimPlaying = true;
    }

    public void Tutorial() {
        subAudioSource.Stop();
        subAudioSource.PlayOneShot(buttonClick, 1f);
        gameRunning = false;
        tutorialTextID++;
        tutorialTextList = new string[] {
            "Uh oh! As you were being lowered into the Mariana Trench, the rope broke and you fell down!",
            "Fortunately, your submarine has specialised legs that should help you scale back up.",
            "It's worth noting that you'll likely get damaged along the way, so be sure to upgrade the submarine.",
            "You need resources to upgrade parts, which you'll find along the journey.",
            "You can only upgrade each part a certain amount of times, though. This limit can be increased in two ways:",
            "Report items by pressing the 'report' button - to the left of the 'go' button - granting experience points.",
            "Once you have attained a particular amount of experience, the limit will increase.",
            "You could also use special resources, found in the bottom right.",
            "You'll also find these on the journey, but they're much rarer.",
            "Upgrading the body increases max health, upgrading the legs increases move speed,",
            "and upgrading the window increases the amount of damage dealt.",
            "This is important, because fish will attack the submarine every so often. Click on them to kill them.",
            "Some fish will give you extra health when you kill them.",
            "Once you are ready to begin scaling the trench, press the 'go' button at the top right.",
            "Use WASD to control the submarine's motion once you have begun. Avoid any obstacles.",
            "If you see seaweed, coral, or a large rock, click and hold on it to collect resources.",
            "As you progress up the trench, you will begin to speed up.",
            "That's all for now, good luck!"
        };

        if(isEndless) {
            tutorialTextList = new string[] {
                "Uh oh! As you were being lowered into the Mariana Trench, the rope broke and you fell down!",
                "Fortunately, your submarine has specialised legs that should help you scale back up.",
                "It's worth noting that you'll likely get damaged along the way, so be sure to upgrade the submarine.",
                "You need resources to upgrade parts, which you'll find along the journey.",
                "You can only upgrade each part a certain amount of times, though. This limit can be increased in two ways:",
                "Report items by pressing the 'report' button - to the left of the 'go' button - granting experience points.",
                "Once you have attained a particular amount of experience, the limit will increase.",
                "You could also use special resources, found in the bottom right.",
                "You'll also find these on the journey, but they're much rarer.",
                "Upgrading the body increases max health, upgrading the legs increases move speed,",
                "and upgrading the window increases the amount of damage dealt.",
                "This is important, because fish will attack the submarine every so often. Click on them to kill them.",
                "Some fish will give you extra health when you kill them.",
                "Once you are ready to begin scaling the trench, press the 'go' button at the top right.",
                "Use WASD to control the submarine's motion once you have begun. Avoid any obstacles.",
                "If you see seaweed, coral, or a large rock, click and hold on it to collect resources.",
                "As you progress up the trench, you will begin to speed up.",
                "Also, you are playing 'Endless Mode,' which means that your goal is not to reach the surface,",
                "but to instead see how high up you can go.",
                "That's all for now, good luck!"
            };
        }

        if(tutorialTextID < tutorialTextList.Length) {
            tutorialText.text = tutorialTextList[tutorialTextID];
            tutorialUI.SetActive(true);
        } else {
            tutorialUI.SetActive(false);
            gameRunning = true;
        }
    }
}
