using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Node : MonoBehaviour
{
    public Transform Transform;
    public List<Node> Neighbors = new List<Node>();
    public bool _isBidirectional = false;

    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI _txtNumber;

    private void Awake()
    {
        if (_isBidirectional)
        {
            foreach (var node in Neighbors)
            {
                if (!node.Neighbors.Contains(this)) node.Neighbors.Add(this);
                
            }
        }
    }
    
    private void OnValidate()
    {
        try
        {

            int startIndex = name.IndexOf('(') + 1;
            int endIndex = name.IndexOf(')');

            if (startIndex < 0 || endIndex >= name.Length)
            {
                _txtNumber.text = "0";
                return;
            }

            string id = name.Substring(startIndex, endIndex - startIndex);

            _txtNumber.text = id;

        }
        catch (Exception error)
        {

        }
    }
    
    public Node(Transform t)
    {
        Transform = t;
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (var node in Neighbors)
        {
            Gizmos.DrawLine(Transform.position, node.transform.position);
        }
    }*/
}
