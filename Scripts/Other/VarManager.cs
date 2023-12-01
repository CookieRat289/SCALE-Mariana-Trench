using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VarManager
{
    public static VarManager Instance;

    public float damageDealt = 2f;
    public float submarineHealth = 100f;
    public float maxHealth = 0f;
    public float moveSpeed = 3f;
    public float experiencePoints = 0f;
    public float upgradePoints = 50f;
    public float upgradeCap = 0f;
    public float kilometresTravelled = 0f;
    public Dictionary<string, float> collectedItems = new Dictionary<string, float>() {
        {"Seaweed", 0},
        {"Rocks", 0},
        {"Shells", 0},
        {"Algae", 0},
        {"Metal", 0},
        {"Coins", 0},
        {"Jewels", 0},
        {"Pearls", 0}
    };
    public float seaweedAmount = 0f;
    public float rocksAmount = 0f;
    public float shellsAmount = 0f;
    public float algaeAmount = 0f;
    public float metalAmount = 0f;
    public float coinsAmount = 0f;
    public float jewelsAmount = 0f;
    public float pearlsAmount = 0f;
    public float bodyUpgradeLevel = 0f;
    public float windowUpgradeLevel = 0f;
    public float legUpgradeLevel = 0f;
    public float bodyUpgradeCap = 3f;
    public float windowUpgradeCap = 3f;
    public float legUpgradeCap = 3f;
    public Hashtable bodyUpgradeResources = new Hashtable() {
        {"Seaweed", 0},
        {"Rocks", 0},
        {"Shells", 0},
        {"Algae", 0},
        {"Metal", 0}
    };
    public int bodySeaweedAmount = 0;
    public int bodyRocksAmount = 0;
    public int bodyShellsAmount = 0;
    public int bodyAlgaeAmount = 0;
    public int bodyMetalAmount = 0;

    public Hashtable windowUpgradeResources = new Hashtable() {
        {"Seaweed", 0},
        {"Rocks", 0},
        {"Shells", 0},
        {"Algae", 0},
        {"Metal", 0}
    };
    public int windowSeaweedAmount = 0;
    public int windowRocksAmount = 0;
    public int windowShellsAmount = 0;
    public int windowAlgaeAmount = 0;
    public int windowMetalAmount = 0;

    public Hashtable legUpgradeResources = new Hashtable() {
        {"Seaweed", 0},
        {"Rocks", 0},
        {"Shells", 0},
        {"Algae", 0},
        {"Metal", 0}
    };
    public int legSeaweedAmount = 0;
    public int legRocksAmount = 0;
    public int legShellsAmount = 0;
    public int legAlgaeAmount = 0;
    public int legMetalAmount = 0;

    public bool isLoad = true;
    public bool isIntroCutscene = false;
    public bool isWinCutscene = false;
    public bool isEndless = false;

    public int[] bodyAmounts = new int[5];
    public int[] windowAmounts = new int[5];
    public int[] legAmounts = new int[5];
}
