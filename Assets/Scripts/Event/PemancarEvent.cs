// Taruh file ini di: Assets/Scripts/Event/PemancarEvent.cs
using UnityEngine;

public static class PemancarEvent
{

    public static event BelajarDelegate.ZombieMatiDelegate OnZombieMati;
    public static event BelajarDelegate.PlayerKenaDamageDelegate OnPlayerKenaDamage;
    public static event BelajarDelegate.PlayerMatiDelegate OnPlayerMati;


    public static void PancarkanZombieMati(GameObject zombie, int scoreReward)
    {
        OnZombieMati?.Invoke(zombie, scoreReward);
    }

    public static void PancarkanPlayerKenaDamage(int sisaHp, int maxHp)
    {
        OnPlayerKenaDamage?.Invoke(sisaHp, maxHp);
    }

    public static void PancarkanPlayerMati()
    {
        OnPlayerMati?.Invoke();
    }
}