using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

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
    public List<GameObject> puzzleBoards_noticeboard;
    public List<GameObject> gatedDoors;
    public bool firstVisit;
    public ST_PuzzleDisplay sT_PuzzleDisplay;
    public int puzzlesInWorld;
    public int puzzlesInWorld_noticeboard;
    public int puzzleIndex;
    public GameObject currentPuzzleBoard;
    public bool returnToNoticeBoard;
    public GameObject memoryItem;
    public AudioClip doorOpening;
    public AudioClip fingerSnap;
    public TextMesh PuzzleText; // Text counting puzzles left

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
        resetComplete = true;
        playermovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();

        puzzlesInWorld = puzzleBoards.Count;
        puzzlesInWorld_noticeboard = puzzlesInWorld;
        puzzleIndex = 0;
      
        foreach (GameObject puzzleBoard in puzzleBoards)
        {
            puzzleBoard.SetActive(false);
        }
        foreach (GameObject puzzleBoard in puzzleBoards_noticeboard)
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
        PuzzleText.text = puzzlesInWorld_noticeboard.ToString();
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
        currentPuzzleBoard = puzzleBoards[puzzleIndex];

        foreach (GameObject door in gatedDoors)
        {
            door.transform.Rotate(0, 0, -90);
            AudioSource.PlayClipAtPoint(doorOpening, door.transform.position);
        }
        AudioSource.PlayClipAtPoint(fingerSnap, transform.position);
        puzzleBoards[0].SetActive(true);
        puzzleBoards_noticeboard[0].SetActive(true);
        sT_PuzzleDisplay.NewTileImage(puzzleBoards[0].GetComponent<PuzzleBoard>().PuzzleImage, puzzleBoards[0].GetComponent<PuzzleBoard>().puzzleSize);
    }
    public void NextPuzzle()
    {
        if(puzzleIndex + 1 < puzzlesInWorld)
        {
            puzzleBoards_noticeboard[puzzleIndex].GetComponent<Rigidbody>().isKinematic = false;
            puzzleBoards[puzzleIndex].SetActive(false);
            puzzleIndex++;
            puzzlesInWorld_noticeboard--;
            returnToNoticeBoard = false;
            currentPuzzleBoard = puzzleBoards[puzzleIndex];
            ST_PuzzleDisplay.Complete = false;
            sT_PuzzleDisplay.NewTileImage(puzzleBoards[puzzleIndex].GetComponent<PuzzleBoard>().PuzzleImage, puzzleBoards[puzzleIndex].GetComponent<PuzzleBoard>().puzzleSize);
            AudioSource.PlayClipAtPoint(fingerSnap, transform.position);
        }
        else
        {
            puzzlesInWorld_noticeboard--;
            puzzleBoards_noticeboard[puzzleIndex].GetComponent<Rigidbody>().isKinematic = false;
            memoryItem.SetActive(true);
        }
    }
}
