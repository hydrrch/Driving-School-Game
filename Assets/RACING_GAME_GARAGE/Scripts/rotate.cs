using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	public Vector3 axis;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

		transform.Rotate( axis * Time.deltaTime );

	}
}
