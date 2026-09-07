
using UnityEngine;

public class PenerimaEvent : MonoBehaviour
{
    [Header("Data yang di-update saat event diterima")]
    [SerializeField] private int score = 0;
    [SerializeField] private int totalZombieMati = 0;

    private void OnEnable()
    {
        PemancarEvent.OnZombieMati += HandleZombieMati;
        PemancarEvent.OnPlayerKenaDamage += HandlePlayerKenaDamage;
        PemancarEvent.OnPlayerMati += HandlePlayerMati;
    }

    private void OnDisable()
    {
        PemancarEvent.OnZombieMati -= HandleZombieMati;
        PemancarEvent.OnPlayerKenaDamage -= HandlePlayerKenaDamage;
        PemancarEvent.OnPlayerMati -= HandlePlayerMati;
    }

    private void HandleZombieMati(GameObject zombie, int scoreReward)
    {
        totalZombieMati++;
        score += scoreReward;
        Debug.Log($"[PenerimaEvent] {zombie.name} oof Score: {score} (total zombie mati: {totalZombieMati})");
    }

    private void HandlePlayerKenaDamage(int sisaHp, int maxHp)
    {
        Debug.Log($"[PenerimaEvent] Player HP: {sisaHp}/{maxHp}");
    }

    private void HandlePlayerMati()
    {
        Debug.Log("oof dead");
    }
}