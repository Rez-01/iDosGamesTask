using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Текстовые объекты
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _position;
    [SerializeField] private TMP_Text _rotation;
    
    // Визуальные характеристики
    private Renderer _renderer;
    private Color _originalColor;
    
    // Контролирует возможность манипулирования объекта
    private bool _transformable;
    
    // Скорость вращения и движения
    private float _rotationSpeed = 1f;
    private float _movementSpeed = 5f;

    // Изначальные позиция и вращение
    private Vector3 _defaultPosition;
    private Vector3 _defaultRotation;
    
    /* В Awake мы записываем все характеристики по-умолчанию,
     к которым мы будем возвращаться во время сбросов */
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
        
        _defaultPosition = transform.position;
        _defaultRotation = transform.rotation.eulerAngles;
    }

    /* В Update мы меняем позицию при зажатии кнопок или сбрасываем значения,
     только если с объектом можно взаимодействовать (Perspective Axis в этом случае кнопки Q и E) */
    private void Update()
    {
        if (_transformable)
        {
            transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 
                Input.GetAxis("Vertical"), Input.GetAxis("Perspective")) * (_movementSpeed * Time.deltaTime));

            if (Input.GetKey(KeyCode.Space))
            {
                transform.position = _defaultPosition;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.rotation = Quaternion.Euler(_defaultRotation);
            }

            _position.text = "Position: " + transform.position;
            _rotation.text = "Rotation: " + transform.rotation.eulerAngles;
        }
    }

    /* Когда мы нажимаем мышкой на объект,
     он вызывает эту функцию (объект меняет цвет и разрешается его изменение),
     а также включаются текстовые элементы */
    public void OnMouseClick()
    {
        _renderer.material.color = Color.red;
        
        _name.text = name;

        _transformable = true;
        ActivateTexts(true);
    }

    /* Когда мы нажимаем мышкой на какой-либо другой объект (или на пустое пространство),
     предыдуще выбранный объект вызывает эту функцию (объект возвращает цвет и запрещается его изменение),
     а также отключаются текстовые элементы */
    public void OnMouseAway()
    {
        _renderer.material.color = _originalColor;

        _transformable = false;
        ActivateTexts(false);
    }

    /* При зажатии мышки и его передвижении,
     выбранный объект вращается (если его можно изменять) */
    private void OnMouseDrag()
    {
        if (_transformable)
        {
            float xRotation = Input.GetAxis("Mouse X") * _rotationSpeed;
            float yRotation = Input.GetAxis("Mouse Y") * _rotationSpeed;

            transform.Rotate(Vector3.down, xRotation);
            transform.Rotate(Vector3.right, yRotation);
        }
    }

    /* Контролирует включение и отключение текстовых объектов*/
    private void ActivateTexts(bool active)
    {
        _name.gameObject.SetActive(active);
        _position.gameObject.SetActive(active);
        _rotation.gameObject.SetActive(active);
    }
}