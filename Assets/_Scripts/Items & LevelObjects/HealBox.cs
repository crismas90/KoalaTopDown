using UnityEngine;

public class HealBox : ItemPickUp
{
    public void PickUpHeal()
    {
        GameManager.instance.player.Heal(25);
        Destroy(gameObject);
    }
}
