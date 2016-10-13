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
        foreach (LocalController local in LocalControllers) {
            local.GenerateLocations();
        }
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
