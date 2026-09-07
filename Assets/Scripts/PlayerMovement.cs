using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IDamageable
{
    public float speed = 5f;
    private Vector2 MovementInput;
    public int score = 0;
    public GameManager gameManager;

    [Header("Pengaturan HP")]
    public int maxHp = 100;
    public int currentHp;
    private bool sudahMati = false; // supaya Mati() tidak kepanggil berkali-kali

    void Awake()
    {
        currentHp = maxHp; // HP mulai penuh
    }

    void OnMove(InputValue value)
    {
        MovementInput = value.Get<Vector2>();
    }

    void Update()
    {
        // kalau sudah mati, tidak bisa gerak lagi
        if (sudahMati) return;

        Vector3 Move = new Vector3(MovementInput.x, MovementInput.y, 0);
        transform.position += Move * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (sudahMati) return;

        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            score++;
            Debug.Log("Score: " + score);
            gameManager.CollectCoin();
        }
    }

    // ===== IMPLEMENTASI IDamageable (Abstraction) =====
    // dipanggil oleh Enemy/Zombie saat menyerang player, contoh:
    // playerMovement.KenaDamage(10);
    public void KenaDamage(int jumlah)
    {
        if (sudahMati) return; // sudah mati, tidak bisa kena damage lagi

        currentHp -= jumlah;
        currentHp = Mathf.Max(currentHp, 0); // HP tidak boleh minus

        Debug.Log($"Player kena damage {jumlah}, HP sisa: {currentHp}/{maxHp}");

        // ===== PEMANCAR EVENT =====
        // beritahu subscriber (UI health bar, GameManager, dll) HP terbaru
        PemancarEvent.PancarkanPlayerKenaDamage(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Mati();
        }
    }

    private void Mati()
    {
        sudahMati = true;
        Debug.Log("Player mati!");

        // ===== PEMANCAR EVENT =====
        // beritahu subscriber bahwa player sudah mati (misal untuk munculkan UI Game Over)
        PemancarEvent.PancarkanPlayerMati();

        // karakter hilang dari scene
        Destroy(gameObject);
    }
}