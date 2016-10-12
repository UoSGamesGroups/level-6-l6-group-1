using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public float timer;
	public int reset;
	public TextMesh CountTimer;
	public bool resetComplete;

	// Use this for initialization
	void Start () {
		resetComplete = true;
	}
	
	// Update is called once per frame
	void Update () {

		CountTimer.text = timer.ToString("F1");

		if (timer >= 0 && resetComplete) {
			timer -= Time.deltaTime;
		} 
		if (timer <= 0 ){
			timer = reset;
			resetComplete = false;
		}
			
	}
}
