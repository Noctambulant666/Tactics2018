using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Tactics/New Map")]
[System.Serializable]
public class MapClass : ScriptableObject {
	public bool testGrid = true;
	public CellClass[,] cells;
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
//		new Vector2( 0, 5 ),
//		new Vector2( 0, 6 ),
//		new Vector2( 0, 7 ),
//		new Vector2( 0, 8 ),
//		new Vector2( 0, 9 ),
//		new Vector2( 1, 5 ),
//		new Vector2( 1, 6 ),
//		new Vector2( 1, 7 ),
//		new Vector2( 1, 8 ),
//		new Vector2( 1, 9 ),
//		new Vector2( 2, 5 ),
//		new Vector2( 2, 6 ),
//		new Vector2( 2, 7 ),
//		new Vector2( 2, 8 ),
//		new Vector2( 2, 9 ),
//		new Vector2( 2, 10 ),
		new Vector2( 2, 8 ),
		new Vector2( 2, 9 ),
		new Vector2( 2, 10 ),
	};
		
	public void CreateTestGrid(){
		Debug.Log( "Grid: fetching grid - array");
		int sizeX = 15;
		int sizeY = 15;

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
}