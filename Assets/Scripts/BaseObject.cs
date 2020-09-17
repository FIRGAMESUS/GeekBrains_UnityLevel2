using UnityEngine;

/// <summary>
/// Базовый класс, кэширующий ссылки на компоненты объекта
/// </summary>
public abstract class BaseObject : MonoBehaviour
{

    private Transform _goTransform;
    private GameObject _goInstance;
    private string _name;

    private Rigidbody _rigibody;

    private Material _material;
    private Color _color;
    private Animator _animator;

    private bool _isVisible;

    #region Properties
    public Transform GoTransform { get => _goTransform; set => _goTransform = value; }
    public GameObject GoInstance { get => _goInstance; set => _goInstance = value; }
    public string Name { get => _name; set => _name = value; }
    public Rigidbody GORigibody { get => _rigibody; set => _rigibody = value; }
    public Material GOMaterial { get => _material; set => _material = value; }
    public Color Color { get => _color; set => _color = value; }
    public Animator GOAnimator { get => _animator; set => _animator = value; }


    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }

        private set
        {
            _isVisible = value;
        }

    }

    #endregion



    protected virtual void Awake()
    {
        _goInstance = gameObject;
        _goTransform = gameObject.transform;
        _name = gameObject.name;
        
        if(GetComponent<Rigidbody>())
        {
            _rigibody = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.Log("BaseObject: " + _name + " NO RIGIDBODY");
           // gameObject.AddComponent<Rigidbody>();///????
        }

        if (GetComponent<Animator>())
        {
            _animator = GetComponent<Animator>();
        }
        else
        {
            Debug.Log("BaseObject: " + _name + " NO ANIMATOR");
            gameObject.AddComponent<Animator>();///????
        }

        if (GetComponent<Renderer>())
        {
            _material = GetComponent<Renderer>().material;
        }
    }
}
