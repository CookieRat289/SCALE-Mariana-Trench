using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class SubmarineScript : MonoBehaviour
{
    [SerializeField] LogicScript logicScript;
    [SerializeField] SaveDataScript saveDataScript;
    [SerializeField] GameObject upgradeUI;
    [SerializeField] TextMeshProUGUI upgradeLevelText;
    [SerializeField] TextMeshProUGUI upgradeInfoText;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator leftLegAnim;
    [SerializeField] Animator rightLegAnim;
    [SerializeField] Sprite fullHealthSprite;
    [SerializeField] Sprite halfHealthSprite;
    [SerializeField] Sprite lowHealthSprite;

    public AudioSource subAudioSource;
    public AudioClip upgradeDing;

    public float upgradeLevel;
    public float subUpgradeCap;
    public bool shouldDetermine;
    public bool allItemsZero;

    public Dictionary<string, int> upgradeResources = new Dictionary<string, int>() {
        {"Seaweed", 0},
        {"Rocks", 0},
        {"Shells", 0},
        {"Algae", 0},
        {"Metal", 0},
    };
    public string[] resourceChoices = new string[]  {
        "Seaweed",
        "Rocks",
        "Shells",
        "Algae",
        "Metal"
    };

    void Awake() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        subAudioSource = logicScript.subAudioSource;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        leftLegAnim = GameObject.Find("SubmarineLeg1").GetComponent<Animator>();
        rightLegAnim = GameObject.Find("SubmarineLeg2").GetComponent<Animator>();
        leftLegAnim.SetBool("legAnimPlaying", logicScript.legStretched);
        rightLegAnim.SetBool("legAnimPlaying", logicScript.legStretched);

        if(logicScript.submarineHealth < 20) {
            spriteRenderer.sprite = lowHealthSprite;
        } else if(logicScript.submarineHealth < 60) {
            spriteRenderer.sprite = halfHealthSprite;
        } else if(logicScript.submarineHealth >= 60) {
            spriteRenderer.sprite = fullHealthSprite;
        }

        leftLegAnim.SetFloat("Health", logicScript.submarineHealth);
        rightLegAnim.SetFloat("Health", logicScript.submarineHealth);
        
        if(shouldDetermine) {
            shouldDetermine = false;
            DetermineUpgrade();
        }
    }

    void OnMouseEnter() {
        if(logicScript.gameRunning) {
            foreach(KeyValuePair<string, int> item in upgradeResources) {
            }
            UpgradeTextUpdate();
            upgradeUI.SetActive(true);
        }
        
    }

    void OnMouseExit() {
        upgradeUI.SetActive(false);
    }

    void OnMouseOver() {
        if(Input.GetMouseButtonUp(0)) {
            if(logicScript.isHoldPearl) {
                subUpgradeCap += 5;
                logicScript.collectedItems["Pearls"] = (int)logicScript.collectedItems["Pearls"] - 1;

            } else if(logicScript.isHoldCoin) {
                subUpgradeCap += 2;
                logicScript.collectedItems["Coins"] = (int)logicScript.collectedItems["Coins"] - 1;

            } else if(logicScript.isHoldJewel) {
                subUpgradeCap += 3;
                logicScript.collectedItems["Jewels"] = (int)logicScript.collectedItems["Jewels"] - 1;

            } else {
                UpgradePart();
            }
        }
    }

    public void UpgradeTextUpdate() {
        upgradeInfoText.text = "";

        upgradeLevelText.text = $"LVL: {upgradeLevel}";
        
        foreach(KeyValuePair<string, int> resource in upgradeResources) {
            if((float)resource.Value > 0) {
                if(upgradeInfoText.text == null) {
                    upgradeInfoText.text = $"{resource.Key}: {resource.Value}\n";
                } else {
                    upgradeInfoText.text = $"{upgradeInfoText.text}{resource.Key}: {resource.Value}\n";
                }
            }
        }
    }

    void UpgradePart() {
        if(logicScript.gameRunning) {
            bool insufficientItems = false;
            int resourceListIndex = 0;
            string item = resourceChoices[resourceListIndex];

            if(upgradeLevel < (subUpgradeCap + logicScript.upgradeCap)) {
                for(int i = 0; i < upgradeResources.Count; i++) {



                    if((int)logicScript.collectedItems[item] >= (float)upgradeResources[item]) {
                        logicScript.collectedItems[item] = (int)logicScript.collectedItems[item] - (float)upgradeResources[item];
                        upgradeResources[item] = 0;
                    } else {
    
                        insufficientItems = true;
                    }



                    resourceListIndex++;

                    if(resourceListIndex < resourceChoices.Length) {
                        item = logicScript.itemsArray[resourceListIndex];
                    }
                }

                if(!insufficientItems) {
                    subAudioSource.PlayOneShot(upgradeDing, 1f);
                    upgradeLevel++;
                    DetermineUpgrade();

                    if(gameObject.name == "SubmarineBody") {
                        logicScript.submarineHealth += 10f;
                        logicScript.maxHealth += 10f;
                        logicScript.submarineHealth = Mathf.Clamp(logicScript.submarineHealth, 0, 100f + logicScript.maxHealth);
                    } else if(gameObject.name == "SubmarineWindow") {
                        logicScript.damageDealt += 1f;
                    } else if(gameObject.name == "SubmarineLegs") {
                        logicScript.moveSpeed += 3f;
                    }
                }

                UpgradeTextUpdate();

            }
        }
    }

    public void DetermineUpgrade() {
        float itemsNeeded = Random.Range(1, 4);

        for(int i = 0; i < itemsNeeded; i++) {
            string upgradeItem = resourceChoices[Random.Range(0, upgradeResources.Count)];

            for(int x = 0; x < upgradeResources.Count; x++) {
                string item = resourceChoices[x];


                upgradeResources[item] = 0;

                if(item == upgradeItem) {
                    int y = Random.Range(1, 5);
                    upgradeResources[item] = y;

                    break;
                }
            }
        }
    }
}