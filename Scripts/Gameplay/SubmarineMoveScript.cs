using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SubmarineMoveScript : MonoBehaviour
{
    [SerializeField] AutoScrollScript autoScrollScript;
    [SerializeField] Animator leftLegAnimator;
    [SerializeField] Animator rightLegAnimator;
    [SerializeField] SpriteRenderer bodyRenderer;
    [SerializeField] SpriteRenderer windowRenderer;
    [SerializeField] Sprite bodyFullHealth;
    [SerializeField] Sprite bodyHalfHealth;
    [SerializeField] Sprite bodyLowHealth;
    [SerializeField] Sprite windowFullHealth;
    [SerializeField] Sprite windowHalfHealth;
    [SerializeField] Sprite windowLowHealth;
    [SerializeField] GameObject particleEmitter;
    public AudioSource subAudioSource;
    public AudioClip subMoveWoosh;
    public AudioClip flatlineSound;
    public AudioClip thumpSound;
    public AudioClip winSound;
    string json;
    VarManager data;

    new Camera camera;
    Vector2 point;

    float xPos;
    public bool ySlide;
    float xLerpPos;
    float yLerpPos;
    float xCheckPos;
    bool keyMove;
    bool isLeftSide;
    bool isTransTween;
    bool isDeathTween;
    bool canPlayMoveNoise;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        canPlayMoveNoise = true;
        isDeathTween = false;
    }

    // Update is called once per frame
    void Update()
    {
        json = File.ReadAllText(Application.dataPath + "SaveData.json");
        data = JsonUtility.FromJson<VarManager>(json);

        leftLegAnimator.SetFloat("Health", data.submarineHealth);
        rightLegAnimator.SetFloat("Health", data.submarineHealth);

        if(data.submarineHealth < 20) {
            bodyRenderer.sprite = bodyLowHealth;
            windowRenderer.sprite = windowLowHealth;
        } else if(data.submarineHealth < 60) {
            bodyRenderer.sprite = bodyHalfHealth;
            windowRenderer.sprite = windowHalfHealth;
        } else if(data.submarineHealth >= 60) {
            bodyRenderer.sprite = bodyFullHealth;
            windowRenderer.sprite = windowFullHealth;
        }

        if(Input.GetKeyDown(KeyCode.D) && keyMove == false) {
            if(transform.position.x <= -5f) {
                isLeftSide = false;
                keyMove = true;
                xLerpPos = 7f;
                xCheckPos = 6.5f;
                yLerpPos = transform.position.y;
            } else {
                
            }
        }
        if(Input.GetKeyDown(KeyCode.A) && keyMove == false) {
            if(transform.position.x >= 6.3f) {
                isLeftSide = true;
                keyMove = true;
                xLerpPos = -6f;
                xCheckPos = -6.5f;
                yLerpPos = transform.position.y;
            } else {
                
            }
        }
        if(Input.GetKey(KeyCode.W) && keyMove == false) {
            leftLegAnimator.SetBool("isMove", true);
            rightLegAnimator.SetBool("isMove", true);
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y + data.moveSpeed), data.moveSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.S) && keyMove == false) {
            leftLegAnimator.SetBool("isMove", true);
            rightLegAnimator.SetBool("isMove", true);
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y - data.moveSpeed), data.moveSpeed * Time.deltaTime);
        }

        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) {
            leftLegAnimator.SetBool("isMove", false);
            rightLegAnimator.SetBool("isMove", false);
        }

        if(keyMove) {
            particleEmitter.SetActive(true);
            if(canPlayMoveNoise) {
                subAudioSource.PlayOneShot(subMoveWoosh, 0.5f);
                canPlayMoveNoise = false;
            }
            leftLegAnimator.SetBool("isMove", true);
            rightLegAnimator.SetBool("isMove", true);
            transform.position = Vector2.Lerp(transform.position, new Vector2(xLerpPos, yLerpPos), data.moveSpeed * Time.deltaTime);
        } else {
            particleEmitter.SetActive(false);
        }

        if(data.submarineHealth <= 0) {
            

            string json = File.ReadAllText(Application.dataPath + "SaveData.json");
            VarManager data = JsonUtility.FromJson<VarManager>(json);
            json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.dataPath + "SaveData.json", json);
            
            if(!isDeathTween) {
                subAudioSource.PlayOneShot(flatlineSound, 1f);
                autoScrollScript.fader.gameObject.SetActive(true);
                LeanTween.color(autoScrollScript.fader, Color.red, 0.5f).setOnComplete(() => {
                    LeanTween.color(autoScrollScript.fader, Color.black, 0.5f).setOnComplete(() => {
                        autoScrollScript.fader.gameObject.SetActive(false);
                        SceneManager.LoadScene("TitleScene");
                    });
                });
                isDeathTween = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D target) {
        if(target.gameObject.tag == "Wall") {
            subAudioSource.PlayOneShot(thumpSound, 0.5f);
            
            leftLegAnimator.SetBool("isMove", false);
            rightLegAnimator.SetBool("isMove", false);

            if(isLeftSide) {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            } else if(!isLeftSide) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            transform.position = new Vector2(xCheckPos, transform.position.y);
            keyMove = false;

            if(!ySlide) {
                autoScrollScript.SetData(0, "", 0);
            }

            canPlayMoveNoise = true;

        } else if(target.gameObject.tag == "Rock") {
            subAudioSource.PlayOneShot(thumpSound, 1f);
            

            Destroy(target.gameObject);
            OnHit(10f);
        } else if(target.gameObject.tag == "Win") {
            OnWin();
        }
    }

    public void OnWin() {
        autoScrollScript.SetData(1, "", 0);

        if(!isTransTween) {
            subAudioSource.PlayOneShot(winSound, 1f);

            autoScrollScript.fader.gameObject.SetActive(true);
            LeanTween.alpha(autoScrollScript.fader, 1, 0);
            LeanTween.alpha(autoScrollScript.fader, 0, 0.5f).setOnComplete(() => {
                autoScrollScript.fader.gameObject.SetActive(false);
                SceneManager.LoadScene("Stage1Game");
            });
            isTransTween = false;
        }
    }

    public void OnHit(float damageDealt) {
        data.submarineHealth -= damageDealt;

        transform.position = new Vector3(6.5f, 0, 0);

        json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.dataPath + "SaveData.json", json);
    }
}
