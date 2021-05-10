using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managment : MonoBehaviour
{
    public Camera Camera;
    public SelectableObject Hovered;
    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();

    private void Update()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        RaycastHit hit;
        if(Physics.Raycast(ray,out hit))
        {
            if (hit.collider.GetComponent<SelectableCollider>())
            {
                SelectableObject hitSelectableObject = hit.collider.GetComponent<SelectableCollider>().SelectableObject;
                if (Hovered)
                {
                    if (Hovered != hitSelectableObject)
                    {
                        Hovered.OnUnhover();
                        Hovered = hitSelectableObject;
                        Hovered.OnHover();
                    }
                }
                else
                {
                    Hovered = hitSelectableObject;
                    Hovered.OnHover();
                }
            }
            else
            {
                OnhoverCurrent();
            }
        }
        else
        {
            OnhoverCurrent();
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (Hovered)
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    UnselectAll();
                }                
                SelectCurrent(Hovered);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            UnselectAll();
        }
    }
    void OnhoverCurrent()
    {
        if (Hovered)
        {
            Hovered.OnUnhover();
            Hovered = null;
        }
    }
    void UnselectAll()
    {
        for (int i = 0; i < ListOfSelected.Count; i++)
        {
            ListOfSelected[i].Unselect();
        }
        ListOfSelected.Clear();
    }

    void SelectCurrent(SelectableObject selectableObject)
    {
        if (ListOfSelected.Contains(selectableObject) == false)
        {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }
}
