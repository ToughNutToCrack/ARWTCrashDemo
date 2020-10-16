using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour{
    public bool isStarted = false;
    public PlayerController player;
    public GameObject gameContainer;
    public GameObject startButton;
    [Space]
    public Text score, finalMessage, tapToJump;
    public AudioSource sfx;
    public AudioClip apple;
    [Space]
    public List<Transform> spawnPos;
    public int poolDimension = 8;
    public GameObject appleGO, tntGO, trunkGO;
    public List<GameObject> envGOList;
    public List<Transform> envSpawns;
    public AppleSpawner appleSpawner;
    [Space]
    public GameSynchronizer gameS;

    Queue<GameObject> appleQ, tntQ, envQ, trunkQ;

    int previusPoints;
    int points;

    float timer = 0, timeOffset = 2, envTimer = 0;

    private void Start()
    {
        appleQ = new Queue<GameObject>();
        tntQ = new Queue<GameObject>();
        envQ = new Queue<GameObject>();
        trunkQ = new Queue<GameObject>();
        for (int i = 0; i < poolDimension; i++){
            GameObject appleObj = Instantiate(appleGO, spawnPos[0].position, spawnPos[0].rotation);
            GameObject tntObj = Instantiate(tntGO, spawnPos[0].position, spawnPos[0].rotation);
            GameObject trunkObj = Instantiate(trunkGO, spawnPos[0].position, spawnPos[0].rotation);
            GameObject envObj = Instantiate(envGOList[Random.Range(0, envGOList.Count)], spawnPos[0].position, spawnPos[0].rotation);
            appleObj.transform.SetParent(gameContainer.transform);
            tntObj.transform.SetParent(gameContainer.transform);
            trunkObj.transform.SetParent(gameContainer.transform);
            envObj.transform.SetParent(gameContainer.transform);
            AddAppleToQueue(appleObj);
            AddTNTToQueue(tntObj);
            AddTrunkToQueue(trunkObj);
            AddEnvToQueue(envObj);
        }
    }

    public void AddAppleToQueue(GameObject obj)
    {
        obj.SetActive(false);
        appleQ.Enqueue(obj);
    }

    public void AddTNTToQueue(GameObject obj)
    {
        obj.SetActive(false);
        tntQ.Enqueue(obj);
    }

    public void AddEnvToQueue(GameObject obj)
    {
        obj.SetActive(false);
        envQ.Enqueue(obj);
    }

    public void AddTrunkToQueue(GameObject obj)
    {
        obj.SetActive(false);
        trunkQ.Enqueue(obj);
    }

    private void Update() {
        if (isStarted){
            if(player.isAlive){
                scorePoint();

                var apple = player.getApple();
                if (apple != null){
                    AddAppleToQueue(apple);
                    player.removeApple();
                    appleSpawner.SpawnGrabbedApple(player.transform.position);
                }

                var tnt = player.getLastTNT();
                if (tnt != null){
                    AddTNTToQueue(tnt);
                    player.removeTNT();
                }

                var trunk = player.getLastTrunk();
                if (trunk != null)
                {
                    AddTrunkToQueue(trunk);
                    player.removeTrunk();
                }

                if (Time.time > timer){
                    timer = Time.time + timeOffset;
                    int rnd = Random.Range(0, 3);
                    int rndPos = Random.Range(0, spawnPos.Count);
                    switch(rnd){
                        case 0:
                            var a = appleQ.Dequeue();
                            a.transform.position = spawnPos[rndPos].position;
                            a.SetActive(true);
                            break;
                        case 1:
                            var t = tntQ.Dequeue();
                            t.transform.position = spawnPos[rndPos].position;
                            t.SetActive(true);
                            break;
                        case 2:
                            var q = trunkQ.Dequeue();
                            q.transform.position = spawnPos[0].position;
                            q.SetActive(true);
                            break;

                    }
                }

                if (Time.time > envTimer)
                {
                    envTimer = Time.time + Random.Range(1f, 4f);
                    var e = envQ.Dequeue();
                    e.transform.position = envSpawns[Random.Range(0, envSpawns.Count)].position;
                    e.SetActive(true);
                }

            }
            if(!player.isAlive){
                finishGame();
            }
        }
    }

    void finishGame(){
        finalMessage.gameObject.SetActive(true);
        finalMessage.text = "Score: " + points;
        isStarted = false;
        startButton.SetActive(true);
        player.startGame(false);
        previusPoints = 0;
        tapToJump.gameObject.SetActive(false);
    }

    void scorePoint(){
        points = player.getApplesScore();
        score.text = points.ToString();
        if(previusPoints < points){
            sfx.PlayOneShot(apple);
            previusPoints = points;
            gameS.IncrementDifficulty();
        }
    }

    public void start(){
        player.startGame(true);
        gameS.ActivateSynchronizer(true);
        isStarted = true;
        timer = Time.time + timeOffset;
        player.reset();
        startButton.SetActive(false);
        tapToJump.gameObject.SetActive(true);
        finalMessage.gameObject.SetActive(false);
    }
}
