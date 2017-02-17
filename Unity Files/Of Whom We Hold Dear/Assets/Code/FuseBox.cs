using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct LightShuffle
{
    public int shuffleValue;
    public int lightIndex;
}

public class FuseBox : MonoBehaviour {

    public GameObject[] lightArray;
    public GameObject resetRedLight;
    public GameObject EngagedGreenLight;
    public GameObject CoinInsertedPurpleLight;
    public GameObject FuseBoxFailRedLight;
    public float timer;
    public bool callOnce;
    public bool lastArray;
    public int StartTimer;
    public TextMesh CountTimer;
    public PlayerMovement playermovement;
    public bool puzzleCompleted;


    void Start ()
    {
        playermovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        callOnce = true;
        timer += StartTimer;
        EngagedGreenLight.SetActive(true);
        FuseBoxFailRedLight.SetActive(false);
        resetRedLight.SetActive(false);
    }
	void Update ()
    {
        CountTimer.text = timer.ToString("F1");

        if (timer <= 0 && callOnce)
        {
            LightOff();
            callOnce = false;
            EngagedGreenLight.SetActive(false);
        }

        if(lastArray) 
        {
            FuseBoxFailRedLight.SetActive(true);
            resetRedLight.SetActive(true);
        }
       
        if (timer > 0 && playermovement.enumRespawnLocations != PlayerMovement.respawnLocations.prologue_epilogue)
        {
            EngagedGreenLight.SetActive(true);
            FuseBoxFailRedLight.SetActive(false);
            resetRedLight.SetActive(false);

            if (timer >= 0 && playermovement.enumRespawnLocations == PlayerMovement.respawnLocations.memory2 && playermovement.carerTrigger && !puzzleCompleted)
            {
                timer -= Time.deltaTime;
                EngagedGreenLight.SetActive(true);
                FuseBoxFailRedLight.SetActive(false);
                resetRedLight.SetActive(false);
            }
            else if(timer >= 0 && playermovement.enumRespawnLocations == PlayerMovement.respawnLocations.memory3 || playermovement.enumRespawnLocations == PlayerMovement.respawnLocations.memory5 || playermovement.enumRespawnLocations == PlayerMovement.respawnLocations.memory5)
            {
                timer -= Time.deltaTime;
                EngagedGreenLight.SetActive(true);
                FuseBoxFailRedLight.SetActive(false);
                resetRedLight.SetActive(false);
            }
        }
	}

    private void Swap(ref GameObject a, ref GameObject b)
    {
        GameObject temp = a;
        a = b;
        b = temp;
    }

    public void LightOff()
    {
        List<LightShuffle> list = new List<LightShuffle>();

        //Shuffle the array of gameobjects 
        for (int i = 0; i < lightArray.Length; i++)
        {
            LightShuffle shuf = new LightShuffle();
            shuf.lightIndex = i;
            shuf.shuffleValue = Random.Range(0, 1000);
            list.Add(shuf);
        }

        list = list.OrderBy(o => o.shuffleValue).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            Swap(ref lightArray[i], ref lightArray[list[i].lightIndex]);
        }

        StartCoroutine(Wait(2));
    }

    public void LightsOn(int waitTimer)
    {
        StartCoroutine(Wait(waitTimer));
    }

    IEnumerator Wait(int waitTimer)
    {
        foreach (GameObject light in lightArray)
        {
            if (light.activeSelf == true)
            {
                yield return new WaitForSeconds(0.3f);
                light.SetActive(false);
                continue;
            }
            else
                yield return new WaitForSeconds(0.3f);
                light.SetActive(true);
        } 

        if(lightArray[lightArray.Length-1].activeSelf == false) 
        {
            lastArray = true;
        } else {

            lastArray = false;
        }

    }

}

