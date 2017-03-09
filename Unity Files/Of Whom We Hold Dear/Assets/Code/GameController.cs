using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

	public float timer;
	public int reset;
	public TextMesh CountTimer;
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
  //  public GameObject BlackPlane;

    public int ReplyCount {
        get {
            return replyCount;
        }
    }

    public void RegisterLocalController(LocalController controller) 
    {
        LocalControllers.Add(controller);
    }

    // Use this for initialization
    void Start () {
        TriggerGeneration();
        resetComplete = true;
        playermovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();
    }
	
    public void LocalComplete() {
        replyCount++;
    }

    private void TriggerGeneration() 
    {
        if (spawnedFurniture.Count > 0){
            foreach (GameObject item in spawnedFurniture){
                item.SetActive(false);                          
            }     
        }
        foreach (LocalController local in LocalControllers){
            local.PopulateChairs(Chairs);
        }
        foreach (LocalController local in LocalControllers)
        {
            local.PopulateBed(Bed);
        }
        foreach (LocalController local in LocalControllers)
        {
            local.PopulateChestDrawers(ChestDrawers);
        }
        foreach (LocalController local in LocalControllers)
        {
            local.PopulateKitchenCounter(KitchenCounter);
        }
        foreach (LocalController local in LocalControllers)
        {
            local.PopulateRug(Rug);
        }
        foreach (LocalController local in LocalControllers) {
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

        //spawnedFurniture.AddRange(GameObject.FindGameObjectsWithTag("SpawnedPrefab"));
        }

// Update is called once per frame
void Update () {

		//CountTimer.text = timer.ToString("F1");

		if (timer >= 0 && playermovement.enumRespawnLocations != PlayerMovement.respawnLocations.prologue_epilogue && playermovement.enumRespawnLocations != PlayerMovement.respawnLocations.memory1) {
			timer -= Time.deltaTime;
		} 
		if (timer < 0 ){
            playermovement.CallAnimations("FallFail",3);
            StartCoroutine(waitforanimation());

            if (fusebox.EngagedGreenLight.activeSelf) 
            {
                fusebox.timer = 15;          
                timer = reset;
            } else {
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
}
