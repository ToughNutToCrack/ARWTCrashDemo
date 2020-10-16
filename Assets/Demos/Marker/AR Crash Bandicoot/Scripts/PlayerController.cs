using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour{

    const string GROUNDTAG = "Ground";
    const string JUMPANIMATION = "Jump";
    const string RUNANIMATION = "Run";
    const string APPLE = "Apple";

    public bool isAlive = true;
    public List<Transform> spawns;
    public float jumpForce = 300;
    public float swipeSensitivity = 200;
    [Space]
    public GameObject realCrash, angelCrash, eyes;
    public AudioSource sfx;
    public AudioClip jumpClip;
    public Transform initialPos;
    public float jumpDistance;
    public float movementTime = 1;
    public ParticleManager particleM;
    
    [HideInInspector]
    public float movementSpeed = 2.5f;
    [HideInInspector]
    public Animator anim;

    float initialSpeed;
    bool gameStarted = false, isJumping;
    int apples = 0;
    GameObject lastApple, lastTNT, lastTrunk;
    Vector3 lastJumpPos;
    float jumpCDTimer = 0, jumpCD = 0.1f;
    float t;
    Vector2 fingerUp, fingerDown;
    int positionIndex = 1;
    bool firstUpdateFrame = true;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        initialSpeed = movementSpeed;

        initialPos.position = transform.position;
        lastJumpPos = (initialPos.position + transform.up * jumpDistance);
    }

    private void OnEnable()
    {
        if (gameStarted)
            anim.SetBool(RUNANIMATION, true);
    }

    private void Update()
    { 
        if (gameStarted && !firstUpdateFrame){

            movementTime =  movementSpeed / 5;

            if(!isJumping)
            {
                if(Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        fingerUp = touch.position;
                        fingerDown = touch.position;
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        fingerDown = touch.position;
                        checkSwipe();
                    }
                }

                if(Application.isEditor){
                    if(Input.GetKeyUp(KeyCode.A)){
                        moveTotPosition(-1);
                    }
                    if(Input.GetKeyUp(KeyCode.D)){
                        moveTotPosition(+1);
                    }
                    if(Input.GetKeyUp(KeyCode.Space)){
                        if(Time.time > jumpCDTimer){
                            jump();
                        }
                    }
                }
            }

            if (isJumping)
            {
                // transform.position = lastJumpPos;
                lastJumpPos = (initialPos.position + transform.up * jumpDistance);
                t += Time.deltaTime / movementTime;
                transform.position = Vector3.Lerp(initialPos.position, lastJumpPos, Mathf.PingPong(t, 1));
                if( t>= 2){
                    isJumping = false;
                    jumpCDTimer = Time.time + jumpCD;
                    t = 0;
                    transform.position = initialPos.position;
                }
            }
        }
        else if(gameStarted)
        {
            firstUpdateFrame = false;
            print("can play");
        }
    }

    void jump(){
        isJumping = true;
        anim.SetTrigger(JUMPANIMATION);
        sfx.PlayOneShot(jumpClip);
    }

    void checkSwipe(){
        float dist = Mathf.Abs(fingerDown.x - fingerUp.x);

        if(dist > swipeSensitivity){
            if (fingerDown.x - fingerUp.x > 0){
                moveTotPosition(+1);
            }else if (fingerDown.x - fingerUp.x < 0){
                moveTotPosition(-1);
            }
            fingerUp = fingerDown;
        }else{
            if(Time.time > jumpCDTimer){
                jump();
            }
        }
    }

    void moveTotPosition(int val){
        positionIndex += val;

        if(positionIndex < 0 ){
            positionIndex = 0;
        } else if(positionIndex > spawns.Count - 1 ){
            positionIndex = spawns.Count - 1;
        }

        transform.position = spawns[positionIndex].position;
        initialPos.position = spawns[positionIndex].position;
    }

    public void die(int type){
        angelCrash.SetActive(true);
        realCrash.SetActive(false);
        eyes.SetActive(false);
        movementSpeed = 0;
        isAlive = false;
        startGame(false);
        particleM.PlayParticle(type);
        firstUpdateFrame = true;
    }

    public int getApplesScore(){
        return apples;
    }

    public GameObject getApple(){
        return lastApple;
    }

    public void removeApple(){
        lastApple = null;
    }

    public void addTNT(GameObject tnt){
        lastTNT = tnt;
    }

    public GameObject getLastTNT(){
        return lastTNT;
    }
    public void removeTNT()
    {
        lastTNT = null;
    }
    public void removeTrunk(){
        lastTrunk = null;
    }

    public void addTrunk(GameObject trunk)
    {
        lastTrunk = trunk;
    }

    public GameObject getLastTrunk()
    {
        return lastTrunk;
    }

    public void score(){
        apples ++;
    }

    public void reset(){
        anim.SetBool(RUNANIMATION, true);
        apples = 0;
        isAlive = true;
        angelCrash.SetActive(false);
        realCrash.SetActive(true);
        eyes.SetActive(true);
        movementSpeed = initialSpeed;
        jumpCDTimer = Time.time + jumpCD;
    }

    public void startGame(bool isStarted){
        gameStarted = isStarted;
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag(APPLE)){
            score();
            lastApple = other.gameObject;
        }
    }
}
