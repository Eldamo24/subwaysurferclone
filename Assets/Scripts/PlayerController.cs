using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerPosition
{
    Left = -2,
    Middle = 0,
    Right = 2
}

public class PlayerController : MonoBehaviour
{

    private PlayerPosition playerPosition;
    private Transform playerTransform;
    private bool swipeLeft, swipeRight;
    private float newXPosition;

    void Start()
    {
        playerPosition = PlayerPosition.Middle;
        playerTransform = GetComponent<Transform>();    
    }

    
    void Update()
    {
        GetSwipe();
        SetPlayerPosition();
    }

    private void GetSwipe()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.A);
        swipeRight = Input.GetKeyDown(KeyCode.D);
    }

    private void SetPlayerPosition()
    {
        if (swipeLeft)
        {
            if(playerPosition == PlayerPosition.Middle)
            {
                UpdatePlayerXPosition(PlayerPosition.Left);
            }
            else if (playerPosition == PlayerPosition.Right)
            {
                UpdatePlayerXPosition(PlayerPosition.Middle);
            }
        }
        else if (swipeRight)
        {
            if (playerPosition == PlayerPosition.Middle)
            {
                UpdatePlayerXPosition(PlayerPosition.Right);
            }
            else if (playerPosition == PlayerPosition.Left)
            {
                UpdatePlayerXPosition(PlayerPosition.Middle);
            }
        }
        MovePlayer();
    }

    private void UpdatePlayerXPosition(PlayerPosition plPosition)
    {
        newXPosition = (int)plPosition;   
        playerPosition = plPosition;
    }

    private void MovePlayer()
    {
        playerTransform.position = new Vector3(newXPosition, 0, 0);
    }

}
