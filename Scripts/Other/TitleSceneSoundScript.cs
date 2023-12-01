using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleSceneSoundScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject creditObject;

    public void OnPointerEnter(PointerEventData eventData)
    {
        creditObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        creditObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
