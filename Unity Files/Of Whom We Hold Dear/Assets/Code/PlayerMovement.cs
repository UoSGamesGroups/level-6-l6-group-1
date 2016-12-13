using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 20.0f;
    public Transform[] spawnlocations; 

    public Camera cam1;
    public Camera cam2;
    public Camera puzzlecam3;                   // Sliding puzzle camera 
    public enum coinSelected {TwoPound, Pound, FiftyPence};
    public enum fuseBoxCurrentCoin { TwoPound, Pound, FiftyPence };
    public enum respawnLocations { memory1, memory2, memory3, memory4, memory5,prologue_epilogue };

    public Animator animation;
    public Vector3 move;
    public bool lockcontrols = true;
    public int twoPoundAmount;
    public int poundAmount;
    public int fiftyPenceAmount;

    public GameController gamecontroller;
    public coinSelected CoinSelected;
    public fuseBoxCurrentCoin FuseBoxCurrentCoin;
    public respawnLocations enumRespawnLocations;
    public FuseBox fusebox;
    public bool resetlocked;
    public bool coinInserted;
    public int FuseClickingRestart;
    public GameObject[] CoinTypes;
    public Transform ReturnCoin;
    private bool visible;
    public bool inAnimation;
    static public bool debugPuzzle = true;         // Stops constant animation playing after puzzle completion
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Physics.gravity = new Vector3(0, -35.0F, 0);
        cam1.enabled = true;
        cam2.enabled = false;

        gamecontroller = GameObject.FindGameObjectWithTag("NoticeBoard").GetComponent<GameController>();
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();
        RespawnLocations();
        CallAnimations("WakeUpProgression", 5);
    }

    public void CallAnimations(string failanimation, int animationtime)
    {
        inAnimation = true;
        lockcontrols = false;
        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;    
        GetComponent<Renderer>().enabled = false;
        animation.SetBool(failanimation, true);
        StartCoroutine(waitforanimation(animationtime, failanimation));
    }
    void ResetAnimations(string inprogressanimation)
    {
        cam1.enabled = !cam1.enabled;
        cam2.enabled = !cam2.enabled;
        animation.SetBool(inprogressanimation, false);
        GetComponent<Renderer>().enabled = true;
        lockcontrols = true;
        inAnimation = false;
    }
    IEnumerator waitforanimation(int waitTimer, string animationname)
    {
        yield return new WaitForSeconds(waitTimer);
        ResetAnimations(animationname);
    }

    public void RespawnLocations()
    {
        if (enumRespawnLocations == respawnLocations.prologue_epilogue) 
        { 
            move = spawnlocations[2].transform.position;
            transform.position = move;
        } else if (enumRespawnLocations == respawnLocations.memory1)
        {
            move = spawnlocations[3].transform.position;
            transform.position = move;          
        }
        else if (enumRespawnLocations == respawnLocations.memory2 || enumRespawnLocations == respawnLocations.memory3)
        {
            int index = UnityEngine.Random.Range(0, 2);
            move = spawnlocations[index].transform.position;
            transform.position = move;
        }
        else {
            int index = UnityEngine.Random.Range(0, spawnlocations.Length);
            move = spawnlocations[index].transform.position;
            transform.position = move;
        }
    }

    void Update()
    {
        transform.Rotate(0, 90, 0);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CoinSelected = coinSelected.TwoPound;
            Debug.Log("You have " + twoPoundAmount + " Â£2 Coins");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CoinSelected = coinSelected.Pound;
            Debug.Log("You have " + poundAmount + " Â£1 Coins");
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
        if (puzzlecam3.enabled == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (ST_PuzzleDisplay.Complete == true)
        {
            if (debugPuzzle == true)
            {
                CallAnimations("FallProggession", 10);
                puzzlecam3.enabled = false;
                debugPuzzle = false;
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
            inAnimation = true;
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

            if (Input.GetKeyDown("e") && fusebox.lastArray)
            {
                if(coinInserted) 
                {
                    if(FuseBoxCurrentCoin == fuseBoxCurrentCoin.TwoPound) 
                    {
                        Instantiate(CoinTypes[0], ReturnCoin.transform.position, Quaternion.identity);
                    }

                    if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.Pound) {
                        Instantiate(CoinTypes[1], ReturnCoin.transform.position, Quaternion.identity);
                    }

                    if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.FiftyPence) {
                        Instantiate(CoinTypes[2], ReturnCoin.transform.position, Quaternion.identity);
                    }

                }

                RestartLights(15);
            }
        }
        if(other.gameObject.tag == "Fusebox")
        {
            if (Input.GetKeyDown("e") && CoinSelected == coinSelected.TwoPound && twoPoundAmount > 0 && !fusebox.EngagedGreenLight.activeSelf && !coinInserted && fusebox.lastArray)
            {
                fusebox.CoinInsertedPurpleLight.SetActive(true);
                twoPoundAmount--;
                coinInserted = true;
                FuseBoxCurrentCoin = fuseBoxCurrentCoin.TwoPound;
            }
            if (Input.GetKeyDown("e") && CoinSelected == coinSelected.Pound && poundAmount > 0 && !fusebox.EngagedGreenLight.activeSelf && !coinInserted && fusebox.lastArray)
            {
                fusebox.CoinInsertedPurpleLight.SetActive(true);
                poundAmount--;
                coinInserted = true;
                FuseBoxCurrentCoin = fuseBoxCurrentCoin.Pound;
            }
            if (Input.GetKeyDown("e") && CoinSelected == coinSelected.FiftyPence && fiftyPenceAmount > 0 && !fusebox.EngagedGreenLight.activeSelf && !coinInserted && fusebox.lastArray)
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
        if (other.gameObject.tag == "TestSceneMovement")
        {
            if (Input.GetKeyDown("e"))
            {
                CallAnimations("FallProggession", 10);
            }
        }
        if (other.gameObject.tag == "SignificantItem")              // If player presses E when in trigger of significant item, activates or deactivates camera to puzzle
        {
            if (Input.GetKeyDown("e"))
                {
                inAnimation = !inAnimation;
                puzzlecam3.enabled = !puzzlecam3.enabled;
                lockcontrols = !lockcontrols;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void RestartLights(int timerAddidition)
    {
        fusebox.timer = timerAddidition;
        fusebox.callOnce = true;
        fusebox.CoinInsertedPurpleLight.SetActive(false);
        FuseClickingRestart = 0;
        coinInserted = false;
        fusebox.LightOff();
    }
}
