using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AnimAttack")]
public class AnimAttack : ScriptableObject
{
    public int hitBoxID;
    public float range;

    public float damage;
    public float knockbackX;
    public float knockbackY;

    public bool headShot;

}
