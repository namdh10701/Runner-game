using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotBehaviour : MonoBehaviour
{
    [SerializeField] private float dmg;
    public float Dmg => dmg;
    public abstract void OnHit();
}
