using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField] private int numberOfPoint;
    [SerializeField] private string textOfPoint;
    [SerializeField] private Canvas canvasReference;
    [SerializeField] private Sprite canvasSprite;
    [SerializeField] private Font canvasFont;
    [SerializeField] private GameObject prefabPointOfInterest;
    [SerializeField] private GameObject prefabTextPanel;
    private GameObject _canvasImage;
    private GameObject _canvasTextPanel;


    void ToggleAvitvity(GameObject _gm)
    {
        if (_gm.activeSelf){
            _gm.SetActive(false);
        }
        else{
            _gm.SetActive(true);
        }
    }

    void Start()
    {
        _canvasImage = Instantiate(prefabPointOfInterest, canvasReference.transform, false);
        //_canvasImage.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
        _canvasImage.name = numberOfPoint + "_image";

        _canvasTextPanel = Instantiate(prefabTextPanel, _canvasImage.transform, false);
        _canvasTextPanel.SetActive(false);
        //_canvasTextPanel.transform.localScale
        _canvasTextPanel.name = numberOfPoint + "_panelText";


        _canvasImage.GetComponent<Button>().onClick.AddListener(delegate { ToggleAvitvity(_canvasTextPanel); });
        _canvasImage.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = numberOfPoint.ToString();

        _canvasImage.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = textOfPoint;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = gameObject.transform.position;  // get the game object position
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);  //convert game object position to VievportPoint
 
        // set MIN and MAX Anchor values(positions) to the same position (ViewportPoint)
        _canvasImage.GetComponent<RectTransform>().anchorMin = viewportPoint;
        _canvasImage.GetComponent<RectTransform>().anchorMax = viewportPoint;
    }
}
