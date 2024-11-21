using System.Collections;
using UnityEngine;

public class Runners : MonoBehaviour
{
    [HideInInspector]
    public PlayerControl playerControlScript;

    private bool isChecked = false;

    public float droneAreaTimer = 0;

    int _color; //1: Blue, 2: Red, 3: Yellow, 4: Magenta, 5: Green, 6: Orange, 7: Pink, 8: Peach

    public int _animationState = 0; //0: Running, 1: Sneaking

    [HideInInspector]
    public Animator _animator;
    private Coroutine resetAnimationStateCoroutine;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (droneAreaTimer > 0)
        droneAreaTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Mans"))
        {
            Color collisionColor = collision.gameObject.GetComponentInChildren<Renderer>().material.color;
            Color runnerColor = this.gameObject.GetComponentInChildren<Renderer>().material.color;

            if (collisionColor.Equals(runnerColor))
            {
                playerControlScript.AddRunner();
                Destroy(collision.gameObject);
            }

            else
            {
                Debug.Log("Man collision this color: " + runnerColor
                    + "Man Collision collision color" + collisionColor);
                playerControlScript.DeleteFrontRunner();
                Destroy(collision.gameObject);
            }
        }
        if (collision.gameObject.tag.Equals("Obstacle"))
        {
            Destroy(collision.gameObject);
            playerControlScript.DeleteFrontRunner();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Color Changer"))
        {
            if (other.transform.name.Equals("Blue Gate"))               _color = 1;
            else if (other.transform.name.Equals("Red Gate"))           _color = 2;
            else if (other.transform.name.Equals("Yellow Gate"))        _color = 3;
            else if (other.transform.name.Equals("Magenta Gate"))       _color = 4;
            else if (other.transform.name.Equals("Green Gate"))         _color = 5;
            else if (other.transform.name.Equals("Orange Gate"))        _color = 6;
            else if (other.transform.name.Equals("Pink Gate"))          _color = 7;
            else if (other.transform.name.Equals("Peach Gate"))         _color = 8;
            else if (other.transform.name.Equals("Cyan Gate"))          _color = 9;
            else if (other.transform.name.Equals("Light Green Gate"))   _color = 10;

            ChangeColor(this.gameObject);
        }

        if (other.CompareTag("Drone Area"))
        {
            if(droneAreaTimer <= 0f)
            {
                _animationState = 2;
                _animator.SetInteger("State", _animationState);

                Color areaColor = other.GetComponent<Renderer>().material.color;
                Color runnerColor = GetComponentInChildren<Renderer>().material.color;

                if (other.gameObject.transform.name.Equals("Left Area"))
                {
                    playerControlScript.leftAreaColor = other.GetComponent<Renderer>().material.color;
                    Debug.Log("Left area color: " + other.GetComponent<Renderer>().material.color);

                    playerControlScript.AddToLeftRunnerList(this.gameObject);
                }

                else if (other.gameObject.transform.name.Equals("Right Area"))
                {
                    playerControlScript.rightAreaColor = other.GetComponent<Renderer>().material.color;
                    Debug.Log("Right area color: " + other.GetComponent<Renderer>().material.color);

                    playerControlScript.AddToRightRunnerList(this.gameObject);
                }

                if (resetAnimationStateCoroutine != null)
                {
                    StopCoroutine(resetAnimationStateCoroutine);
                }
                resetAnimationStateCoroutine = StartCoroutine(ResetAnimationStateAfterTime(4f));

                droneAreaTimer = 10f;
            }          
        }

        if (other.CompareTag("Turret Area"))
        {
            _animationState = 1;    //Sneaking
            _animator.SetInteger("State", _animationState);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Turret Area"))
        {
            _animationState = 0;    //Running
            _animator.SetInteger("State", _animationState);
        }
        if (other.CompareTag("Drone Area"))
        {
            _animationState = 0;    //Running
            _animator.SetInteger("State", _animationState);
        }
    }
    private IEnumerator ResetAnimationStateAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        _animationState = 0;
        _animator.SetInteger("State", _animationState);
        resetAnimationStateCoroutine = null; // Coroutine tamamlandý
    }

    public void ChangeColor(GameObject obj)
    {
        Color _orange = new Color(1, .5f, 0);
        Color _pink = new Color(1, .7f, 1);
        Color _peach = new Color(1, .75f, .5f);
        Color _lightGreen = new Color(.4f, 1, .6f);
        Color _yellow = new Color(1, 1, 0);
        Color color;
        switch (_color)
        {
            case 1: color = Color.blue; break;
            case 2: color = Color.red; break;
            case 3: color = _yellow; break;
            case 4: color = Color.magenta; break;
            case 5: color = Color.green; break;
            case 6: color = _orange; break;
            case 7: color = _pink; break;
            case 8: color = _peach; break;
            case 9: color = Color.cyan; break;
            case 10: color = _lightGreen; break;
            default: color = Color.white; break;
        }

        Renderer objRenderer = obj.GetComponentInChildren<Renderer>();
        objRenderer.material.color = color;
    }

    public void SetColor(GameObject obj, Color color)
    {
        Renderer objRenderer = obj.GetComponentInChildren<Renderer>();
        if (objRenderer != null)
        {
            objRenderer.material.color = color;
        }
    }

}
