using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Side
{
    Left = -2,
    Middle = 0,
    Right = 2
}

public class PlayerController : MonoBehaviour
{
    private Transform myTransform;
    private CharacterController _myCharacterController;
    public CharacterController MyCharacterController { get => _myCharacterController; set => _myCharacterController = value; }
    private Animator myAnimator;
    private Vector3 motionVector;
    [Header ("Player Controller")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float jumpPower;
    private Side position;
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    [Header("Player States")]
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isRolling;
    [SerializeField] private bool isGrounded; 

    private float newXPosition;
    private float rollTimer;
    private float xPosition;
    private float yPosition;
    private int IDDodgeLeft = Animator.StringToHash("DodgeLeft");
    private int IDDodgeRight = Animator.StringToHash("DodgeRight");
    private int IDJump = Animator.StringToHash("Jump");
    private int IDFall = Animator.StringToHash("Fall");
    private int IDLanding = Animator.StringToHash("Landing");
    private int IDRoll = Animator.StringToHash("Roll");


    void Start()
    {
        position = Side.Middle;
        myTransform = GetComponent<Transform>();
        myAnimator = GetComponent<Animator>();
        _myCharacterController = GetComponent<CharacterController>();
        yPosition = -7;
    }

    
    void Update()
    {
        GetSwipe();
        SetPlayerPosition();
        MovePlayer();
        Jump();
        Roll();
        isGrounded = _myCharacterController.isGrounded;
    }

    private void GetSwipe()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.A);
        swipeRight = Input.GetKeyDown(KeyCode.D);
        swipeUp = Input.GetKeyDown(KeyCode.Space);
        swipeDown = Input.GetKeyDown(KeyCode.S);
    }

    private void SetPlayerPosition()
    {
        if (swipeLeft && !isRolling)
        {
            if(position == Side.Middle)
            {
                UpdatePlayerXPosition(Side.Left);
                SetPlayerAnimator(IDDodgeLeft, false);
            }
            else if (position == Side.Right)
            {
                UpdatePlayerXPosition(Side.Middle);
                SetPlayerAnimator(IDDodgeLeft, false);
            }
        }
        else if (swipeRight && !isRolling)
        {
            if (position == Side.Middle)
            {
                UpdatePlayerXPosition(Side.Right);
                SetPlayerAnimator(IDDodgeRight, false);
            }
            else if (position == Side.Left)
            {
                UpdatePlayerXPosition(Side.Middle);
                SetPlayerAnimator(IDDodgeRight, false);
            }
        }
       
    }

    private void UpdatePlayerXPosition(Side plPosition)
    {
        newXPosition = (int)plPosition;   
        position = plPosition;
    }

    private void SetPlayerAnimator(int id, bool isCrossFade, float fadeTime = 0.1f)
    {
        if(isCrossFade)
        {
            myAnimator.CrossFadeInFixedTime(id, fadeTime);
        }
        else
        {
            myAnimator.Play(id);
        }
    }

    private void MovePlayer()
    {
        motionVector = new Vector3(xPosition - myTransform.position.x, yPosition * Time.deltaTime, forwardSpeed * Time.deltaTime);
        xPosition = Mathf.Lerp(xPosition, newXPosition, Time.deltaTime * dodgeSpeed);
        _myCharacterController.Move(motionVector);
    }

    private void Jump()
    {
        if (_myCharacterController.isGrounded)
        {
            isJumping = false;
            if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
                SetPlayerAnimator(IDLanding, false);
            if (swipeUp && !isRolling)
            {
                isJumping = true;
                yPosition = jumpPower;
                SetPlayerAnimator(IDJump, true);
            }
        }
        else
        {
            yPosition -= jumpPower * 2 * Time.deltaTime;
            if(_myCharacterController.velocity.y <=0)
                SetPlayerAnimator(IDFall, false);

        }
    }


    private void Roll()
    {
        rollTimer -= Time.deltaTime;
        if(rollTimer < 0)
        {
            isRolling = false;
            rollTimer = 0;
            //Character controller tamaño normal
            _myCharacterController.center = new Vector3(0, .45f, 0);
            _myCharacterController.height = .9f;
        }
        if (swipeDown &&  !isJumping)
        {
            isRolling = true;
            rollTimer = .5f;
            SetPlayerAnimator(IDRoll, true);
            //Achicar character controller
            _myCharacterController.center = new Vector3(0, .2f, 0);
            _myCharacterController.height = .4f;
        }
    }


}
