using UnityEngine;
using System.Collections;

public class SpawnCoin : MonoBehaviour {

    private string[] nameArray = new string[] { "TwoPound", "Pound", "FiftyPence" };
    private string coinType;
    public enum Coin { TwoPound, Pound, FiftyPence };

    public Coin coin;

    // Use this for initialization
    void Start() {
        int index = UnityEngine.Random.Range(0, nameArray.Length);
        coinType = nameArray[index];

        if (coinType == "TwoPound") {
            coin = Coin.TwoPound;
            gameObject.tag = "TwoPound";
        } else if (coinType == "Pound") {
            coin = Coin.Pound;
            gameObject.tag = "Pound";
        } else {
            coin = Coin.FiftyPence;
            gameObject.tag = "FiftyPence";
        }
    }

}
