using UnityEngine;
using System.Collections;

public enum InputStatus
{
	None = 0,
	Down = 1,
	Move = 2
}

public class Drag : MonoBehaviour
{
	Vector2 lastInputPos;
	Vector2 inputDragDelta;
	Vector3 lastWorldPos;
	Vector3 worldDragDelta;
	Vector3 dragTotal;
	//BoxCollider2D box2D;
	Vector2 inputPos;
	InputStatus inputStatus;

	void Start ()
	{
		//box2D = GetComponent<BoxCollider2D> ();
	}

//	void OnGUI ()
//	{
//		GUI.Label (new Rect (0, 0, 100, 100), inputPos.ToString ());
//		GUI.Label (new Rect (0, 20, 100, 100), worldDragDelta.ToString ());
//		GUI.Label (new Rect (0, 40, 100, 100), inputStatus.ToString ());
//	}

	// Update is called once per frame
	void Update ()
	{
		UpdateInputPos ();
		UpdateDragDelta ();
		ApplyDrag ();
	}

	void UpdateInputPos ()
	{
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			if (Input.touchCount > 0) {
				inputPos = Input.GetTouch (0).position;
			
				if (inputStatus == InputStatus.None) {
					inputStatus = InputStatus.Down;
					
				} else if (inputStatus == InputStatus.Down) {
					inputStatus = InputStatus.Move;
				}
				
			} else {			
				inputStatus = InputStatus.None;
			}
		} else {
			inputPos = Input.mousePosition;

			if (inputStatus == InputStatus.Down) {
				inputStatus = InputStatus.Move;
			}
			if (Input.GetMouseButtonDown (0)) {
				if (inputStatus == InputStatus.None) {
					inputStatus = InputStatus.Down;
				}
			} else if (Input.GetMouseButtonUp (0)) {
				inputStatus = InputStatus.None;
			}
		}
	}

	void UpdateDragDelta ()
	{
		var worldPos = Camera.main.ScreenPointToRay (new Vector3 (inputPos.x, inputPos.y, 0)).GetPoint (-Camera.main.transform.position.z);

		worldDragDelta *= 0.8f;
		if (inputStatus == InputStatus.Move) {
			worldDragDelta = worldPos - lastWorldPos;
		}

		lastWorldPos = worldPos;
	}

	void ApplyDrag ()
	{
		//Movimenta os items de acordo com o drag
		var itemPos = this.transform.position;
			
		//if (Direction == UIScrollDirection.Vertical) {
		itemPos.y += worldDragDelta.y;
		//} else {
		itemPos.x += worldDragDelta.x;
		//}
			
		this.transform.position = itemPos;
	}
}
