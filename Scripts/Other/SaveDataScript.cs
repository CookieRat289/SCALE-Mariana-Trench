using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataScript : MonoBehaviour
{
    private VarManager data;

    [SerializeField] LogicScript logicScript;
    [SerializeField] UIScript uiScript;
    [SerializeField] SubmarineScript bodySubmarineScript;
    [SerializeField] SubmarineScript windowSubmarineScript;
    [SerializeField] SubmarineScript legSubmarineScript;

    bool isNew = false;

    void Start()
    {
        data = new VarManager();
        data.bodyAmounts = new int[5] {
            data.bodySeaweedAmount,
            data.bodyRocksAmount,
            data.bodyShellsAmount,
            data.bodyAlgaeAmount,
            data.bodyMetalAmount
        };
        data.windowAmounts = new int[5] {
            data.windowSeaweedAmount,
            data.windowRocksAmount,
            data.windowShellsAmount,
            data.windowAlgaeAmount,
            data.windowMetalAmount
        };
        data.legAmounts = new int[5] {
            data.legSeaweedAmount,
            data.legRocksAmount,
            data.legShellsAmount,
            data.legAlgaeAmount,
            data.legMetalAmount
        };
    }

    void Update() {
        if(isNew) {
            data = new VarManager();

            data.bodyAmounts = new int[5] {
                data.bodySeaweedAmount,
                data.bodyRocksAmount,
                data.bodyShellsAmount,
                data.bodyAlgaeAmount,
                data.bodyMetalAmount
            };
            data.windowAmounts = new int[5] {
                data.windowSeaweedAmount,
                data.windowRocksAmount,
                data.windowShellsAmount,
                data.windowAlgaeAmount,
                data.windowMetalAmount
            };
            data.legAmounts = new int[5] {
                data.legSeaweedAmount,
                data.legRocksAmount,
                data.legShellsAmount,
                data.legAlgaeAmount,
                data.legMetalAmount
            };

            bodySubmarineScript.shouldDetermine = true;
            windowSubmarineScript.shouldDetermine = true;
            legSubmarineScript.shouldDetermine = true;

            uiScript.CancelInvoke("ReportItems");

            isNew = false;
        }
    }

    public void NewData() {
        isNew = true;
    }

    public void SaveData() {
        data = new VarManager();

        data.damageDealt = logicScript.damageDealt;
        data.submarineHealth = logicScript.submarineHealth;
        data.maxHealth = logicScript.maxHealth;
        data.moveSpeed = logicScript.moveSpeed;
        data.experiencePoints = logicScript.experiencePoints;
        data.upgradePoints = logicScript.upgradePoints;
        data.upgradeCap = logicScript.upgradeCap;
        data.kilometresTravelled = logicScript.kilometresTravelled;
        data.isEndless = logicScript.isEndless;
        data.seaweedAmount = (float)logicScript.collectedItems["Seaweed"];
        data.rocksAmount = (float)logicScript.collectedItems["Rocks"];
        data.shellsAmount = (float)logicScript.collectedItems["Shells"];
        data.algaeAmount = (float)logicScript.collectedItems["Algae"];
        data.metalAmount = (float)logicScript.collectedItems["Metal"];
        data.pearlsAmount = (float)logicScript.collectedItems["Pearls"];
        data.coinsAmount = (float)logicScript.collectedItems["Coins"];
        data.jewelsAmount = (float)logicScript.collectedItems["Jewels"];

        data.bodyUpgradeLevel = bodySubmarineScript.upgradeLevel;
        data.windowUpgradeLevel = windowSubmarineScript.upgradeLevel;
        data.legUpgradeLevel = legSubmarineScript.upgradeLevel;
        data.bodyUpgradeCap = bodySubmarineScript.subUpgradeCap;
        data.windowUpgradeCap = windowSubmarineScript.subUpgradeCap;
        data.legUpgradeCap = legSubmarineScript.subUpgradeCap;

        for(int i = 0; i < bodySubmarineScript.upgradeResources.Count; i++) {
            string item = bodySubmarineScript.resourceChoices[i];
            data.bodyAmounts[i] = (int)bodySubmarineScript.upgradeResources[item];
        }

        for(int i = 0; i < windowSubmarineScript.upgradeResources.Count; i++) {
            string item = windowSubmarineScript.resourceChoices[i];
            data.windowAmounts[i] = (int)windowSubmarineScript.upgradeResources[item];
        }

        for(int i = 0; i < legSubmarineScript.upgradeResources.Count; i++) {
            string item = legSubmarineScript.resourceChoices[i];
            data.legAmounts[i] = (int)legSubmarineScript.upgradeResources[item];
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.dataPath + "SaveData.json", json);
    }

    public void LoadData() {
        string json = File.ReadAllText(Application.dataPath + "SaveData.json");
        data = JsonUtility.FromJson<VarManager>(json);

        data.isLoad = true;

        if(isNew) {
            data = new VarManager();
            data.bodyAmounts = new int[5] {
                data.bodySeaweedAmount,
                data.bodyRocksAmount,
                data.bodyShellsAmount,
                data.bodyAlgaeAmount,
                data.bodyMetalAmount
            };
            data.windowAmounts = new int[5] {
                data.windowSeaweedAmount,
                data.windowRocksAmount,
                data.windowShellsAmount,
                data.windowAlgaeAmount,
                data.windowMetalAmount
            };
            data.legAmounts = new int[5] {
                data.legSeaweedAmount,
                data.legRocksAmount,
                data.legShellsAmount,
                data.legAlgaeAmount,
                data.legMetalAmount
            };
        }

        logicScript.damageDealt = data.damageDealt;
        logicScript.submarineHealth = data.submarineHealth;
        logicScript.maxHealth = data.maxHealth;
        logicScript.moveSpeed = data.moveSpeed;
        logicScript.experiencePoints = data.experiencePoints;
        logicScript.upgradePoints = data.upgradePoints;
        logicScript.upgradeCap = data.upgradeCap;
        logicScript.kilometresTravelled = data.kilometresTravelled;
        logicScript.isEndless = data.isEndless;
        logicScript.collectedItems["Seaweed"] = data.seaweedAmount;
        logicScript.collectedItems["Rocks"] = data.rocksAmount;
        logicScript.collectedItems["Shells"] = data.shellsAmount;
        logicScript.collectedItems["Algae"] = data.algaeAmount;
        logicScript.collectedItems["Metal"] = data.metalAmount;
        logicScript.collectedItems["Pearls"] = data.pearlsAmount;
        logicScript.collectedItems["Coins"] = data.coinsAmount;
        logicScript.collectedItems["Jewels"] = data.jewelsAmount;

        bodySubmarineScript.upgradeLevel = data.bodyUpgradeLevel;
        windowSubmarineScript.upgradeLevel = data.windowUpgradeLevel;
        legSubmarineScript.upgradeLevel = data.legUpgradeLevel;
        bodySubmarineScript.subUpgradeCap = data.bodyUpgradeCap;
        windowSubmarineScript.subUpgradeCap = data.windowUpgradeCap;
        legSubmarineScript.subUpgradeCap = data.legUpgradeCap;

        for(int i = 0; i < bodySubmarineScript.upgradeResources.Count; i++) {
            string item = bodySubmarineScript.resourceChoices[i];
            bodySubmarineScript.upgradeResources[item] = (int)data.bodyAmounts[i];
        }

        for(int i = 0; i < windowSubmarineScript.upgradeResources.Count; i++) {
            string item = windowSubmarineScript.resourceChoices[i];


            windowSubmarineScript.upgradeResources[item] = (int)data.windowAmounts[i];
        }

        for(int i = 0; i < legSubmarineScript.upgradeResources.Count; i++) {
            string item = legSubmarineScript.resourceChoices[i];


            legSubmarineScript.upgradeResources[item] = (int)data.legAmounts[i];
        }

        bodySubmarineScript.shouldDetermine = false;
        windowSubmarineScript.shouldDetermine = false;
        legSubmarineScript.shouldDetermine = false;
    }
}
