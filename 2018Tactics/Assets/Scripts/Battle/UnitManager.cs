using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UnitManager : MonoBehaviour {
	public static UnitManager instance;
	public List<UnitClass> units = new List<UnitClass>();
	public bool unitHasMoved = false;
	public bool unitHasAttacked = false;
	public float unitMoveSpeed = 2f;

	public UnitSO testUnit;
	public GameObject defaultUnitPrefab;

	public TeamSO teamA;
	public TeamSO teamB;

	void Awake(){
		if ( instance == null ){
			instance = this;
		}
		else { Destroy( this ); }
	}
	#region Properties
	public bool HasMoved{
		get{
			return unitHasMoved;
		}
		set{
			unitHasMoved = value;
		}
	}
	public bool HasAttacked{
		get{
			return unitHasAttacked;
		}
		set{
			unitHasAttacked = value;
		}
	}
	#endregion

	#region Load functions
	public IEnumerator Load(){
//		yield return TestCreateUnits();
		if ( teamA != null ) teamA = Instantiate( teamA );
		if ( teamB != null ) teamB = Instantiate( teamB );
		yield return CreateUnits();
	}

	// TODO: 
	public UnitClass FindUnitFromCell( CellClass cell ){
		UnitClass unit = units.Find( (UnitClass u) => u.Cell == cell );
//		UnitClass unit = Controller.instance.units.Find( (UnitClass u) => u.Cell == cell );
		return unit;
	}
	public void UnitKilled( UnitClass unit ){
		unit.UnitGO.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.black;
		Destroy( UIManager.instance.InitiativeList[units.FindIndex( (UnitClass u) => u == unit )] );

		Controller.instance.EndCondition();
	}

	// Create units
	public IEnumerator CreateUnits(){
		int length;
		yield return null;
		// Side A units
		if ( teamA.units == null || teamB.units == null || teamA.units.Length == 0 || teamB.units.Length == 0 )
		{
			Debug.Log("Teams are invalid!");
			Debug.LogAssertion("Teams are invalid!");
			yield break;
		}

		if ( teamA.units.Length <= GridClass.instance.sideAStart.Length)
			length = teamA.units.Length;
		else
			length = GridClass.instance.sideAStart.Length;
		
		for( int i = 0; i < length; i++ ){
			UnitClass unit = InstantiateUnit( GridClass.instance.sideAStart[i], teamA.units[i], true );

			if ( GameStatus.playerTeam == null )
				unit.Name = "A" + i + ": " + RandomNames.RandomName();
			else
				unit.Name = "A" + i + ": " + unit.Name;
			
			SetStartFacing( unit, Facing.east);
			units.Add(unit);
		}

		if ( teamB.units.Length <= GridClass.instance.sideBStart.Length)
			length = teamB.units.Length;
		else
			length = GridClass.instance.sideBStart.Length;
		if ( teamB.units.Length == 0 ){
			Debug.LogAssertion( "Here it is");
		}
		
		// Side B units
		for( int i = 0; i < length; i++ ){
			UnitClass unit = InstantiateUnit( GridClass.instance.sideBStart[i], teamB.units[i], false );
			unit.Name = "B" + i + ": " + RandomNames.RandomName();
			SetStartFacing( unit, Facing.west);
			units.Add(unit);
		}

	}
	public UnitClass InstantiateUnit( Vector2 position, UnitSO unitSO, bool playerSide ){
		Vector3 pos = new Vector3( position.x* GridClass.instance.Offset, 0, position.y*GridClass.instance.Offset );
		Transform gO; // Unit's game object

		UnitClass unit = Instantiate( unitSO ).unit;
		unit.FullHealth();

		// If not given a prefab to instantiate, use the default one.
		if ( unit.UnitGO == null ){
			gO = (Transform)Instantiate( defaultUnitPrefab.transform, pos, Quaternion.identity, this.transform );
			unit.UnitGO = gO.gameObject;
		}
		else {
//			gO = (Transform)Instantiate( unit.UnitGO.transform, pos, Quaternion.identity, this.transform );
			Debug.Log("Unit Instantiation: Hello");
		}

		Material mat = unit.UnitGO.transform.GetChild(0).GetComponent<Renderer>().material;
		if ( playerSide == true )
		{
			mat.color = Color.blue;
			unit.PlayerSide = true;
		}
		else
		{
			mat.color = Color.red;
			unit.PlayerSide = false;
		}

		//InstantiateEquipment
		if ( unit._weapon != null && unit._weapon._prefab != null ){
			Transform[] children = unit.UnitGO.transform.GetComponentsInChildren<Transform>();
			foreach ( Transform child in children ){
				if ( child.name == "Weapon Point"){
					GameObject item = Instantiate( unit._weapon._prefab );
					item.transform.SetParent( child, false );
				}
			}
		}
		if ( unit._armour != null && unit._armour._prefab != null ){
			Transform[] children = unit.UnitGO.transform.GetComponentsInChildren<Transform>();
			foreach ( Transform child in children ){
				if ( child.name == "Armour Point"){
					GameObject item = Instantiate( unit._armour._prefab );
					item.transform.SetParent( child, false );
				}
			}
		}

		CellClass unitCell;
		unitCell = GridClass.instance.FindCellInArray( ( CellClass c ) => c.GridPosition == position );
		if ( unitCell != null ){
			unit.Cell = unitCell;
			unitCell.Occupied = true;
		}
		else Debug.Log( "Uh oh... MakeUnit malfunction");

		return unit;
	}
	#endregion

	#region Helper functions
//	public float FindTargetAngle( UnitClass unitA, UnitClass unitB ){
//		
//		Vector3 targetDir = unitA.UnitGO.transform.position - unitB.UnitGO.transform.position;
//		float angle = Vector3.Angle( targetDir, unitB.UnitGO.transform.forward);
//
//		return angle;
//	}
	public static float FindFacing( Transform curTransform, Transform desTransform){
		Vector3 targetDir = desTransform.position - curTransform.position;
		Vector3 forward = Vector3.forward;
		Vector3 refRight= Vector3.Cross(Vector3.up, curTransform.position);

		float angle = Vector3.Angle(targetDir, forward);
		float sign = Mathf.Sign(Vector3.Dot(desTransform.position, refRight));
		float facingAngle = angle*sign;

		return facingAngle;
	}
	public static string FacingDirection( UnitClass targetUnit, UnitClass currentUnit){
		//Find the facing of the target unit in regards to attacking unit; i.e. back, front, or side. This is in one of the 4 cardinal directions.
		Vector3 targetAngle = currentUnit.UnitGO.transform.position - targetUnit.UnitGO.transform.position;
		float angleFloat = Vector3.Angle( targetAngle, targetUnit.UnitGO.transform.forward);

		//		Debug.Log( ""+angleFloat);
		if ( angleFloat < 46 ) {
			//			Debug.Log("Front");
			return "Front";
		}
		else if ( angleFloat < 135 ) {
			//			Debug.Log( "Side");
			return "Side";
		}
		else {
			//			Debug.Log( "Back");
			return "Back";
		}
	}

	public void SetStartFacing( UnitClass unit, Facing facing ){
		// Sets the starting orientation of the units
		switch ( facing ){
		case Facing.north:
			break;
		case Facing.south:
			unit.UnitGO.transform.localRotation *= Quaternion.Euler( 0f, 180f, 0f);
			break;
		case Facing.east:
			unit.UnitGO.transform.localRotation *= Quaternion.Euler( 0f, 90f, 0f);
			break;
		case Facing.west:
			unit.UnitGO.transform.localRotation *= Quaternion.Euler( 0f, 270f, 0f);
			break;
		default:
			goto case Facing.north;
		}
		unit.UnitFacing = facing;
	}
	public List<UnitClass> EnemyUnits(){
		List<UnitClass> enemies = new List<UnitClass>();
//		foreach ( UnitClass unit in Controller.instance.units){
		foreach ( UnitClass unit in units){
			if ( unit.PlayerSide == false && unit.CurrentHealth > 0 ) enemies.Add(unit);
		}
		return enemies;
	}
	public List<UnitClass> PlayerUnits(){
		List<UnitClass> playerUnits = new List<UnitClass>();
		foreach ( UnitClass unit in units){
			if ( unit.PlayerSide == true && unit.CurrentHealth > 0 ) playerUnits.Add(unit);
		}
		return playerUnits;
	}
	public bool RollAttack( UnitClass attacker, UnitClass target ){

		// attacker's attack value versus target's defense value
		// random number generator
		// if attacker.attack + random number > target.defense + static value

		int baseDefense = 8;
		int roll = Random.Range( 1, 20);
		bool hit = false;

		if ( attacker.Attack+roll >= target.Defence+baseDefense){
			// attack success
			hit = true;
		}
		else {
			// attack fail
			hit = false;
		}

		Debug.Log("Rolling attack!");
		return hit;
	}
	#endregion
	#region AI
	public void AIAction(){
		StartCoroutine( AIMove() );
	}
	public IEnumerator AIMove(){
		List<UnitClass> enemies;
		UnitClass aiUnit = Controller.instance.currentUnit;

		//get info
		if ( aiUnit.PlayerSide ){
			enemies = EnemyUnits();
		}
		else {
			enemies = PlayerUnits();
		}

		// Follow nearest enemy
		List<CellClass> moves = Pathfinder.FindPath( aiUnit.Cell, GridClass.instance, aiUnit.Move );
//		List<CellClass> allMoves = Pathfinder.FindPath( aiUnit.Cell, GridClass.instance, int.MaxValue );
		List<CellClass> adjacents = new List<CellClass>();

		if ( moves.Count != 0 ){
			foreach( UnitClass u in enemies ){ // Find each cell adjacent to enemy within move range 
				foreach( CellClass c in u.Cell.adjacentCells ){
					if ( moves.Contains( c ) ){
						adjacents.Add( c );
					}
				}
			}

			if ( adjacents.Count != 0 ){
				adjacents.Sort( (CellClass a, CellClass b )=> a.Distance.CompareTo(b.Distance) );
				if ( adjacents[0]!=null )
					yield return StartCoroutine( Controller.instance.MoveAction( adjacents[0] ) );
				Controller.instance.NextTurn();
			}
			else{
				enemies.Sort( ( UnitClass a, UnitClass b) => a.Cell.Distance.CompareTo( b.Cell.Distance ) );
//				List< CellClass> fullPath = Pathfinder.TracePath( enemies[0].Cell, aiUnit.Cell );
				Pathfinder.TracePath( enemies[0].Cell, aiUnit.Cell );

			}
		}
		yield return null;
	}
	int SortByHealth( UnitClass a, UnitClass b ){
		return a.Health.CompareTo( b.Health );
	}
	#endregion
}
public enum WinCondition {
	DefeatEnemies
}
public enum LoseCondition {
	Defeat
}