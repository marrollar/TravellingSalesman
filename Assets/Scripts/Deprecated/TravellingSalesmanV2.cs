using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingSalesmanV2 : MonoBehaviour {

	private List<Path> allPaths = new List<Path>();

	private GameObject citiesHolder;
	private Vector3 startCoordinate = Vector3.zero;
	private Vector3 endCoordiante = Vector3.zero;

	private bool isMouseDown = false;

	public int numCities;
	public int boundaryX;
	public int boundaryY;

	public LineRenderer line;

	private struct Path {
		public Vector3 startCoordinate;
		public Vector3 endCoordinate;
	}

	// Use this for initialization
	void Start () {
		citiesHolder = new GameObject ();
		citiesHolder.name = "All Cities";

		generateCities ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray mouseToWorldRay = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (mouseToWorldRay, out hit)) {
				startCoordinate = hit.transform.gameObject.transform.position;
				isMouseDown = true;
			}
		}
	
		if (Input.GetMouseButtonUp (0)) {
			RaycastHit hit;
			Ray mouseToWorldRay = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (mouseToWorldRay, out hit) && isMouseDown) {

				if (hit.transform.gameObject.transform.position != startCoordinate) {
					endCoordiante = hit.transform.gameObject.transform.position;

					Path pathHolder = new Path ();
					pathHolder.startCoordinate = startCoordinate;
					pathHolder.endCoordinate = endCoordiante;
					allPaths.Add (pathHolder);
				
					for (int x = 0; x < allPaths.Count; x++) {
						Debug.Log (allPaths [x].startCoordinate);
						Debug.Log (allPaths [x].endCoordinate);
					}
				
				}

			}
			isMouseDown = false;
			drawLines ();
		}
	}
		
	public void drawLines() {
		Vector3[] allPathsArray = new Vector3[allPaths.Count * 2];

		if(allPaths.Count > 0) {
			for(int x = 0; x < allPaths.Count; x++) {
				Vector3 startPoint = allPaths[x].startCoordinate;
				Vector3 endPoint = allPaths[x].endCoordinate;

				allPathsArray [x] = startPoint;
				allPathsArray [x + 1] = endPoint;
			}

			line.numPositions = allPathsArray.Length;

			for (int x = 0; x < allPathsArray.Length; x++) {
				line.SetPosition (x, allPathsArray [x]);
			}

		}
	}

	public void generateCities() {
		GameObject tempHolder;

		for (int c = 0; c < numCities; c++) {
			int xPos = Random.Range (-boundaryX, boundaryX + 1);
			int yPos = Random.Range (-boundaryY, boundaryY + 1);

			tempHolder = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			tempHolder.transform.position = new Vector3 (xPos, yPos, 0);
			tempHolder.transform.Rotate (90, 0, 0);
			tempHolder.name = "City " + xPos + " " + yPos;
			tempHolder.transform.parent = citiesHolder.transform;
		}
	}

}
