using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private Transform playerTransform;
    private float objPos = 2000f;


    private void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        float playerPosZ = playerTransform.position.z;
        if (playerPosZ >= objPos)
        {
            Vector3 resetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, 0f);
            playerTransform.position = resetPosition;
        }
    }
}
