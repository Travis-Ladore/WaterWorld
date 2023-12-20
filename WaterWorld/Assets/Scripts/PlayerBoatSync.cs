using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoatSync : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Make the player a child of the boat
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Remove the player as a child of the boat when they exit the collider
            other.transform.parent = null;
        }
    }
}
