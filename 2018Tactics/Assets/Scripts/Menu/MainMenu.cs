using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject credits;

	public TeamSO savedTeam;

	public void NewGame(){
		// Start a new game
		if ( GameObject.FindWithTag("GameStatus") ){
			GameStatus.SceneOverWorld();
		}
	}
	public void ContinueGame(){
		// Load previous game state into GameStatus then switch to Overworld scene
		if ( GameObject.FindWithTag("GameStatus") ){
			LoadGame();
			GameStatus.SceneOverWorld();
		}
	}
	public void Credits(){
		mainMenu.SetActive(false);
		credits.SetActive(true);
	}
	public void Back(){
	mainMenu.SetActive(true);
		credits.SetActive(false);
	}
	public void Quit(){
		Application.Quit();
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
	void LoadGame()
	{
		// TEST TEST TODO: DO ACTUAL CODE
		GameStatus.playerTeam = Instantiate(savedTeam);
		GimmeNames();
	}
	void GimmeNames()
	{
		for ( int i = 0; i < GameStatus.playerTeam.units.Length; i++ )
		{
			UnitSO u = GameStatus.playerTeam.units[i];
			u = Instantiate(u);
			u.unit.Name = RandomNames.RandomName();
			GameStatus.playerTeam.units[i] = u;
		}
	}
}