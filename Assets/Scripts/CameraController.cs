using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject tree;
	private Camera cam;
	private Rigidbody rigidBody;
	private float speed = 0.5f;

	void Start(){
		cam = Camera.main;
		rigidBody = cam.GetComponent<Rigidbody>();
	}

	void Update () {
		Bounds bounds = GetBounds(tree.transform.GetChild(0));
		foreach(Transform child in tree.transform){
			bounds.Encapsulate(GetBounds(child));
		}
		float camDistance = - cam.transform.position.z;
		if(camDistance < 70 &&
			(IsOutOfView(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z))
				|| IsOutOfView(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z)))){
			rigidBody.AddForce(new Vector3(0, 0, - (2+camDistance) * speed));
		}
		Vector3 rotation = cam.transform.eulerAngles;
		float rotX = Mathf.Max (-20, - Mathf.Sqrt ((camDistance+1) / 70f) * 10);
		cam.transform.eulerAngles = new Vector3 (rotX, rotation.y, rotation.z);
	}

	private Bounds GetBounds(Transform t){
		return t.gameObject.GetComponent<MeshRenderer> ().bounds;
	}

	private bool IsOutOfView(Vector3 point){
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
		return !(planes[0].GetSide(point) && planes[1].GetSide(point) && planes[3].GetSide(point));
	}
}
