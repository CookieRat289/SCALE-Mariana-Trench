using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class ResourceInteractionScript : MonoBehaviour
{
    [SerializeField] AutoScrollScript autoScrollScript;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite resourceRock;
    [SerializeField] Sprite resourceSeaweed;
    [SerializeField] Sprite resourceCoral;

    [SerializeField] float holdTimer;
    [SerializeField] bool isHold;

    bool canTween;
    bool canReverseTween;

    Sprite[] resourceSprites = new Sprite[3];

    // Start is called before the first frame update
    void Start()
    {   
        Sprite[] resourceSprites = new Sprite[] {
            resourceRock,
            resourceSeaweed,
            resourceCoral
        };

        canTween = false;
        canReverseTween = true;

        spriteRenderer.sprite = resourceSprites[Random.Range(0, 3)];
        autoScrollScript = GameObject.Find("AutoScroller").GetComponent<AutoScrollScript>();
        holdTimer = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHold && holdTimer > 0) {
            holdTimer -= Time.deltaTime;

            if(canTween) {
                LeanTween.scale(this.gameObject, new Vector3(1f, 1.25f, 1f), 2);
                canTween = false;
            }
        } else {
            if(canReverseTween) {
                LeanTween.scale(this.gameObject, new Vector3(0.5f, 0.75f, 1f), 1);
                canReverseTween = false;
            }
        }

        if(holdTimer <= 0) {
            ResourceCollected();
        }
    }

    void OnMouseDown() {
        holdTimer = 2f;
        isHold = true;
        canTween = true;
        canReverseTween = false;
    }

    void OnMouseExit() {
        isHold = false;
        canTween = false;
        canReverseTween = true;

    }

    void OnMouseUp() {
        isHold = false;
        canTween = false;
        canReverseTween = true;

    }

    void ResourceCollected() {
        int resourceType = Random.Range(1, 101);
        int resourceAmount = Random.Range(1, 6);
        string itemType = "Seaweed";

        autoScrollScript.isDingNoise = true;

        /* Resource chances:
            - Seaweed = 20%
            - Rock = 20%
            - Shell = 20%
            - Algae = 20%
            - Metal = 15%
            - Coin = 2%
            - Jewel = 2%
            - Pearl = 1%
        */

        if(resourceType <= 20) {
            itemType = "Seaweed";
        } else if(resourceType <= 40) {
            itemType = "Rocks";
        } else if(resourceType <= 60) {
            itemType = "Shells";
        } else if(resourceType <= 80) {
            itemType = "Algae";
        } else if(resourceType <= 95) {
            itemType = "Metal";
        } else if(resourceType <= 97) {
            itemType = "Coins";
        } else if(resourceType <= 99) {
            itemType = "Jewels";
        } else if(resourceType <= 100) {
            itemType = "Pearls";
        }

        autoScrollScript.SetData(0, itemType, resourceAmount);

        Destroy(this.gameObject);
    }
}
