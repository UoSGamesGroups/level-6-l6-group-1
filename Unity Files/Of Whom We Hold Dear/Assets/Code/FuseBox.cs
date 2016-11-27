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


	void Start ()
    {
        callOnce = true;
        resetRedLight.SetActive(false);
        timer = 7;
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
            shuf.shuffleValue = Random.Range(0, 5000);
            list.Add(shuf);
        }

        list = list.OrderBy(o => o.shuffleValue).ToList();

        for (int i = 0; i < list.Count; i++)
        {
            Swap(ref lightArray[i], ref lightArray[list[i].lightIndex]);
        }

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

