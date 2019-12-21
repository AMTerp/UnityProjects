using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Versatile, but exists for the purpose of being called by the "quit to desktop" button.
public class QuitScript : MonoBehaviour
{
    public void OnQuitClick()
    {
        Application.Quit();
    }
}
