using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private GameObject canvas;
    // Start is called before the first frame update

    private void Awake()
    {
        
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buttonPlayClicked()
    {
        SceneManager.LoadScene("Level");
    }

    public void buttonCreateFileClicked()
    {
        
    }
}
