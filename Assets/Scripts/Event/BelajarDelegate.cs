using UnityEngine;

public static class BelajarDelegate
{
    public delegate void ZombieMatiDelegate(GameObject zombie, int scoreReward);
    public delegate void PlayerKenaDamageDelegate(int sisaHp, int maxHp);
    public delegate void PlayerMatiDelegate();
}