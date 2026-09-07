using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalCoins;
    private int coinsCollected = 0;

    void Start()
    {
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    public void CollectCoin()
    {
        coinsCollected++;
        if (coinsCollected == totalCoins) Win();
    }

    void Win()
    {
        Debug.Log("YOU WIN!");
    }
}