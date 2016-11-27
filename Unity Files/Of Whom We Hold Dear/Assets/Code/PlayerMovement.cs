using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 20.0f;
    public Transform[] spawnlocations; 

    public Camera cam1;
    public Camera cam2;
    public enum coinSelected {TwoPound, Pound, FiftyPence};
    public enum fuseBoxCurrentCoin { TwoPound, Pound, FiftyPence };

    public Animator animation;
    public Vector3 move;
    public bool lockcontrols = true;
    public int twoPoundAmount;
    public int poundAmount;
    public int fiftyPenceAmount;

    public GameController gamecontroller;
    public coinSelected CoinSelected;
    public fuseBoxCurrentCoin FuseBoxCurrentCoin;
    public FuseBox fusebox;
    public bool resetlocked;
    public bool coinInserted;
    public int FuseClickingRestart;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Physics.gravity = new Vector3(0, -35.0F, 0);
        cam1.enabled = true;
        cam2.enabled = false;

        gamecontroller = GameObject.FindGameObjectWithTag("NoticeBoard").GetComponent<GameController>();
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();

        Scene scene = SceneManager.GetActiveScene();

        if (gamecontroller.sceneName == "prologue_epilogue" || gamecontroller.sceneName == "ScaleExample")
        {
            move = spawnlocations[3].transform.position;
            transform.position = move;
        }

        else if (gamecontroller.sceneName == "Memory1" || gamecontroller.sceneName == "Memory2" || gamecontroller.sceneName == "Memory3")
        {
            int index = UnityEngine.Random.Range(0, 2);
            move = spawnlocations[index].transform.position;
            transform.position = move;
        }
        else 
        {
            int index = UnityEngine.Random.Range(0, spawnlocations.Length);
            move = spawnlocations[index].transform.position;
            transform.position = move;
        }

        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;
        lockcontrols = false;
        animation.SetBool("WakeUpProgression", true);
        GetComponent<Renderer>().enabled = false;

        StartCoroutine(waitforanimation(5,"WakeUpProgression"));

    }

    void ResetAnimations(string inprogressanimation)
    {
        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;
        animation.SetBool(inprogressanimation, false);
        GetComponent<Renderer>().enabled = true;
        lockcontrols = true;
    }

    void Update()
    {
        transform.Rotate(0, 90, 0);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CoinSelected = coinSelected.TwoPound;
            Debug.Log("You have " + twoPoundAmount + " £2 Coins");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CoinSelected = coinSelected.Pound;
            Debug.Log("You have " + poundAmount + " £1 Coins");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CoinSelected = coinSelected.FiftyPence;
            Debug.Log("You have " + fiftyPenceAmount + " 50p Coins");
        }

        if (lockcontrols)
        {
            float translation = Input.GetAxis("Vertical") * speed;
            float straffe = Input.GetAxis("Horizontal") * speed;
            translation *= Time.deltaTime;
            straffe *= Time.deltaTime;

            transform.Translate(straffe, 0, translation);
        }

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (FuseClickingRestart >= 20)
        {
            if(FuseBoxCurrentCoin == fuseBoxCurrentCoin.TwoPound)
            {
                RestartLights(100);
            }
            if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.Pound)
            {
                RestartLights(50);
            }
            if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.FiftyPence)
            {
                RestartLights(25);
            }
        }
    }

    void ResetMovementControls()
    {
        lockcontrols = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fall")
        {
            cam1.enabled = !cam1.enabled;
            cam2.enabled = !cam2.enabled;
            lockcontrols = false;
            animation.SetBool("Fall", true);
            GetComponent<Renderer>().enabled = false;
         }
 
        }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "TwoPound" || other.gameObject.tag == "Pound" || other.gameObject.tag == "FiftyPence")
        {
            if (Input.GetKeyDown("e"))
            {
               if (other.gameObject.tag == "TwoPound")
                {
                    twoPoundAmount++;
                    Destroy(other.gameObject);
                }
               else if(other.gameObject.tag == "Pound")
                {
                    poundAmount++;
                    Destroy(other.gameObject);
                }
                else
                {
                    fiftyPenceAmount++;
                    Destroy(other.gameObject);
                }
            }
        }
        if (other.gameObject.tag == "ResetLights")
        {
            if (Input.GetKeyDown("e") && fusebox.resetRedLight.activeSelf == true)
            {
                fusebox.timer += 15;
                fusebox.callOnce = true;
                FuseClickingRestart = 0;
                fusebox.LightOff();
            }
        }
        if(other.gameObject.tag == "Fusebox")
        {
            if (Input.GetKeyDown("e") && CoinSelected == coinSelected.TwoPound && twoPoundAmount > 0 && !fusebox.EngagedGreenLight.activeSelf && !coinInserted)
            {
                fusebox.CoinInsertedPurpleLight.SetActive(true);
                twoPoundAmount--;
                coinInserted = true;
                FuseBoxCurrentCoin = fuseBoxCurrentCoin.TwoPound;
            }
            if (Input.GetKeyDown("e") && CoinSelected == coinSelected.Pound && poundAmount > 0 && !fusebox.EngagedGreenLight.activeSelf && !coinInserted)
            {
                fusebox.CoinInsertedPurpleLight.SetActive(true);
                poundAmount--;
                coinInserted = true;
                FuseBoxCurrentCoin = fuseBoxCurrentCoin.Pound;
            }
            if (Input.GetKeyDown("e") && CoinSelected == coinSelected.FiftyPence && fiftyPenceAmount > 0 && !fusebox.EngagedGreenLight.activeSelf && !coinInserted)
            {
                fusebox.CoinInsertedPurpleLight.SetActive(true);
                fiftyPenceAmount--;
                coinInserted = true;
                FuseBoxCurrentCoin = fuseBoxCurrentCoin.FiftyPence;
            }
        }
        if (other.gameObject.tag == "Fusebox" && coinInserted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FuseClickingRestart++;
            }
        }
        

        }

    IEnumerator waitforanimation(int waitTimer, string animationname)
    {
        yield return new WaitForSeconds(waitTimer);
        ResetAnimations(animationname);
    }


    void RestartLights(int timerAddidition)
    {
        fusebox.timer += timerAddidition;
        fusebox.callOnce = true;
        fusebox.CoinInsertedPurpleLight.SetActive(false);
        FuseClickingRestart = 0;
        coinInserted = false;
        fusebox.LightOff();
    }
}
