using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public float targetTime = 60.0f;
    public float elapsedTime = 0f;

    public TextMeshProUGUI targetTimeText;
    public TextMeshProUGUI elapsedTimeText;

    public GameObject player;

    void Update()
    {

        //targetTime -= Time.deltaTime;
      //  targetTimeText.text = targetTime.ToString("0.00");

        elapsedTime += Time.deltaTime;
        elapsedTimeText.text = elapsedTime.ToString("0.00");

        if (targetTime <= 0.0f | !player.gameObject.activeSelf )
        {
            Invoke("timerEnded", 60);
        }

    }

    void timerEnded()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}