using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneLogic : MonoBehaviour
{
    private VarManager data;
    string json;
    bool isLeap;
    bool isTween;
    int cutsceneID;

    [SerializeField] RectTransform fader;
    
    public List<GameObject> introCutsceneObjects = new List<GameObject>();
    public List<GameObject> leapObjects = new List<GameObject>();
    public List<GameObject> targetLeapObjects = new List<GameObject>();
    public GameObject wave;
    public GameObject waveSplash;
    public GameObject winNewspaper;
    public RectTransform winKilometreUI;
    public RectTransform titleScreenButton;
    public AudioSource audioSource;
    public AudioClip splashSound;
    public AudioClip thumpSound;
    public AudioClip photoSound;

    // Start is called before the first frame update
    void Start()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 1, 0);
        LeanTween.alpha(fader, 0, 0.5f).setOnComplete(() => {
            fader.gameObject.SetActive(false);
        });

        isTween = false;

        isLeap = false;
        wave.SetActive(false);
        waveSplash.SetActive(false);

        for(int i = 0; i < introCutsceneObjects.Count; i++) {
            introCutsceneObjects[i].SetActive(false);
        }

        json = File.ReadAllText(Application.dataPath + "SaveData.json");
        data = JsonUtility.FromJson<VarManager>(json);

        cutsceneID = 0;

        if(data.isIntroCutscene) {
            InvokeRepeating("PlayIntroCutscene", 2f, 2f);
        }
        if(data.isWinCutscene) {
            wave.SetActive(true);
            Invoke("PlayWinAnim", 2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isLeap) {
            for(int x = 0; x < leapObjects.Count; x++) {
                leapObjects[x].SetActive(true);
                Vector3 targetPos = targetLeapObjects[x].transform.position;
                leapObjects[x].transform.position = Vector2.Lerp(leapObjects[x].transform.position, targetPos, 3 * Time.deltaTime);
            }
        }

        if(cutsceneID >= introCutsceneObjects.Count) {
            CancelInvoke("PlayIntroCutscene");
            data.isIntroCutscene = false;

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.dataPath + "SaveData.json", json);

            Invoke("StartGame", 2f);
        }
    }

    public void PlayIntroCutscene() {
        if(cutsceneID == 0 || cutsceneID == 5) {
            audioSource.PlayOneShot(thumpSound, 1f);
        } else {
            audioSource.PlayOneShot(photoSound, 1f);
        }
        GameObject currentCutsceneObject = introCutsceneObjects[cutsceneID];
        currentCutsceneObject.SetActive(true);
        cutsceneID++;
    }

    void StartGame() {
        if(!isTween) {
            fader.gameObject.SetActive(true);
            LeanTween.alpha(fader, 0, 0);
            LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() => {
                SceneManager.LoadScene("Stage1Game");
            });

            isTween = true;
        }
    }

    public void PlayWinAnim() {
        waveSplash.SetActive(true);
        isLeap = true;
        audioSource.PlayOneShot(splashSound, 1f);
        Invoke("EndWinAnim", 3f);
    }

    public void EndWinAnim() {
        audioSource.PlayOneShot(thumpSound, 1f);
        winNewspaper.SetActive(true);
        Invoke("EndUIAnim", 2f);
    }

    public void EndUIAnim() {
        winKilometreUI.gameObject.SetActive(true);
        LeanTween.alpha(winKilometreUI, 0, 0);
        LeanTween.alpha(winKilometreUI, 1, 1f);

        titleScreenButton.gameObject.SetActive(true);
        LeanTween.alpha(titleScreenButton, 0, 0);
        LeanTween.alpha(titleScreenButton, 1, 1f);
    }
}
