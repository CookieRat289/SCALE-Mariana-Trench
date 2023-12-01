using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class GrabItemScript : MonoBehaviour, IDragHandler, IEndDragHandler
{
    RectTransform rt;

    [SerializeField] LogicScript logicScript;
    [SerializeField] Vector2 initialPos;
    [SerializeField] TextMeshProUGUI specialAmount;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = GetComponent<RectTransform>().anchoredPosition;

        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        specialAmount.text = logicScript.collectedItems[gameObject.name].ToString();
    }

    public void OnDrag(PointerEventData eventData) {
        if(logicScript.gameRunning) {
            if(gameObject.name == "Coins") {
                if((int)logicScript.collectedItems["Coins"] >= 1) {
                    logicScript.isHoldCoin = true;
                    transform.position = Input.mousePosition;
                } else {
        
                }

            } else if(gameObject.name == "Jewels") {
                if((int)logicScript.collectedItems["Jewels"] >= 1) {
                    logicScript.isHoldJewel = true;
                    transform.position = Input.mousePosition;
                } else {
        
                }

            } else if(gameObject.name == "Pearls") {
                if((int)logicScript.collectedItems["Pearls"] >= 1) {
                    logicScript.isHoldPearl = true;
                    transform.position = Input.mousePosition;
                } else {
        
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        RectTransform rt = GetComponent<RectTransform>();

        rt.anchoredPosition = initialPos;

        logicScript.isHoldCoin = false;
        logicScript.isHoldJewel = false;
        logicScript.isHoldPearl = false;
    }
}
