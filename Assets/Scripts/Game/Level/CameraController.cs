using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform sun;
    public Camera mainCam { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
    }

    public void DisableTheCamera()
    {
        sun.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
