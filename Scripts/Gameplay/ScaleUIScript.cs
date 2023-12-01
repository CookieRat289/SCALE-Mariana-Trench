using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ScaleUIScript : MonoBehaviour
{
    [SerializeField] Image distanceBar;
    [SerializeField] AutoScrollScript autoScrollScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceBar.fillAmount = (autoScrollScript.metresTravelled / 1000f);
    }
}
