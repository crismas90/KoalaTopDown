using UnityEngine;
using System.Collections.Generic;

public class WeaponHolder : MonoBehaviour
{
    public List<GameObject> weapons;                    // Список оружий    
    Weapon currentWeapon;                               // текущее оружие
    [HideInInspector] public int selectedWeapon = 0;    // индекс оружия (положение в иерархии WeaponHolder)
    [HideInInspector] public bool fireStart;            // начать стрельбу
    [HideInInspector] public bool attackHitBoxStart;            // начать стрельбу

    void Start()
    {      
        //BuyWeapon(0);
        //BuyWeapon(1);
        //BuyWeapon(2);
        //SelectWeapon();
    }

    private void Update()
    {
        //Debug.Log(weapons.Count - 1);

        // Стрельба
        if (Input.GetMouseButtonDown(0))       
        {
            fireStart = true;                     // вызываем функцию стрельбы у текущего оружия
        }
        if (Input.GetMouseButtonUp(0))         
        {
            fireStart = false;                    // вызываем функцию стрельбы у текущего оружия
        }

        // Удар мечом
        if (Input.GetMouseButtonDown(1))
        {
            attackHitBoxStart = true;           // вызываем функцию стрельбы у текущего оружия
        }
        if (Input.GetMouseButtonUp(1))
        {
            attackHitBoxStart = false;           // вызываем функцию стрельбы у текущего оружия
        }



        // Выбор оружия
        int previousWeapon = selectedWeapon;                                // присваиваем переменной индекс оружия

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)                        // управление колёсиком (для правого холдера)
        {
            if (selectedWeapon >= transform.childCount - 1)                 // сбрасываем в 0 индекс, если индекс равен кол-ву объекто в иерархии WeaponHolder - 1(?)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)                        // управление колёсиком (для левого холдера)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

/*        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }*/

        if (previousWeapon != selectedWeapon)                   // если индекс оружия изменился - вызываем функцию
        {
            SelectWeapon();
        }
    }

    // Смена оружия
    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                      // активируем оружие в иерархии
                //currentWeapon = weapon.gameObject.GetComponentInChildren<Weapon>();     // получаем его скрипт
            }
            else
                weapon.gameObject.SetActive(false);                                     // остальные оружия дезактивируем
            i++;
        }
    }

    public void BuyWeapon(int weaponNumber)
    {
        GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position), transform.rotation);
        weaponGO.transform.SetParent(transform, true);  
        weaponGO.SetActive(false);
    }
}