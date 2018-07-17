using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class finds paths between cells.
public static class Pathfinder {
	/// <summary>
	/// Find valid unit move cells
	/// </summary>
	/// <returns>List of cells in move range</returns>
	/// <param name="moveRange">How far the unit can move</param>
	public static List<CellClass> FindPath( CellClass startCell, GridClass grid, int moveRange ){
		List<CellClass> openList;
		List<CellClass> closedList;
		openList = new List<CellClass>();
		closedList =  new List<CellClass>();
		
		startCell.Distance = 0;
		startCell.ParentCell = null;
		CellClass curCell = startCell;

		openList.Add( curCell );

		while ( curCell.Distance <= moveRange ){
			closedList.Add( curCell);
			openList.Remove(curCell);

			if ( curCell.adjacentCells.Count == 0 ) Debug.Log("Uh.. no adjacent cells. Whee!");
			else
			foreach ( CellClass cell in curCell.adjacentCells ){
//				System.Predicate<CellClass> listPredicate = c => c == cell;
					cell.Distance = curCell.Distance+cell.Cost;
					if ( cell.Cost != 0 && cell.Occupied == false && cell.Distance <= moveRange
					&& !openList.Contains( cell) && !closedList.Contains( cell ) ){
					cell.ParentCell = curCell;
					openList.Add( cell );
				}
			}
			if ( openList.Count == 0 ) break;
			openList.Sort( SortByDistance );
			curCell = openList[0];
		}
//		Debug.Log( closedList.Count);
		closedList.TrimExcess();
		return closedList;
	}
	/// <summary>
	/// Valid attack cells
	/// </summary>
	/// <returns>List of cells in attack range</returns>
	public static List<CellClass> FindPath( CellClass startCell, GridClass grid, int attackRange, UnitClass currentUnit ){
		List<CellClass> openList;
		List<CellClass> closedList;
		openList = new List<CellClass>();
		closedList =  new List<CellClass>();
		Debug.Log("hello?");

		startCell.Distance = 0;
		startCell.ParentCell = null;
		CellClass curCell = startCell;

		openList.Add( curCell );

		while ( curCell.Distance <= attackRange ){
			closedList.Add( curCell);
			openList.Remove(curCell);

			foreach ( CellClass cell in curCell.adjacentCells ){
				cell.Distance = curCell.Distance+cell.Cost;

				if ( cell.Cost != 0 && cell.Distance <= attackRange
					&& !openList.Contains( cell) && !closedList.Contains( cell ) ){

//					UnitClass unit = UnitManager.instance.units.Find( (UnitClass u) => u.Cell == cell);
//
//					if ( unit != null){
//						if ( unit.playerSide != currentUnit.playerSide ) {
//							cell.ParentCell = curCell;
//							openList.Add( cell );
//						}
//					}
//					else {
						cell.ParentCell = curCell;
						openList.Add( cell );
//					}
				}
			}
			if ( openList.Count == 0 ) break;
			openList.Sort( SortByDistance );
			curCell = openList[0];
		}
		Debug.Log( closedList.Count);
		closedList.TrimExcess();
		return closedList;
	}
	///<summary> Find the path between a pathfinder origin cell and a valid target cell </summary>
	public static List<CellClass> TracePath( CellClass startCell, CellClass endCell ){
		List<CellClass> pathList = new List<CellClass>();
		CellClass curCell = startCell;

		while ( curCell != endCell || curCell.ParentCell != null ){ // Null clause is superfluous infinite loop protection
			pathList.Add( curCell);
			curCell = curCell.ParentCell;
		}
		pathList.Reverse();
		return pathList;
	}

	/// <summary>Used as a predicate by List.Sort(), this method compares the distance of cells A and B </summary>
	static int SortByDistance( CellClass cellA, CellClass cellB ){
		int a = cellA.Distance;
		int b = cellB.Distance;
		return a.CompareTo(b);
	}
}