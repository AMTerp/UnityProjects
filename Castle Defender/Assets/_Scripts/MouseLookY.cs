using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookY : MonoBehaviour
{
    public float sensitivityY;

    internal float rotationY;

    private GameController gameController;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.uiDisableMouseLook)
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

            transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        }
    }
}