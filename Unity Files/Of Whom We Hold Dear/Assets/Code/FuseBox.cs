using UnityEngine;
using System.Collections;

public class FuseBox : MonoBehaviour {

    public GameObject[] lightArray;
    public GameObject resetRedLight;
    public GameObject EngagedGreenLight;
    public GameObject CoinInsertedPurpleLight;
    public GameObject FuseBoxFailRedLight;
    public float timer;
    public bool callOnce;


	void Start ()
    {
        callOnce = true;
        resetRedLight.SetActive(false);
        timer = 5;
    }
	void Update ()
    {
	    if (timer <= 0 && callOnce)
        {
            LightOff();
            callOnce = false;
            EngagedGreenLight.SetActive(false);
            FuseBoxFailRedLight.SetActive(true);
        }
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
            EngagedGreenLight.SetActive(true);
            FuseBoxFailRedLight.SetActive(false);
        }

	}

    public void LightOff()
    {
        //Shuffle the array of gameobjects 
            StartCoroutine(Wait(2));
    }

    IEnumerator Wait(int waitTimer)
    {
        if (resetRedLight.activeSelf)
        {
            resetRedLight.SetActive(false);
        }
        else
            resetRedLight.SetActive(true);

        foreach (GameObject light in lightArray)
        {
            if (light.activeSelf == true)
            {
                yield return new WaitForSeconds(1f);
                light.SetActive(false);
                continue;
            }
            else
                yield return new WaitForSeconds(1f);
                light.SetActive(true);
        } 

    }

}

