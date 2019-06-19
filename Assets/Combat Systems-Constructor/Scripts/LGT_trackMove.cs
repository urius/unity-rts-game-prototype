using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LGT_trackMove : MonoBehaviour {
	private Animator animator;
	public float scrollSpeed=30;	
	private Renderer rend;
	private float offset;

	// Use this for initialization
	void Start () {
	 rend=GetComponent<Renderer> ();
		animator = transform.root.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("MoveForward") || 
			animator.GetCurrentAnimatorStateInfo(0).IsName("MoveBack") ||
			animator.GetCurrentAnimatorStateInfo(0).IsName("MoveLeft") ||
			animator.GetCurrentAnimatorStateInfo(0).IsName("MoveRight")
		)
 {

			if(animator.GetCurrentAnimatorStateInfo(0).IsName("MoveBack"))	offset +=  scrollSpeed/1000; 
			else offset -=  scrollSpeed/1000; 
			rend.material.mainTextureOffset = new Vector2 (0, offset);
		}
	}
}
