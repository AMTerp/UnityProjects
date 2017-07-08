using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookX : MonoBehaviour
{
    public float sensitivityX;

    internal float rotationX;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, rotationX, 0);
    }
}