using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScript : MonoBehaviour
{
    [SerializeField] LogicScript logicScript;
    [SerializeField] TextMeshProUGUI seaweedAmountText;
    [SerializeField] TextMeshProUGUI rockAmountText;
    [SerializeField] TextMeshProUGUI shellAmountText;
    [SerializeField] TextMeshProUGUI algaeAmountText;
    [SerializeField] TextMeshProUGUI metalAmountText;
    [SerializeField] TextMeshProUGUI kilometresTravelledText;
    [SerializeField] TextMeshProUGUI experiencePointsText;
    [SerializeField] TextMeshProUGUI healthText;

    int rItemListIndex;
    string rItem;

    Hashtable reportedItems = new Hashtable() {
        {"Seaweed", 0},
        {"Rocks", 0},
        {"Shells", 0},
        {"Algae", 0},
        {"Metal", 0},
        {"Pearls", 0},
        {"Coins", 0},
        {"Jewels", 0}
    };

    // Start is called before the first frame update
    void Start()
    {
        rItemListIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TextUpdate();
    }

    void TextUpdate() {
        kilometresTravelledText.text = $"{logicScript.kilometresTravelled}km";
        experiencePointsText.text = $"{logicScript.experiencePoints}xp";

        seaweedAmountText.text = $"{logicScript.collectedItems["Seaweed"]}x";
        rockAmountText.text = $"{logicScript.collectedItems["Rocks"]}x";
        shellAmountText.text = $"{logicScript.collectedItems["Shells"]}x";
        algaeAmountText.text = $"{logicScript.collectedItems["Algae"]}x";
        metalAmountText.text = $"{logicScript.collectedItems["Metal"]}x";
        healthText.text = $"{logicScript.submarineHealth}/{logicScript.maxHealth + 100}";
    }

    public void ReportInvoke() {
        if(logicScript.gameRunning) {
            logicScript.subAudioSource.PlayOneShot(logicScript.buttonClick, 1f);
            rItemListIndex = 0;
            InvokeRepeating("ReportItems", 0, 0.5f);
        }
    }

    void ReportItems() {
        if(rItemListIndex < logicScript.itemsArray.Length) {
            rItem = logicScript.itemsArray[rItemListIndex];
        }
        bool isAllZero = true;

        foreach(float resourceValue in logicScript.collectedItems.Values) {
            if(resourceValue != 0) {
                isAllZero = false;
                break;
            }
        }

        if(isAllZero) {
            CancelInvoke();
        } else {
            if((float)logicScript.collectedItems[rItem] > 0) {
                for(int y = 0; y < (float)logicScript.collectedItems[rItem]; y++) {
                    logicScript.collectedItems[rItem] = (float)logicScript.collectedItems[rItem] - 1;
                    reportedItems[rItem] = (int)reportedItems[rItem] + 1;
                    logicScript.experiencePoints += (5 * logicScript.kilometresTravelled);
                    
                    if(logicScript.experiencePoints >= logicScript.upgradePoints) {
                        logicScript.LevelUpgrade();
                    }
                }
            } else if((float)logicScript.collectedItems[rItem] == 0) {
                rItemListIndex++;
            }
        }
    }
}
