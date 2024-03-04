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
    private PlayerCollision playerCollision;
    private CapsuleCollider playerCollider;

    private Animator myAnimator;
    private Vector3 motionVector;
    [Header ("Player Controller")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float jumpPower;
    private Side position;
    private Side _lastPosition;
    public Side LastPosition { get => _lastPosition; set => _lastPosition = value; }

    private bool swipeLeft, swipeRight, swipeUp, swipeDown;
    [Header("Player States")]
    [SerializeField] private bool isJumping;
    [SerializeField] private bool _isRolling;
    [SerializeField] private bool isGrounded;
    private bool _isDead;
    public bool IsDead { get => _isDead; set => _isDead = value; }

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
    private int _IDStumbleLow = Animator.StringToHash("StumbleLow");
    private int _IDStumbleCornerRight = Animator.StringToHash("StumbleCornerRight");
    private int _IDStumbleCornerLeft = Animator.StringToHash("StumbleCornerLeft");
    private int IDStumbleFall = Animator.StringToHash("StumbleFall");
    private int IDStubmleOffLeft = Animator.StringToHash("StumbleOffLeft");
    private int IDStumbleOffRight = Animator.StringToHash("StumbleOffRight"); 
    private int _IDStumbleSideLeft = Animator.StringToHash("StumbleSideLeft");
    private int _IDStumbleSideRight = Animator.StringToHash("StumbleSideRight");
    private int _IIDDeathBounce = Animator.StringToHash("DeathBounce");
    private int _IDDeathLower = Animator.StringToHash("DeathLower");
    private int _IDDeathMovingTrain = Animator.StringToHash("DeathMovingTrain");
    private int _IDDeathUpper = Animator.StringToHash("DeathUpper");

    public int IDStumbleLow { get => _IDStumbleLow; set => _IDStumbleLow = value; }
    public int IDDeathLower { get => _IDDeathLower; set => _IDDeathLower = value; }
    public int IDDeathMovingTrain { get => _IDDeathMovingTrain; set => _IDDeathMovingTrain = value; }
    public int IIDDeathBounce { get => _IIDDeathBounce; set => _IIDDeathBounce = value; }
    public int IDDeathUpper { get => _IDDeathUpper; set => _IDDeathUpper = value; }
    public bool IsRolling { get => _isRolling; set => _isRolling = value; }
    public int IDStumbleCornerRight { get => _IDStumbleCornerRight; set => _IDStumbleCornerRight = value; }
    public int IDStumbleCornerLeft { get => _IDStumbleCornerLeft; set => _IDStumbleCornerLeft = value; }
    public int IDStumbleSideLeft { get => _IDStumbleSideLeft; set => _IDStumbleSideLeft = value; }
    public int IDStumbleSideRight { get => _IDStumbleSideRight; set => _IDStumbleSideRight = value; }

    void Start()
    {
        position = Side.Middle;
        myTransform = GetComponent<Transform>();
        myAnimator = GetComponent<Animator>();
        _myCharacterController = GetComponent<CharacterController>();
        playerCollision = GetComponent<PlayerCollision>();
        yPosition = -7;
        _isDead = false;
        playerCollider = GameObject.Find("Character Model").GetComponent<CapsuleCollider>();
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
        if(swipeDown || swipeLeft || swipeRight || swipeUp)
        {
            _lastPosition = position;
        }
    }

    private void SetPlayerPosition()
    {
        
        if (swipeLeft && !_isRolling)
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
        else if (swipeRight && !_isRolling)
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

    public void UpdatePlayerXPosition(Side plPosition)
    {
        
        newXPosition = (int)plPosition;   
        position = plPosition;
    }

    public void SetPlayerAnimator(int id, bool isCrossFade, float fadeTime = 0.1f)
    {
        myAnimator.SetLayerWeight(0,1);
        if(isCrossFade)
        {
            myAnimator.CrossFadeInFixedTime(id, fadeTime);
        }
        else
        {
            myAnimator.Play(id);
        }
        ResetCollision();
    }

    public void SetPlayerAnimatorWithLayer(int id)
    {
        myAnimator.SetLayerWeight(1, 1);
        myAnimator.Play(id);
        ResetCollision();
    } 

    private void ResetCollision()
    {
        playerCollision.CollisionX = CollisionX.None;
        playerCollision.CollisionY = CollisionY.None;
        playerCollision.CollisionZ = CollisionZ.None;
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
            if (swipeUp && !_isRolling)
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
            _isRolling = false;
            rollTimer = 0;
            //Character controller tamaño normal
            _myCharacterController.center = new Vector3(0, .45f, 0);
            _myCharacterController.height = .9f;
            playerCollider.height = 0.8f;
            playerCollider.center = new Vector3(0f, 0.47f, 0.18f);
        }
        if (swipeDown &&  !isJumping)
        {
            _isRolling = true;
            rollTimer = .5f;
            SetPlayerAnimator(IDRoll, true);
            //Achicar character controller
            _myCharacterController.center = new Vector3(0, .2f, 0);
            _myCharacterController.height = .4f;
            playerCollider.height = .4f;
            playerCollider.center = new Vector3(0f, .2f, 0.18f);
        }
    }


}
