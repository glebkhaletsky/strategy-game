using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : SelectableObject
{
    public int Price;
    public int XSize = 3;
    public int ZSize = 3;

    private Color _startColor;
    public Renderer Renderer;

    private void Awake()
    {
        _startColor = Renderer.material.color;
    }
    private void OnDrawGizmos()
    {
        float cellSize = FindObjectOfType<BuildingPlacer>().CellSize;

        for (int x = 0; x < XSize; x++)
        {
            for (int z = 0; z < ZSize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0f, z) * cellSize, new Vector3(1f, 0f, 1f) * cellSize);
            }
        }
    }

    public void DisplyAcceptablePosition()
    {
        Renderer.material.color = _startColor;
    }

    public void DisplyUnacceptablePosition()
    {
        Renderer.material.color = Color.red;
    }
}
