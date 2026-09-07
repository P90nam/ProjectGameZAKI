// Taruh file ini di: Assets/Scripts/Enemy/Enemy.cs
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected int hp = 100;          // protected: bisa di-override nilainya lewat subclass/inspector
    [SerializeField] protected int scoreReward = 10;  // skor yang didapat player saat zombie ini mati
    public float ms = 2f;
    protected Transform player;

    [Header("State")]
    [SerializeField] protected float jarakDeteksi = 6f;  // batas Chase
    [SerializeField] protected float jarakSerang = 1.2f; // batas Attack
    [SerializeField] protected float jedaSerang = 1f;    // Cooldown serangan
    [SerializeField] protected float Damage = 5f;        // Damage yang diberikan ke player saat menyerang

    // state sekarang -- mulai dari IDLE
    protected StateZombie state = StateZombie.IDLE;
    protected float waktuSerangTerakhir;

    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    protected virtual void Update()
    {
        // tentukan state (aturan pindah)
        Transisi();

        // jalankan perilaku sesuai state sekarang
        switch (state)
        {
            case StateZombie.IDLE:   PerilakuIdle(); break;
            case StateZombie.PATROL: PerilakuPatrol(); break;
            case StateZombie.CHASE:  PerilakuChase(); break;
            case StateZombie.ATTACK: PerilakuAttack(); break;
        }
    }

    public float JarakKePlayer()
    {
        if (player == null) return Mathf.Infinity;
        return Vector2.Distance(transform.position, player.position);
    }

    protected virtual void Transisi()
    {
        StateZombie stateBaru;

        if (player == null)
        {
            stateBaru = StateZombie.IDLE;
        }
        else
        {
            float jarak = JarakKePlayer();

            if (jarak <= jarakSerang)
            {
                stateBaru = StateZombie.ATTACK; // sangat dekat -> serang
            }
            else if (jarak <= jarakDeteksi)
            {
                stateBaru = StateZombie.CHASE;  // terlihat -> kejar
            }
            else
            {
                stateBaru = StateZombie.PATROL; // jauh -> patrol
            }
        }


        if (stateBaru != state)
        {
            state = stateBaru;
            Debug.Log($"[STATE] {name} masuk ke state: {state}");
        }
    }

    protected virtual void PerilakuIdle()
    {
        // Malas coding
    }

    protected virtual void PerilakuPatrol()
    {
        // Malas coding
    }

    protected virtual void PerilakuChase()
    {
        Kejar();
    }

    protected virtual void PerilakuAttack()
    {
        // menyerang berkala, tidak tiap frame
        if (Time.time >= waktuSerangTerakhir + jedaSerang)
        {
            Serang();
            waktuSerangTerakhir = Time.time;

        }
    }

    public virtual void Kejar()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
    }

    public virtual void Serang()
    {
        Debug.Log(name + ": ATTACK");
 
        if (player == null) return;
        IDamageable target = player.GetComponent<IDamageable>();
        target?.KenaDamage((int)Damage);
    }

    public virtual void KenaDamage(int jumlah)
    {
        hp -= jumlah;
        Debug.Log($"{gameObject.name} kena damage {jumlah}, HP sisa: {hp}");

        if (hp <= 0)
        {
            Mati();
        }
    }

    protected virtual void Mati()
    {
        Debug.Log($"{gameObject.name} mati!");

        PemancarEvent.PancarkanZombieMati(gameObject, scoreReward);

        Destroy(gameObject);
    }
}