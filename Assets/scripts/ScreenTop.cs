using UnityEngine;
using System.Collections;

public class ScreenTop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
	void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log ("Colision ");
        foreach (ContactPoint2D contact in collision.contacts) {
			Debug.Log(contact.point);
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }
}
