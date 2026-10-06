using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    float timer = 10;
    public void Enter()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                Debug.Log("Quitting");
                Application.Quit();
            }
        }
    }
}
