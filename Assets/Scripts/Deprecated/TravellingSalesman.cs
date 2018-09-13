using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TravellingSalesman : MonoBehaviour {

	//private List<Vector3> currentPath = new List<Vector3> ();
	private List<Path> allPaths = new List<Path>();
	private List<GameObject> allCities = new List<GameObject> ();
	//private Dictionary<int, Vector3> positionCoordinateDict = new Dictionary<int, Vector3>();

	private GameObject citiesHolder;
	private Vector3 startCoordinate = Vector3.zero;
	private Vector3 endCoordinate = Vector3.zero;

	private bool isMouseDown = false;

	public int numberOfCities;
	public int boundaryX;
	public int boundaryY;

	public LineRenderer lineDrawer;


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
			//Debug.Log ("Mouse Down");

			RaycastHit hit;
			Ray mouseToWorldRay = Camera.main.ScreenPointToRay (Input.mousePosition);

			if(Physics.Raycast(mouseToWorldRay, out hit)) {
				//Debug.Log ("City clicked");
				startCoordinate = hit.transform.gameObject.transform.position;
				isMouseDown = true;
			}
		}

		if (isMouseDown) {

		}


		if (Input.GetMouseButtonUp (0) ) {
			//Debug.Log ("Mouse Up");

			RaycastHit hit;
			Ray mouseToWorldRay = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (mouseToWorldRay, out hit) && isMouseDown) {
				//Debug.Log ("City on mouse release");

				if (hit.transform.gameObject.transform.position != startCoordinate) {
					endCoordinate = hit.transform.gameObject.transform.position;

					Path pathHolder = new Path ();
					pathHolder.startCoordinate = startCoordinate;
					pathHolder.endCoordinate = endCoordinate;
					allPaths.Add (pathHolder);

					//currentPath.Add (startCoordinate);
					//positionCoordinateDict.Add (currentPath.Count, startCoordinate);

					//currentPath.Add (endCoordinate);
					//positionCoordinateDict.Add (currentPath.Count, endCoordinate);

					/*foreach (int key in positionCoordinateDict.Keys) {
						Debug.Log (key);
						Debug.Log (positionCoordinateDict [key]);
					}*/
				}
				isMouseDown = false;
				drawLines ();

			} else {
				//Debug.Log ("No city on mouse release");
				isMouseDown = false;
			}
		}

		if (Input.GetMouseButtonDown (1)) {
			Debug.Log ("Right Mouse Down");

			RaycastHit hit;
			Ray mouseToWorldRay = Camera.main.ScreenPointToRay (Input.mousePosition); 

			if (Physics.Raycast (mouseToWorldRay, out hit)) {
				Vector3 coordinateSelected = hit.transform.gameObject.transform.position;

				for (int x = allPaths.Count - 1; x >= 0; x--) {
					if (allPaths [x].startCoordinate == coordinateSelected || allPaths [x].endCoordinate == coordinateSelected) {
						allPaths.RemoveAt(x);
					}
				}

				drawLines ();

			}
		}

		
	}

	public void generateCities() {
		GameObject tempHolder;

		for (int c = 0; c < numberOfCities; c++) {
			int xPos = Random.Range (-boundaryX, boundaryX + 1);
			int yPos = Random.Range (-boundaryY, boundaryY + 1);

			tempHolder = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
			tempHolder.transform.position = new Vector3 (xPos, yPos, 0);
			tempHolder.transform.Rotate (90, 0, 0);
			tempHolder.name = "City " + xPos + " " + yPos;
			tempHolder.transform.parent = citiesHolder.transform;



		}
	}

	public void drawLines() {
		//lineDrawer.numPositions = currentPath.Count;
		//Vector3[] allPointsArray = currentPath.ToArray ();
		List<Vector3> allPoints = new List<Vector3> ();

		for (int x = 0; x < allPaths.Count; x++) {
			allPoints.Add (allPaths[x].startCoordinate);
			allPoints.Add (allPaths[x].endCoordinate);
		}

		Vector3[] allPointsArray = allPoints.ToArray ();

		lineDrawer.numPositions = allPointsArray.Length;
		lineDrawer.SetPositions (allPointsArray);
	}
}
