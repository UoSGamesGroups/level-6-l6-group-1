using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Transform[] spawnlocations;
    public Text nameText;               // Text for prompting player of an object
    public GameObject currentCoin;
    public Transform coinPos;
    public bool holdingCoin;
    public Camera cam1;
    public Camera cam2;
    public Camera puzzlecam3;                   // Soiding puzzle camera 
    public enum coinSelected { TwoPound, Pound, FiftyPence };
    public enum fuseBoxCurrentCoin { TwoPound, Pound, FiftyPence };
    public enum respawnLocations { memory1, memory2, memory3, memory4, memory5, prologue_epilogue };

    public Animator animation;
    public Vector3 move;
    public bool lockcontrols = true;  
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
    public bool carerTrigger;
    public GameObject memoryItem;
    public GameObject Carer;
    public bool playerCanSeeCarer;
    public Vector3 scaleSize;
    public GameObject CrankHandle;                  // Handle for fuse box

    public AudioClip HandleCrank;
    public AudioClip CoinPickup;
    public AudioClip CoinInSound;
    public AudioClip EmergencyHandle;
    public AudioClip LowPowerSound;
    public bool m_MovementSetup;
    private AudioSource Source;
    public 

    void Start()
    {
       // GetComponent<Rigidbody>().freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        nameText = GameObject.Find("PromptText").GetComponent<Text>();
        Source = GetComponent<AudioSource>();
        gamecontroller = GameObject.FindGameObjectWithTag("NoticeBoard").GetComponent<GameController>();
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();
        RespawnLocations();
    }

    public void FirstAnimation()
    {
        cam1.enabled = enabled;
        cam2.enabled = !enabled;
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
        int index;
        switch (enumRespawnLocations)
        {
            case respawnLocations.prologue_epilogue:
                move = spawnlocations[2].transform.position;
                transform.position = move;
                break;

            case respawnLocations.memory1:
                move = spawnlocations[3].transform.position;
                transform.position = move;
                break;

            case respawnLocations.memory2:
                move = spawnlocations[4].transform.position;
                transform.position = move;
                break;

            case respawnLocations.memory3:
                index = UnityEngine.Random.Range(0, 2);
                move = spawnlocations[index].transform.position;
                transform.position = move;
                break;

            case respawnLocations.memory4:
                index = UnityEngine.Random.Range(0, spawnlocations.Length);
                move = spawnlocations[index].transform.position;
                transform.position = move;
                break;

            case respawnLocations.memory5:
                index = UnityEngine.Random.Range(0, spawnlocations.Length);
                move = spawnlocations[index].transform.position;
                transform.position = move;
                break;

            default:
                break;
        }
    }

    public bool isPlayerLookingAtCarer()
    {
        if (Carer.GetComponent<Renderer>().isVisible)
        {
            playerCanSeeCarer = true;
        }
        else
        {
            playerCanSeeCarer = false;
        }
        return playerCanSeeCarer;
    }

    void Update()
    {
        if (Input.GetKeyDown("r") && holdingCoin)
        {
            coinPos.DetachChildren();
            currentCoin.transform.localScale = scaleSize;
            currentCoin.GetComponent<Rigidbody>().useGravity = true;
            currentCoin.layer = 11;
            currentCoin = null;
            holdingCoin = false;
        }
        if (holdingCoin && currentCoin != null)
        {
            currentCoin.transform.position = coinPos.transform.position;
        }

        /*if (lockcontrols)
        {
            float translation = Input.GetAxis("Vertical") * speed;
            float straffe = Input.GetAxis("Horizontal") * speed;
            translation *= Time.deltaTime;
            straffe *= Time.deltaTime;

            transform.Translate(straffe, 0, translation);
        }*/

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (FuseClickingRestart >= 10 && fusebox.lastArray)
        {
            if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.TwoPound)
            {
                RestartLights(180);
            }
            if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.Pound)
            {
                RestartLights(90);
            }
            if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.FiftyPence)
            {
                RestartLights(45);
            }
        }else if(FuseClickingRestart >= 10 && !fusebox.lastArray && fusebox.EngagedGreenLight.activeSelf)
        {
            FuseClickingRestart = 0;
            fusebox.CoinInsertedPurpleLight.SetActive(false);
            coinInserted = false;

            if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.TwoPound)
            {
                fusebox.timer += 200;
            }
            if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.Pound)
            {
                fusebox.timer += 130;
            }
            if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.FiftyPence)
            {
                fusebox.timer += 60;
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
                memoryItem.SetActive(true);
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
        if (other.gameObject.tag == "TriggerCarer")
        {
            carerTrigger = true;
        }
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
            if (other.gameObject.GetComponent<Renderer>().isVisible && !holdingCoin)
            {
            nameText.text = "Press E to pickup";
            }
            else
            {
                nameText.text = "";
            }
            
            if (Input.GetKeyDown("e") && !holdingCoin)
            {
                currentCoin = other.gameObject;
                currentCoin.layer = 12;
                scaleSize = currentCoin.transform.localScale;
                holdingCoin = true;
                currentCoin.transform.parent = coinPos.transform;
                currentCoin.GetComponent<Rigidbody>().useGravity = false;
                Source.PlayOneShot(CoinPickup);

                if (other.gameObject.tag == "TwoPound")
                {
                    CoinSelected = coinSelected.TwoPound;
                }
                if (other.gameObject.tag == "FiftyPence")
                {
                    CoinSelected = coinSelected.FiftyPence;
                }
                if (other.gameObject.tag == "Pound")
                {
                    CoinSelected = coinSelected.Pound;
                }
             
            }
        }
        if (other.gameObject.tag == "ResetLights")
        {
            if (other.gameObject.GetComponent<Renderer>().isVisible)
            {
            nameText.text = "Press E to interact";
            }
            else
            {
                nameText.text = "";
            }
            if (Input.GetKeyDown("e") && fusebox.lastArray)
            {
                
                if (coinInserted)
                {
                   
                    if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.TwoPound)
                    {
                        Instantiate(CoinTypes[0], ReturnCoin.transform.position, Quaternion.identity);
                    }

                    if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.Pound)
                    {
                        Instantiate(CoinTypes[1], ReturnCoin.transform.position, Quaternion.identity);
                    }

                    if (FuseBoxCurrentCoin == fuseBoxCurrentCoin.FiftyPence)
                    {
                        Instantiate(CoinTypes[2], ReturnCoin.transform.position, Quaternion.identity);
                    }
                }
                Source.PlayOneShot(EmergencyHandle);
                RestartLights(15);
            }
        }
        if (other.gameObject.tag == "Fusebox")
        {
            if (other.gameObject.GetComponent<Renderer>().isVisible)
            {
               nameText.text = "Please insert coin";
            }
           else
            {
               nameText.text = "";

            }
            if (holdingCoin)
            {
                nameText.text = "Press E to insert coin";
            }
            else
            {
                nameText.text = "";
            }

            if (Input.GetKeyDown("e") && CoinSelected == coinSelected.TwoPound && fusebox.timer <= 15 && !coinInserted && holdingCoin )
            {
                holdingCoin = false;
                fusebox.CoinInsertedPurpleLight.SetActive(true);
                coinInserted = true;
                FuseBoxCurrentCoin = fuseBoxCurrentCoin.TwoPound;
                Destroy(currentCoin);
                Source.PlayOneShot(CoinInSound);
            }
            if (Input.GetKeyDown("e") && CoinSelected == coinSelected.FiftyPence && fusebox.timer <= 15 && !coinInserted && holdingCoin)
            {
                holdingCoin = false;
                fusebox.CoinInsertedPurpleLight.SetActive(true);
                coinInserted = true;
                FuseBoxCurrentCoin = fuseBoxCurrentCoin.FiftyPence;
                Destroy(currentCoin);
                Source.PlayOneShot(CoinInSound);
            }
            if (Input.GetKeyDown("e") && CoinSelected == coinSelected.Pound && fusebox.timer <= 15 && !coinInserted && holdingCoin)
            {
                holdingCoin = false;
                fusebox.CoinInsertedPurpleLight.SetActive(true);
                coinInserted = true;
                FuseBoxCurrentCoin = fuseBoxCurrentCoin.Pound;
                Destroy(currentCoin);
                Source.PlayOneShot(CoinInSound);
            }
        }
        if (other.gameObject.tag == "Fusebox" && coinInserted)
        {
            if (other.gameObject.GetComponent<Renderer>().isVisible)
            {
               nameText.text = "Click to crank handle";
            }
            else
            {
                nameText.text = "";
            }

            if (Input.GetMouseButtonDown(0))
            {
                FuseClickingRestart++;
                CrankHandle.transform.Rotate(0,0,36);
                Source.PlayOneShot(HandleCrank);
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
            if (other.gameObject.GetComponent<Renderer>().isVisible)
            {
                nameText.text = "Press E to Interact";
            }
            else
            {
                nameText.text = "";
            }

            if (Input.GetKeyDown("e"))
            {
                PuzzleCameraSwap();
            }
        }
    }

    // When player leaves a trigger, the text will reset
    void OnTriggerExit ()
    {
        nameText.text = "";
    }
    public void RestartLights(int timerAddidition)
    {
        FuseClickingRestart = 0;
        coinInserted = false;
        fusebox.LightsOn(timerAddidition);
    }
    public void PuzzleCameraSwap()
    {
        nameText.enabled = !nameText.enabled;
        puzzlecam3.enabled = !puzzlecam3.enabled;
        inAnimation = !inAnimation;
        lockcontrols = !lockcontrols;
        Cursor.lockState = CursorLockMode.None;
    }
}
