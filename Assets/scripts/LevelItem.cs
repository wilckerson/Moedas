using UnityEngine;
using System.Collections;

public class LevelItem : MonoBehaviour
{

	UIScroll parentScroll;
	//BoxCollider2D box2D;
	PulseAnimation pulseAnim;
	// Use this for initialization
	void Start ()
	{
	
		//box2D = GetComponent<BoxCollider2D> ();

		pulseAnim = GetComponent<PulseAnimation>();
		pulseAnim.enabled = false;

		var components = GetComponentsInParent<UIScroll> ();
		if (components.Length > 0) {
			parentScroll = components [0];
			parentScroll.OnItemSelected += HandleOnItemSelected;
		}
	}

	void HandleOnItemSelected (Transform itemSelected)
	{
		if(itemSelected == this.transform)
		{
			pulseAnim.enabled = true;
			//pulseAnim.Reset();
		}
		else
		{
			pulseAnim.enabled = false;
			//pulseAnim.Reset();
		}
	}

	//Down + esperar um tempo para ver se nao houve drag = click
	//Down/Up + focado = click

	// Update is called once per frame
//	void Update () {
//	
//		bool mouseInside = box2D.bounds.IntersectRay(Camera.main.ScreenPointToRay(Input.mousePosition));
//
//		if(Input.GetMouseButtonUp(0) && mouseInside){
//			//Debug.Log("LevelItemMouseDown");
//
//			if(parentScroll.SelectedItem() == this.transform)
//			{
//				Application.LoadLevel("Game");
//			}
//			else
//			{
//				parentScroll.SnapToItem(this.transform);
//			}
//
//		}
//
//	}

	void OnMouseUp ()
	{

		if (parentScroll.SelectedItem () == this.transform) {
			//Application.LoadLevel ("Game");
			Debug.Log("SELECT");
		} else {
			parentScroll.SnapToItem (this.transform);
			Debug.Log("SNAP");
		}
	}
}
