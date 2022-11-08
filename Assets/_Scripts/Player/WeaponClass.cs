using UnityEngine;

[CreateAssetMenu]
public class WeaponClass : ScriptableObject
{
    public string weaponName;
    public GameObject bullet;
    public int damage;
    public float pushForce;
    public int bulletSpeed;
    public float fireRate;
    public float forceBackFire;
    //public GameObject flashEffect;
}
