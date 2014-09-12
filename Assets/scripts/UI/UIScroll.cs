using UnityEngine;
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
	public float dragDeltaMax = 30;
	bool pressed = false;
	Vector3 dragDelta;
	Vector3 dragTotal;
	Vector3 mouseLastPosition;
	Bounds itemsBounds;
	Vector3 boundsCenterFix;
	BoxCollider2D box2D;
	int snapItemIdx = 0;
	int nextSnapItemIdx;
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
		updateDragDelta ();

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

//	void OnMouseDown ()
//	{
//		Debug.Log("UIScrollMouseDown");
////		pressed = true;
////		mouseLastPosition = Input.mousePosition;
//	}
//
//	void OnMouseUp ()
//	{
//		pressed = false;
//		dragDelta = Vector3.zero;
//	}

	void updateDragDelta ()
	{
		bool mouseInside = box2D.bounds.IntersectRay (Camera.main.ScreenPointToRay (Input.mousePosition));
		//Debug.Log(mouseInside);

		if (Input.GetMouseButtonDown (0) && mouseInside) {
			//Debug.Log("UIScrollMouseDown");
			pressed = true;
			mouseLastPosition = Input.mousePosition;
			dragDelta = Vector3.zero;
			dragTotal = Vector3.zero;
		} else if (Input.GetMouseButtonUp (0)) {
			pressed = false;
			dragDelta = Vector3.zero;
			dragTotal = Vector3.zero;
		}

		if (pressed) {

			dragDelta = Input.mousePosition - mouseLastPosition;
			dragTotal += dragDelta;

			if (Mathf.Abs (dragDelta.x) > dragDeltaMax) {

				dragDelta = new Vector3 (dragDeltaMax * Mathf.Sign (dragDelta.x), dragDelta.y, dragDelta.z);
				;
			}
			if (Mathf.Abs (dragDelta.y) > dragDeltaMax) {
				dragDelta = new Vector3 (dragDelta.x, dragDeltaMax * Mathf.Sign (dragDelta.y), dragDelta.z);
				;
			}

			//Debug.Log (dragDelta);
			
			mouseLastPosition = Input.mousePosition;
		}
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
				itemPos.y += dragDelta.y * Time.deltaTime;
			} else {
				itemPos.x += dragDelta.x * Time.deltaTime;
			}
			
			items.transform.position = itemPos;
		}
	}

	void SnapContent ()
	{
		//SNAP
		if (items.transform.childCount > 0) {

			if (pressed) {
				float minDist = int.MaxValue;

				for (int i = 0; i < items.transform.childCount; i++) {
					var child = items.transform.GetChild (i);
					float dist = 0;
					if (Direction == UIScrollDirection.Vertical) {
						dist = child.position.y - this.transform.position.y;
					} else {
						dist = child.position.x - this.transform.position.x;
					}

					//Debug.Log(dist);

					//Resolvendo bug da prioridade quando tenta voltar o item inicial ou tenta ir alem do item final
					bool isEdge = false;
					if ((dist > 0 && snapItemIdx == 0) || (dist < 0 && snapItemIdx == items.transform.childCount - 1)) {
						isEdge = true;
					}

					//Priorizando os outros itens (Não precisa arrastar completamente até o outro item)
					if (i != snapItemIdx || isEdge) {
						//Prioridade
						dist = dist * (0.168f * 3);
					}
					dist = Mathf.Abs (dist);
					//Debug.DrawLine (this.transform.position, child.position + new Vector3 (0, i, 0), Color.blue);

					if (dist < minDist) {
						minDist = dist;
						nextSnapItemIdx = i;
						//Debug.Log ("nextsnapIdx " + nextSnapItemIdx);
					}
				}

			}

			if (pressed == false) {

				snapItemIdx = nextSnapItemIdx;
				//Debug.Log (snapItemIdx);
				//Obtendo o item
				var snapItem = items.transform.GetChild (snapItemIdx);

				//Movendo para focar o item
				if (Direction == UIScrollDirection.Horizontal) {
					if (snapItem.position.x > this.transform.position.x) {
						items.transform.position -= new Vector3 (snapVelocity, 0, 0) * Time.deltaTime;
					}
			
					if (snapItem.position.x < this.transform.position.x) {
						items.transform.position += new Vector3 (snapVelocity, 0, 0) * Time.deltaTime;
					}
				} else {
					if (snapItem.position.y > this.transform.position.y) {
						items.transform.position -= new Vector3 (0, snapVelocity, 0) * Time.deltaTime;
					}
					
					if (snapItem.position.y < this.transform.position.y) {
						items.transform.position += new Vector3 (0, snapVelocity, 0) * Time.deltaTime;
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
				if (child.gameObject.renderer != null) {
					bounds.Encapsulate (child.gameObject.renderer.bounds);   
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
				if (dragTotal.x == 0 || dragTotal.y == 0) {
					//Debug.Log ("SnapItem " + i.ToString ());
					nextSnapItemIdx = i;
				}

				break;
			}
		}
	}

	public Transform SelectedItem()
	{
		Transform item = null;
		if(items.transform.childCount > 0){
			item = items.transform.GetChild(snapItemIdx);
		}
		return item;
	}
}
