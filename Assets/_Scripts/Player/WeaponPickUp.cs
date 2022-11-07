using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : ItemPickUp
{
    public GameObject item;
    public GameObject weaponToPickUp;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            //Debug.Log("Игрок вошёл в тригер");
            player.weaponHolder.weapons.Add(weaponToPickUp);                        // добавляем оружие в список оружий
            player.weaponHolder.BuyWeapon(player.weaponHolder.weapons.Count - 1);   // создаем его в инвентаре игрока
                                                                                    // (длинна списка - 1 и будет номер последнего добавленного оружия)
            if (player.weaponHolder.weapons.Count - 1 > 0)
                player.weaponHolder.selectedWeapon++;
            player.weaponHolder.SelectWeapon();
            Destroy(item);
        }
    }

    private void Update()
    {
        
    }
}
