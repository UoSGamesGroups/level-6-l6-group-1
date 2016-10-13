using UnityEngine;
using System.Collections;


public class LocalController : MonoBehaviour {

	public enum itemLocation {Bedroom1, Bedroom2, LivingRoom, DiningRoom, Kitchen, Study_Lib, Atruim};

	public Transform[] itemTransformLocation; 
	public GameObject[] Chairs;
	public GameObject[] Bed;
	public GameObject[] ChestDrawers;
	public GameObject[] KitchenCounter;
	public GameObject[] Rug;

	public float Timer;
	public bool ChangeRoom;
    public int replyCount;

	public itemLocation ItemLocation;

	public GameController gamecontroller;

	void Awake()
	{
		gamecontroller = GameObject.FindGameObjectWithTag("NoticeBoard").GetComponent<GameController>();
		ChangeRoom = true;
        gamecontroller.RegisterLocalController(this);
	}
	
    public void GenerateLocations() 
    { 
        for (int i = 0; i<itemTransformLocation.Length; i++)
        {
			int caseRandom = Random.Range(0, 5);
            print(caseRandom);
			switch(caseRandom)
            {
///////////////////////////////////////////////////////////////////////////////////////
			case 0:
				foreach(GameObject item in Chairs){


					break;
				}	
			break;
////////////////////////////////////////////////////////////////////////////////////////

			case 1:
				foreach(GameObject item in Bed){
						
						
					break;
				}
			break;
////////////////////////////////////////////////////////////////////////////////////////

			case 2:
				foreach(GameObject item in ChestDrawers){
						
						
					break;
				}
			break;
////////////////////////////////////////////////////////////////////////////////////////

			case 3:
				foreach(GameObject item in KitchenCounter){
						
						
					break;
				}
			break;
/////////////////////////////////////////////////////////////////////////////////////////

			case 4:
				foreach(GameObject item in Rug){
						
						
					break;
				}
				break;
/////////////////////////////////////////////////////////////////////////////////////////
			}
		ChangeRoom = false;
		}
    }

    // Update is called once per frame
    void Update() {


        Timer = gamecontroller.timer;
        replyCount = gamecontroller.ReplyCount;


        if (replyCount >= 7) {
            ChangeRoom = true;
        }

        //If timer reaches zero run the checks
    }
}
