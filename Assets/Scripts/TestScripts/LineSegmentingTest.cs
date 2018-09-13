using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegmentingTest : MonoBehaviour {

    public LineRenderer line;

	// Use this for initialization
	void Start () {
        line.numPositions = 3;
        line.SetPosition(0, new Vector3(0, 0, 0));
        line.SetPosition(1, new Vector3(1, 3, 0));
        line.SetPosition(2, new Vector3(-4, -3, 0));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
