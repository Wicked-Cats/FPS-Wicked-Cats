using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hover : MonoBehaviour
{
    [SerializeField] float hoverAmount;
    [SerializeField] int hoverSpeed;
    [SerializeField] float maxHeight;

    public void Update()
    {
        Vector3 pos = transform.position;
        float newY = (Mathf.Sin((Time.time) * hoverSpeed) * hoverAmount) + maxHeight;
        transform.position = new Vector3(pos.x, newY, pos.z);
    }
}
