using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployUnitScreenControl : MonoBehaviour {
	public static DeployUnitScreenControl instance;
	[SerializeField] ScrollRect scrollRect;

	void Start(){
		if ( instance == null ){
			instance = this;
		}
		else Destroy(this);
	}
	public void StopScroll( bool stopOrStart ){
		if ( stopOrStart == true ){
			scrollRect.StopMovement();
			scrollRect.enabled = false;
		}
		else {
			scrollRect.enabled = true;
		}
	}
}