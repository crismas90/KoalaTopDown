using UnityEngine;

public class BotAIAnimator : MonoBehaviour
{
    BotAIMeleeWeaponHolder botAIWeaponHolder;
    private void Start()
    {
        botAIWeaponHolder = GetComponentInChildren<BotAIMeleeWeaponHolder>();
    }

    public void TrailStatus(int number)
    {
        botAIWeaponHolder.currentWeapon.TrailOn(number);
    }

    public void CurrentWeaponAttack()
    {
        botAIWeaponHolder.currentWeapon.MeleeAttack();
    }
}
