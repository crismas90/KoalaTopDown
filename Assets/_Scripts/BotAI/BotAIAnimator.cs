using UnityEngine;

public class BotAIAnimator : MonoBehaviour
{
    BotAIWeaponHolder botAIWeaponHolder;
    private void Start()
    {
        botAIWeaponHolder = GetComponentInChildren<BotAIWeaponHolder>();
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
