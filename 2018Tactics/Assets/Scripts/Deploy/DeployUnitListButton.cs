using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeployUnitListButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
//	Transform parent;
	public void OnBeginDrag( PointerEventData data ){
//		parent = this.transform.parent;
		DeployUnitScreenControl.instance.StopScroll( true );
//		this.GetComponent<LayoutElement>().ignoreLayout = true;
//		this.transform.SetParent( this.transform.parent.parent.parent );
	}
	public void OnDrag( PointerEventData data ){
		this.gameObject.transform.position = Input.mousePosition;
	}
	public void OnEndDrag( PointerEventData data ){
//		this.transform.SetParent( parent );
		DeployUnitScreenControl.instance.StopScroll( false );
//		this.GetComponent<LayoutElement>().ignoreLayout = false;
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform.parent.GetComponent<RectTransform>());
	}
	public void OnDrop( PointerEventData data ){}
}