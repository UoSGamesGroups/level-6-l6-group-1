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

    public int[] SpawnTypes;

	void Awake()
	{
		gamecontroller = GameObject.FindGameObjectWithTag("NoticeBoard").GetComponent<GameController>();
		ChangeRoom = true;
        gamecontroller.RegisterLocalController(this);
	}

    public void GenerateLocations()
    {
        if (itemTransformLocation.Length > 0)
        {
            switch (ItemLocation)
            {
                case itemLocation.Bedroom1:

                    SpawnTypes = new int[] { 1, 2, 4 };
                    break;

                case itemLocation.Bedroom2:

                    SpawnTypes = new int[] { 1, 2, 4 };
                    break;

                case itemLocation.LivingRoom:

                    SpawnTypes = new int[] { 0, 2, 4 };
                    break;

                case itemLocation.DiningRoom:

                    SpawnTypes = new int[] { 0, 4 };
                    break;

                case itemLocation.Kitchen:

                    SpawnTypes = new int[] { 0, 3 };
                    break;

                case itemLocation.Study_Lib:

                    SpawnTypes = new int[] { 0 };
                    break;

                case itemLocation.Atruim:

                    SpawnTypes = new int[] {2, 4 };
                    break;

            }

            for (int i = 0; i < itemTransformLocation.Length; i++)
            {
                //Make sure only specific cases can be selected for each room 

                int index = Random.Range(0, SpawnTypes.Length);
                int caseChooser = SpawnTypes[index];
                print(caseChooser);

                int arrayPicker;
                Vector3 transform = new Vector3();
                GameObject instaPrefab;

                    switch (caseChooser)
                {
                    ///////////////////////////////////////////////////////////////////////////////////////
                    case 0:
                        arrayPicker = Random.Range(0, Chairs.Length);                     
                        transform = itemTransformLocation[i].transform.position;
                        instaPrefab = Chairs[arrayPicker];
                        Instantiate(Chairs[arrayPicker], transform, Quaternion.identity);
                        break;
                    ////////////////////////////////////////////////////////////////////////////////////////

                    case 1:
                        arrayPicker = Random.Range(0, Bed.Length);
                        transform = itemTransformLocation[i].transform.position;
                        instaPrefab = Bed[arrayPicker];
                        Instantiate(Bed[arrayPicker], transform, Quaternion.identity);
                        break;
                    ////////////////////////////////////////////////////////////////////////////////////////

                    case 2:
                        arrayPicker = Random.Range(0, ChestDrawers.Length);
                        transform = itemTransformLocation[i].transform.position;
                        instaPrefab = ChestDrawers[arrayPicker];
                        Instantiate(ChestDrawers[arrayPicker], transform, Quaternion.identity);
                        break;
                    ////////////////////////////////////////////////////////////////////////////////////////

                    case 3:
                        arrayPicker = Random.Range(0, KitchenCounter.Length);
                        transform = itemTransformLocation[i].transform.position;
                        instaPrefab = KitchenCounter[arrayPicker];
                        Instantiate(KitchenCounter[arrayPicker], transform, Quaternion.identity);
                        break;
                    /////////////////////////////////////////////////////////////////////////////////////////

                    case 4:
                        arrayPicker = Random.Range(0, Rug.Length);
                        transform = itemTransformLocation[i].transform.position;
                        instaPrefab = Rug[arrayPicker];
                        Instantiate(Rug[arrayPicker], transform, Quaternion.identity);
                        break;
                        /////////////////////////////////////////////////////////////////////////////////////////
                }
                ChangeRoom = false;
            }
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
