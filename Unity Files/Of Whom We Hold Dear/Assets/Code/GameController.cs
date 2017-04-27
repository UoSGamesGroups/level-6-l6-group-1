using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{

    public float timer;
    public int reset;
    public bool resetComplete;
    private int replyCount;

    public List<LocalController> LocalControllers;
    public List<GameObject> spawnedFurniture;

    public List<GameObject> coinsGameobject;

    public List<GameObject> Chairs;
    public List<GameObject> Bed;
    public List<GameObject> ChestDrawers;
    public List<GameObject> KitchenCounter;
    public List<GameObject> Rug;
    public PlayerMovement playermovement;
    public FuseBox fusebox;
    public List<GameObject> puzzleBoards;
    public List<GameObject> gatedDoors;
    public bool firstVisit;
    public ST_PuzzleDisplay sT_PuzzleDisplay;
    public int puzzlesInWorld;
    public int puzzleIndex;
    public GameObject currentPuzzleBoard;
    public bool returnToNoticeBoard;
    public GameObject memoryItem;
    public AudioClip doorOpening;
    public AudioClip fingerSnap;

    public int ReplyCount
    {
        get
        {
            return replyCount;
        }
    }

    public void RegisterLocalController(LocalController controller)
    {
        LocalControllers.Add(controller);
    }

    // Use this for initialization
    void Start()
    {
        sT_PuzzleDisplay = GameObject.FindGameObjectWithTag("SlideTile").GetComponent<ST_PuzzleDisplay>();
        returnToNoticeBoard = true;
        firstVisit = false;
        TriggerGeneration();
        resetComplete = true;
        playermovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();

        foreach (GameObject puzzleBoard in puzzleBoards)
        {
            puzzleBoard.SetActive(false);
        }
    }

    public void LocalComplete()
    {
        replyCount++;
    }

    private void TriggerGeneration()
    {
        if (spawnedFurniture.Count > 0)
        {
            foreach (GameObject item in spawnedFurniture)
            {
                item.SetActive(false);
            }
        }
        if (Chairs.Count > 0)
        {
            foreach (LocalController local in LocalControllers)
            {
                local.PopulateChairs(Chairs);
            }
        }
        if (Bed.Count > 0)
        {
            foreach (LocalController local in LocalControllers)
            {
                local.PopulateBed(Bed);
            }
        }
        if (ChestDrawers.Count > 0)
        {
            foreach (LocalController local in LocalControllers)
            {
                local.PopulateChestDrawers(ChestDrawers);
            }
        }
        if (KitchenCounter.Count > 0)
        {
            foreach (LocalController local in LocalControllers)
            {
                local.PopulateKitchenCounter(KitchenCounter);
            }
        }
        if (Rug.Count > 0)
        {
            foreach (LocalController local in LocalControllers)
            {
                local.PopulateRug(Rug);
            }
        }
        foreach (LocalController local in LocalControllers)
        {
            local.GenerateLocations();
        }

        spawnedFurniture = new List<GameObject>();

        List<GameObject> allFurniture = new List<GameObject>();
        allFurniture.AddRange(GameObject.FindGameObjectsWithTag("SpawnedPrefab"));

        for (int i = 0; i < allFurniture.Count; i++)
        {
            if (allFurniture[i].activeSelf)
                spawnedFurniture.Add(allFurniture[i]);
            else
                Destroy(allFurniture[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0)
        {
            playermovement.CallAnimations("FallFail", 3);
            StartCoroutine(waitforanimation());

            if (fusebox.EngagedGreenLight.activeSelf)
            {
                fusebox.timer = 15;
                timer = reset;
            }
            else
            {
                playermovement.RestartLights(15);
            }
            timer = reset;

        }
    }

    IEnumerator waitforanimation()
    {
        yield return new WaitForSeconds(2.5f);
        playermovement.RespawnLocations();
        playermovement.puzzlecam3.enabled = false;
        TriggerGeneration();
        Cursor.visible = false;
        yield return new WaitForSeconds(1f);
    }
    public void FirstVisit()
    {
        puzzleIndex = 0;

        foreach (GameObject door in gatedDoors)
        {
            door.transform.Rotate(0, 0, -90);
            AudioSource.PlayClipAtPoint(doorOpening, door.transform.position);
        }
        AudioSource.PlayClipAtPoint(fingerSnap, transform.position);
        puzzleBoards[0].SetActive(true);
        sT_PuzzleDisplay.NewTileImage(puzzleBoards[0].GetComponent<PuzzleBoard>().PuzzleImage, puzzleBoards[0].GetComponent<PuzzleBoard>().puzzleSize);
        currentPuzzleBoard = puzzleBoards[puzzleIndex];
        puzzlesInWorld = puzzleBoards.Count;
    }
    public void NextPuzzle()
    {
        if(puzzleIndex + 1 < puzzlesInWorld)
        {
            puzzleIndex++;     
            returnToNoticeBoard = false;
            currentPuzzleBoard = puzzleBoards[puzzleIndex];
            ST_PuzzleDisplay.Complete = false;
            sT_PuzzleDisplay.NewTileImage(puzzleBoards[puzzleIndex].GetComponent<PuzzleBoard>().PuzzleImage, puzzleBoards[puzzleIndex].GetComponent<PuzzleBoard>().puzzleSize);
            AudioSource.PlayClipAtPoint(fingerSnap, transform.position);
        }
        else
        {
            memoryItem.SetActive(true);
        }
    }
}
