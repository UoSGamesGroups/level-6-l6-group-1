using UnityEngine;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour {

	public float timer;
	public int reset;
	public TextMesh CountTimer;
	public bool resetComplete;
    private int replyCount;

    public List<LocalController> LocalControllers;
    public List<GameObject> spawnedFurniture;

    public List<GameObject> Chairs;
    public List<GameObject> Bed;
    public List<GameObject> ChestDrawers;
    public List<GameObject> KitchenCounter;
    public List<GameObject> Rug;

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
		resetComplete = true;
	}
	
    public void LocalComplete() {
        replyCount++;
    }

    private void TriggerGeneration() 
    {
        if (spawnedFurniture.Count > 0){
            foreach (GameObject item in spawnedFurniture){
                Destroy(item);                          
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

        //spawnedFurniture.Clear();
        spawnedFurniture.AddRange(GameObject.FindGameObjectsWithTag("SpawnedPrefab"));
}

// Update is called once per frame
void Update () {

		CountTimer.text = timer.ToString("F1");

		if (timer >= 0 ) {
			timer -= Time.deltaTime;
		} 
		if (timer < 0 ){
			timer = reset;
            TriggerGeneration();
		}
    }
}
