using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 0.5f;
    public Transform _transform;
    public Text loadingText;
    Vector3 rot;
    public static LoadingView Instance;

    private void Awake()
    {
        if( Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Show(string txt = "")                                                                         
    {
        transform.GetChild(0).gameObject.SetActive(true);
        if( txt != "")
        {
            loadingText.text = txt;
        }
        else
        {
            loadingText.text = "Connecting to server...";
        }
    }

    public void Hide()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    void Start()
    {
        rot = _transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        rot.z += speed;
        _transform.eulerAngles = rot;
    }
}
