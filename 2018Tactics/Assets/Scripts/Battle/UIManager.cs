using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public static UIManager instance;
	bool waitForUI = false;
	public int messageTimer; // Length of time to display message
	public Text inspectorText; // Info on units/cells
	public GameObject attackPanel; // Message displaying attack results
	public GameObject AnnouncePanel; // Mission intro
	public GameObject LoadScreen; // Splash screen to mask game loading
	public GameObject InitiativeContent; // List of unit turns
	public GameObject InitiativePanel;
	public GameObject endBattlePanel; // Panel displayed on when battle ends

	public GameObject turnOptionMenu;
	public GameObject placeUnitsPanel;

	public GameObject UnitInitiativeButtonPrefab;
	public List<GameObject> InitiativeList = new List<GameObject>();

	//HUD
	public Image unitPortrait;
	public Text unitNameText;
	public Text unitHpText;
	public Image hpBar;
	public Text unitInfoText;


	void Awake(){
		if ( instance == null ){
			instance = this;
		}
		else {
			Destroy(this);
		}
	}
	// Start listening to events
	void OnEnable()
	{
		EventManager.StartListening( "Wait for UI", WaitForUI );
	}
	// Stop listening to events
	void OnDisable()
	{
		EventManager.StopListening( "Wait for UI", WaitForUI );
	}
	void Update(){
		HotKeys();
	}
	public void Load(){
		// Load UI elements such as initiative list

		// Turns List
//		foreach ( UnitClass u in Controller.instance.units ){
		foreach ( UnitClass u in UnitManager.instance.units ){
			GameObject unitButton = Instantiate(
				UIManager.instance.UnitInitiativeButtonPrefab,
				UIManager.instance.InitiativeContent.transform );
			unitButton.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = u.Name;
			InitiativeList.Add( unitButton);

			if ( u.PlayerSide == true ){ 
				// colour blue
				unitButton.GetComponent<UnityEngine.UI.Image>().color = new Color( 0,0.7f,1,1);
			}
			else { 
				// colour red
				unitButton.GetComponent<UnityEngine.UI.Image>().color = new Color( 1,0.2f,0.2f,1);
			}
		}
		placeUnitsPanel.SetActive(true);
	}
	public IEnumerator Intro()
	{
		LoadScreen.SetActive(false);
		AnnouncePanel.SetActive(true);
		yield return new WaitForSeconds(2);
		UIManager.instance.AnnouncePanel.SetActive(false);
	}
	void WaitForUI()
	{
		if ( Controller.instance != null )
		{
			waitForUI = Controller.instance.waitForUI;
		}
	}
	public void SetInspectorText( string message ){
		if ( Controller.instance.waitForUI == false )
		inspectorText.text = message;
	}
	public void ShowUnitMove(){
		// Button shows where unit can move
		if ( !Controller.instance.waitForUI )
			EventManager.TriggerEvent("Find Move");
	}
	public void ShowUnitAttack(){
		// Button shows where unit can attack
		if ( !Controller.instance.waitForUI )
			EventManager.TriggerEvent("Find Attack");
	}
	public void ConfirmUnitPlacement(){
		// Button ends place unit phase
		turnOptionMenu.SetActive( true);
		placeUnitsPanel.SetActive( false);
		EventManager.TriggerEvent("Confirm Placement");
	}
	public void NextTurn(){
		// Button ends current turn and starts the next one
//		if ( Controller.instance.waitForUI == false )
		EventManager.TriggerEvent("Next Turn");
	}
	public void ShowTurns(){
		InitiativePanel.SetActive( !InitiativePanel.activeSelf );
	}
	public void EndBattle()
	{
		GameStatus.SceneOverWorld();
	}
	// Show info about current unit in a nice little HUD
	public void CurrentUnitHUD()
	{
		//portait
		//name
		//health bar
		//hp text current/max
		UnitClass unit = Controller.instance.currentUnit;
		float cur = unit.CurrentHealth;
		float max = unit.BaseHealth;
		float hpPercentage = Mathf.Clamp01(cur/max);
//
		unitPortrait.sprite = unit._sprite;
		unitNameText.text = unit.Name;
		unitHpText.text = unit.CurrentHealth+" / "+unit.BaseHealth;
		hpBar.fillAmount = hpPercentage;

		string info = "";
		string hasMoved = "M:";
		string hasAttacked = "A:";
		if ( UnitManager.instance.HasMoved )
			hasMoved += "0";
		else
			hasMoved += "1";
		if ( UnitManager.instance.HasAttacked )
			hasAttacked += "0";
		else
			hasAttacked += "1";
		info += hasMoved + " | " + hasAttacked;
		info += "\r\n";
		info += "Attack: " + unit.Attack;
		info += "\r\n";
		info += "Defence: " + unit.Defence;
		info += "\r\n";
		info += "Move: " + unit.Move;
		info += "\r\n";
		info += "STR: " + unit.Strength;
		info += "\r\n";
		info += "AGI: " + unit.Agility;
		info += "\r\n";
		info += "WILL: " + unit.Will;
		info += "\r\n";
		info += "Weapon: " + unit._weapon._name;

		unitInfoText.text = info;
	}
	// Hotkey shortcuts for all buttons
	public void HotKeys() {
		if (Controller.instance.waitForUI ) return;
		
		if ( Input.GetKeyDown( KeyCode.A )){
			ShowUnitAttack();
		}
		if ( Input.GetKeyDown( KeyCode.S )){
			ShowUnitMove();
		}
		if ( Input.GetKeyDown( KeyCode.Space )){
			NextTurn();
		}
	}
}