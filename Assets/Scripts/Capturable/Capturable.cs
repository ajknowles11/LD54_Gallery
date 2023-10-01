using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Capturable : MonoBehaviour
{
    [NonSerialized]
    public static HashSet<Capturable> Rendered = new();
    [NonSerialized]
    public Collider Collider;

    public bool unstable;

    private void Start()
    {
        Collider = GetComponent<Collider>();
    }

    private void OnBecameVisible()
    {
        Rendered.Add(this);
    }

    private void OnBecameInvisible()
    {
        Rendered.Remove(this);
    }
}
