using UnityEngine;
using System.Collections;

public class SpawnCoin : MonoBehaviour {

    private string[] nameArray = new string[] { "TwoPound", "Pound", "FiftyPence" };
    private string coinType;
    public enum Coin { TwoPound, Pound, FiftyPence };
    public Coin coin;
    public GameController gamecontroller;

    // Use this for initialization
    void Start() {
        gamecontroller = GameObject.FindGameObjectWithTag("NoticeBoard").GetComponent<GameController>();

        int index = UnityEngine.Random.Range(0, nameArray.Length);
        coinType = nameArray[index];

        if (coinType == "TwoPound") {
            coin = Coin.TwoPound;
            gameObject.tag = "TwoPound";
            Instantiate(gamecontroller.coinsGameobject[0], transform.position, Quaternion.identity);
        } else if (coinType == "Pound") {
            coin = Coin.Pound;
            gameObject.tag = "Pound";
            Instantiate(gamecontroller.coinsGameobject[1], transform.position, Quaternion.identity);
        } else {
            coin = Coin.FiftyPence;
            gameObject.tag = "FiftyPence";
            Instantiate(gamecontroller.coinsGameobject[2], transform.position, Quaternion.identity);
        }
    }
}
