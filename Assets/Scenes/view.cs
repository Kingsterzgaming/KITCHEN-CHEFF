using UnityEngine;
using UnityEngine.UI;

public class view : MonoBehaviour
{
    public Canvas canvas;
    public GameObject canvas1;
    public GameObject Gameobject;
    public Button play;
    public Button quit;
    public Button settings;
    string name;
    void OnValidate()
    {

        if (canvas != null)
        {
            var t = canvas.transform.Find("canvas");
            if (t != null)
            {
                var b = t.GetComponent<Button>();
                if (b != null) return b;
            }
        }

        var go = GameObject.Find(name);
        if (go != null) return go.GetComponent<Button>();
        return null;
    }
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
