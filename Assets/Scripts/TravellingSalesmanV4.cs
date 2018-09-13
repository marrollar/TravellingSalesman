using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingSalesmanV4 : MonoBehaviour
{

    public List<Vector3> currentPath = new List<Vector3>();
    public List<GameObject> allCities = new List<GameObject>();
    public List<float> allDistances = new List<float>();
    public LineRenderer line;

    public AIV4 ai;

    private GameObject citiesHolder;

    private List<Vector3> playerPath = new List<Vector3>();
    private List<Vector3> aiPath = new List<Vector3>();
    private Referee referee = new Referee();

    private float currentDistance;
    private bool isPlayerTurn = false;

    public int numCities;
    public int boundaryX;
    public int boundaryY;


    // Use this for initialization
    void Start()
    {
        citiesHolder = new GameObject();
        citiesHolder.name = "All Cities";

        generateCities();



        //// Testing AI section
        ai.instantiateVariables();
        ai.startSearch("hard");

    }

    // Update is called once per frame
    void Update()
    {

        // Check if its player turn or AI turn
        drawLines();



    }

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

    public void organizePointsList()
    {
        if (currentPath.Count > 3)
        {
            for (int i = 0; i < currentPath.Count; i++)
            {
                for (int j = i; j < currentPath.Count; j++)
                {

                    if ((currentPath[i].x == currentPath[j].x) && (currentPath[i].y == currentPath[j].y) && (i != j))
                    {
                        Vector3 movedVector = currentPath[j];
                        Vector3 fillerVector = new Vector3(999, 999, 999);

                        currentPath[j] = fillerVector;
                        currentPath.Insert(i, movedVector);
                        currentPath.Remove(fillerVector);

                    }
                }
            }
        }
    }

    public void drawLines()
    {
        Vector3[] pointsListAsArray = currentPath.ToArray();
        line.numPositions = pointsListAsArray.Length;

        for (int x = 0; x < pointsListAsArray.Length; x++)
        {
            line.SetPosition(x, pointsListAsArray[x]);
        }
    }

    void OnGUI()
    {
        GUI.color = Color.cyan;

        for (int i = 0; i < currentPath.Count - 1; i++)
        {
            float firstCityX = currentPath[i].x;
            float firstCityY = currentPath[i].y;
            float secondCityX = currentPath[i + 1].x;
            float secondCityY = currentPath[i + 1].y;

            // Converts world position to screen position so the text can be positioned in the middle of each path
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(new Vector3((firstCityX + secondCityX) / 2,
                                                                                (firstCityY + secondCityY) / 2,
                                                                                 0));

            // Displays the length of the path in the middle of the path
            GUI.Label(new Rect(screenPosition.x, Camera.main.pixelHeight - screenPosition.y, 100, 20),
                      (int)allDistances[i] + "");
        }

        currentDistance = 0;
        foreach (float i in allDistances)
        {
            currentDistance += i;
        }

        GUI.Label(new Rect(0, 0, 500, 20), "Total Distance Traveled: " + (int)currentDistance);
        GUI.Label(new Rect(0, 20, 500, 20), "Current Temperature: " + ai.getTemperature());

    }

    public bool checkValidCity(Vector3 cityCoords)
    {
        return referee.checkValidCity(cityCoords);
    }

    public void receiveAIPoints(List<Vector3> aiPath)
    {
        currentPath.Clear();
        for (int i = 0; i < aiPath.Count; i++)
        {
            currentPath.Add(aiPath[i]);
        }
        calculateDistances();
    }

    public void calculateDistances()
    {
        allDistances.Clear();
        for (int i = 0; i < currentPath.Count - 1; i++)
        {
            allDistances.Add(Vector3.Distance(currentPath[i], currentPath[i + 1]));
        }

    }

    public float getDistance()
    {
        return currentDistance;
    }

    public string getCurrentPlayer()
    {
        if (isPlayerTurn)
        {
            return "player";

        }
        else
        {
            return "ai";
        }
    }
}
