using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [SerializeField] GameObject fader;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip fwoomp;
    [SerializeField] AudioClip buttonClick;

    bool isTween = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartScene() {
        if(!isTween) {
            fader.gameObject.SetActive(true);
            LeanTween.alpha(fader, 0, 0);
            LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() => {
                SceneManager.LoadScene("TitleScene");
            });

            isTween = true;
        }
    }
}
