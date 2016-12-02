using UnityEngine;
using System.Collections.Generic;
using System;


public class LocalController : MonoBehaviour {

    public enum itemLocation { Bedroom1, Bedroom2, LivingRoom, DiningRoom, Kitchen, Study_Lib, Atruim };

    public Transform[] itemTransformLocation;
    public List<GameObject> Chairs;
    public List<GameObject> Bed;
    public List<GameObject> ChestDrawers;
    public List<GameObject> KitchenCounter;
    public List<GameObject> Rug;

    public float Timer;
    public bool ChangeRoom;
    public int replyCount;

    public itemLocation ItemLocation;

    public GameController gamecontroller;

    public int[] SpawnTypes;

    void Awake() {
        gamecontroller = GameObject.FindGameObjectWithTag("NoticeBoard").GetComponent<GameController>();
        ChangeRoom = true;
        gamecontroller.RegisterLocalController(this);
    }

    public void PopulateChairs(List<GameObject> chairs) {
        Chairs = chairs;
    }
    public void PopulateBed(List<GameObject> bed) {
        Bed = bed;
    }
    public void PopulateChestDrawers(List<GameObject> chestDrawers) {
        ChestDrawers = chestDrawers;
    }

    public void PopulateKitchenCounter(List<GameObject> kitchenCounter) {
        KitchenCounter = kitchenCounter;
    }

    public void PopulateRug(List<GameObject> rug) {
        Rug = rug;
    }

    public void GenerateLocations() {
        if (itemTransformLocation.Length > 0) {
            switch (ItemLocation) {
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

                    SpawnTypes = new int[] { 2, 4 };
                    break;

            }

            for (int i = 0; i < itemTransformLocation.Length; i++) {
                //Make sure only specific cases can be selected for each room 

                int index = UnityEngine.Random.Range(0, SpawnTypes.Length);
                int x = UnityEngine.Random.Range(-5, 5);
                int z = UnityEngine.Random.Range(-5, 5);
                int caseChooser = SpawnTypes[index];

                int arrayPicker;
                Vector3 transform = new Vector3();
                GameObject instaPrefab;

                switch (caseChooser) {
                    ///////////////////////////////////////////////////////////////////////////////////////
                    case 0:
                        arrayPicker = UnityEngine.Random.Range(0, Chairs.Count);
                        transform = itemTransformLocation[i].transform.position;
                        transform.x += x;
                        transform.z += z;
                        instaPrefab = Chairs[arrayPicker];
                        Instantiate(instaPrefab, transform, Quaternion.identity);
                        break;
                    ////////////////////////////////////////////////////////////////////////////////////////

                    case 1:
                        arrayPicker = UnityEngine.Random.Range(0, Bed.Count);
                        transform = itemTransformLocation[i].transform.position;
                        transform.x += x;
                        transform.z += z;
                        instaPrefab = Bed[arrayPicker];
                        Instantiate(instaPrefab, transform, Quaternion.identity);
                        break;
                    ////////////////////////////////////////////////////////////////////////////////////////

                    case 2:
                        arrayPicker = UnityEngine.Random.Range(0, ChestDrawers.Count);
                        transform = itemTransformLocation[i].transform.position;
                        transform.x += x;
                        transform.z += z;
                        instaPrefab = ChestDrawers[arrayPicker];
                        Instantiate(instaPrefab, transform, Quaternion.identity);
                        break;
                    ////////////////////////////////////////////////////////////////////////////////////////

                    case 3:
                        arrayPicker = UnityEngine.Random.Range(0, KitchenCounter.Count);
                        transform = itemTransformLocation[i].transform.position;
                        transform.x += x;
                        transform.z += z;
                        instaPrefab = KitchenCounter[arrayPicker];
                        Instantiate(instaPrefab, transform, Quaternion.identity);
                        break;
                    /////////////////////////////////////////////////////////////////////////////////////////

                    case 4:
                        arrayPicker = UnityEngine.Random.Range(0, Rug.Count);
                        transform = itemTransformLocation[i].transform.position;
                        transform.x += x;
                        transform.z += z;
                        instaPrefab = Rug[arrayPicker];
                        Instantiate(instaPrefab, transform, Quaternion.identity);
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