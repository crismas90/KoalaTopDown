using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
