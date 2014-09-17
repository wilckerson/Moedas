using UnityEngine;
using System.Collections;

public class LevelItem : MonoBehaviour
{

	UIScroll parentScroll;

	// Use this for initialization
	void Start ()
	{
		var components = GetComponentsInParent<UIScroll> ();
		if (components.Length > 0) {
			parentScroll = components [0];
		
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void OnMouseUp ()
	{
		if (parentScroll.SelectedItem () == this.transform) {
			Application.LoadLevel ("Game");
			//Debug.Log("SELECT");
		} else {
			parentScroll.SnapToItem (this.transform);
			//Debug.Log("SNAP");
		}
	}
}
