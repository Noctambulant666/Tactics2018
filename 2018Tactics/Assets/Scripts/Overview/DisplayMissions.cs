using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayMissions : MonoBehaviour {
	public GameObject missionButtonPrefab;
	public GameObject missionList;
	public List<MissionClassSO> missions;

	void Start(){
		PopulateList();
	}
	void PopulateList(){
		for( int i = 0; i < missions.Count; i++ ){
			GameObject item = (GameObject)Instantiate( missionButtonPrefab, missionList.transform.position, Quaternion.identity,missionList.transform );
			item.GetComponent<ListMissionDisplay>().mission = missions[i];
		}
	}
}