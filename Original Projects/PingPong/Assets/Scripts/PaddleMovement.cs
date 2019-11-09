using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
   public void left(float aSpeed) {
       transform.Translate(new Vector2(-aSpeed * Time.deltaTime, 0));
   }

   public void right(float aSpeed) {
       transform.Translate(new Vector2(aSpeed * Time.deltaTime, 0));
   }
}
