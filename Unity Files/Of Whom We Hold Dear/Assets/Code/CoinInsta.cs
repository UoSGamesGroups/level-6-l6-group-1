using UnityEngine;
using System.Collections;

public class CoinInsta : MonoBehaviour {

    public enum Coin { TwoPound, Pound, FiftyPence };

    public Coin coin;

    // Use this for initialization
    void Start() {

        if (coin == Coin.TwoPound) {
            gameObject.tag = "TwoPound";
        } else if (coin == Coin.Pound) {
            gameObject.tag = "Pound";
        } else {
            gameObject.tag = "FiftyPence";
        }
    }
}
