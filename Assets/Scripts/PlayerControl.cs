using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    Runners runnersScript;
    SceneManagerScript sceneManagerScript;
    ScalenFiller scalenFiller;

    public GameObject player, runnersPrefab, cameraObject;
    public Transform runnersTransform, leftArea, rightArea;

    public List<GameObject> runnerList = new List<GameObject>();
    public List<GameObject> leftRunnerList = new List<GameObject>();
    public List<GameObject> rightRunnerList = new List<GameObject>();

    private float speed = 5f, minX = -1.8f, maxX = 1.8f, followSmoothness = 7f, speedoDrone = 1f;
    private float stayTimer = 0f, timeThreshold = .3f;
    private float countdown = 3f;
    private bool allRunnersProcessed = false, isProcessing = false;

    public Color leftAreaColor, rightAreaColor;
    Color runnerColor;

    public TextMeshProUGUI leftCounter, theCounter, rightCounter;

    private bool droneArea;
    private float droneAreaSayac;

    public float touchSpeed = 10f; // Hareket hýzý
    //public float touchMinX = -1.8f; // Minimum x sýnýrý
    //public float touchMaxX = 1.8f;  // Maksimum x sýnýrý

    private float targetX;


    void Start()
    {
        droneArea = false;
        for (int i = 0; i < 5; i++)
        {
            AddRunner();
        }
        sceneManagerScript = GameObject.FindGameObjectWithTag("Scene Manager").GetComponent<SceneManagerScript>();

        foreach (GameObject runner in runnerList)
        {
            runnersScript = runner.gameObject.GetComponent<Runners>();
        }

        scalenFiller = GameObject.FindGameObjectWithTag("Scene Manager").GetComponent<ScalenFiller>();

    }

    void Update()
    {

        MoveForward();
        FollowParent();

        if (Input.touchCount > 0) // Ekrana dokunma algýlanýrsa
        {
            Touch touch = Input.GetTouch(0); // Ýlk dokunmayý al
            Vector3 touchPosition = touch.position;

            // Dokunulan ekran pozisyonunu dünya pozisyonuna çevir
            Vector3 worldPosition = cameraObject.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(
                touchPosition.x,
                touchPosition.y,
                Mathf.Abs(cameraObject.GetComponent<Camera>().transform.position.z - player.transform.position.z)
            ));

            // Sadece yatay (x) pozisyonu güncelle
            player.transform.position = new Vector3(
                worldPosition.x,
                player.transform.position.y, // Yatayda kalýyor
                player.transform.position.z  // Derinlik ayný kalýyor
            );
        }

        if (allRunnersProcessed && !isProcessing)
        {
                StartCoroutine(ProcessRunnersWithDelay());
        }

        if (droneAreaSayac > 0)
        {
                droneAreaSayac -= Time.deltaTime;
        }
        else if (droneAreaSayac <= 0)
        {
            droneArea = false;
        }

        //----------------------- PC ----------------------------//

        /*float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * Time.deltaTime * speed;
        transform.Translate(movement);

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;

        if (allRunnersProcessed && !isProcessing)
        {
            StartCoroutine(ProcessRunnersWithDelay());
        }

        
        if (Input.GetKey(KeyCode.W))
        {
            speed = 10f;
        }
        */
        //--------------------------------------------------//
    }

    private void MoveForward()
    {
        if (!droneArea)
        {
            Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + (speed * Time.deltaTime));
            player.transform.position = playerPos;
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, cameraObject.transform.position.y, cameraObject.transform.position.z + (speed * Time.deltaTime));
        }

        else
        {
            Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + (speedoDrone * Time.deltaTime));
            player.transform.position = playerPos;
            cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, cameraObject.transform.position.y, cameraObject.transform.position.z + (speedoDrone * Time.deltaTime));
        }
    }

    private void FollowParent()
    {
        if (runnerList.Count > 0)
        {
            GameObject firstRunner = runnerList[0];
            firstRunner.transform.position = Vector3.Lerp(firstRunner.transform.position, player.transform.position, followSmoothness * Time.deltaTime);   
        }

        for (int i = 1; i < runnerList.Count; i++)
        {
            GameObject currentRunner = runnerList[i];
            GameObject previousRunner = runnerList[i - 1];

            Vector3 newPosition = new Vector3(previousRunner.transform.position.x, currentRunner.transform.position.y, previousRunner.transform.position.z);
            currentRunner.transform.position = Vector3.Lerp(currentRunner.transform.position, newPosition, followSmoothness * Time.deltaTime);
        }
    }

    private IEnumerator ProcessRunnersWithDelay()
    {
        isProcessing = true;


        // Hýzý sýfýrla
        speed = 0f;

        // Geri sayýmý baþlat
        while (countdown > 0f)
        {
            countdown -= Time.deltaTime;
            yield return null;
        }

        // Ýþlemleri tamamla
        HandleRunners();
        allRunnersProcessed = false;
        countdown = 3f;

        // Hýzý geri ayarla
        speed = 5f;

        isProcessing = false;
    }


    public void AddRunner()
    {

        Vector3 spawnPosition; 
            
        if (runnerList.Count > 0)
        {
            GameObject lastRunner = runnerList[runnerList.Count - 1];

            spawnPosition = lastRunner.transform.position - (lastRunner.transform.forward * .8f);

            Color firstRunnerColor = runnerList[0].GetComponentInChildren<Renderer>().material.color;
            
        }

        else
        {
            spawnPosition = runnersTransform.position;
        }

        GameObject newRunner = Instantiate(runnersPrefab, spawnPosition, Quaternion.identity, runnersTransform);
        Runners runnersScript = newRunner.GetComponent<Runners>();
        runnersScript.playerControlScript = this;

        if (runnerList.Count > 0)
        {
            Color lastRunnerColor = runnerList[runnerList.Count - 1].GetComponentInChildren<Renderer>().material.color;
            runnersScript.SetColor(newRunner, lastRunnerColor);
        }
        else
        {
            runnersScript.ChangeColor(newRunner);
        }

        runnerList.Add(newRunner);
    }
    public void DeleteRunner()
    {
        if (runnerList.Count > 0)
        {
            GameObject lastRunner = runnerList[runnerList.Count - 1];
            runnerList.RemoveAt(runnerList.Count - 1);
            Destroy(lastRunner);
        }
        else
            sceneManagerScript.GameOver();
    }

    public void DeleteFrontRunner()
    {
        if (runnerList.Count > 0)
        {
            GameObject frontRunner = runnerList[0];

            runnerList.RemoveAt(0);

            Destroy(frontRunner);
        }
        else sceneManagerScript.GameOver();
    }

    private void HandleRunners()
    {
        // Sol alandaki runner'larý kontrol et
        for (int i = leftRunnerList.Count - 1; i >= 0; i--)
        {
            GameObject runner = leftRunnerList[i];
            Color runnerColor = runner.GetComponentInChildren<Renderer>().material.color;

            if (!runnerColor.Equals(leftAreaColor))
            {
                Destroy(runner);
                leftRunnerList.RemoveAt(i);
            }
        }

        // Sað alandaki runner'larý kontrol et
        for (int i = rightRunnerList.Count - 1; i >= 0; i--)
        {
            GameObject runner = rightRunnerList[i];
            Color runnerColor = runner.GetComponentInChildren<Renderer>().material.color;

            if (!runnerColor.Equals(rightAreaColor))
            {
                Destroy(runner);
                rightRunnerList.RemoveAt(i);
            }
        }

        runnerList.AddRange(leftRunnerList);
        runnerList.AddRange(rightRunnerList);

        leftRunnerList.Clear();
        rightRunnerList.Clear();

        ReorganizeRunnerList();

    }

    public void AddToLeftRunnerList(GameObject runner)
    {
        leftRunnerList.Add(runner);
        runnerList.Remove(runner);
        ReorganizeRunnerList();

        Vector3 targetPosition = new Vector3 (leftArea.position.x, runner.transform.position.y, leftArea.position.z - leftRunnerList.Count / (2));
        StartCoroutine(MoveToPosition(runner, targetPosition));
    }

    public void AddToRightRunnerList(GameObject runner)
    {
        rightRunnerList.Add(runner);
        runnerList.Remove(runner);
        ReorganizeRunnerList();

        Vector3 targetPosition = new Vector3(rightArea.position.x, runner.transform.position.y, rightArea.position.z - (rightRunnerList.Count / (2)));
        StartCoroutine(MoveToPosition(runner, targetPosition));
    }
    private void ReorganizeRunnerList()
    {
        
        runnerList.RemoveAll(item => item == null);
        allRunnersProcessed = runnerList.Count == 0;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Finish"))
        {
            Debug.Log("Runner List Count: " + runnerList.Count);
            sceneManagerScript.manHolding += runnerList.Count;

            sceneManagerScript.Finish();           
        }
        if (other.gameObject.tag.Equals("Drone Area"))
        {
            followSmoothness = 1f;
            droneArea = true;
            droneAreaSayac = 1f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Turret Area"))
        {
            Color areaColor = other.gameObject.GetComponentInChildren<Renderer>().material.color;
            
            if (runnerList.Count > 0)
            {
                runnerColor = runnerList[0].GetComponentInChildren<Renderer>().material.color;
            }

            speed = 3f;
            
            if (!(areaColor.Equals(runnerColor)))
            {
                stayTimer += Time.deltaTime;

                if (stayTimer >= timeThreshold)
                {
                    DeleteFrontRunner();
                    stayTimer = 0;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Turret Area"))
        {
            speed = 5f;
        }

        if (other.gameObject.tag.Equals("Drone Area"))
        {
            followSmoothness = 7f;
            droneArea = false;
        }
        
    }

    private IEnumerator MoveToPosition(GameObject runner, Vector3 targetPosition)
    {
        float speed = 5f;
        while (runner != null && Vector3.Distance(runner.transform.position , targetPosition) >= 0.1f)
        {
            runner.transform.position = Vector3.Lerp(runner.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        if (runner != null)
        {
            runner.transform.position = targetPosition; // Hedef pozisyona tam oturt
        }
    }  

}
