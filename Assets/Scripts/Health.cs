using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _startHealth = 10;
    private int _currentHealth = 0;

    private void Awake()
    {
        _currentHealth = _startHealth;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            Kill();
        }    
    }

    void Kill()
    {
        Destroy(gameObject);
    }

}
