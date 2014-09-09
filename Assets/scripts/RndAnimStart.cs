using UnityEngine;
using System.Collections;

public class RndAnimStart : MonoBehaviour {

	public float range = 10;
	float rndTime = 0;
	Animator animator;

	// Use this for initialization
	void Start () {
	
		animator = GetComponent<Animator>();
		animator.speed = 0;
		rndTime = Time.time + Random.Range(1,range);
	}
	
	// Update is called once per frame
	void Update () {
	
		if(rndTime != 0 && Time.time > rndTime)
		{
			animator.speed = 1;
			rndTime = 0;
		}
	}
}
