using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewController : MonoBehaviour {
	public static OverviewController instance;
	public GameObject[] tabs;
	public GameObject shopListViewport;
	public GameObject teamListViewport;
	public GameObject missionListViewport;
	public GameObject optionsMenu;

	public TeamSO playerTeam;

	void Awake(){
		// "Singleton" air quotes
		if ( instance == null )
			instance = this;
		else 
		{
			Destroy( this );
			return;
		}

		// Clone assets

		// GameStatus
		if ( GameObject.FindWithTag("GameStatus") )
		{
			if ( GameStatus.playerTeam == null ){
				NewGame();
			}
			else {
				ContinueGame();
			}
		}
//		playerTeam = Instantiate( playerTeam);
	}
	void Start(){
		
	}
	void NewGame()
	{
		if ( playerTeam != null )
			playerTeam = Instantiate( playerTeam);
		else 
		{
			Debug.LogError( "Error: No default player team");
			return;
		}
		Debug.Log("Loading default team");
		GimmeNames();
	}
	void ContinueGame()
	{
		Debug.Log("Loading saved team");
		if ( GameStatus.playerTeam != null)
			playerTeam = Instantiate(GameStatus.playerTeam);
		else
			Debug.LogError( "Error: Cannot load team" );
//		GimmeNames();
	}
	void GimmeNames()
	{
		for ( int i = 0; i < playerTeam.units.Length; i++ )
		{
			UnitSO u = playerTeam.units[i];
			if ( u == null )
			{
				Debug.LogError( "Error( Names): Unit not found");
				continue;
			}
			u = Instantiate(u);
			u.unit.Name = RandomNames.RandomName();
			playerTeam.units[i] = u;
		}
	}
	#region UI
	public void SwitchTab( string tabName ){
		// Disable all tabs in 'tabs' and activate tab specified by name
		foreach ( GameObject tab in tabs ){
//			if ( tab == null ) continue;
			tab.SetActive(false);
			if ( tab.name == tabName ){
				tab.SetActive(true);
			}
		}
	}
	public void OptionsMenu()
	{
		if ( optionsMenu != null )
		{
			optionsMenu.SetActive( !optionsMenu.activeSelf );
		}
	}
	#endregion
}