using UnityEngine;

public class GameSynchronizer : MonoBehaviour
{
    public bool isStarted;
    public GameObject ground;
    public GameObject gameContainer;
    public PlayerController player;
    public float movementTime = 1f;

    float playerSpeedConst = 0.1f;
    bool wantIncrement;
    float t = 0;
    float newSpeed, oldSpeed;
    

    Material groundMat;
    float textureSpeed = 0.1f;
    float desiredTextureSpeed;
    float texturePos;

    void Start(){
        groundMat = ground.GetComponent<MeshRenderer>().sharedMaterial;
        textureSpeed = player.movementSpeed / ground.transform.lossyScale.z;
        desiredTextureSpeed = textureSpeed;
    }

    void LateUpdate()
    {
        if (isStarted && player.isAlive)
        {
            gameContainer.transform.position -= gameContainer.transform.forward * player.movementSpeed * Time.deltaTime;

            if (wantIncrement)
            {
                t += Time.deltaTime / movementTime;
                player.movementSpeed = Mathf.Lerp(oldSpeed, newSpeed, t);
                if (t >= 1)
                {
                    wantIncrement = false;
                    t = 0;
                }
            }

            scrollGroundTexture();          
        }

    }

    void scrollGroundTexture(){

        textureSpeed += (desiredTextureSpeed - textureSpeed) * 0.01f;

        texturePos += textureSpeed * Time.deltaTime;
        Vector2 offset = new Vector2(0, texturePos);
        groundMat.SetTextureOffset("_MainTex", offset);
    }



    public void ActivateSynchronizer(bool wantActive)
    {
        isStarted = wantActive;
    }

    public void IncrementDifficulty()
    {
        newSpeed = player.movementSpeed + playerSpeedConst;
        oldSpeed = player.movementSpeed;
        wantIncrement = true;
        desiredTextureSpeed = newSpeed / ground.transform.lossyScale.z;
    }
}
