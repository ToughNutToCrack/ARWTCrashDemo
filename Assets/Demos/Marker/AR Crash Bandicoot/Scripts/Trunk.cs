using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : MonoBehaviour
{
    const string COLLISIONTAG = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(COLLISIONTAG))
        {
            var playerC = other.GetComponent<PlayerController>();
            playerC.addTrunk(gameObject);
            playerC.die(1);
            gameObject.SetActive(false);
        }
    }
}
