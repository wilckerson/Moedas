using UnityEngine;
using System.Collections;

public class UIScroll : MonoBehaviour
{

	bool pressed = false;
	Vector3 dragDelta;
	Vector3 mouseLastPosition;

	public GameObject items;
	public GameObject top;
	public GameObject bottom;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		updateDragDelta();

		var itemPos = items.transform.position;
		itemPos.y += dragDelta.y * Time.deltaTime;
		items.transform.position = itemPos;

		foreach (Transform listItem in items.transform) {

			if(listItem.position.y > top.transform.position.y
			   || listItem.position.y < bottom.transform.position.y)
			{
				listItem.gameObject.SetActive(false);
			}
			else
			{
				listItem.gameObject.SetActive(true);
			}
		}
	}

	void updateDragDelta(){

		if (Input.GetMouseButtonDown (0)) {
			pressed = true;
			mouseLastPosition = Input.mousePosition;
			dragDelta = Vector3.zero;
		} else if (Input.GetMouseButtonUp (0)) {
			pressed = false;
			dragDelta = Vector3.zero;
		}
		
		if (pressed) {
			
			//Debug.Log (Input.mousePosition);
			dragDelta = Input.mousePosition - mouseLastPosition;
			Debug.Log(dragDelta);
			
			mouseLastPosition = Input.mousePosition;
		}
	}
}
