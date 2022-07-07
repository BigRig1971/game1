using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoobInertia : MonoBehaviour
{
    Transform boobBone;
    [SerializeField] Transform desiredBoobParent;
    [SerializeField] Transform target;
    [SerializeField] float maxDistanceFromStart = .2f;
    [SerializeField] float inertiaLerp = 1f;
    [SerializeField] float speed = 1f;
   
    private void Start()
    {
        target = gameObject.AddComponent<Transform>();
        target.SetParent(desiredBoobParent);
        boobBone = GetComponent<Transform>();
           
        boobBone.SetParent(null);
    }
    private void FixedUpdate()
    {
        float _distance = Vector3.Distance(boobBone.position, target.position);
        var step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(boobBone.position, target.position, step);

        Debug.Log(boobBone.position);
    }
    float OnInertiaLerp(float goal)
    {

        float delta = goal - inertiaLerp;
        delta *= Time.deltaTime;
        inertiaLerp += delta;
        return inertiaLerp;
    }
}
