using UnityEngine;

public class AppleSpawner : MonoBehaviour
{
    public GameObject apple;
    public Camera mainCamera;
    public float speed = 0.1f;
    Vector3 worldPos;
    
    private void Start()
    {
        var screenPoint = new Vector3(30, 30, 1);
        worldPos = mainCamera.ScreenToWorldPoint(screenPoint);
    }

    public void SpawnGrabbedApple(Vector3 position)
    {
        var a = Instantiate(apple, position, apple.transform.rotation).GetComponent<Apple>();
        a.moveToUI(worldPos, speed);
    }
}
