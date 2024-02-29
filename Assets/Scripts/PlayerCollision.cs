using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollisionX
{
    None,
    Left,
    Middle,
    Right
}
public enum CollisionY
{
    None,
    Up,
    Middle,
    Down,
    LowDown
}
public enum CollisionZ
{
    None,
    Forward,
    Middle,
    Backward
}

public class PlayerCollision : MonoBehaviour
{
    private PlayerController playerController;
    private CollisionX _collisionX;
    private CollisionY _collisionY;
    private CollisionZ _collisionZ;

    public CollisionX CollisionX { get => _collisionX; set => _collisionX = value; }
    public CollisionY CollisionY { get => _collisionY; set => _collisionY = value; }
    public CollisionZ CollisionZ { get => _collisionZ; set => _collisionZ = value; }

    private void Awake()
    {
        playerController = gameObject.GetComponent<PlayerController>();
    }

    public void OnCharacterCollision(Collider col)
    {
        _collisionX = GetCollisionX(col);
        _collisionY = GetCollisionY(col);
        _collisionZ = GetCollisionZ(col);
        SetAnimatorByCollision(col);
    }



    private CollisionX GetCollisionX(Collider collider)
    {
        Bounds characterControllerBounds = playerController.MyCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minX = Mathf.Max(colliderBounds.min.x, characterControllerBounds.min.x);
        float maxX = Mathf.Min(colliderBounds.max.x, characterControllerBounds.max.x);
        float average = (minX + maxX) / 2 - colliderBounds.min.x;
        CollisionX colX;
        if(average > colliderBounds.size.x - 0.33f)
        {
            colX = CollisionX.Right;
        }
        else if(average < 0.33f)
        {
            colX = CollisionX.Left;
        }
        else
        {
            colX = CollisionX.Middle;
        }
        return colX;
    }

    private CollisionY GetCollisionY(Collider collider)
    {
        Bounds characterControllerBounds = playerController.MyCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minY = Mathf.Max(colliderBounds.min.y, characterControllerBounds.min.y);
        float maxY = Mathf.Min(colliderBounds.max.y, characterControllerBounds.max.y);
        float average = (minY + maxY) / 2 - colliderBounds.min.y;
        CollisionY colY;
        if (average > colliderBounds.size.y - 0.33f)
        {
            colY = CollisionY.Up;
        }
        else if (average < 0.17f)
        {
            colY = CollisionY.LowDown;
        }
        else if (average < 0.33f)
        {
            colY = CollisionY.Down;
        }
        else
        {
            colY = CollisionY.Middle;
        }
        return colY;
    }

    private CollisionZ GetCollisionZ(Collider collider)
    {
        Bounds characterControllerBounds = playerController.MyCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minZ = Mathf.Max(colliderBounds.min.z, characterControllerBounds.min.z);
        float maxZ = Mathf.Min(colliderBounds.max.z, characterControllerBounds.max.z);
        float average = (minZ + maxZ) / 2 - colliderBounds.min.z;
        CollisionZ colZ;
        if (average > colliderBounds.size.z - 0.33f)
        {
            colZ = CollisionZ.Forward;
        }
        else if (average < 0.33f)
        {
            colZ = CollisionZ.Backward;
        }
        else
        {
            colZ = CollisionZ.Middle;
        }
        return colZ;
    }

    private void SetAnimatorByCollision(Collider collider)
    {
        if(_collisionZ == CollisionZ.Backward && _collisionX == CollisionX.Middle)
        {
            if(_collisionY == CollisionY.LowDown)
            {
                collider.enabled = false;
                playerController.SetPlayerAnimator(playerController.IDStumbleLow, false);
            }
            else if(_collisionY == CollisionY.Down)
            {
                playerController.SetPlayerAnimator(playerController.IDDeathLower, false);
            }
            else if(_collisionY == CollisionY.Middle)
            {
                if (collider.CompareTag("TrainOn"))
                {
                    playerController.SetPlayerAnimator(playerController.IDDeathMovingTrain, false);
                }
                else if(!collider.CompareTag("Ramp"))
                {
                    playerController.SetPlayerAnimator(playerController.IIDDeathBounce, false);
                }
            }
            else if(_collisionY == CollisionY.Up && !playerController.IsRolling)
            {
                playerController.SetPlayerAnimator(playerController.IDDeathUpper, false);
            }
        }
        else if(_collisionZ == CollisionZ.Middle)
        {
            if(_collisionX == CollisionX.Right)
            {
                playerController.SetPlayerAnimator(playerController.IDStumbleSideRight, false);
            }
            else if(_collisionX == CollisionX.Left)
            {
                playerController.SetPlayerAnimator(playerController.IDStumbleSideLeft, false);
            }
        }
        else
        {
            if(_collisionX == CollisionX.Right)
            {
                playerController.SetPlayerAnimatorWithLayer(playerController.IDStumbleCornerRight);
            }
            else if(_collisionX == CollisionX.Left)
            {
                playerController.SetPlayerAnimatorWithLayer(playerController.IDStumbleCornerLeft);
            }
        }
    }
}
