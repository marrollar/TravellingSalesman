using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIV2 : MonoBehaviour
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

    public void greedy(int currentCityIndex, int citiesVisited)
    {

        // If one city remains, add the first city to close the loop. Then exit
        if (citiesNotVisited.Count == 1)
        {
            //pathToDraw.Add(pathToDraw[pathToDraw.Count - 1]);
            //pathToDraw.Add(pathToDraw[0]);

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

            citiesNotVisited.Remove(allCities[currentCityIndex]);

            for (int i = 0; i < citiesNotVisited.Count; i++)
            {
                float distance = Vector3.Distance(allCities[currentCityIndex], citiesNotVisited[i]);

                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    closestCityIndex = allCities.IndexOf(citiesNotVisited[i]);
                }
            }

            pathToDraw.Add(allCities[currentCityIndex]);
           // pathToDraw.Add(allCities[closestCityIndex]);
            distances.Add(smallestDistance);

            greedy(closestCityIndex, citiesVisited + 1);
        }
    }

    public IEnumerator twoOptSwap()
    {
        Debug.Log("Attempting Swap");
        yield return new WaitForSeconds(.1f);

        // Backsup old path so if the swap is a failure, code can revert
        List<Vector3> pathToDrawBackup = new List<Vector3>();
        for (int i = 0; i < pathToDraw.Count; i++)
        {
            pathToDrawBackup.Add(pathToDraw[i]);
        }

        int swapIndex1 = Random.Range(1, pathToDraw.Count - 1);
        int swapIndex2 = Random.Range(1, pathToDraw.Count - 1);

        // Obtain the two points we want to swap randomly
        //int swapIndex1 = Random.Range(0, pathToDraw.Count);
        //int swapIndex2 = Random.Range(0, pathToDraw.Count);

        // If they end up being the same or they refer to the first and last index, keep randoming until they aren't
        while (swapIndex1 == swapIndex2 /* || (swapIndex1 + pathToDraw.Count - 1 == swapIndex2) || (swapIndex2 + pathToDraw.Count - 1 == swapIndex1) */)
        {
            swapIndex2 = Random.Range(1, pathToDraw.Count - 1);
        }

        

        // If we select the first or second index as one of the points to swap
        //if ((swapIndex1 == 0 || swapIndex1 == pathToDraw.Count - 1) ||
        //    swapIndex2 == 0 || swapIndex2 == pathToDraw.Count - 1)
        //{
        //    // If it is the first index that refers to the first or last point
        //    if ((swapIndex1 == 0 || swapIndex1 == pathToDraw.Count - 1) ? true : false)
        //    {
        //    }
        //    // If it is the second index that refers to the first or last point
        //    else if ((swapIndex2 == 0 || swapIndex2 == pathToDraw.Count - 1) ? true : false)
        //    {
        //    }
        //}
        // Records the total distance of the old path to compare to the new path's distance
        float distanceBeforeSwap = mainScript.getDistance();

        // Obtains the two vectors which we are swapping
        //Vector3 swapVector1 = pathToDraw[swapIndex1];
        //Vector3 swapVector2 = pathToDraw[swapIndex2];

        // Creates a VectorPair object which has a method that can compare pairs of vectors. 

        //int swapVectorIndex1 = pathToDraw.IndexOf(swapVector1);
        //int swapVectorIndex2 = pathToDraw.IndexOf(swapVector2);

        //Vector3 tempVector1 = pathToDraw[swapVectorIndex1];
        //Vector3 tempVector2 = pathToDraw[swapVectorIndex1 + 1];

        float distanceAfterSwap = 0;

        //pathToDraw[swapVectorIndex1] = pathToDraw[swapVectorIndex2];
        //pathToDraw[swapVectorIndex1 + 1] = pathToDraw[swapVectorIndex2 + 1];

        //pathToDraw[swapVectorIndex2] = tempVector1;
        //pathToDraw[swapVectorIndex2 + 1] = tempVector2;

        Vector3 tempVector = pathToDraw[swapIndex1];
        pathToDraw[swapIndex1] = pathToDraw[swapIndex2];
        pathToDraw[swapIndex2] = tempVector;

        for (int i = 0; i < pathToDraw.Count - 1; i++)
        {
            distanceAfterSwap += Vector3.Distance(pathToDraw[i], pathToDraw[i + 1]);
        }

        Debug.Log("Distance After Swap: " + distanceAfterSwap);

        if (distanceAfterSwap < distanceBeforeSwap)
        {
            //Debug.Log("Swapping: " + swapVector1 + " with " + swapVector2);

            passPathToMain();
        }
        else
        {
            for (int i = 0; i < pathToDrawBackup.Count; i++)
            {
                pathToDraw[i] = pathToDrawBackup[i];
            }
        }
        yield return StartCoroutine(twoOptSwap());

    }

    /////////////////////////// DEBUG STATEMENTS //////////////////////////////////////
    //Debug.Log("===============================");
    //Debug.Log("Distance Before Swap: " + distanceBeforeSwap);
    //Debug.Log("Swap Vector 1: " + swapVector1);
    //Debug.Log("Swap Vector 2: " + swapVector2);
    /////////////////////////// DEBUG STATEMENTS //////////////////////////////////////

    public void simulatedAnnealing()
    {

    }

    public void passPathToMain()
    {
        pathToDraw.Reverse();

        
        

        mainScript.receiveAIPoints(pathToDraw);
    }

    public void instantiateVariables()
    {
        foreach (GameObject g in mainScript.allCities)
        {
            allCities.Add(g.transform.position);
            citiesNotVisited.Add(g.transform.position);
        }

    }

}
