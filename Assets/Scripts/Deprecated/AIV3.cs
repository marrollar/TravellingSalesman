using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIV3 : MonoBehaviour
{
    public TravellingSalesmanV4 mainScript;

    private List<Vector3> allCities = new List<Vector3>();
    private List<Vector3> citiesNotVisited = new List<Vector3>();
    private List<Vector3> pathToDraw = new List<Vector3>();

    private List<float> distances = new List<float>();

    public void startSearch()
    {
        greedy(0, 1);
        StartCoroutine(twoOptSwap());


        //debugMethod();
        passPathToMain();
    }

    public void debugMethod()
    {

        for (int i = 0; i < pathToDraw.Count; i++)
        {
            Debug.Log("Path To Draw: " + i + " " + pathToDraw[i]);
        }


    }

    // Calculates a path which travels the shortest path from city to city.
    public void greedy(int currentCityIndex, int citiesVisited)
    {

        // If one city remains, add the first city to close the loop. Then exit
        if (citiesNotVisited.Count == 1)
        {
            pathToDraw.Add(citiesNotVisited[0]);
            pathToDraw.Add(pathToDraw[0]);
            return;
        }
        // If all cities are visited, exit
        else if (citiesVisited == allCities.Count)
        {
            return;
        }
        // Otherwise find the city closest to the current city
        else
        {
            int closestCityIndex = -1;
            float smallestDistance = 9999;

            // Remove the current city from the cities not visited list
            citiesNotVisited.Remove(allCities[currentCityIndex]);

            // Calculate distance from current city to all other cities. Find the smallest distance of all of these sets.
            for (int i = 0; i < citiesNotVisited.Count; i++)
            {
                float distance = Vector3.Distance(allCities[currentCityIndex], citiesNotVisited[i]);

                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    closestCityIndex = allCities.IndexOf(citiesNotVisited[i]);
                }
            }

            // Add the current city to the travel route. Add distance between the two cities to list for future calculations for 2-opt.
            pathToDraw.Add(allCities[currentCityIndex]);
            distances.Add(smallestDistance);

            // Recurse with the next city which is the closest city to the current one
            greedy(closestCityIndex, citiesVisited + 1);
        }
    }

    // Randomly swaps two city positions in the route. If the new distance is smaller than the previous one, keep the swap.
    public IEnumerator twoOptSwap()
    {
        float startTime = Time.time;

        while (Time.time - startTime < 20)
        {

            //Debug.Log("Attempting Swap");
            //Debug.Log(Time.time - startTime);
            yield return new WaitForSeconds(0.01f);


            // Backsup old path so if the swap is a failure, code can revert
            List<Vector3> pathToDrawBackup = new List<Vector3>();
            for (int i = 0; i < pathToDraw.Count; i++)
            {
                pathToDrawBackup.Add(pathToDraw[i]);
            }


            // Get both indices to swap
            int swapIndex1 = Random.Range(0, pathToDraw.Count - 1);
            int swapIndex2 = Random.Range(0, pathToDraw.Count - 1);

            // If they end up being the same, get a new index
            while (swapIndex1 == swapIndex2)
            {
                swapIndex2 = Random.Range(1, pathToDraw.Count - 1);
            }


            // Two variables keep track of new and old distance to determine if swap goes through
            float distanceBeforeSwap = mainScript.getDistance();
            float distanceAfterSwap = 0;

            // If the first city in the path is being swapped, it also needs to be appended to the end of the list to complete the path.
            if (swapIndex1 == 0)
            {
                Vector3 tempVector = pathToDraw[swapIndex2];
                pathToDraw[swapIndex2] = pathToDraw[swapIndex1];
                pathToDraw[0] = tempVector;
                pathToDraw[pathToDraw.Count - 1] = tempVector;
            }
            else if (swapIndex2 == 0)
            {
                Vector3 tempVector = pathToDraw[swapIndex1];
                pathToDraw[swapIndex1] = pathToDraw[swapIndex2];
                pathToDraw[0] = tempVector;
                pathToDraw[pathToDraw.Count - 1] = tempVector;
            }
            // Otherwise if two cities in the middle are being swapped, swap normally.
            else
            {
                Vector3 tempVector = pathToDraw[swapIndex1];
                pathToDraw[swapIndex1] = pathToDraw[swapIndex2];
                pathToDraw[swapIndex2] = tempVector;
            }


            // Calculates new total distance
            for (int i = 0; i < pathToDraw.Count - 1; i++)
            {
                distanceAfterSwap += Vector3.Distance(pathToDraw[i], pathToDraw[i + 1]);
            }
           // Debug.Log("Distance After Swap: " + distanceAfterSwap);

            // If the new distance is lower than the old distance, the swap goes through
            if (distanceAfterSwap < distanceBeforeSwap)
            {
                passPathToMain();
            }

            // Otherwise, revert to the backup route
            else
            {
                for (int i = 0; i < pathToDrawBackup.Count; i++)
                {
                    pathToDraw[i] = pathToDrawBackup[i];
                }
            }
        }


        // Recurse
        //
        //
        // IMPLEMENT A LIMIT FOR THIS METHOD TO STOP
        //
        //
       // yield return StartCoroutine(twoOptSwap());

    }

    public void simulatedAnnealing()
    {

    }

    public void passPathToMain()
    {
        //pathToDraw.Reverse();
        mainScript.receiveAIPoints(pathToDraw);
    }

    // Variables in this class need to be instantiated after certain instantiations in the main class
    public void instantiateVariables()
    {
        foreach (GameObject g in mainScript.allCities)
        {
            allCities.Add(g.transform.position);
            citiesNotVisited.Add(g.transform.position);
        }

    }

}
