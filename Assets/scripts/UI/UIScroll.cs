using UnityEngine;
using System;
using System.Collections;

public enum UIScrollDirection
{
	Vertical,
	Horizontal
}

public class UIScroll : MonoBehaviour
{
	public UIScrollDirection Direction;
	public GameObject items;
	public float hideDuration = 1.5f;
	public float startMargin = 0;
	public bool snapItem = false;
	public float snapVelocity = 2;
	//public float dragDeltaMax = 30;

	public delegate void OnItemSelectedDelegate (Transform itemSelected);

	public event OnItemSelectedDelegate OnItemSelected;

	static int currentInstance = -1;
	Vector3 dragDelta;
	Vector3 dragTotal;
	Vector3 lastWorldPos;
	Bounds itemsBounds;
	Vector3 boundsCenterFix;
	BoxCollider2D box2D;
	int snapItemIdx = 0;
	int nextSnapItemIdx;
	InputStatus inputStatus;
	Vector3 inputPos;
	Vector3 worldPos;
	float inputDirty = 0.01f;

	// Use this for initialization
	void Start ()
	{
		itemsBounds = CalculateBounds (items.transform);
		boundsCenterFix = itemsBounds.center - items.transform.position;
		box2D = GetComponent<BoxCollider2D> ();
		nextSnapItemIdx = snapItemIdx;

	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateInputPos ();

		UpdateDragDelta ();

		ApplyDrag ();

		if (snapItem) {
			SnapContent ();
		} else {
			ScrollContent ();
		}

		HideItemsFX ();

		//ScaleHide
//			if (item.position.y > start.transform.position.y) {
//				float dist = item.position.y - start.transform.position.y;
//				item.localScale = new Vector3(1,1-(Mathf.Clamp(dist,0,1)*2));
//
//				if(item.localScale.y <= 0)
//				{
//					item.gameObject.SetActive(false);
//				}
//			}
//			else
//			{
//				item.localScale = Vector3.one;
//				item.gameObject.SetActive(true);
//			}

		Debug.DrawLine (itemsBounds.min, itemsBounds.max, Color.red);

	}

	void HideItemsFX ()
	{

		foreach (Transform item in items.transform) {
			var components = item.gameObject.GetComponentsInChildren<Component> (false);
		
			foreach (var component in components) {
				if (component is SpriteRenderer) {
					var render = (SpriteRenderer)component;
					render.color = HideItemColor (item, render.color);
				} else if (component is TextMesh) {
					var txMesh = (TextMesh)component;
					txMesh.color = HideItemColor (item, txMesh.color);
				}
			}
		}
	}

	Color HideItemColor (Transform item, Color itemColor)
	{
		Color newColor = itemColor;
		if (Direction == UIScrollDirection.Vertical) {
			if (item.position.y > box2D.bounds.max.y) {
			
				var alpha = 1 - (Easing (1, Mathf.Abs (item.position.y - box2D.bounds.max.y), hideDuration) - 1);
			
				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, alpha);

			} else if (item.position.y < box2D.bounds.min.y) {
				var alpha = 1 - (Easing (1, Mathf.Abs (item.position.y - box2D.bounds.min.y), hideDuration) - 1);
			
				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, alpha);

			} else {
			
				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, 1);
			}
		} else {
			if (item.position.x > box2D.bounds.max.x) {
					
				var alpha = 1 - (Easing (1, Mathf.Abs (item.position.x - box2D.bounds.max.x), hideDuration) - 1);

				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, alpha);

			} else if (item.position.x < box2D.bounds.min.x) {
				var alpha = 1 - (Easing (1, Mathf.Abs (item.position.x - box2D.bounds.min.x), hideDuration) - 1);
					
				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, alpha);
					
			} else {
					
				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, 1);
			}
		}

		return newColor;
	}

	bool NeedScroll ()
	{
		var needScroll = false;
		if (Direction == UIScrollDirection.Vertical) {
			
			needScroll = (itemsBounds.size.y > box2D.size.y - startMargin) 
				|| (itemsBounds.max.y > box2D.bounds.max.y)
				|| (itemsBounds.min.y < box2D.bounds.min.y);
		} else {
			needScroll = (itemsBounds.size.x > box2D.size.x - startMargin) 
				|| (itemsBounds.max.x > box2D.bounds.max.x)
				|| (itemsBounds.min.x < box2D.bounds.min.x);
			
		}
		return needScroll;
	}

	void ApplyDrag ()
	{
		if (NeedScroll ()) {
			
			//Movimenta os items de acordo com o drag
			var itemPos = items.transform.position;
			
			if (Direction == UIScrollDirection.Vertical) {
				itemPos.y += dragDelta.y;
			} else {
				itemPos.x += dragDelta.x;
			}
			
			items.transform.position = itemPos;
		}
	}

	void UpdateDragDelta ()
	{
		worldPos = Camera.main.ScreenPointToRay (new Vector3 (inputPos.x, inputPos.y, 0)).GetPoint (-Camera.main.transform.position.z);

		dragDelta *= 0.8f;

		if (box2D.OverlapPoint (worldPos)) {
		
			var instanceId = this.GetInstanceID();
			if (inputStatus == InputStatus.Move && (currentInstance == -1 || currentInstance == instanceId)) {
				dragDelta = worldPos - lastWorldPos;
				dragTotal += dragDelta;
				currentInstance = instanceId;
			}
			
			lastWorldPos = worldPos;
		}

		if (inputStatus == InputStatus.None) {
			dragTotal = Vector3.zero;
			currentInstance = -1;
		}
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

	void SnapContent ()
	{
		if (inputStatus == InputStatus.Move) {
			float minDist = int.MaxValue;
			for (int i = 0; i < items.transform.childCount; i++) {
				var child = items.transform.GetChild (i);
				float dist = 0;
				if (Direction == UIScrollDirection.Vertical) {
					dist = child.position.y - this.transform.position.y;
				} else {
					dist = child.position.x - this.transform.position.x;
				}

				//Resolvendo bug da prioridade quando tenta voltar o item inicial ou tenta ir alem do item final
				bool isEdge = false;
				if ((dist > 0 && snapItemIdx == 0) || (dist < 0 && snapItemIdx == items.transform.childCount - 1)) {
					isEdge = true;
				}
				
				//Priorizando os outros itens (Não precisa arrastar completamente até o outro item)
				if (i != snapItemIdx || isEdge) {
					//Prioridade
					dist = dist * (0.168f * 2);
				}

				dist = Mathf.Abs (dist);

				if (dist < minDist) {
					minDist = dist;
					nextSnapItemIdx = i;
				}
			}
		}

		if (inputStatus == InputStatus.None) {
			var snapItem = items.transform.GetChild (nextSnapItemIdx);
			
			//Movendo para focar o item
			if (Direction == UIScrollDirection.Horizontal) {
				var vel = Mathf.Abs (snapVelocity * Time.deltaTime);

				if (snapItem.position.x > this.transform.position.x) {
					items.transform.position -= new Vector3 (vel, 0, 0);
					
				} else if (snapItem.position.x < this.transform.position.x) {
					items.transform.position += new Vector3 (vel, 0, 0);
				}
				
				float dist = this.transform.position.x - snapItem.position.x;
				if (Mathf.Abs (dist) <= (vel*2)) {
					items.transform.position += new Vector3 (dist,0,0);

					snapItemIdx = nextSnapItemIdx;
					//nextSnapItemIdx = -1;
					if (OnItemSelected != null) {
						OnItemSelected (snapItem);
					}
				}
			}
			else if (Direction == UIScrollDirection.Vertical) {
				var vel = Mathf.Abs (snapVelocity * Time.deltaTime);

				if (snapItem.position.y > this.transform.position.y) {
					items.transform.position -= new Vector3 (0,vel, 0);

				} else if (snapItem.position.y < this.transform.position.y) {
					items.transform.position += new Vector3 (0,vel, 0);
				}
				
				float dist = this.transform.position.y - snapItem.position.y;
				if (Mathf.Abs (dist) <= vel) {
					items.transform.position += new Vector3 (0,dist,0);
					
					snapItemIdx = nextSnapItemIdx;
					//nextSnapItemIdx = -1;
					if (OnItemSelected != null) {
						OnItemSelected (snapItem);
					}
				}
			}
		}
	}

	void ScrollContent ()
	{


		itemsBounds.center = items.transform.position + boundsCenterFix;
		
		//Calcula a posiçao da parte inferior (onde termina os items)
		//var itemsBottom = items.transform.position  - (itemsBounds.size / 2);// + boundsCenterFix
			
		if (Direction == UIScrollDirection.Vertical) {

			//Limita os items ao final do scroll
			if (itemsBounds.min.y > box2D.bounds.min.y
				&& itemsBounds.size.y > box2D.size.y - startMargin) {
				var newPos = box2D.bounds.min + (itemsBounds.size / 2) - boundsCenterFix;
				items.transform.position = new Vector3 (items.transform.position.x,
			                                        	newPos.y,
				                                        items.transform.position.z);
			}

			//Limita os items ao início do scroll
			if (itemsBounds.max.y < box2D.bounds.max.y - startMargin) {
				var newPos = box2D.bounds.max - (itemsBounds.size / 2) - boundsCenterFix;
				items.transform.position = new Vector3 (items.transform.position.x,
				                                        newPos.y - startMargin,
				                                        items.transform.position.z);
			}
		} else {
			//Limita os items ao final do scroll (Direita)
			if (itemsBounds.max.x < box2D.bounds.max.y
				&& itemsBounds.size.x > box2D.size.x - startMargin) {
				var newPos = box2D.bounds.max - (itemsBounds.size / 2) - boundsCenterFix;
				items.transform.position = new Vector3 (newPos.x,
				                                        items.transform.position.y,
				                                        items.transform.position.z);

			}

			//Limita os items ao inicio do scroll (Esquerda)
			if (itemsBounds.min.x > box2D.bounds.min.x + startMargin) {
				var newPos = box2D.bounds.min + (itemsBounds.size / 2) - boundsCenterFix;
				items.transform.position = new Vector3 (newPos.x + startMargin,
				                                        items.transform.position.y,
				                                        items.transform.position.z);

			}


		}
	}

	float Easing (float startValue, float time, float duration)
	{
		var c = 1;

		//Linear
		//return c*time/duration + startValue;

		//ExpoOut
		return c * (-Mathf.Pow (2, -10 * time / duration) + 1) + startValue;
	}

	Bounds CalculateBounds (Transform transform)
	{
		Bounds bounds = new Bounds (transform.position, Vector3.zero); 
		
		foreach (Transform child in transform) {
			if (child.gameObject.activeSelf) {
				if (child.gameObject.GetComponent<Renderer>() != null) {
					bounds.Encapsulate (child.gameObject.GetComponent<Renderer>().bounds);   
				} else {
					bounds.Encapsulate (CalculateBounds (child));
				}
			}
		}
		
		return bounds;
	}

	public void SnapToItem (Transform item)
	{
	
		for (int i = 0; i < items.transform.childCount; i++) {

			if (items.transform.GetChild (i) == item) {
				if (Mathf.Abs(dragTotal.x) <= inputDirty || Mathf.Abs(dragTotal.y) <= inputDirty) {
					//Debug.Log ("SnapItem " + i.ToString ());
					nextSnapItemIdx = i;
				}

				break;
			}
		}
	}

	public Transform SelectedItem ()
	{
		Transform item = null;
		if (items.transform.childCount > 0 && (Mathf.Abs(dragTotal.x) < inputDirty || Mathf.Abs(dragTotal.y) < inputDirty)) {
			item = items.transform.GetChild (snapItemIdx);
		}
		return item;
	}
}
