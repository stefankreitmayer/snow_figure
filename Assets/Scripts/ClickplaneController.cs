using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickplaneController : MonoBehaviour {
	public GameObject tree;
	public GameObject nodePrefab;
	private GameObject offspring;

	void OnMouseDown(){
		SpawnOffspring ();
	}   

	void Update(){
		if (Input.GetMouseButton (0)) {
			offspring.transform.localScale *= 1 + Time.deltaTime;
		}
	}

	private void SpawnOffspring(){
		Camera camera = Camera.main;
		Vector2 mousePos = Input.mousePosition;
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)){
			Vector3 pos = FindNearestSilhouettePoint (hit.point);
			offspring = Instantiate (nodePrefab);
			offspring.transform.position = new Vector3(pos.x, pos.y, 0);
			offspring.transform.localScale = Vector3.one * .25f;
			offspring.transform.parent = tree.transform;
		}
	}

	private Vector3 FindNearestSilhouettePoint(Vector3 userPoint){
		Vector3 winner = Vector3.zero;
		foreach (Transform child in tree.transform) {
			Vector3 candidate = FindSilhouettePoint (child, userPoint);
			if (Vector3.Distance (candidate, userPoint) < Vector3.Distance (winner, userPoint)) {
				winner = candidate;
			}
		}
		return winner;
	}

	private Vector3 FindSilhouettePoint(Transform transform, Vector3 userPoint){
		Vector3 center = transform.position;
		float dx = userPoint.x - center.x;
		float dy = userPoint.y - center.y;
		float angle = Mathf.Atan2(dy, dx);
		float radius = transform.localScale.x / 2;
		float x = center.x + Mathf.Cos (angle) * radius;
		float y = center.y + Mathf.Sin (angle) * radius;
		return new Vector3 (x, y, 0);
	}
}
