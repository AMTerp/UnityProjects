using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject playerExplosion;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(playerExplosion, transform.position, transform.rotation);
            Destroy(other.gameObject);

            GameObject[] sendObjects;
            string[] tags = new string[] { "GameController", "MainCamera" , "Light", "Player" };
            foreach (string tag in tags)
            {
                sendObjects = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject sendObject in sendObjects){
                    sendObject.SendMessage("GameOver");
                }
            }
        }
    }
}
