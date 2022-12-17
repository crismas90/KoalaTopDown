using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ��������� ������, ����� �������������� ��� �������� ������
/// </summary>

public class BotAIRangeWeaponHolder : MonoBehaviour
{
    BotAI botAI;
    //public BotAIMeleeWeaponHolder botAIweaponHolderMelee;         // ������ �� ������ ��� ���� ������
    public List<GameObject> weapons;                    // ������ ������    
    [HideInInspector] public BotAIWeaponRange currentWeapon;      // ������� ������ 
    [HideInInspector] public int selectedWeapon = 0;    // ������ ������ (��������� � �������� WeaponHolder)
    [HideInInspector] public bool fireStart;            // ������ ��������
    bool weaponChanged;                                 // ���� ��� ���� ������

    //[HideInInspector] public bool attackHitBoxStart;    // ������ ����� �����
    //[HideInInspector] public float aimAngle;            // ���� �������� ��� �������� ������� � ������� � �������������    
    //bool meleeWeapon;                                   // ���� ������ ��� ����

    private void Awake()
    {
        botAI = GetComponentInParent<BotAI>();
    }

    void Start()
    {
        
        int i = 0;
        foreach (GameObject weapon in weapons)          // �������� ��� ������ �� ������ ������ ��� ������
        {
            BuyWeapon(i);
            i++;
        }

        if (botAI.rangeAttackType)
        {
            SelectWeapon();                                 // �������� ������
            weaponChanged = true;
        }
        else
        {
            HideWeapons();                      // ������ ������
        }
    }

    private void Update()
    {
        //Debug.Log(weapons.Count - 1);

        // ��������
        if (botAI.readyToAttack && botAI.rangeAttackType)
        {
            fireStart = true;                   // ��������
        }
        else
        {
            fireStart = false;                   // ��������
        }

        // ����� ������ �� ����
        if (botAI.rangeAttackType && !weaponChanged)
        {
            SelectWeapon();
            weaponChanged = true;
        }
        if (botAI.meleeAttackType && weaponChanged)
        {
            HideWeapons();                      // ������ ������
            weaponChanged = false;
        }

        /*        // ������� ������
                if (!stopAiming)
                {
                    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);                        // ��������� ����                  
                    Vector3 aimDirection = mousePosition - transform.position;                                  // ���� ����� ���������� ���� � pivot ������          
                    aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;                     // ������� ���� � ��������             
                    Quaternion qua1 = Quaternion.Euler(0, 0, aimAngle);                                         // ������� ���� ���� � Quaternion
                    transform.rotation = Quaternion.Lerp(transform.rotation, qua1, Time.fixedDeltaTime * 15);   // ������ Lerp ����� weaponHoder � ����� �����
                    //Debug.Log(aimAngle);
                }*/

        // ����� ������
        int previousWeapon = selectedWeapon;                                // ����������� ���������� ������ ������

        if (Input.GetKeyDown(KeyCode.K))                         // ���������� �������� (��� ������� �������)
        {
            if (selectedWeapon >= transform.childCount - 1)                 // ���������� � 0 ������, ���� ������ ����� ���-�� ������� � �������� WeaponHolder - 1(?)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        if (previousWeapon != selectedWeapon)               // ���� ������ ������ ��������� - �������� �������
        {
            SelectWeapon();
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
    }

    // ����� ������
    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);                                              // ���������� ������ � ��������
                //currentWeapon = weapon.gameObject.GetComponentInChildren<BotAIWeaponRange>();   // �������� ��� ������                    
            }
            else
            {
                weapon.gameObject.SetActive(false);                                             // ��������� ������ �������������
            }
            i++;
        }
    }

    public void HideWeapons()
    {
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);                                     // ��������� ������ �������������
        }
    }

    // ������� ������ (��������� ������)
    public void BuyWeapon(int weaponNumber)
    {
        GameObject weaponGO = Instantiate(weapons[weaponNumber], (transform.position), transform.rotation);
        weaponGO.transform.SetParent(transform, true);
        weaponGO.SetActive(false);
    }
}