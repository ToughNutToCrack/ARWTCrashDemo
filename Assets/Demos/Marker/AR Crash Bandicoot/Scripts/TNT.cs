using UnityEngine;

public class TNT : MonoBehaviour
{
    const string COLLISIONTAG = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(COLLISIONTAG))
        {
            var playerC = other.GetComponent<PlayerController>();
            playerC.addTNT(gameObject);
            playerC.die(0);
            gameObject.SetActive(false);
        }
    }
}
