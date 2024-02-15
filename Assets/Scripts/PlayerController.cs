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
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    private float newXPosition;
    private Animator playerAnimator;
    private int IDDodgeLeft = Animator.StringToHash("DodgeLeft");
    private int IDDodgeRight = Animator.StringToHash("DodgeRight");
    private int IDJump = Animator.StringToHash("Jump");
    private int IDFall = Animator.StringToHash("Fall");
    private int IDLanding = Animator.StringToHash("Landing");
    private float xPosition;
    [SerializeField] private float dodgeSpeed;
    private CharacterController characterController;
    private Vector3 motionVector;
    [SerializeField] private float jumpPower;
    private float yPosition;

    void Start()
    {
        playerPosition = PlayerPosition.Middle;
        playerTransform = GetComponent<Transform>();
        playerAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        yPosition = -7;
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
        swipeUp = Input.GetKeyDown(KeyCode.Space);
    }

    private void SetPlayerPosition()
    {
        if (swipeLeft)
        {
            if(playerPosition == PlayerPosition.Middle)
            {
                UpdatePlayerXPosition(PlayerPosition.Left);
                SetPlayerAnimator(IDDodgeLeft, false);
            }
            else if (playerPosition == PlayerPosition.Right)
            {
                UpdatePlayerXPosition(PlayerPosition.Middle);
                SetPlayerAnimator(IDDodgeLeft, false);
            }
        }
        else if (swipeRight)
        {
            if (playerPosition == PlayerPosition.Middle)
            {
                UpdatePlayerXPosition(PlayerPosition.Right);
                SetPlayerAnimator(IDDodgeRight, false);
            }
            else if (playerPosition == PlayerPosition.Left)
            {
                UpdatePlayerXPosition(PlayerPosition.Middle);
                SetPlayerAnimator(IDDodgeRight, false);
            }
        }
        MovePlayer();
        Jump();
    }

    private void UpdatePlayerXPosition(PlayerPosition plPosition)
    {
        newXPosition = (int)plPosition;   
        playerPosition = plPosition;
    }

    private void SetPlayerAnimator(int id, bool isCrossFade, float fadeTime = 0.1f)
    {
        if(isCrossFade)
        {
            playerAnimator.CrossFadeInFixedTime(id, fadeTime);
        }
        else
        {
            playerAnimator.Play(id);
        }
    }

    private void MovePlayer()
    {
        motionVector = new Vector3(xPosition - playerTransform.position.x, yPosition * Time.deltaTime, 0);
        xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * dodgeSpeed);
        characterController.Move(motionVector);
    }

    private void Jump()
    {
        if (characterController.isGrounded)
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                SetPlayerAnimator(IDLanding, false);
            if (swipeUp)
            {
                yPosition = jumpPower;
                SetPlayerAnimator(IDJump, true);
            }
        }
        else
        {
            yPosition -= jumpPower * 2 * Time.deltaTime;
            SetPlayerAnimator(IDFall, false);

        }
    }

}
