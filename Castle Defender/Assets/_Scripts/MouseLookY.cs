using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookY : MonoBehaviour
{
    public float sensitivityY;

    internal float rotationY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

        transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
    }
}