using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: WHERE'S THE FUCKING COMMENTS, YO? FIX THAT SHIT
// ALSO, CHANGING GRID TO USE ARRAYS INSTEAD OF RETARDED LIST
public class Controller : MonoBehaviour {
	#region Declarations
	public static Controller instance; // Singleton
	public GameObject cursor; // 3d game object for selecting units and cells
	List<CellClass> cells = new List<CellClass>(); // Used for pathfinding
	public List<UnitClass> units = new List<UnitClass>(); // All units in the battle. Why is this not under unit manager? WTF
	CellClass currentCell; // Currently selected cell
	public UnitClass currentUnit; // Unit currently taking it's turn. Shouldn't this be under unit manager or something?
	public int currentTurn = 0; // Fuck knows why there is both a current turn and a current unit, but there you go
	public float actionSpeed = 5f; // Change to constant later or something

	public MissionClassSO mission; // Victory/lose conditions, enemies, and map. Not sure why this is needed out side of loading functions
	public MapClass testMap;


	public bool waitForUI = true; // Allow user input
	public enum CursorMode {
		load, // waits for game to load
		inspectCell, // Shows user data about selected cell or unit
		showMove, // Show movement range and move units
		showAttack, // Show attack range and attack units
		placeUnit // Place units at start of game
	}
	public CursorMode cursorMode = CursorMode.load;
	UnitClass placeUnit;
	#endregion
	#region Unity functions
	void Awake() {
		if ( instance == null ){
			instance = this;
		}
		else { Destroy( this ); }
	}
	void Start() {
		StartCoroutine( LoadBattle() );
	}
	void Update () {
//		StartCoroutine( MouseModeControl() );
		MouseModeControl();

		if ( Input.GetKeyDown( KeyCode.Escape ) )
		{
			Debug.Log("Pressing Escape key");
			// TODO: Show menu
			// For now, go to overworld
			if ( GameObject.FindWithTag("GameStatus") != null ){
				GameStatus.SceneOverWorld();
			}
		}
	}
	void OnEnable()
	{
		EventManager.StartListening( "Find Move", FindUnitMove );
		EventManager.StartListening( "Find Attack", FindUnitAttack );
		EventManager.StartListening( "Next Turn", NextTurn );
		EventManager.StartListening( "Confirm Placement", StartFirstTurn );
	}
	void OnDisable()
	{
		EventManager.StopListening( "Find Move", FindUnitMove );
		EventManager.StopListening( "Find Attack", FindUnitAttack );
		EventManager.StopListening( "Next Turn", NextTurn );
		EventManager.StopListening( "Confirm Placement", StartFirstTurn );
	}
	#endregion
	#region Control functions
	IEnumerator LoadBattle(){
		Debug.Log( "Loading...");
		// Sort the load order for the game to ensure systems load in correct order
		// TODO: Find more elegant and comprehensive solution

//		// Find Scene-inbetweener-thing

		TeamSO playerTeam = null;
		MissionClassSO thisMission = null;

		if ( GameObject.FindWithTag("GameStatus") != null ){
			if ( GameStatus.playerTeam != null )
				playerTeam = GameStatus.playerTeam;
			if ( GameStatus.mission != null )
				thisMission = GameStatus.mission;
		if ( thisMission != null )
			mission = thisMission;
		if ( playerTeam != null )
			UnitManager.instance.teamA = playerTeam;
			Debug.Log("Test Success! Loaded from memory");
		}
		// Mission
		if ( mission != null ){
			if ( mission.map != null )
				testMap = mission.map;
			if ( mission.enemies != null ){
				UnitManager.instance.teamB = mission.enemies;
			}
		}

		// Grid
		if ( testMap != null ){
			Debug.Log("Test Success! Map loading...");
			yield return StartCoroutine( GridClass.instance.LoadGrid( testMap ) );
		}
		else
			yield return StartCoroutine( GridClass.instance.LoadGrid() );

		// Units
		Debug.Log("Units: loading...");
		yield return StartCoroutine( UnitManager.instance.Load());

		// Intro
		// This section is for making some cool battle intro. Naturally, this will likely go unused
		yield return StartCoroutine( UIManager.instance.Intro() );
		//TODO: animation

		//UI
		yield return null;
		UIManager.instance.Load();
		
		// Controller settings
		StartSettings();
	}
	void StartSettings(){
		// Initialise controller
		Debug.Log( "Controller settings");
		cursorMode = CursorMode.placeUnit;
//		waitForUI = false;
		waitForUI = true;
		EventManager.TriggerEvent( "Wait For UI" );
		Debug.Log("Load complete");

		FindUnitPlacing();
	}
	void MouseModeControl(){
		// Determine mouse functionality for action context
		// left click
		// TODO: Rework this horrid, janky mess
		switch( cursorMode ){
		case CursorMode.load:
			break;
		case CursorMode.inspectCell:
			CursorInspectCell();
			break;
		case CursorMode.showAttack:
			StartCoroutine( CursorFindAttack() );
			break;
		case CursorMode.showMove:
			StartCoroutine( CursorFindMove() );
			break;
		case CursorMode.placeUnit:
			StartCoroutine( CursorPlaceUnit() );
			break;
		default:
			goto case CursorMode.inspectCell;
		}
		//right click
		MouseRightClick();

//		yield return null;
	}
	void MouseRightClick(){
		// Set state to inspector mode
		if ( Input.GetMouseButtonDown(1) && cursorMode != CursorMode.inspectCell
			&& cursorMode != CursorMode.placeUnit && cursorMode != CursorMode.load){
			cursorMode = CursorMode.inspectCell;
			ClearPath();
			HighLightCell( currentUnit.Cell);
		}
	}
	// This method allows users to move units during the placement phase
	IEnumerator CursorPlaceUnit(){

		FindUnitPlacing();
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;

		if ( Input.GetMouseButtonDown(0) && Physics.Raycast( ray, out hit ) ){
			CellClass cell = GridClass.instance.GetCellFromTransform(hit.transform);

			if ( cell != null && cells.Contains(cell) ){
				UnitClass unit = UnitManager.instance.FindUnitFromCell( cell);
				FindUnitPlacing();

				if ( unit == null ){
//					Debug.Log( UnitManager.instance.units.Count);
					foreach ( UnitClass u in UnitManager.instance.units){
						
					}
				}

				// if unit
				if ( unit != null){
//					Debug.Log("Something here!");
					HighLightCell( cell );
					placeUnit = unit;
				}
				// if valid empty cell
				if ( placeUnit != null ){
					HighLightCell( cell );
					placeUnit.Cell.Occupied = false;
					placeUnit.Cell = cell;
					cell.Occupied = true;
					Vector3 pos = new Vector3( cell.CellGO.transform.position.x, placeUnit.UnitGO.transform.position.y, cell.CellGO.transform.position.z );
					placeUnit.UnitGO.transform.position = pos;
				}
			}
		}
		yield return null;
	}
	void CursorInspectCell(){
		// Mouse function
		// Show information on a highlighted cell or unit in a UI window
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;

		if ( Physics.Raycast( ray, out hit) ){ // Find a cell or unit from mouse over
			CellClass cell = GridClass.instance.GetCellFromTransform(hit.transform);
			if ( cell != null ){
				string uiText = "";
				if ( cell.Occupied == true ){
					UnitClass unit = UnitManager.instance.FindUnitFromCell(cell);
					if ( unit == null) { 
						uiText = "Fallen warrior";
					}
					else{
						uiText = InspectUnitText( uiText, unit );
					}
				}
				else{
					uiText = cell.Name;
				} 
				// Display info in UI window
				UIManager.instance.SetInspectorText(uiText);
			}
		}
	}
	// Select cell to move unit to
	IEnumerator CursorFindMove(){
		// Mouse function
		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
		RaycastHit hit;

		if ( waitForUI == true ) yield break;

		InspectorActionText();

		bool moved = UnitManager.instance.unitHasMoved;

		if ( !moved && Input.GetMouseButtonDown(0) && Physics.Raycast( ray, out hit ) ){
			CellClass cell = GridClass.instance.GetCellFromTransform( hit.transform);

			if ( cell != null && cells.Contains(cell) && cell != currentUnit.Cell ){
				if ( cell != currentCell){ //Select cell
					FindUnitMove();
					HighLightCell( cell );
					currentCell = cell;

					List<CellClass> pathList = Pathfinder.TracePath( cell, currentUnit.Cell );
					foreach( CellClass c in pathList ){
						GridClass.instance.ColourCell( c, Color.green );
					}
//					Debug.Log("Path length: "+pathList.Count);
				}
				else{ // Confirm selection - move cell
					StartCoroutine( MoveAction( cell ) );
				}
			}
		}
		yield return null;
	}
	// Show valid attack target cells and attack units
	IEnumerator CursorFindAttack(){
		// Mouse function

		if ( UnitManager.instance.unitHasAttacked ) yield break; // If the unit has attacked, exit
		if ( waitForUI == true ) yield break;

		Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition);
		RaycastHit hit;

		InspectorActionText(); // Sets info about current unit TODO: WHY THE FUCK IS THIS HERE?

		if ( Input.GetMouseButtonDown(0) && Physics.Raycast( ray, out hit ) )
		{
			CellClass cell = GridClass.instance.GetCellFromTransform( hit.transform);
			UnitClass targetUnit;
			if ( cell != null && cells.Contains(cell) && cell != currentUnit.Cell && cell.Occupied == true )
				targetUnit = UnitManager.instance.units.Find( (UnitClass u) => u.Cell == cell);
			else yield break;

//			if ( cell != null && cells.Contains(cell) && cell != currentUnit.Cell
//				&& cell.Occupied == true && targetUnit !=null  && targetUnit.playerSide != currentUnit.playerSide
//				&& targetUnit.CurrentHealth >= 0 )
			if ( targetUnit !=null  && targetUnit.playerSide != currentUnit.playerSide && targetUnit.CurrentHealth >= 0 )
			{
				if ( cell != currentCell ){ // select cell
					FindUnitAttack();
					HighLightCell( cell );
					currentCell = cell;
				}
				else { // attack cell
					//TODO: Add attack animation or something
					Debug.Log( "Hit! " + currentUnit.Name + " has attacked " + targetUnit.Name );
					ClearLastCell();
					ClearPath();

					UnitManager.instance.unitHasAttacked = true;
					StartCoroutine( AttackAction( targetUnit) );
				}
			}
		}
		yield return null;
	}
	// This coroutine makes the current unit make an attack
	public IEnumerator AttackAction( UnitClass targetUnit ){
		// currently this attack targets 1 enemy unit

		// Vars
		UnityEngine.UI.Text attackText = UIManager.instance.attackPanel.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>();
		string resultString;

		// UI layer
		ClearPath();
		cursor.SetActive(false);
		waitForUI = true;
		EventManager.TriggerEvent( "Wait For UI" );
//		UIManager.instance.attackPanel.SetActive( false);

		//Rotate unit
		float targetAngle = UnitManager.FindFacing( currentUnit.UnitGO.transform, targetUnit.UnitGO.transform);
		Debug.Log( "Rotate me this much: " + Mathf.Round( targetAngle/90f )*90f );

		Transform curT = currentUnit.UnitGO.transform;
		curT.rotation = Quaternion.Euler( new Vector3( curT.rotation.x, Mathf.Round( targetAngle/90f)*90f, curT.rotation.z ) );

		float angle = UnitManager.FindFacing( currentUnit.UnitGO.transform, targetUnit.UnitGO.transform);
		Debug.Log( "Facing target's: "+UnitManager.FacingDirection(targetUnit,currentUnit) );
		Debug.Log( ""+ angle);

		// Attack and damage calculation
		if ( UnitManager.instance.RollAttack( currentUnit, targetUnit ) == true ){
			// Do damage
			int damage = currentUnit.Damage;
			Debug.Log ( "Attack damage: " + damage );
			targetUnit.CurrentHealth -= damage;
			resultString = "Hit! \r\n" + currentUnit.Name + " strikes " + targetUnit.Name + "!";
			resultString += "\r\n" + damage + " damage!";

			// Check if enemy has been defeated
			if ( targetUnit.CurrentHealth <= 0 ){
				resultString += "\r\n";
				resultString += "\r\n";
				resultString += targetUnit.Name += " has been slain!";
				UnitManager.instance.UnitKilled( targetUnit );
			}
		}
		else {
			// Miss!
			Debug.Log( "Attack: Miss!");
			resultString = "Miss! \r\n" + targetUnit.Name + " dodges " + currentUnit.Name + "!";
		}

		// UIManager show results ( string "unit a strikes unit b x damage", wait attackTimer )
		UIManager.instance.attackPanel.SetActive( true);
		attackText.text = resultString;
		yield return new WaitForSeconds( UIManager.instance.messageTimer );
		UIManager.instance.attackPanel.SetActive( false);

		waitForUI = false;
		EventManager.TriggerEvent( "Wait For UI" );
		HighLightCell( currentUnit.Cell);
		InspectorActionText();
		UIManager.instance.CurrentUnitHUD();
		
		//Check win condition
		EndCondition();

		yield return null;
	}
	// This coroutine moves unit from current cell to target cell
	public IEnumerator MoveAction( CellClass cell ){
		// TODO: Animation

		waitForUI = true;
		EventManager.TriggerEvent( "Wait For UI" );
		cursor.SetActive(false);
		ClearPath();

		List<CellClass> path = Pathfinder.TracePath( cell, currentUnit.Cell ); // Get the path

		// Show path
		foreach ( CellClass c in path)
		{
			GridClass.instance.ColourCell( c, Color.green );
		}
		GridClass.instance.ColourCell( currentUnit.Cell, Color.yellow );
		GridClass.instance.ColourCell( cell, Color.yellow );

		// variables to move unit smoothly
		float speed = actionSpeed;
		Transform uT = currentUnit.UnitGO.transform;
		Transform destT;

		// Move unit
		for( int i = 0; i < path.Count; i++){
			destT = path[i].CellGO.transform;

			//TODO: Work on facing
			currentUnit.UnitGO.transform.LookAt(path[i].CellGO.transform); // Rotate unit to correct direction

			//Moves unit
//			// TODO: Animation
			while ( uT.position != destT.position ){
				float step = Time.deltaTime*speed;
				uT.position = Vector3.MoveTowards( uT.position, destT.position, step );
				yield return null;
			}
		}
		// Clear path
		foreach ( CellClass c in path)
		{
			GridClass.instance.ClearCell( c );
		}
		GridClass.instance.ClearCell( currentUnit.Cell );

		// Important! Update cells
		currentUnit.Cell.Occupied = false;
		currentUnit.Cell = cell;
		cell.Occupied = true;

		HighLightCell(cell);

		UnitManager.instance.unitHasMoved = true;
		UIManager.instance.CurrentUnitHUD();
		currentCell = null;
		waitForUI = false;
		EventManager.TriggerEvent( "Wait For UI" );

		yield return null;
	}
	#endregion
	#region UI functions
	// Pathfinder to find valid move cells
	public void FindUnitMove(){
		if ( waitForUI ) return;
		cursorMode = CursorMode.showMove;
		ClearPath();
//		currentUnit = units[currentTurn];
		currentUnit = UnitManager.instance.units[currentTurn];
		cells = Pathfinder.FindPath( currentUnit.Cell, GridClass.instance, currentUnit.Move );
		ShowMoveArea();
		HighLightCell( currentUnit.Cell);
		InspectorActionText();
	}
	// Pathfinder to find valid attack cells
	public void FindUnitAttack(){
		if ( waitForUI ) return;
		cursorMode = CursorMode.showAttack;
		ClearPath();
//		currentUnit = units[currentTurn];UnitManager.instance.
		currentUnit = UnitManager.instance.units[currentTurn];
		cells = Pathfinder.FindPath( currentUnit.Cell, GridClass.instance, currentUnit.Reach, currentUnit );
		ShowAttackArea();
		HighLightCell( currentUnit.Cell);
	}
	public void FindUnitPlacing(){
		cursorMode = CursorMode.placeUnit;
		cells = GridClass.instance.placingList;
		ShowPlacingArea();
	}
	public void NextTurn(){
		// End the current unit turn and prepare for the next unit

		if ( waitForUI ) return;

		UnitManager.instance.unitHasMoved = false;
		UnitManager.instance.unitHasAttacked = false;

		int max = UnitManager.instance.units.Count;
		if ( currentTurn < UnitManager.instance.units.Count-1 ){
			currentTurn++;
		}
		else currentTurn = 0;
		ClearLastCell();
		ClearPath();
		currentUnit = UnitManager.instance.units[currentTurn];
		if ( currentUnit.CurrentHealth <=0 )
		{
//			EndCondition();
			NextTurn();
		}
		HighLightCell( currentUnit.Cell);
		FindUnitMove();
		UIManager.instance.CurrentUnitHUD();
	}
	public void StartFirstTurn()
	{
		// TODO: Initiative or something
		waitForUI = false;
		EventManager.TriggerEvent( "Wait For UI" );
		cursorMode = CursorMode.inspectCell;
		ClearPath();
		HighLightCell( UnitManager.instance.units[currentTurn].Cell);
		FindUnitMove();
		UIManager.instance.CurrentUnitHUD();
	}
	void InspectorActionText(){
		// Set UI window text
		string uiText = "";
		uiText += currentUnit.Name;
		uiText += "\r\n" + "=======";
		uiText += "\r\n";
		uiText += "HP: " + currentUnit.CurrentHealth + "/" + currentUnit.Health;
		uiText += "\r\n";
		uiText += "m = " + UnitManager.instance.unitHasMoved;
//		uiText += "\r\n";
		uiText += " | ";
		uiText += "a = " + UnitManager.instance.unitHasAttacked;
		uiText += "\r\n";
		uiText += "Move\t: " + currentUnit.Move;
		uiText += "\r\t\n";
		uiText += "Defence: " + currentUnit.Defence;
		uiText += "\r\n";
		uiText += "Attack: " + currentUnit.Attack;

		UIManager.instance.SetInspectorText( uiText);
	}
	string InspectUnitText( string uiText, UnitClass unit ){
		uiText = unit.Name;
		uiText += "\r\n";
		uiText += "HP: " + unit.CurrentHealth + "/" + unit.Health;
		uiText += "\r\n";
		uiText += "Move\t: " + unit.Move;
		uiText += "\r\t\n";
		uiText += "Defence: " + unit.Defence;
		uiText += "\r\n";
		uiText += "Attack: " + unit.Attack;
		return uiText;
	}
	void HighLightCell( CellClass cell ){
		//colour and highlight the cell
		cursor.SetActive(true);
		cursor.transform.position = cell.CellGO.transform.position;
		currentCell = cell;
		GridClass.instance.ColourCell( cell, Color.yellow );
	}
	void ClearLastCell(){
		if ( currentCell != null){
			GridClass.instance.ClearCell(currentCell);
		}
	}
	void ShowMoveArea(){
		// Highlights valid move cells
		if ( cells.Count != 0 ){
			foreach( CellClass cell in cells ){
				GridClass.instance.ColourCell(cell, Color.cyan );
			}
		}
	}
	void ShowAttackArea(){
		// Highlights valid attack cells
		if ( cells.Count != 0 ){
			foreach( CellClass cell in cells ){
				GridClass.instance.ColourCell(cell, Color.magenta );
			}
		}
	}
	void ShowPlacingArea(){
		// Highlights placement cells
		if ( cells.Count != 0 ){
			foreach( CellClass cell in cells ){
				GridClass.instance.ColourCell(cell, Color.green );
			}
		}
	}
	void ClearPath(){
		// Clears all highlighted cells
		if ( cells.Count != 0 ){
			foreach ( CellClass cell in cells ){
				GridClass.instance.ClearCell( cell );
			}
		}
	}
	#endregion
	#region Helper functions
//	UnitClass FindUnitFromCell( CellClass unitTransform ){
//		UnitClass unit = Units.Find( (UnitClass u) => u.Cell == unitTransform );
//		return unit;
//	}
	IEnumerator WinBattle(){
		UIManager.instance.endBattlePanel.SetActive( true);
		UnityEngine.UI.Text attackText = UIManager.instance.endBattlePanel.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>();
		attackText.text = "Victory!";
		yield return new WaitForSeconds( UIManager.instance.messageTimer*5);
		GameStatus.SceneOverWorld();
	}
	IEnumerator LoseBattle(){
		UIManager.instance.endBattlePanel.SetActive( true);
		UnityEngine.UI.Text attackText = UIManager.instance.endBattlePanel.transform.GetChild(2).GetComponent<UnityEngine.UI.Text>();
//		UnityEngine.UI.Text attackText = UIManager.uiManager.attackPanelText;
		attackText.text = "Defeat!";
		yield return new WaitForSeconds( UIManager.instance.messageTimer*5 );
		GameStatus.SceneOverWorld();
	}
//	void UnitKilled( UnitClass unit ){
//		unit.UnitGO.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.black;
//		Destroy( UIManager.uiManager.InitiativeList[Units.FindIndex( (UnitClass u) => u == unit )] );
//
//		EndCondition();
//	}
	public void EndCondition(){
		// Check to see if game is won or lost
		// TODO: Metric butt load

		List<UnitClass> e = UnitManager.instance.EnemyUnits();
		List<UnitClass> p = UnitManager.instance.PlayerUnits();
		if ( e.Count == 0 || p.Count == 0 )
		{
			waitForUI = true;
			EventManager.TriggerEvent( "Wait For UI" );
			ClearPath();
			UIManager.instance.turnOptionMenu.SetActive(false);
			if ( e.Count == 0){
				// win
				Debug.Log("Win");
				StartCoroutine( WinBattle() );
			}
			else if ( p.Count == 0){
				// lose
				Debug.Log("Lose");
				StartCoroutine( LoseBattle() );
			}
		}
	}
	#endregion
}
