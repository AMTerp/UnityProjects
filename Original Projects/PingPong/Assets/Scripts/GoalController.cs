using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public event Action goalEvent;

    public void OnCollisionEnter2D(Collision2D aCollision) {
        if (aCollision.gameObject.tag.Equals("Ball")) {
            goalEvent();
        }
    }
}
