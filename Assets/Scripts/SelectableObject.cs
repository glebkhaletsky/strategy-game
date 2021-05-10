using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public GameObject SlectionIndicator;

    private void Start()
    {
        SlectionIndicator.SetActive(false); 
    }
    public virtual void OnHover()
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    public virtual void OnUnhover()
    {
        transform.localScale = Vector3.one;
    }

    public virtual void Select()
    {
        SlectionIndicator.SetActive(true);
    }

    public virtual void Unselect()
    {
        SlectionIndicator.SetActive(false);
    }

    public virtual void WhenClickOnGround(Vector3 point)
    {

    }
}
