using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour {
	// This object is responsible data persistance between scenes

	static private bool created = false; // "Singleton" check

//	public static bool newGame = false;
	public static TeamSO playerTeam = null;
	public static MissionClassSO mission = null;
	public static string lastScene = "";
	public static string currentScene = "";

	public const string sceneOverworld = "Overview";
	public const string sceneMainMenu = "MainMenu";
	public const string sceneBattle = "BattleScene";
	
//	int playerMoney = 0;
//	string playerName = "Player";
//	Sprite playerIcon = null;

	void Awake()
	{
		if ( !created )
		{
			created = true;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy( this.gameObject);
		}
	}
	void Start(){
		currentScene = SceneManager.GetActiveScene().name;
	}
	public static void SceneOverWorld(){
		ChangeScene(sceneOverworld);
	}
	public static void SceneBattle(){
		ChangeScene(sceneBattle);
	}
	public static void ChangeScene( string scene ){
		lastScene = currentScene;
		currentScene = scene;
		SceneManager.LoadScene( scene );
	}
}