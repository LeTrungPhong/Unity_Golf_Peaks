using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    //private Rigidbody rb;
    private ObstacleManager obstacleManager;
    [SerializeField] private Button buttonT;
    [SerializeField] private Button buttonB;
    [SerializeField] private Button buttonL;
    [SerializeField] private Button buttonR;
    //[SerializeField] private float force = 1;


    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        obstacleManager = GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>();
        buttonT.onClick.AddListener(OnButtonTClick);
        buttonB.onClick.AddListener(OnButtonBClick);
        buttonL.onClick.AddListener(OnButtonLClick);
        buttonR.onClick.AddListener(OnButtonRClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

//      Vector3.up(0, 1, 0)	Lên trên(trục Y)
//      Vector3.down(0, -1, 0)	Xuống dưới(trục Y)
//      Vector3.left(-1, 0, 0)	Trái(trục X)
//      Vector3.right(1, 0, 0)	Phải(trục X)
//      Vector3.forward(0, 0, 1)	Tiến lên(trục Z)
//      Vector3.back(0, 0, -1)	Lùi lại(trục Z)

    void OnButtonTClick()
    {
        // -z
        //rb.AddForce(Vector3.back * force, ForceMode.Impulse);
    }

    void OnButtonBClick()
    {
        // +z
        //rb.AddForce(Vector3.forward * force, ForceMode.Impulse);
    }

    void OnButtonLClick()
    {
        // +x
        //rb.AddForce(Vector3.right * force, ForceMode.Impulse);
    }

    void OnButtonRClick()
    {
        // -x
        //rb.AddForce(Vector3.left * force, ForceMode.Impulse);
    }
}
