using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLerpScript : MonoBehaviour
{
    [SerializeField] List<GameObject> lerpObjects;
    [SerializeField] List<GameObject> lerpTargets;

    public GameObject titleObject;
    public float height;
    public float speed;
    public float bobHeight;
    public float bobSpeed;

    bool isLerp;
    bool isBob;

    // Start is called before the first frame update
    void Start()
    {
        isLerp = true;
        bobHeight = Random.Range(0.1f, 0.4f);
        bobSpeed = Random.Range(0.5f, 2f);
        Invoke("StartBobAnim", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < lerpObjects.Count; i++) {
            if(isLerp) {
                Vector3 targetLerpPos = lerpTargets[i].transform.position;
                lerpObjects[i].transform.position = Vector3.Lerp(lerpObjects[i].transform.position, targetLerpPos, 1 * Time.deltaTime);
            } else if(isBob) {
                if(i < 6 || i > 12 && i < 16 || i > 17) {
                    if(i == 18) {
                        height = 0.15f;
                        speed = 1f;
                    } else {
                        height = bobHeight;
                        speed = bobSpeed;
                    }

                    lerpObjects[i].transform.position = new Vector3(lerpTargets[i].transform.position.x, Mathf.Sin(Time.time * speed) * height + lerpTargets[i].transform.position.y, lerpTargets[i].transform.position.z);
                }
            }
        }
    }

    void StartBobAnim() {
        isBob = true;
        isLerp = false;
    }
}
