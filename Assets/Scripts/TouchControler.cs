using UnityEngine;
using System.Collections;

public class TouchControler : MonoBehaviour {

    static public Vector2 TouchValue
    {
        get
        {
            return Instance.touchValue;
        }
    }
    
    static private TouchControler Instance;
    private Vector2 touchValue;
    private Vector2 center;
    private int size;

    public Texture2D PadTexture;
    public Texture2D CursorTexture;

	void Start () {
        TouchControler.Instance = this;
        size = Mathf.Min(Screen.width / 2, Screen.height / 4);
        center.x = Screen.width - size / 2;
        center.y = size / 2;
	}
	
	void Update () {
        /*
        int fingersCount = Input.touchCount;
        for (int i = 0; i < fingersCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (Vector2.SqrMagnitude(touch.position - center) < size * size)
            {
                touchValue = (touch.position - center) / size / 2f;
            }
        }
         * */

        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (Vector2.SqrMagnitude(mousePosition - center) < (size / 2 * 1.5f) * (size / 2 * 1.5f))
        {
            touchValue = (mousePosition - center) / size * 2f;
            if (touchValue.sqrMagnitude > 1f)
            {
                touchValue = touchValue.normalized;
            }
        }
        else
        {
            touchValue = Vector2.zero;
        }
	}

    public void OnGUI()
    {
        /*GUILayout.Label("X0 : " + center.x);
        GUILayout.Label("Y0 : " + center.y);
        GUILayout.Label("Mouse X : " + Input.mousePosition.x);
        GUILayout.Label("Mouse Y : " + Input.mousePosition.y);
        GUILayout.Label("X : " + touchValue.x);
        GUILayout.Label("Y : " + touchValue.y);*/
        GUI.Label(new Rect(center.x - size / 2, Screen.height - center.y - size / 2, size, size), this.PadTexture);
        GUI.Label(new Rect(center.x - size / 4 + touchValue.x * size / 4, Screen.height - center.y - size / 4 - touchValue.y * size / 4, size / 2, size / 2), this.CursorTexture);
    }
}
