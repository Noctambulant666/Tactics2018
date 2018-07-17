using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {
	public static EventManager instance;
	public static Dictionary< string, UnityEvent > eventDictionary = new Dictionary< string, UnityEvent>();

	// Use this for initialization
	void Awake () {
		if ( instance == null ){
			instance = this;
		}
		else{
			Destroy( this );
			return;
		}
		if ( eventDictionary == null )
		{
			new Dictionary< string, UnityEvent>();
		}
	}
	public static void StartListening( string eventName, UnityAction listener )
	{
		UnityEvent thisEvent = null;
		if ( eventDictionary.TryGetValue( eventName, out thisEvent ) )
		{
			thisEvent.AddListener(listener);
		}
		else
		{
			thisEvent = new UnityEvent();
			thisEvent.AddListener( listener );
			eventDictionary.Add( eventName, thisEvent );
		}
	}
	public static void StopListening( string eventName, UnityAction listener )
	{
		if ( instance == null ) return;
		UnityEvent thisEvent = null;
		if ( eventDictionary.TryGetValue( eventName, out thisEvent ) )
		{
			thisEvent.RemoveListener( listener );
		}
	}
	public static void TriggerEvent( string eventName )
	{
		UnityEvent thisEvent = null;
		if ( eventDictionary.TryGetValue( eventName, out thisEvent) )
		{
			thisEvent.Invoke();
		}
	}
}