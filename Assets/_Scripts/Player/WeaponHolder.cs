using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public GameObject[] weapons;            // массив оружий    
    Weapon currentWeapon;                   // текущее оружие
    int selectedWeapon = 0;                 // индекс оружия (положение в иерархии WeaponHolder)

    void Start()
    {      
        BuyWeapon(0);
        //BuyWeapon(1);
        //BuyWeapon(2);
        SelectWeapon();
    }

    private void Update()
    {
        // Стрельба
        if (Input.GetMouseButton(0) && currentWeapon && Time.time >= currentWeapon.nextTimeToFire)  // для левого холдера
        {
            currentWeapon.nextTimeToFire = Time.time + 1f / currentWeapon.fireRate;
            currentWeapon.Fire();                                                           // вызываем функцию стрельбы у текущего оружия
        }

        // Выбор оружия
        int previousWeapon = selectedWeapon;                                                // присваиваем переменной индекс оружия

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)                         // управление колёсиком (для правого холдера)
        {
            if (selectedWeapon >= transform.childCount - 1)                                 // сбрасываем в 0 индекс, если индекс равен кол-ву объекто в иерархии WeaponHolder - 1(?)
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

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                      // активируем оружие в иерархии
                currentWeapon = weapon.gameObject.GetComponentInChildren<Weapon>();     // получаем его скрипт
            }
            else
                weapon.gameObject.SetActive(false);                                     // остальные оружия дизактивируем
            i++;
        }
    }

    void BuyWeapon(int weaponNumber)
    {
        GameObject weaponGO = Instantiate(weapons[weaponNumber], transform.position, transform.rotation);
        weaponGO.transform.SetParent(this.transform, true);
        weaponGO.SetActive(false);
    }
}
