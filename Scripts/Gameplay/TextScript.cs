using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class TextScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    VarManager data;
    string json;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        json = File.ReadAllText(Application.dataPath + "SaveData.json");
        data = JsonUtility.FromJson<VarManager>(json);

        healthText.text = data.submarineHealth.ToString();
    }
}
