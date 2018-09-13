using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingSalesmanV3 : MonoBehaviour
{

    private List<Vector3> allPoints = new List<Vector3>();
    private List<GameObject> allCities = new List<GameObject>();
    private List<float> allDistances = new List<float>();

    private GameObject citiesHolder;
    private Vector3 startCoordinate = Vector3.zero;
    private Vector3 endCoordinate = Vector3.zero;

    private bool isMouseDown = false;

    public int numCities;
    public int boundaryX;
    public int boundaryY;

    public LineRenderer line;

    // Use this for initialization
    void Start()
    {
        citiesHolder = new GameObject();
        citiesHolder.name = "All Cities";

        generateCities();
    }

    // Update is called once per frame
    void Update()
    {

        // If the left mouse button is pushed
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check if it interacts with a city
            if (Physics.Raycast(mouseToWorldRay, out hit))
            {
                startCoordinate = hit.transform.gameObject.transform.position;

                // See if the city position can be used
                if (checkValidCity(startCoordinate))
                {
                    isMouseDown = true;
                }
            }
        }

        // If left mouse button is released. Code segment practically the same as above
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseToWorldRay, out hit) && isMouseDown)
            {
                endCoordinate = hit.transform.gameObject.transform.position;

                if (checkValidCity(endCoordinate))
                {
                    if (!((startCoordinate.x == endCoordinate.x) && (startCoordinate.y == endCoordinate.y)))
                    {
                        allPoints.Add(startCoordinate);
                        allPoints.Add(endCoordinate);
                    }
                }
            }
            isMouseDown = false;
            organizePointsList();
            drawLines();
        }

        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit hit;
            Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mouseToWorldRay, out hit))
            {
                Vector3 cityToClear = hit.transform.gameObject.transform.position;

                if (allPoints.Count <= 2)
                {
                    allPoints.Clear();
                }
                else
                {
                    for (int i = 0; i < allPoints.Count; i++)
                    {
                        if (allPoints[i].x == cityToClear.x && allPoints[i].y == cityToClear.y)
                        {
                            allPoints.RemoveRange(i + 1, allPoints.Count - i - 1);
                        }
                    }
                }


            }
            organizePointsList();
            drawLines();
        }
    }

    public void drawLines()
    {
        Vector3[] pointsListAsArray = allPoints.ToArray();
        line.numPositions = pointsListAsArray.Length;

        for (int x = 0; x < pointsListAsArray.Length; x++)
        {
            line.SetPosition(x, pointsListAsArray[x]);
        }
    }

    public void organizePointsList()
    {
        if (allPoints.Count > 3)
        {
            for (int i = 0; i < allPoints.Count; i++)
            {
                for (int j = i; j < allPoints.Count; j++)
                {

                    if ((allPoints[i].x == allPoints[j].x) && (allPoints[i].y == allPoints[j].y) && (i != j))
                    {
                        Vector3 movedVector = allPoints[j];
                        Vector3 fillerVector = new Vector3(999, 999, 999);

                        allPoints[j] = fillerVector;
                        allPoints.Insert(i, movedVector);
                        allPoints.Remove(fillerVector);

                    }
                }
            }
        }

        allDistances.Clear();
        for (int i = 0; i < allPoints.Count - 1; i++)
        {
            allDistances.Add(Vector3.Distance(allPoints[i], allPoints[i+1]));
        }


        Debug.Log("--------------------------");
        for (int x = 0; x < allPoints.Count; x++)
        {
            Debug.Log(x + " " + allPoints[x]);
        }


    }

    public bool checkValidCity(Vector3 coord)
    {
        int counter = 0;

        // Checks to see if the coordinate appears twice in the cities-already-pathed array
        for (int i = 0; i < allPoints.Count; i++)
        {
            if ((allPoints[i].x == coord.x) && (allPoints[i].y == coord.y))
            {
                counter++;
            }
        }

        // If there are 2 or more occurrences of the city, it is no longer valid (one path going in, one path going out)
        if (counter >= 2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //// !!!!!!!!!!!!!!!!
    //// TRY TO DEPRECATE
    //// !!!!!!!!!!!!!!!!
    //public bool checkValidCity(Vector3 startCoord, Vector3 endCoord)
    //{
    //    if ((startCoord.x == endCoord.x) && (startCoord.y == endCoord.y))
    //    {
    //        return false;
    //    }

    //    int counter = 0;

    //    for (int i = 0; i < currentPath.Count; i++)
    //    {
    //        if ((currentPath[i].x == endCoord.x) && (currentPath[i].y == endCoord.y))
    //        {
    //            counter++;
    //        }
    //    }

    //    if (counter >= 2)
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}


    // Generates cities randomly in range specified in inspector. Positions are clamped to integer locations
    public void generateCities()
    {
        GameObject tempHolder;

        for (int c = 0; c < numCities; c++)
        {
            int xPos = Random.Range(-boundaryX, boundaryX + 1);
            int yPos = Random.Range(-boundaryY, boundaryY + 1);

            tempHolder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tempHolder.transform.position = new Vector3(xPos, yPos, 0);
            tempHolder.transform.Rotate(90, 0, 0);
            tempHolder.name = "City " + xPos + " " + yPos;
            tempHolder.transform.parent = citiesHolder.transform;

            allCities.Add(tempHolder);
        }
    }

    void OnGUI()
    {
        GUI.color = Color.cyan;

        for (int i = 0; i < allPoints.Count - 1; i += 2)
        {
            float firstCityX = allPoints[i].x;
            float firstCityY = allPoints[i].y;
            float secondCityX = allPoints[i + 1].x;
            float secondCityY = allPoints[i + 1].y;

            // Converts world position to screen position so the text can be positioned in the middle of each path
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(new Vector3((firstCityX + secondCityX) / 2,
                                                                                (firstCityY + secondCityY) / 2,
                                                                                 0));

            // Displays the length of the path in the middle of the path
            GUI.Label(new Rect(screenPosition.x, Camera.main.pixelHeight - screenPosition.y, 100, 20),
                       Vector3.Distance(allPoints[i], allPoints[i + 1]).ToString());
        }

        float totalDistance = 0;
        foreach (float i in allDistances)
        {
            totalDistance += i;
        }

        GUI.Label(new Rect(0, 0, 500, 20), "Total Distance Traveled: " + totalDistance);

    }
}
