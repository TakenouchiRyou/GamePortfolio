using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    Interactable currentTarget;
    bool isReading;
    public GameObject interactText;
    public PlayerMove playerMove;

    private void Start()
    {
        interactText.SetActive(false);
    }

    private void Update()
    {
        if(currentTarget == null)
        {
            return;
        }

        if (!isReading && Input.GetKeyDown(KeyCode.F))
        {
            isReading = true;
            playerMove.canMove = false;
            interactText.SetActive(false);
            if (currentTarget.interactSE != null)
            {
                AudioManager.Instance.PlaySE(
                    currentTarget.interactSE
                );
            }
            MessageUI.instance.ShowMessage(currentTarget.objectName, currentTarget.message);
        }
        else if (isReading && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space))){
            MessageUI.instance.HideMessage();
            playerMove.canMove = true;
            Interactable target = currentTarget;
            isReading = false;
            if (target != null) 
            {
                target.OnInteract();
            }
            if (currentTarget != null) 
            {
                interactText.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            currentTarget = interactable;
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable == currentTarget) 
        {
            currentTarget = null;
            if (interactText != null)
            {
                interactText.SetActive(false);
            }
        }
    }
}
