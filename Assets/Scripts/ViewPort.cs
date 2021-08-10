using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ViewPort : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 8f;    //Скорость поворота камеры
    [SerializeField] private float zoomDistance = 3f;    //Дистанция до таргета
    
    [SerializeField] private Transform target;    //Gameobject для центрирования камеры
    
    [SerializeField] private float lerpConst = 0.18f;    //Константа для сглаживания поворота
    [SerializeField] private float zoomConst = -24f;    //Множитель ввода оси зума
    [SerializeField] private Vector2 zoomBounds = new Vector2(1.8f,3.6f); //Границы зума
    [SerializeField] private Vector2 cameraRotateBounds = new Vector2(-90f,135f);

    [SerializeField] private Material outlineMaterial;
    
    private Vector2 _pointerInput = new Vector2(150.4f, 4.8f);
    private GameObject _parentGameObject;
    private GameObject _hoveredGameObject;
    private GameObject _outlineGameObject;
    
    private void Start()
    {
        _parentGameObject = transform.parent.gameObject;
    }

    GameObject PhysicRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;
         
        if(Physics.Raycast( ray, out hit, 100))
        {
            return hit.transform.gameObject;
        }

        else
        {
            return null;
        }
    }
    
    void Update()
    {
        if (Input.GetButton("Fire1")){
            
            _pointerInput += new Vector2(Input.GetAxis("Mouse X") * rotationSpeed, Input.GetAxis("Mouse Y") * -rotationSpeed);}
        
        _pointerInput.y = Mathf.Clamp(_pointerInput.y,cameraRotateBounds.x,cameraRotateBounds.y);

        if (Input.GetAxis("Mouse ScrollWheel") != 0){
            zoomDistance += zoomConst * Input.GetAxis("Mouse ScrollWheel");
            zoomDistance = Mathf.Clamp(zoomDistance, zoomBounds.x, zoomBounds.y);
        }
    }

    public void LateUpdate(){
        //TODO: Добавить интреполяцию движения камеры
        //TODO: Добавить разные иконки курсоров
        //TODO: Ограничить поворот камеры ниже горизонта
        
        //Поворот камеры в пространстве
        _parentGameObject.transform.rotation = Quaternion.Lerp(_parentGameObject.transform.rotation, Quaternion.Euler(_pointerInput.y, _pointerInput.x, 0), lerpConst);
        
        //Зум
        //Говно, переписать
        transform.localPosition = target.position - (transform.localRotation * Vector3.forward * zoomDistance);
        
        //_parentGameObject.transform.rotation.eulerAngles = Mathf.Clamp(_parentGameObject.transform.rotation.eulerAngles.x, -90f, 90f);
        /*transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(_pointerInput.y, _pointerInput.x, 0), lerpConst);
        transform.localPosition = target.position - (transform.localRotation * Vector3.forward * zoomDistance);
        //print(Mathf.Clamp(transform.rotation.x,0,1));
        print(transform.rotation.eulerAngles);*/
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
