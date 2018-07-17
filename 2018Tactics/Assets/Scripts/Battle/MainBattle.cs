using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBattle : MonoBehaviour {
	public static MainBattle mainBattle;

	void Awake() {
		Debug.Log("What is going on?");
		if ( mainBattle == null ){
			mainBattle = this;
		}
		else { Destroy( this ); }
	}
	IEnumerator LoadAll(){
		// grid>units>ui>intro
		//Show load screen
		//load grid
		//catch if fails
		yield return StartCoroutine( GridClass.instance.LoadGrid() );
		yield return null;

		//load units
		yield return StartCoroutine( UnitManager.instance.Load() );

		//load ui
		UIManager.instance.Load();
		yield return null;

		//remove load screen
		yield return null;
		//start animation
		yield return null;
	}
	void LoadMain(){
		//prep for first turn
	}
	void NextTurn(){
		
	}
	void Move(){}
	void Attack(){}
}