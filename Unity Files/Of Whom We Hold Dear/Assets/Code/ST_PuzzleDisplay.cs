using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ST_PuzzleDisplay : MonoBehaviour 
{
	
	public Texture PuzzleImage;                                     // What picture will be used
	public int Height = 3;                                          // Squares picture is broken into upwards
	public int Width  = 3;                                          // Squares picture is broken into across
	public Vector3 PuzzleScale = new Vector3(1.0f, 1.0f, 1.0f);     // How much does it take up on the screen
	public float SpaceBetweenTiles = 0.5f;                          // Space between the square tile pieces
	public GameObject Tile;                                         // Tile displaying the object
	public Shader PuzzleShader;                                     // Shows the texture on the object
    static public bool Complete = false;                            // Checks if the puzzle is complete 

    private GameObject[,] TileDisplayArray;                         // array of the spawned tiles
    private List<Vector3>  DisplayPositions = new List<Vector3>();
	private Vector3 Scale;                                          // position and scale values
    private Vector3 Position;
    public FuseBox fusebox;
    public PlayerMovement playermovement;

    public AudioClip CompleteSound;                                // Will play when complete

    private AudioSource Source;

	void Start () 
	{
        fusebox = GameObject.FindGameObjectWithTag("Fusebox").GetComponent<FuseBox>();
        // Creates the puzzle
        CreatePuzzleTiles();  
                     
        // Mixes the puzzle
		StartCoroutine(JugglePuzzle());

        Source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{

        this.transform.localScale = PuzzleScale;
	}

	public Vector3 GetTargetLocation(ST_PuzzleTile thisTile)
	{
		// checks position the tile can move to
		ST_PuzzleTile MoveTo = CheckIfWeCanMove((int)thisTile.GridLocation.x, (int)thisTile.GridLocation.y, thisTile);

		if(MoveTo != thisTile)
		{
			// gets new target position for the tile to be going to
			Vector3 TargetPos = MoveTo.TargetPosition;
			Vector2 GridLocation = thisTile.GridLocation;
			thisTile.GridLocation = MoveTo.GridLocation;

			// moves empty tile space to this position
			MoveTo.LaunchPositionCoroutine(thisTile.TargetPosition);
			MoveTo.GridLocation = GridLocation;

			// returns the new target position
			return TargetPos;
		}

		// else no movement
		return thisTile.TargetPosition;
	}

    // Checks to see if the puzzle tiles can move across on the grid and that there is nothing in its way
	private ST_PuzzleTile CheckMoveLeft(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		if((Xpos - 1)  >= 0)
		{
			return GetTileAtThisGridLocation(Xpos - 1, Ypos, thisTile);
		}
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveRight(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		if((Xpos + 1)  < Width)
		{
			return GetTileAtThisGridLocation(Xpos + 1, Ypos , thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveDown(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		if((Ypos - 1)  >= 0)
		{
			return GetTileAtThisGridLocation(Xpos, Ypos  - 1, thisTile);
		}
		return thisTile;
	}
	
	private ST_PuzzleTile CheckMoveUp(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		if((Ypos + 1)  < Height)
		{
			return GetTileAtThisGridLocation(Xpos, Ypos  + 1, thisTile);
		}
		
		return thisTile;
	}
	
	private ST_PuzzleTile CheckIfWeCanMove(int Xpos, int Ypos, ST_PuzzleTile thisTile)
	{
		if(CheckMoveLeft(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveLeft(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveRight(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveRight(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveDown(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveDown(Xpos, Ypos, thisTile);
		}
		
		if(CheckMoveUp(Xpos, Ypos, thisTile) != thisTile)
		{
			return CheckMoveUp(Xpos, Ypos, thisTile);
		}
		return thisTile;
	}

	private ST_PuzzleTile GetTileAtThisGridLocation(int x, int y, ST_PuzzleTile thisTile)
	{
		for(int j = Height - 1; j >= 0; j--)
		{
			for(int i = 0; i < Width; i++)
			{
				// checks tile has correct grid location
				if((TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().GridLocation.x == x)&&
				   (TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().GridLocation.y == y))
				{
					if(TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().Active == false)
					{ 
						return TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>();
					}
				}
			}
		}

		return thisTile;
	}

	private IEnumerator JugglePuzzle()
	{
		yield return new WaitForSeconds(0.1f);

		// hides one tile piece
		TileDisplayArray[0,0].GetComponent<ST_PuzzleTile>().Active = false;

		yield return new WaitForSeconds(0.1f);

		for(int k = 0; k < 20; k++)
		{
			// use random to position each puzzle section in the array delete the number once the space is filled.
			for(int j = 0; j < Height; j++)
			{
				for(int i = 0; i < Width; i++)
				{		
					// attempt to execute a move for this tile.
					TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().ExecuteAdditionalMove();

                    yield return new WaitForSeconds(0.0001f);
                }
			}
		}

		// checks if puzzle is complete constantly
		StartCoroutine(CheckForComplete());

		yield return null;
	}

	public IEnumerator CheckForComplete()
	{
		while(Complete == false)
		{
			// will continually check to see if tiles are all correct if boolean says false
			Complete = true;
			for(int j = Height - 1; j >= 0; j--)
			{
				for(int i = 0; i < Width; i++)
				{
					if(TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>().CorrectLocation == false)  
					{
						Complete = false;
					}
                }
			}

			yield return null;
		}
				
		if(Complete)
		{
			Debug.Log("Puzzle Complete!");
            if (fusebox.lightArray[fusebox.lightArray.Length - 1].activeSelf == true)
            {
                fusebox.LightOff();
            }
            fusebox.timerText.SetActive(false);
            playermovement.PuzzleCameraSwap();
            Source.PlayOneShot(CompleteSound);
            Complete = false;    
        }
		yield return null;
	}

    private Vector2 ConvertIndexToGrid(int index)
	{
		int WidthIndex = index;
		int HeightIndex = 0;

		// take the index value and return the grid array location X,Y
		for(int i = 0; i < Height; i++)
		{
			if(WidthIndex < Width)
			{
				return new Vector2(WidthIndex, HeightIndex);
			}
			else
			{
				WidthIndex -= Width;
				HeightIndex++;
			}
		}

		return new Vector2(WidthIndex, HeightIndex);
	}

	private void CreatePuzzleTiles()
	{
		// creates array
		TileDisplayArray = new GameObject[Width,Height];

		// set scale and position values for puzzle
		Scale = new Vector3(1.0f/Width, 1.0f, 1.0f/Height);
		Tile.transform.localScale = Scale;

		// counts tiles and assigns value
		int TileValue = 0;

		// spawn the tiles into an array
		for(int j = Height - 1; j >= 0; j--)
		{
			for(int i = 0; i < Width; i++)
			{
				// calculates position of tiles
				Position = new Vector3(((Scale.x * (i + 0.5f))-(Scale.x * (Width/2.0f))) * (10.0f + SpaceBetweenTiles), 
				                       0.0f, 
				                      ((Scale.z * (j + 0.5f))-(Scale.z * (Height/2.0f))) * (10.0f + SpaceBetweenTiles));

				// set this location on the display grid
				DisplayPositions.Add(Position);

				// spawn the object into play
				TileDisplayArray[i,j] = Instantiate(Tile, new Vector3(0.0f, 0.0f, 0.0f) , Quaternion.Euler(90.0f, -180.0f, 0.0f)) as GameObject;
				TileDisplayArray[i,j].gameObject.transform.parent = this.transform;

				// set and increment the display number counter
				ST_PuzzleTile thisTile = TileDisplayArray[i,j].GetComponent<ST_PuzzleTile>();
				thisTile.ArrayLocation = new Vector2(i,j);
				thisTile.GridLocation = new Vector2(i,j);
				thisTile.LaunchPositionCoroutine(Position);
				TileValue++;

				// applies material texture
				Material thisTileMaterial = new Material(PuzzleShader);
				thisTileMaterial.mainTexture = PuzzleImage;
					
				// set the offset and tile values for this material
				thisTileMaterial.mainTextureOffset = new Vector2(1.0f/Width * i, 1.0f/Height * j);
				thisTileMaterial.mainTextureScale  = new Vector2(1.0f/Width, 1.0f/Height);
					
				// assign the new material to this tile for display
				TileDisplayArray[i,j].GetComponent<Renderer>().material = thisTileMaterial;
			}
		}
	}
}
