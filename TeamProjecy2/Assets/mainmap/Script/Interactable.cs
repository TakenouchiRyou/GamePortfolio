using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string objectName;
    public AudioClip interactSE;
    [TextArea(2, 5)]
    public string message;

    public virtual void OnInteract()
    {

    }
}
