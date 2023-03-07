using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interaction : MonoBehaviour
{
    public TextMeshProUGUI PromptText;
  

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out RaycastHit hit, 5))
        {
            var interactable = hit.transform.GetComponent<IInteractable>();
            if (interactable != null && hit.transform.CompareTag("Interactable"))
            {
                PromptText.gameObject.SetActive(true);
                PromptText.text = interactable.prompt();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.interact();
                }
            }
            else
            {
                PromptText.gameObject.SetActive(false);
            }
        }
        else
        {
            PromptText.gameObject.SetActive(false);
        }
    }
}
