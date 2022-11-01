using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            Vector2 vec2 = (collision.transform.position - GameManager.instance.player.transform.position).normalized;
            enemy.rb2D.AddForce(vec2 * 6, ForceMode2D.Impulse);
        }
        Destroy(gameObject);
    }
}
