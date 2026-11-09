
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class Menu : MonoBehaviour
{
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.Find("Player");
        transform.Find("LayoutGroup/ButtonRestart").gameObject.SetActive(false);
    }

    private void Update()
    {
        Health health = _player.GetComponent<Health>();
        if (health.IsDead)
        {
            transform.Find("LayoutGroup/ButtonRestart").gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        transform.Find("LayoutGroup/ButtonRestart").gameObject.SetActive(false);
    }

    /* for future possible implementations
    public void ExitGame()
    {
        UnityEngine.Application.Quit();
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
    } 
    */
}