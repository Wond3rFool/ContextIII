using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            foreach (Outline lin in highlight.gameObject.GetComponentsInChildren<Outline>())
            {
                lin.enabled = false;
            }
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
            {
                if (highlight.gameObject.GetComponentInChildren<Outline>() != null)
                {
                    foreach (Outline lin in highlight.gameObject.GetComponentsInChildren<Outline>())
                    {
                        lin.enabled = true;
                    }
                }
                else
                {
                    foreach (Transform child in highlight.transform)
                    {
                        // Add the Outline component to each child
                        Outline outline = child.gameObject.AddComponent<Outline>();

                        // Enable the Outline
                        outline.enabled = true;

                        // Set the OutlineColor and OutlineWidth as desired
                        outline.OutlineColor = Color.black;
                        outline.OutlineWidth = 7.0f;
                    }
                }
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        if (Input.GetMouseButtonDown(0))
        {
            if (highlight)
            {
                if (selection != null)
                {
                    foreach (Outline lin in selection.gameObject.GetComponentsInChildren<Outline>())
                    {
                        lin.enabled = false;
                    }
                }
                selection = raycastHit.transform;
                foreach (Outline lin in selection.gameObject.GetComponentsInChildren<Outline>())
                {
                    lin.enabled = true;
                }
                highlight = null;
            }
        }
    }
}
