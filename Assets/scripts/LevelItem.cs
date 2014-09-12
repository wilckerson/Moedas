using UnityEngine;
using System.Collections;

public class LevelItem : MonoBehaviour {

	UIScroll parentScroll;
	BoxCollider2D box2D;

	// Use this for initialization
	void Start () {
	
		box2D = GetComponent<BoxCollider2D>();
		var components = GetComponentsInParent<UIScroll>();
		if(components.Length > 0){
			parentScroll = components[0];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		bool mouseInside = box2D.bounds.IntersectRay(Camera.main.ScreenPointToRay(Input.mousePosition));

		if(Input.GetMouseButtonUp(0) && mouseInside){
			//Debug.Log("LevelItemMouseDown");

			if(parentScroll.SelectedItem() == this.transform)
			{
				Application.LoadLevel("Game");
			}
			else
			{
				parentScroll.SnapToItem(this.transform);
			}

		}

	}

//	void OnMouseDown(){
//		Debug.Log("LevelItemMouseDown");
//		//parentScroll.SnapToItem(this.transform);
//	}
}
