using UnityEngine;

public class Recycler : MonoBehaviour
{
    public GameController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Apple"))
            controller.AddAppleToQueue(other.gameObject);

        if (other.CompareTag("TNT"))
            controller.AddTNTToQueue(other.gameObject);

        if (other.CompareTag("Trunk"))
            controller.AddTrunkToQueue(other.gameObject);

        if (other.CompareTag("Environment"))
            controller.AddEnvToQueue(other.gameObject);

    }
}
