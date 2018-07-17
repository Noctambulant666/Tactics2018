using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListMissionDisplay : MonoBehaviour {
	[SerializeField] Image icon;
	[SerializeField] Text nameText;

	public MissionClassSO mission;

	void Start(){
		if ( this.mission != null ){
			Prime();
		}
		Button button = this.transform.GetComponent<Button>();
		if ( button != null ){
			button.onClick.AddListener( ButtonMethod );
		}
	}

	public void Prime(){
		if ( nameText != null ) {
			nameText.text = mission._name;
		}
		if ( icon != null ){
			icon.sprite = mission._sprite;
		}
	}
	void ButtonMethod(){
		Debug.Log( mission._name + ": " + mission._description);

		if ( GameObject.FindWithTag("GameStatus") != null )
		{
			GameStatus.mission = mission;
			GameStatus.playerTeam = OverviewController.instance.playerTeam;
			GameStatus.SceneBattle();
		}
	}
}