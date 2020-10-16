using UnityEngine;

public class Apple : MonoBehaviour {

    bool isMoving;
    Vector3 targetPosition;
    float speed;

    public void moveToUI(Vector3 targetPosition, float speed){
        this.speed = speed;
        this.targetPosition = targetPosition;
        isMoving = true;
    }

    void Update(){
        if(isMoving){
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
            if(Vector3.Distance(transform.position, targetPosition) < 0.1f){
                Destroy(gameObject);
            }
        }
    }
}