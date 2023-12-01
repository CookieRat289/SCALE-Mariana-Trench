using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] LogicScript logicScript;
    [SerializeField] GameObject targetSubmarine;

    [SerializeField] float moveSpeed;
    [SerializeField] float damage;
    [SerializeField] float enemyHealth;
    [SerializeField] float healthAmount;

    public AudioSource fishAudioSource;
    public AudioClip fishDeath;
    public AudioClip fishCollide;

    // Start is called before the first frame update
    void Start()
    {
        fishAudioSource = GetComponent<AudioSource>();
        
        logicScript = GameObject.Find("LogicManager").GetComponent<LogicScript>();
        targetSubmarine = GameObject.Find("SubmarineBody");

        if(logicScript.kilometresTravelled > 0) {
            enemyHealth *= logicScript.kilometresTravelled;
        }

        moveSpeed = moveSpeed / 50f;

        Vector3 relativePos = targetSubmarine.transform.position - transform.position;

        float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 2);

        if(transform.position.x > targetSubmarine.transform.position.x) {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 2);
            transform.Rotate(0, 180, 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth <= 0) {
            fishAudioSource.PlayOneShot(fishDeath, 1f);
            Destroy(this.gameObject);
            logicScript.submarineHealth += healthAmount;
            logicScript.submarineHealth = Mathf.Clamp(logicScript.submarineHealth, 0, 100f + logicScript.maxHealth);
        }
    }

    void FixedUpdate() {
        if(logicScript.gameRunning) {
            transform.position = Vector3.MoveTowards(transform.position, targetSubmarine.transform.position, moveSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D target) {
        if(target.gameObject.tag == "Submarine") {
            fishAudioSource.PlayOneShot(fishCollide, 1f);
            Destroy(this.gameObject);

            logicScript.submarineHealth -= damage;
        }
    }

    void OnMouseDown() {
        enemyHealth -= logicScript.damageDealt;
    }
}
