using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerController playerController;
    [SerializeField] private float timeToStart = 3f;
    private ShaderController shaderController;

    private void Start()
    {
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        shaderController = GameObject.Find("CurveLevel").GetComponent<ShaderController>();
        shaderController.ResetValues();
        shaderController.enabled = false;
        StartCoroutine("StartGame");
    }

    IEnumerator StartGame()
    {
        while(timeToStart > 0)
        {
            yield return new WaitForSeconds(1);
            timeToStart--;
        }
    }

    private void Update()
    {
        if (timeToStart == 0)
        {
            playerAnimator.enabled = true;
            playerController.enabled = true;
            shaderController.enabled = true;
        }
        if (playerController.IsDead)
        {
            Death();
        }
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUI.skin.button.fontSize = 25;
        GUILayout.BeginVertical();
        GUILayout.Label("Time to start: " + timeToStart);
        GUILayout.EndVertical();
        if (GUILayout.Button("RESTART"))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Prototype");
    }

    private void Death()
    {
        shaderController.enabled = false;
    }

}
