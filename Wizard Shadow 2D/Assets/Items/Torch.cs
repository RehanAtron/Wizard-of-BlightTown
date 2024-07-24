using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Torch : ScriptableObject
{
    public Sprite sprite;
    [Space]
    public string torchName;
    public int bulletNumber, fireRate, burnDamage;
    public bool fireDamage, iceDamage;
    public float burnTimer, freezeTimer;
}
