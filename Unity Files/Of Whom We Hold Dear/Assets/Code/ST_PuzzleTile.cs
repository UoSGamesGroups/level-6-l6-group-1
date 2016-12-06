using UnityEngine;
using System.Collections;

public class ST_PuzzleTile : MonoBehaviour 
{
	
	public Vector3 TargetPosition;                  // the target position for this tile
	public bool Active = true;                      // If not active, will become the blank tile
	public bool CorrectLocation = false;            // If false, means that it is not in the correct spot
	public Vector2 ArrayLocation = new Vector2();   // Stores location of tile
	public Vector2 GridLocation = new Vector2();    // Stores location of tile


	void Awake()
	{
		// Assigns new position for object
		TargetPosition = this.transform.localPosition;

		// Starts moving the object towards new position
		StartCoroutine(UpdatePosition());
	}


	public  void LaunchPositionCoroutine(Vector3 newPosition)
	{
		// Assigns new position for tile to go to
		TargetPosition = newPosition;
		StartCoroutine(UpdatePosition());
	}

	public IEnumerator UpdatePosition()
	{
		while(TargetPosition != this.transform.localPosition)
		{
			// Lerp towards position
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, TargetPosition, 10.0f * Time.deltaTime);
			yield return null;
		}

		// Checks if in the correct location
		if(ArrayLocation == GridLocation){CorrectLocation = true;}else{CorrectLocation = false;}

		// Hides tile if not active
		if(Active == false)
		{
			this.GetComponent<Renderer>().enabled = false;
			this.GetComponent<Collider>().enabled = false;
		}

		yield return null;
	}
    
    // Moves tile when an action occurs
	public void ExecuteAdditionalMove()
	{
		LaunchPositionCoroutine(this.transform.parent.GetComponent<ST_PuzzleDisplay>().GetTargetLocation(this.GetComponent<ST_PuzzleTile>()));
	}

	void OnMouseDown()
    {
		LaunchPositionCoroutine(this.transform.parent.GetComponent<ST_PuzzleDisplay>().GetTargetLocation(this.GetComponent<ST_PuzzleTile>()));
	}
}
