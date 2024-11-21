using UnityEngine;

public class ScalenFiller : MonoBehaviour
{
    //-------Scene Manager--------//

    public int _isFinished = 0;

    SceneManagerScript sceneManagerScript;

    //---------------------------//
    private float scaleAmount;
    public GameObject englishManinNewYork;

    public TMPro.TextMeshProUGUI textHoldingScore, levelText;

    public GameObject btnNextLevel;

    public int resto, house, manav, otoservice, market, gas;   //Buildings
    public int buildingCase;
    public TextMesh restotxt, housetxt, fruittxt, ototxt, markettxt, gastxt;

    JoyStick joystickScript;


    // Start is called before the first frame update
    void Start()
    {
        
        sceneManagerScript = GameObject.FindGameObjectWithTag("Scene Manager").GetComponent<SceneManagerScript>(); 
        joystickScript = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<JoyStick>();

        if (englishManinNewYork == null) 
        {
            englishManinNewYork = GameObject.FindGameObjectWithTag("English Man");
        }

        

        if (!(sceneManagerScript._level >= 1 && sceneManagerScript._level <= 5)) sceneManagerScript._level = 1;


        if (_isFinished == 1)
        {
            btnNextLevel = GameObject.FindGameObjectWithTag("Next Level");
            btnNextLevel.SetActive(false);
        }

        else 
        {
            btnNextLevel.SetActive(true);            
        }

        levelText.text = ("Level: " + sceneManagerScript._level).ToString();
        textHoldingScore.text = sceneManagerScript.manHolding.ToString();

        RememberCity();
        UpdateAreas();

        SetManSize();
    }

    
    private void SetManSize()
    {
        englishManinNewYork.transform.localScale = new Vector3((40 + sceneManagerScript.manHolding), (40 + sceneManagerScript.manHolding), (40 + sceneManagerScript.manHolding));
        scaleAmount = (40 + sceneManagerScript.manHolding) / 40;
        joystickScript.moveSpeed *= scaleAmount;
    }


    //resto, house, manav, otoservice, market, gas

    public void FillBuilding()
    {
        if (buildingCase >= 0 &&  buildingCase <= 4)
        {
            switch (buildingCase)
            {
                case 0:
                    if (resto < 20)
                    {
                        resto++;
                        sceneManagerScript.manHolding--;
                        SetManSize();
                    }
                    break;
                case 1:
                    if (house < 25)
                    {
                        house++;
                        sceneManagerScript.manHolding--;
                        SetManSize();
                    }
                    break;
                case 2:
                    if (manav < 30)
                    {
                        manav++;
                        sceneManagerScript.manHolding--;
                        SetManSize();
                    }
                    break;
                case 3:
                    if (otoservice < 35)
                    {
                        otoservice++;
                        sceneManagerScript.manHolding--;
                        SetManSize();
                    }
                    break;
                case 4:
                    if (market < 40)
                    {
                        market++;
                        sceneManagerScript.manHolding--;
                        SetManSize();
                    }
                    break;
                case 5:
                    if (gas < 45)
                    {
                        gas++;
                        sceneManagerScript.manHolding--;
                        SetManSize();
                    }
                    break;
            }
        }          
        UpdateAreas();
    }

    private void UpdateAreas()
    {
        restotxt.text = (resto + "/20");
        housetxt.text = (house + "/25");
        fruittxt.text = (manav + "/30");
        ototxt.text = (house + "/35");
        markettxt.text = (house + "/40");
        gastxt.text = (house + "/45");

        textHoldingScore.text = textHoldingScore.text = sceneManagerScript.manHolding.ToString();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("Fill Area"))
        {
            if(sceneManagerScript.manHolding > 0)
            {
                if (other.gameObject.name.Equals("Restaurant Area")) buildingCase = 0;
                else if (other.gameObject.name.Equals("House Area")) buildingCase = 1;
                else if (other.gameObject.name.Equals("Fruit Area")) buildingCase = 1;
                else if (other.gameObject.name.Equals("Oto Service Area")) buildingCase = 1;
                else if (other.gameObject.name.Equals("Market Area")) buildingCase = 1;
                else if (other.gameObject.name.Equals("Gas Area")) buildingCase = 1;
                FillBuilding();
            }

        }   //resto, house, manav, otoservice, market, gas
    }

    public void SaveCity()
    {
        PlayerPrefs.SetInt("resto", resto);
        PlayerPrefs.SetInt("house", house);
        PlayerPrefs.SetInt("manav", manav);
        PlayerPrefs.SetInt("otoservice", otoservice);
        PlayerPrefs.SetInt("market", market);
        PlayerPrefs.SetInt("gas", gas);

        PlayerPrefs.Save();
    }

    public void RememberCity()
    {
        resto = PlayerPrefs.GetInt("resto", resto);
        house = PlayerPrefs.GetInt("house", house);
        manav = PlayerPrefs.GetInt("manav", manav);
        otoservice = PlayerPrefs.GetInt("otoservice", otoservice);
        market = PlayerPrefs.GetInt("market", market);
        gas = PlayerPrefs.GetInt("gas", gas);
    }

}
