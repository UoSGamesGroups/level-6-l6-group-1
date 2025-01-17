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
    public GameObject[] lightSources;
    public GameObject resetRedLight;
    public GameObject EngagedGreenLight;
    public GameObject CoinInsertedPurpleLight;
    public GameObject FuseBoxFailRedLight;
    public GameObject noticeBoardLight;
    public GameObject timerText;
    public GameObject FuseBoxObject;
    public float timer;
    public bool callOnce;
    public bool lastArray;
    public float StartTimer;
    public TextMesh CountTimer;
    public PlayerMovement playermovement;
    public bool puzzleCompleted;
    public bool WarnNoise = true;

    public AudioClip SoundLightOn;
    public AudioClip SoundLightOff;
    public AudioClip GeneratorOn;
    public AudioClip GeneratorOff;
    public AudioClip LowPowerSound;
    public Color normalAmbientColour;
    public Color powerOutAmbientColour;


    private AudioSource Source;

    void Start ()
    {
        ColorUtility.TryParseHtmlString("#353535", out normalAmbientColour);
        ColorUtility.TryParseHtmlString("#000000", out powerOutAmbientColour);
        RenderSettings.ambientLight = normalAmbientColour;

        lightSources = GameObject.FindGameObjectsWithTag("Light");
        lightArray = GameObject.FindGameObjectsWithTag("MainLight");
        playermovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        callOnce = true;
        timer += StartTimer;
        EngagedGreenLight.SetActive(true);
        FuseBoxFailRedLight.SetActive(false);
        resetRedLight.SetActive(false);
        Source = GetComponent<AudioSource>();
    }
	void Update ()
    {
        CountTimer.text = timer.ToString("F1");

        if (timer <= 0 && callOnce)
        {
            EngagedGreenLight.SetActive(false);
            LightOff();
            callOnce = false;
        }
        if (timer <= 15 && WarnNoise)
        {
            StartCoroutine(LowPowerBeep());
            WarnNoise = false;
        }
        if(lastArray) 
        {
            FuseBoxFailRedLight.SetActive(true);
            resetRedLight.SetActive(true);
        }
       
        if (timer > 0 && playermovement.enumRespawnLocations != PlayerMovement.respawnLocations.prologue_epilogue)
        {
            FuseBoxFailRedLight.SetActive(false);
            resetRedLight.SetActive(false);

            if (timer >= 0 && playermovement.enumRespawnLocations == PlayerMovement.respawnLocations.memory2 && !puzzleCompleted)
            {
                timer -= Time.deltaTime;
                EngagedGreenLight.SetActive(true);
                FuseBoxFailRedLight.SetActive(false);
                resetRedLight.SetActive(false);
            }
            else if(timer >= 0 && playermovement.enumRespawnLocations == PlayerMovement.respawnLocations.memory1 || playermovement.enumRespawnLocations == PlayerMovement.respawnLocations.memory3 || playermovement.enumRespawnLocations == PlayerMovement.respawnLocations.memory5 || playermovement.enumRespawnLocations == PlayerMovement.respawnLocations.memory5)
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
        StartCoroutine(Wait());
        AudioSource.PlayClipAtPoint(GeneratorOff, FuseBoxObject.transform.position);
    }

    public void LightsOn(int timerAddition)
    {
        timer = timerAddition;
        callOnce = true;
        CoinInsertedPurpleLight.SetActive(false);
        StartCoroutine(Wait());
        WarnNoise = true;
        
        AudioSource.PlayClipAtPoint(GeneratorOn, FuseBoxObject.transform.position);
    }
    IEnumerator LowPowerBeep()
    {
        AudioSource.PlayClipAtPoint(LowPowerSound, FuseBoxObject.transform.position);
        yield return new WaitForSeconds(2f);
        AudioSource.PlayClipAtPoint(LowPowerSound, FuseBoxObject.transform.position);
        if(timer <= 15 && timer > 0)
        {
            StartCoroutine(LowPowerBeep());
        }
    }

    IEnumerator Wait()
    {
        foreach(GameObject lightObj in lightSources)
        {
            GameObject light = lightObj.transform.GetChild(0).gameObject;

            if (light.activeSelf == true)
            {
                light.SetActive(false);
            }
            else
            {
                light.SetActive(true);
            }
        }
        foreach (GameObject light in lightArray)
        {
            if (light.activeSelf == true)
            {
                yield return new WaitForSeconds(0.3f);
                light.SetActive(false);
                AudioSource.PlayClipAtPoint(SoundLightOff, light.transform.position);
                continue;
            }
            else
                yield return new WaitForSeconds(0.3f);
                light.SetActive(true);
                AudioSource.PlayClipAtPoint(SoundLightOn, light.transform.position);
        } 

        if(lightArray[lightArray.Length-1].activeSelf == false) 
        {
            lastArray = true;
            noticeBoardLight.SetActive(true);
            RenderSettings.ambientLight = powerOutAmbientColour;
        } else {

            lastArray = false;
            noticeBoardLight.SetActive(false);
            RenderSettings.ambientLight = normalAmbientColour;
        }

    }

}

