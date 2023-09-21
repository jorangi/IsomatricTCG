using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public bool focus;
    Vector3 mousePos;
    bool middleButtonDown;
    private const int CAMMAX = 1;
    private const int CAMMIN = 3;
    public Character curCha;
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = CAMMAX;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cameraMoveable= true;
            mousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            cameraMoveable= false;
        }
    }
    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            float size = cam.orthographicSize;
            Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - Input.mouseScrollDelta.y, CAMMAX, CAMMIN);
            size = cam.orthographicSize;
            Transform tr += mouse - cam.ScreenToWorldPoint(Input.mousePosition);
            tr.position = new(Mathf.Clamp(tr.position.x, -(CAMMIN - size) * (12.5f / 7), (CAMMIN - size) * (12.5f / 7)), Mathf.Clamp(tr.position.y, -(CAMMIN - size), (CAMMIN - size)), -CAMMIN);
        }
        if (cameraMoveable&& !focus)
        {
            Transform tr = cam.transform;
            float size = cam.orthographicSize;
            Vector2 vector2 = cam.ScreenToViewportPoint(mousePos - Input.mousePosition);
            tr.Translate((CAMMIN - cam.orthographicSize) * 2 * Time.deltaTime * vector2);
            tr.position = new(Mathf.Clamp(tr.position.x, -(CAMMIN - size) * (12.5f / 7), (CAMMIN - size) * (12.5f / 7)), Mathf.Clamp(tr.position.y, -(CAMMIN - size), (CAMMIN - size)), -CAMMIN);
        }
        if (focus)
        {
            Transform tr = cam.transform;
            float size = cam.orthographicSize;
            cam.transform.position = new(curCha.transform.position.x, curCha.transform.position.y, -10);
            tr.position = new(Mathf.Clamp(tr.position.x, -(CAMMIN - size) * (12.5f / 7), (CAMMIN - size) * (12.5f / 7)), Mathf.Clamp(tr.position.y, -(CAMMIN - size), (CAMMIN - size)), -CAMMIN);
        }
    }
}
