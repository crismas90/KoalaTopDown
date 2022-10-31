using UnityEngine;

[CreateAssetMenu]
public class WeaponClass : ScriptableObject
{
    public string weaponName;
    public GameObject bullet;
    public int damage;
    public int bulletSpeed;
    public float fireRate;
    public Color color;
}
