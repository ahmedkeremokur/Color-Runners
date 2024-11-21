using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{

    ScalenFiller scalenFiller;

    public int _level;

    public int manHolding;


    private void Awake()
    {
        if (_level == 0) _level = 1;
        manHolding = PlayerPrefs.GetInt("manHolding", manHolding);
        _level = PlayerPrefs.GetInt("_level", _level);

        if (SceneManager.GetActiveScene().buildIndex == 0)
        scalenFiller = GameObject.FindGameObjectWithTag("English Man").GetComponent<ScalenFiller>();
    }
 
    public void Finish()
    {
        _level++;
        SavePrefs();
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(_level);
    }

    public void ResetGame()
    {
        _level = 1;
        manHolding = 0;

        scalenFiller.resto = 0;
        scalenFiller.house = 0;
        scalenFiller.manav = 0;
        scalenFiller.otoservice = 0;
        scalenFiller.market = 0;
        scalenFiller.gas = 0;


        SavePrefs();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        SavePrefs();
        Application.Quit();
    }

    private void SavePrefs()
    {
        PlayerPrefs.SetInt("manHolding", manHolding);
        PlayerPrefs.SetInt("_level", _level);

        if (SceneManager.GetActiveScene().buildIndex == 0)
            scalenFiller.SaveCity();

        PlayerPrefs.Save();
    }

    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }

    private void OnApplicationQuit()
    {
        SavePrefs();       
    }
}
