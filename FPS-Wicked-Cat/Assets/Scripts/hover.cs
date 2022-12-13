using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hover : MonoBehaviour
{
    [Range(0.01f, 1.0f)] [SerializeField] float hoverAmount;
    [SerializeField] int hoverSpeed;

    public void Update()
    {
        Vector3.Lerp(transform.position, transform.up, 3);
    }
}
