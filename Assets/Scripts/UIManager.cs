// This script is a Manager that controls the UI HUD (deaths, health, and score) for the 
// project. All HUD UI commands are issued through the static methods of this class

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern. Other
    //scripts access this one through its public static methods
    static UIManager current;
    public TextMeshProUGUI gameOverText;    //Text element showing the Game Over message
    public Image healthBar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bombs;

    void Awake()
    {
        //If an UIManager exists and it is not this...
        if (current != null && current != this)
        {
            //...destroy this and exit. There can be only one UIManager
            Destroy(gameObject);
            return;
        }

        //This is the current UIManager and it should persist between scene loads
        current = this;
        DontDestroyOnLoad(gameObject);

        // disable game over text
        current.gameOverText.gameObject.SetActive(false);

        // set score text to 0
        UpdateScoreUI();
    }

    public static void UpdateScoreUI()
    {
        //If there is no current UIManager, exit
        if (current == null)
            return;

        //Refresh the score
        current.scoreText.SetText(GameManager.GetScore().ToString());
    }

   public static void UpdateBombsUI()
    {
        //If there is no current UIManager, exit
        if (current == null)
            return;

        //Refresh the score
        current.bombs.SetText(GameManager.GetBombs().ToString());
    }

    public static void DisplayGameOverText()
    {
        //If there is no current UIManager, exit
        if (current == null)
            return;

        //Show the game over text
        current.gameOverText.gameObject.SetActive(true);
    }

    public static void UpdateHealthUI(float health, float maxHealth)
    {
        //If there is no current UIManager, exit
        if (current == null)
            return;

        //update the player death count element
        current.healthBar.fillAmount = health / maxHealth;
    }
}
