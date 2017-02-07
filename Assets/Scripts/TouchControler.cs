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
    static public bool Shoot;
    
    static private TouchControler Instance;
    private Vector2 touchValue;
    private Vector2 centerPad;
    private Vector2 centerShoot;
    private int size;

    public Texture2D PadTexture;
    public Texture2D CursorTexture;

	void Start () {
        TouchControler.Instance = this;
        size = Mathf.Min(Screen.width / 3, Screen.height / 3);
        centerPad.x = Screen.width - size / 2;
        centerPad.y = size / 2;
        centerShoot.x = size / 2;
        centerShoot.y = size / 2;
	}
	
	void Update ()
    {
        Shoot = false;
        touchValue = Vector2.zero;

        if (Application.platform == RuntimePlatform.Android)
        {
            this.MobileUpdate();
        }
		else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            this.DesktopUpdate();
        }
    }

    public void MobileUpdate()
    {
        int fingersCount = Input.touchCount;
        for (int i = 0; i < fingersCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (Vector2.SqrMagnitude(touch.position - centerPad) < (size / 2 * 1.5f) * (size / 2 * 1.5f))
            {
                touchValue = (touch.position - centerPad) / size * 2f;
                if (touchValue.sqrMagnitude > 1f)
                {
                    touchValue = touchValue.normalized;
                }
            }
            else if (Vector2.SqrMagnitude(touch.position - centerShoot) < (size / 4) * (size / 4))
            {
                Shoot = true;
            }
        }
    }

    public void DesktopUpdate()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        if (Vector2.SqrMagnitude(mousePosition - centerPad) < (size / 2 * 1.5f) * (size / 2 * 1.5f))
        {
            touchValue = (mousePosition - centerPad) / size * 2f;
            if (touchValue.sqrMagnitude > 1f)
            {
                touchValue = touchValue.normalized;
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Shoot = true;
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
        GUI.Label(new Rect(centerShoot.x - size / 4, Screen.height - centerShoot.y - size / 4, size / 2, size / 2), this.CursorTexture);
        GUI.Label(new Rect(centerPad.x - size / 2, Screen.height - centerPad.y - size / 2, size, size), this.PadTexture);
        GUI.Label(new Rect(centerPad.x - size / 4 + touchValue.x * size / 4, Screen.height - centerPad.y - size / 4 - touchValue.y * size / 4, size / 2, size / 2), this.CursorTexture);
    }
}
