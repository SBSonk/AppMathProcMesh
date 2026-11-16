using UnityEngine;

public class PerspectiveCamera : MonoBehaviour
{
    public static PerspectiveCamera Instance { get; private set; }

    [Header("Projection Settings")]
    [Range(1f, 500f)]
    [SerializeField] private float focalLength = 20f;

    public float FocalLength
    {
        get => focalLength;
        set => focalLength = Mathf.Clamp(value, 1f, 500f);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}