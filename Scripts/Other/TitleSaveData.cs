using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSaveData : MonoBehaviour
{
    private VarManager data;
    [SerializeField] RectTransform fader;
    public AudioSource titleAudioSource;
    public AudioClip sceneWoosh;
    public AudioClip buttonClick;
    string json;

    // Start is called before the first frame update
    void Start()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1, 0);
        LeanTween.alpha(fader, 0, 1f).setOnComplete(() => {
            fader.gameObject.SetActive(false);
        });

        json = File.ReadAllText(Application.dataPath + "SaveData.json");
        data = JsonUtility.FromJson<VarManager>(json);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewData() {
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

        data.isIntroCutscene = true;

        json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.dataPath + "SaveData.json", json);

        titleAudioSource.PlayOneShot(buttonClick, 1f);
        titleAudioSource.PlayOneShot(sceneWoosh, 1f);

        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() => {
            SceneManager.LoadScene("Cutscene");
        });
    }

    public void NewEndless() {
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

        data.isIntroCutscene = true;
        data.isEndless = true;

        json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.dataPath + "SaveData.json", json);

        titleAudioSource.PlayOneShot(buttonClick, 1f);
        titleAudioSource.PlayOneShot(sceneWoosh, 1f);

        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() => {
            SceneManager.LoadScene("Cutscene");
        });
    }

    public void LoadData() {
        data.isLoad = true;

        titleAudioSource.PlayOneShot(buttonClick, 1f);
        titleAudioSource.PlayOneShot(sceneWoosh, 1f);

        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() => {
            SceneManager.LoadScene("Stage1Game");
        });
    }
}
