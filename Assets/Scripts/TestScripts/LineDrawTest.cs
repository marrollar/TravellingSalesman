using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawTest : MonoBehaviour {

	private Vector3[] lineCoordinates;
	public LineRenderer lineDrawer;
	private bool isMouseDown;

	private int totalPositions = 2;

	// Use this for initialization
	void Start () {
		lineCoordinates = new Vector3[2];
		isMouseDown = false;

/*		lineDrawer.SetVertexCount (5);
		lineDrawer.SetPosition(0, new Vector3(-1,1,0));
		lineDrawer.SetPosition (1, new Vector3 (1, 1, 0));
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Mouse Down");
			isMouseDown = true;

			GameObject tempHolder;

			Vector3 startCoordinate = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			startCoordinate.z = 0;
			Debug.Log (startCoordinate);

			lineCoordinates [0] = startCoordinate;
			tempHolder = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			tempHolder.transform.position = startCoordinate;
		}


		if (isMouseDown) {
			lineCoordinates [1] = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			totalPositions = totalPositions + 2;

			lineDrawer.numPositions = 2;
			lineDrawer.SetPosition (0, lineCoordinates[0]);
			lineDrawer.SetPosition (1, lineCoordinates [1]);

			Debug.Log (lineDrawer.numPositions);


		}

		if (Input.GetMouseButtonUp (0)) {
			Debug.Log ("Mouse Up");
			GameObject tempHolder;
			isMouseDown = false;

			Vector3 endCoordinate = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			endCoordinate.z = 0;

			tempHolder = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			tempHolder.transform.position = endCoordinate;

			totalPositions += 2;
			Debug.Log(Vector3.Distance(lineCoordinates[0], endCoordinate));
		}

	}

	/*void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (new Vector3 (-1, 1, 0), 0.1f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (new Vector3 (1, 1, 0), 0.1f);



	}*/
}
