using System;
using System.Collections;
using UnityEngine;

public class SplashScreen : MonoBehaviour{
    public int desiredFPS = 60;
    public float timeOut = 10;
    public int minGoodFrames = 20;
    public float animationSpeed = 10;

    public GameObject splashImage;
    public GameObject gameHandler;
    public GameObject helperUI;

    RectTransform rectTransform;

    void Start(){
        rectTransform = splashImage.GetComponent<RectTransform>();

        gameHandler.SetActive(false);
        splashImage.SetActive(true);
        helperUI.SetActive(false);


        waitUntilStable(
            () => { 
                gameHandler.SetActive(true);
                helperUI.SetActive(true);
                reduceSplashImage();
            }
        );
    }
    
    void waitUntilStable(Action a){
        StartCoroutine(waitUntilStableCoroutine(a));
    }
 
    IEnumerator waitUntilStableCoroutine(Action action){
        float startTime = Time.time;
        float targetFrameTime = 1 / (desiredFPS * 0.75f);
 
        int goodFrames = 0;
        float elapsedTime = Time.time - startTime;

        while ((elapsedTime < timeOut) && (goodFrames < minGoodFrames)){
            yield return null;
 
            if (Time.deltaTime <= targetFrameTime){
                ++goodFrames;
            } else {
                goodFrames = 0;
            }

            elapsedTime = Time.time - startTime;
        }
   
        action();
    }

    void reduceSplashImage(){
        StartCoroutine(reduceCoroutine());
    }

    IEnumerator reduceCoroutine(){
        Vector2 dim = rectTransform.sizeDelta;

        while (dim.y > 0 ){
            dim.y -= 1 * animationSpeed * Time.deltaTime;
            rectTransform.sizeDelta = dim;
            yield return null;
        }

        splashImage.SetActive(false); 
    }
}
