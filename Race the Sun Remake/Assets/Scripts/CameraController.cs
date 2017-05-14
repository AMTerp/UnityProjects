using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float fovScaler;
    public float deathRotateAmount;
    public float deathZoomOut;

    private bool gameOver;
    private float initFov;
    private float initSpeed;
    private Vector3 offset;
    private Vector3 playerFormerTransform;
    private GameController gameController;

    // Use this for initialization
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }

        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        initFov = Camera.main.fieldOfView;
        initSpeed = gameController.DOWN_SPEED;
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        if (!gameOver)
        {
            transform.position = player.transform.position + offset;
            Camera.main.fieldOfView = CalculateFOV(gameController.DOWN_SPEED);
        } else
        {
            transform.LookAt(playerFormerTransform);
            transform.Translate(Vector3.right * Time.deltaTime * deathRotateAmount);
            Camera.main.fieldOfView = initFov/2;
        }
    }

    float CalculateFOV(float speed)
    {
        return fovScaler * Mathf.Log(speed / initSpeed, 2) + initFov;
    }

    void GameOver()
    {
        gameOver = true;
        playerFormerTransform = transform.position - offset;
        transform.position += offset * deathZoomOut;
    }
}
