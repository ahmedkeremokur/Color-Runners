using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform joystickBackground; // Joystick'in arka planý
    public RectTransform joystickHandle;    // Joystick'in tutacaðý

    public float moveSpeed = 7; // Karakterin ileri-geri hareket hýzý
    public float rotationSpeed = 150f; // Karakterin sað-sol dönüþ hýzý

    public Transform player; // Hareket ettirilecek karakter

    private Vector2 inputVector; // Joystick'in hareket vektörü

    private int animState;    //0: Idle, 1: Running front, 2: Back, 9: Hiphop
    private Animator _animator;

    ScalenFiller scalenFiller;
    void Start()
    {
        _animator = player.GetComponent<Animator>();
        scalenFiller = GameObject.FindGameObjectWithTag("English Man").GetComponent<ScalenFiller>();

        
    }

    void Update()
    {
        Debug.Log("X: " + inputVector.x + "   Y: " + inputVector.y);
     
        // Ýleri-geri hareket
        if (inputVector.y != 0)
        {
            player.Translate(Vector3.forward * inputVector.y * moveSpeed * Time.deltaTime);
            if (inputVector.y > 0)
            {
                animState = 1;
            }
            else if(inputVector.y < 0)
            {
                animState = 2;
            }
            _animator.SetInteger("animationint", animState);
        }

        // Sað-sol dönüþ
        if (inputVector.x != 0)
        {
            player.Rotate(Vector3.up * inputVector.x * rotationSpeed * Time.deltaTime);           
        }

        if (inputVector.y == 0)
        {
            animState = 0;
            _animator.SetInteger("animationint", animState);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // Joystick'in hareketini algýla
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out position);

        position = Vector2.ClampMagnitude(position, joystickBackground.sizeDelta.x / 2);
        joystickHandle.anchoredPosition = position;

        // Joystick yönünü normalize et
        inputVector = position / (joystickBackground.sizeDelta.x / 2);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickHandle.anchoredPosition = Vector2.zero; // Joystick'i sýfýrla
        inputVector = Vector2.zero; // Hareketi sýfýrla
    }
}
