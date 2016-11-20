using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    private string[] nameArray = new string[] { "TwoPound", "Pound", "FiftyPence" };
    private string coinType;

	// Use this for initialization
	void Start ()
    {
        int index = UnityEngine.Random.Range(0, nameArray.Length);
        coinType = nameArray[index];

        if (coinType == "TwoPound")
        {
            gameObject.tag = "TwoPound";
        }
        else if (coinType == "Pound")
        {
            gameObject.tag = "Pound";
        }
        else
        {
            gameObject.tag = "FiftyPence";
        }
    }
}
