using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class MyJoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    // ** ������ ���
    [SerializeField] private Transform Target;

    // ** ���� ������ ���̽�ƽ
    [SerializeField] private RectTransform Stick;

    // ** ���̽�ƽ�� ��� (��ƽ�� ������ �� �ִ� �ִ�ݰ����� ���)
    [SerializeField] private RectTransform BackBoard;

    [Header("Look at")]
    [Tooltip("Look at the direction of the joystick")]
    // ** Target�� ���̽�ƽ�� ����Ű�� ������ �ٶ��� ������.
    [SerializeField] private bool Look = true;

    [Header("2D and 3D")]
    [Tooltip("Horizontal & Vertical")]
    // ** ��鿡�� �����̴� ��������, �������� �����̴� �������� ����.
    [SerializeField] private bool HorizontalSpace = true;

    // ** ������(���̽�ƽ�� ������ �� �ִ� �ִ� �ݰ��� ���ϱ� ����.)
    private float Radius;

    // ** TouchCheck �Է� Ȯ��.
    private bool TouchCheck = false;

    // ** �̵��ӵ�.
    private float Speed = 5.0f;

    // ** ���Ⱚ
    private Vector2 Direction;

    // ** Direction * Speed * Time.deltaTime;
    // ** �� ��ŭ �������������� ���� ��.
    private Vector3 Movement;
    



    private float Angle;


    void Awake()
    {
        /* [BackBoard]
         * "BackBoard" Image UI ����  =>  [OutLineCircle.png]
         * size : (x:256, y:256)
         * anchor position : (x:192, y:192)
         * 
         * [Stick]
         * BackBoard ������ "Stick" Image UI ����  => [FilledCircle.png]
         * size : (x:128, y:128)
         * anchor position : (x:0, y:0)
         */

        RectTransform rectTransPos = transform.gameObject.AddComponent<RectTransform>();

        BackBoard = transform.Find("BackBoard").GetComponent<RectTransform>();
        Stick = transform.Find("BackBoard/Stick").GetComponent<RectTransform>();

        Target = GameObject.Find("Player").transform;
    }


    void Start()
    {
        // ** BackBoard�� �������� ����.
        Radius = BackBoard.rect.width / 2;

        Angle = 0.0f;
    }

    void Update()
    {
        if (TouchCheck)
            Target.position += Movement;
    }

    void OnTouch(Vector2 vecTouch)
    {
        Stick.localPosition = new Vector2(vecTouch.x - BackBoard.position.x, vecTouch.y - BackBoard.position.y);

        // ** ClampMagnitude �Լ��� Vector�� ���̸� �����Ѵ�.
        // ** Stick.localPosition�� ���� Radius���� Ŀ���� �ʵ��� �ϰ���.
        Stick.localPosition = Vector2.ClampMagnitude(Stick.localPosition, Radius);

        Debug.Log(Stick.localPosition);

        // ** ��ƽ�� ����� �߽����� ���� �� ��ŭ�� ������ ���������� Ȯ��.
        // ** ��ƽ�� ��濡�� ������ ����.
        // ** Ratio = ��ƽ�� ������ ����.
        float Ratio = (BackBoard.position - Stick.position).sqrMagnitude / (Radius * Radius);

        // ** ��ƽ�� ����Ű�� ������ ����.
        Direction = Stick.localPosition.normalized;

        Target.transform.rotation = Quaternion.Euler(0.0f, Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg, 0.0f);

        // ** Target�� ������ ����� �ӵ��� ����.
        if (HorizontalSpace)
        {
            // ** Horizontal Space
            Movement = new Vector3(
                Direction.x * Speed * Time.deltaTime * Ratio,
                0f,
                Direction.y * Speed * Time.deltaTime * Ratio);
        }
        else
        {
            // ** Vertical Space
            Movement = new Vector3(
                Direction.x * Speed * Time.deltaTime * Ratio,
                Direction.y * Speed * Time.deltaTime * Ratio,
                0f);
        }
    }

    /****************************************************
     * Ŭ�� �Ǵ� ��ġ
     * nDrag      : ��ư�� �������� �ִ� ���� ��� �����
     * OnPointerUp      : ��ư�� ������ ���� �����
     * OnPointerDown    : ��ư�� ���� ���� �����
     * ������� : eventData = ��ġ�� ��Ÿ��.
     * ����
     * ���� ��ũ��Ʈ�� ���� ��ġ������ �۵��ϸ�, ���� 
     * ��ũ��Ʈ�� ���̽�ƽ�� �۵���Ű�� ���� ������� 
     * ��ũ��Ʈ�̴�. ���̽�ƽ�� Canvas������ ������� ���̸�,
     * ���� ��ũ��Ʈ�� Canvas�� ���������� Canvas������ ����
     * UI������ �����ϰ� �ȴ�.
    ****************************************************/
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log(eventData.ToString());
        OnTouch(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(eventData.ToString());
        OnTouch(eventData.position);
        TouchCheck = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log(eventData.ToString());
        Stick.localPosition = Vector2.zero;
        TouchCheck = false;
    }
}