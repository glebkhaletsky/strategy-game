using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionState
{
    UnitsSelected,
    Frame,
    Other
}

public class Managment : MonoBehaviour
{
    public Camera Camera;
    public SelectableObject Hovered;
    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();

    public Image FrameImage;
    private Vector2 _frameStart;
    private Vector2 _frameEnd;

    public SelectionState CurrentSelectionState;

    private void Update()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
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
                CurrentSelectionState = SelectionState.UnitsSelected;
                SelectCurrent(Hovered);
            }
        }
        if (CurrentSelectionState == SelectionState.UnitsSelected)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (hit.collider.tag == "Ground")
                {
                    for (int i = 0; i < ListOfSelected.Count; i++)
                    {
                        ListOfSelected[i].WhenClickOnGround(hit.point);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            UnselectAll();
        }

        if (Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            _frameEnd = Input.mousePosition;

            Vector2 minPoint = Vector2.Min(_frameStart, _frameEnd);
            Vector2 maxPoint = Vector2.Max(_frameStart, _frameEnd);
            Vector2 sizeFrame = maxPoint - minPoint;
            if (sizeFrame.magnitude > 10f)
            {
                FrameImage.enabled = true;
                FrameImage.rectTransform.anchoredPosition = minPoint;
                FrameImage.rectTransform.sizeDelta = sizeFrame;
                Rect rect = new Rect(minPoint, sizeFrame);
                UnselectAll();
                Unit[] allUnits = FindObjectsOfType<Unit>();
                for (int i = 0; i < allUnits.Length; i++)
                {
                    Vector2 screenPosition = Camera.WorldToScreenPoint(allUnits[i].transform.position);
                    if (rect.Contains(screenPosition))
                    {
                        SelectCurrent(allUnits[i]);
                    }
                }
                CurrentSelectionState = SelectionState.Frame;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            FrameImage.enabled = false;
            if (ListOfSelected.Count > 0)
            {
                CurrentSelectionState = SelectionState.UnitsSelected;
            }
            else
            {
                CurrentSelectionState = SelectionState.Other;
            }
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
        CurrentSelectionState = SelectionState.Other;
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
