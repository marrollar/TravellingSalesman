using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referee {

    public TravellingSalesmanV4 mainGameScript;

    public bool checkValidCity(Vector3 coord)
    {
        int counter = 0;

        // Checks to see if the coordinate appears twice in the cities-already-pathed array
        for (int i = 0; i < mainGameScript.currentPath.Count; i++)
        {
            if ((mainGameScript.currentPath[i].x == coord.x) && (mainGameScript.currentPath[i].y == coord.y))
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
}
