using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookX : MonoBehaviour
{
    public float sensitivityX;

    internal float rotationX;

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
            rotationX += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        }

        transform.localEulerAngles = new Vector3(0, rotationX, 0);
    }
}