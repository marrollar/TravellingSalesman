using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public TravellingSalesmanV4 mgs;
    private Vector3 startCoordinate = Vector3.zero;
    private Vector3 endCoordinate = Vector3.zero;

    private bool isMouseDown;

    // Use this for initialization
    void Start()
    {
        isMouseDown = false;

    }

    public void startPathing(int seconds)
    {

        float currentTime = Time.time;
        while (Time.time - currentTime < seconds)
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
                    if (mgs.checkValidCity(startCoordinate))
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

                    // If the second point selected is both valid and not the same point as the first
                    if (mgs.checkValidCity(endCoordinate))
                    {
                        if (!((startCoordinate.x == endCoordinate.x) && (startCoordinate.y == endCoordinate.y)))
                        {
                            // Add both points to the points list
                            mgs.currentPath.Add(startCoordinate);
                            mgs.currentPath.Add(endCoordinate);
                        }
                    }
                }
                isMouseDown = false;
                mgs.organizePointsList();
                mgs.drawLines();
            }

            // If the right mouse button is pressed down
            if (Input.GetMouseButtonUp(1))
            {
                RaycastHit hit;
                Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(mouseToWorldRay, out hit))
                {
                    Vector3 cityToClear = hit.transform.gameObject.transform.position;

                    // If there are only two points in the list, clear it all
                    if (mgs.currentPath.Count <= 2)
                    {
                        mgs.currentPath.Clear();
                    }
                    // Otherwise, remove the range of indices from the point selected and onwards
                    else
                    {
                        for (int i = 0; i < mgs.currentPath.Count; i++)
                        {
                            if (mgs.currentPath[i].x == cityToClear.x && mgs.currentPath[i].y == cityToClear.y)
                            {
                                mgs.currentPath.RemoveRange(i + 1, mgs.currentPath.Count - i - 1);
                            }
                        }
                    }


                }
                mgs.organizePointsList();
                mgs.drawLines();
            }
        }
    }


}

