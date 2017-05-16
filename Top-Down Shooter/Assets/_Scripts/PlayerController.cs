using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float gunOffset;
    public Boundary boundary;
    public GameObject gun;

    private Rigidbody rb;
    private bool gameOver;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameOver = false;
        SetGuns(1, 0);
    }

    void Update()
    {
        if (!gameOver)
        {
            RotateToMouse();
        }
    }

    void FixedUpdate()
    {
        if (!gameOver)
        {
            Move();
        }
    }

    internal void SetGuns(int num, float totalAngle)
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            Debug.Log(transform.GetChild(i).gameObject.name);
            Destroy(transform.GetChild(i).gameObject);
        }

        float angleChange;

        if (num == 1)
        {
            angleChange = 0;
        } else
        {
            angleChange = totalAngle / (num - 1);
        }

        float angle = -angleChange;
        float x, z;
        Vector3 spawnPos;
        GameObject clone;
        for (int i = 0; i < num; i++)
        {
            transform.rotation = Quaternion.identity;

            x = gunOffset * Mathf.Sin(Mathf.Deg2Rad * angle);
            z = gunOffset * Mathf.Cos(Mathf.Deg2Rad * angle);
            spawnPos = new Vector3(x, 0.0f, z) + transform.position;
            Debug.Log(string.Format("Set gun at {0}, {1}", x, z));
            clone = Instantiate(gun, spawnPos, Quaternion.identity) as GameObject;
            clone.transform.Rotate(new Vector3(0, angle, 0));
            clone.transform.parent = transform;

            angle += angleChange;
        }
    }

    /* Taken from http://answers.unity3d.com/comments/664261/view.html */
    void RotateToMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(Camera.main.transform.position.y - transform.position.y);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.LookAt(mousePos);
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );
    }

    void GameOver()
    {
        gameOver = true;
        rb.velocity = Vector3.zero;
    }
}