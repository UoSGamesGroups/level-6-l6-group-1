using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenMug : MonoBehaviour {

    public PlayerMovement playerMovement;
    public float timeTillNextMemory;
    public bool callOnce;
    public AudioClip SmashSound;
    public bool prop;

    // Use this for initialization
    void Start () {
        callOnce = false;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        timeTillNextMemory = 5;
        AudioSource.PlayClipAtPoint(SmashSound, transform.position);
    }
	
	// Update is called once per frame
	void Update () {

        if(timeTillNextMemory <=0 && !callOnce)
        {
            playerMovement.CallAnimations("FallProggession", 6);
            callOnce = true;
        }
        else
        {
            if (!prop)
            {
                timeTillNextMemory -= Time.deltaTime;
            }
        }
    }
}
