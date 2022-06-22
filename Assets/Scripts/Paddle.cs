using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    public float minRelativePosX = 1f;  // assumes paddle size of 1 relative unit
    
    [SerializeField]
    public float maxRelativePosX = 15f;  // assumes paddle size of 1 relative unit
    
    [SerializeField]
    public float fixedRelativePosY = 10f;  // paddle does not move on the Y directiob
    
    // Unity units of the WIDTH of the screen (e.g. 16)
    [SerializeField]
    public float screenWidthUnits = 16;
    public bool checkTime=false;
    public bool checkTimecol = false;
    float dateTime;
    public bool check2 = false;
    public float speedBall=1;
   
    // Start is called before the first frame update
    void Start()
    {
        float startPosX = ConvertPixelToRelativePosition(screenWidthUnits / 2, Screen.width);
        transform.position = GetUpdatedPaddlePosition(startPosX);
    } 

    // Update is called once per frame
    void Update()
    {
        var relativePosX = ConvertPixelToRelativePosition(pixelPosition: Input.mousePosition.x, Screen.width);
        transform.position = GetUpdatedPaddlePosition(relativePosX);
    }

    public Vector2 GetUpdatedPaddlePosition(float relativePosX)
    {
        // clamps the X position
        float clampedRelativePosX = Mathf.Clamp(relativePosX, minRelativePosX, maxRelativePosX);
        
        Vector2 newPaddlePosition = new Vector2(clampedRelativePosX, fixedRelativePosY);
        return newPaddlePosition;
    }
    
    public float ConvertPixelToRelativePosition(float pixelPosition, int screenWidth)
    { 
        var relativePosition = pixelPosition/screenWidth * screenWidthUnits;
        return relativePosition;
    }
    IEnumerator ScaleX(float seconds)
    {
        if (checkTime == false)
        {
            checkTime = true;
            dateTime = DateTime.Now.Second;
            yield return new WaitForSeconds(seconds);
            if (checkTimecol == true) { } else { gameObject.transform.localScale = new Vector3(1, 1, 1); }
            checkTime = false;
            checkTimecol=false; 
        }
        else
        {
            checkTimecol = true;
            float dateTimeafter = DateTime.Now.Second;
            Debug.Log(dateTimeafter - dateTime);
           
            if (check2 == true) { }
            else
            {
                check2 = true;
                StartCoroutine(ScaleX(seconds + (dateTimeafter - dateTime)));
                              
            }
            
        }
    }

    IEnumerator Slow(float second)
    {
        var gameSession = GameSession.Instance;
        speedBall = speedBall - 0.1f;
        if (speedBall <= 0.8f) { speedBall = 0.8f; }
        gameSession.GameSpeed = 0.8f * speedBall;
        
        yield return new WaitForSeconds(second);
     
        speedBall = speedBall + 0.1f;
        Debug.Log("Comeback" + speedBall);
        gameSession.GameSpeed = 0.8f * speedBall;
        if (speedBall >= 1f) { speedBall = 1f; }

    }
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        var gameSession = GameSession.Instance;
       
        if (other.gameObject.CompareTag("BlueGear"))
        {
            StartCoroutine(Slow(10f));
            Debug.Log("padde" + speedBall);
        
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Gear"))
        {
            gameObject.transform.localScale = new Vector3(2, 1, 1);
            StartCoroutine(ScaleX(10f));
            Destroy(other.gameObject);
            
        }
        if (other.gameObject.CompareTag("EmptyGear"))
        {
            gameSession.GameSpeed = 0.8f;
            gameObject.transform.localScale = new Vector3(1, 1, 1);

            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Heart"))
        {
            if (gameSession.PlayerLives <= 5)
            {
                gameSession.PlayerLives = gameSession.PlayerLives + 1;
            }
          
            Destroy(other.gameObject);
          
        }
    }

}