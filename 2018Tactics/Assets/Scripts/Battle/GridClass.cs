using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 
public class GridClass : MonoBehaviour {
	public static GridClass instance;
	public Transform CellPrefab;
//	public List<CellClass> GridList = new List<CellClass>();

	public CellClass[,] cells;

	public List<CellClass> placingList = new List<CellClass>();
	[SerializeField]int sizeX;
	[SerializeField]int sizeY;
	[SerializeField]float offset = 1.0f;

	public float Offset {
		get { return offset; }
	}

	// Unit start positions for grid. This is temp code
	// TODO: Combine this and the GetGrid method into a grid scriptable object
	public Vector2[] sideAStart = new Vector2[]{
		new Vector2( 0, 6 ),
		new Vector2( 0, 7 ),
		new Vector2( 0, 8 ),
		new Vector2( 1, 6 ),
		new Vector2( 1, 7 ),
		new Vector2( 1, 8 ),
	};
	public Vector2[] sideBStart = new Vector2[]{
		new Vector2( 14, 6 ),
		new Vector2( 14, 7 ),
		new Vector2( 14, 8 ),
		new Vector2( 13, 6 ),
		new Vector2( 13, 7 ),
		new Vector2( 13, 8 ),
	};
	public Vector2[] placementCells = new Vector2[]{
		new Vector2( 0, 6 ),
		new Vector2( 0, 7 ),
		new Vector2( 0, 8 ),
		new Vector2( 0, 9 ),
		new Vector2( 1, 6 ),
		new Vector2( 1, 7 ),
		new Vector2( 1, 8 ),
		new Vector2( 1, 9 ),
		new Vector2( 2, 6 ),
		new Vector2( 2, 7 ),
		new Vector2( 2, 8 ),
		new Vector2( 2, 9 ),
	};

	#region Unity functions
	void Awake() {
		// Singleton pattern
		if ( instance == null ){
			instance = this;
		}
		else { Destroy( this.gameObject );}
	}
	void Start(){
	}
	#endregion

	#region Load
	// Retrieves grid data
	void Init(){
	}
	public IEnumerator LoadGrid(){
		CreateTestGrid();
		InstantiateGrid();
		SetAdjacentCells();
		SetPlacementCells();
		yield return null;
	}
	public IEnumerator LoadGrid( MapClass map ){
		map.CreateTestGrid();
		cells = map.cells;
		placementCells = map.placementCells;
		sideAStart = map.sideAStart;
		sideBStart = map.sideBStart;

		InstantiateGrid();
		SetAdjacentCells();
		SetPlacementCells();
		yield return null;
	}
	// Create an array to use for the battle grid
	public void CreateTestGrid(){
		Debug.Log( "Grid: fetching grid - array");
		sizeX = 15;
		sizeY = 15;

		cells = new CellClass[sizeX,sizeY];

		for ( int i = 0; i < sizeX; i++){
			for ( int j = 0; j < sizeY; j++ ){
				CellClass c = new CellClass();
				c.GridPosition = new Vector2( i, j);
				c.Name = ""+i+","+j;
				cells[i,j] = c;
			}
		}
	}
	// Instantiates the grid
	void InstantiateGrid(){
		Debug.Log( "Grid: building - array");
		foreach( CellClass c in cells ){
			GameObject cellGo;
			Vector3 pos = new Vector3( c.GridPosition.x*offset, 0, c.GridPosition.y*offset);
			cellGo = Instantiate( CellPrefab.gameObject, pos, Quaternion.identity, this.transform);
			cellGo.name = "(" + c.Name + ")";
			c.CellGO = cellGo;
		}
	}
	// Finds and sets each cell's collection of neighbours. Does it need to be done at startup? Will it make a difference if this happens upon pathfinding?
	void SetAdjacentCells(){
		Debug.Log( "Grid: Finding adjacent cells - array");
		for( int i = 0; i < cells.GetLength(0); i++ ){
			for( int j = 0; j < cells.GetLength(1); j++ ){
				CellClass currentCell = cells[i,j];
				CellClass adjacentCell;
				// x-1 y0
				if ( i - 1 > -1 ){
					adjacentCell = cells[i-1,j];
					currentCell.adjacentCells.Add( adjacentCell );
				}
				// x+1 y0
				if ( i + 1 < cells.GetLength(0)-1 ){
					adjacentCell = cells[i+1,j];
					currentCell.adjacentCells.Add( adjacentCell );
				}
				// x0 y-1
				if ( j - 1 > -1 ){
					adjacentCell = cells[i,j-1];
					currentCell.adjacentCells.Add( adjacentCell );
				}
				// x0 y+1
				if ( j + 1 < cells.GetLength(1)-1 ){
					adjacentCell = cells[i,j+1];
					currentCell.adjacentCells.Add( adjacentCell );
				}
			}
		}
	}
	// This is where players place their units at the start of the battle
	// TODO: Rework this stupid placement system. It is janky proof of concept code
	void SetPlacementCells(){
		foreach ( Vector2 position in placementCells){
			CellClass cell = FindCellInArray( (CellClass c)=> c.GridPosition == position );
			if ( cell != null ){
				placingList.Add( cell );
			}
		}
	}

	#endregion

	#region Helper functions
	// Finds the cell in array from given predicate.
	public CellClass FindCellInArray( System.Predicate<CellClass> predicate ){
		foreach ( CellClass c in cells ){
			if ( predicate(c) ){
				return c;
			}
		}
		return null;
	}
	// Returns a cell with a given transform. Used to find cells by raycast from player input
	public CellClass GetCellFromTransform( Transform cellTransform ){
		CellClass cell = FindCellInArray( (CellClass c ) => c.CellGO.transform.GetChild(0).transform == cellTransform );
		if ( cell != null ){
			return cell;
		}
		return null;
	}
	// Sets cell colour to white. Somewhat superfluous
	public void ClearCell( CellClass cell ){
		cell.CellGO.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
	}
	public void ColourCell( CellClass cell, Color colour ){
		cell.CellGO.transform.GetChild(0).GetComponent<Renderer>().material.color = colour;
	}
	#endregion
}