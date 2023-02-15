using UnityEngine;

public class CursorController : MonoBehaviour
{
    // Кастомные курсоры
    [SerializeField] private Texture2D _cursor;
    [SerializeField] private Texture2D _cursorClicked;
    
    // InputActions для мыши
    private CursorControls _cursorControls;
    
    // Камера
    private Camera _mainCamera;
    
    // Текущий выбранный объект
    private Target _target;

    /* На Awake настраиваем главную камеру и CursorControls,
    а также меняем вид курсора */
    private void Awake()
    {
        _mainCamera = Camera.main;
        _cursorControls = new CursorControls();
        
        ChangeCursor(_cursor);
    }
    
    // Подписываемся на функции для started и performed для клика мышкой
    private void Start()
    {
        _cursorControls.Mouse.Click.started += _ => StartedClick();
        _cursorControls.Mouse.Click.performed += _ => EndedClick();
    }

    // Начало клика
    private void StartedClick()
    {
        ChangeCursor(_cursorClicked);
    }

    // Конец клика, где мы заполучаем кликнутый объект
    private void EndedClick()
    {
        ChangeCursor(_cursor);
        DetectObject();
    }

    // Включаем контроли при включении скрипта
    private void OnEnable()
    {
        _cursorControls.Enable();
    }

    // Отключаем контроли при отключении скрипта
    private void OnDisable()
    {
        _cursorControls.Disable();
    }

    // Меняем внешний вид курсора на иную текстуру
    private void ChangeCursor(Texture2D cursorType)
    {
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
    }

    /* Получаем объект взависимости от Raycast,
     если он столкнулся с объектом, вызываем его метод OnMouseClick
     если условия выполнены, в противном случае вызываем OnMouseAway */
    private void DetectObject()
    {
        Ray ray = _mainCamera.ScreenPointToRay(_cursorControls.Mouse.Position.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                Target target = hit.collider.gameObject.GetComponent<Target>();

                if (_target != null && (target == null || target != _target))
                {
                    _target.OnMouseAway();
                }
                
                if (target != null)
                {
                    _target = target;
                    _target.OnMouseClick();
                }
            }
        }
        else
        {
            if (_target != null)
            {
                _target.OnMouseAway();
            }
        }
    }
}