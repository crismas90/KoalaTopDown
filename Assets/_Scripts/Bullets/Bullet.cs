using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public float pushForce;
    public GameObject expEffect;
    public LayerMask layerExplousion;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public virtual void Explosion()
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);    // ������� ������
        Destroy(effect, 0.5f);                                                                  // ���������� ������ ����� .. ���     
        Destroy(gameObject);                                                                    // ���������� ����
    }
}
