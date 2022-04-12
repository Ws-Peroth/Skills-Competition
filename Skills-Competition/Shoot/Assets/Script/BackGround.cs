using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{

    [SerializeField] private List<Sprite> sources = new List<Sprite>();
    
    [SerializeField] private Image _image;
    public int bgNumber;

    public void SetImage(int stage)
    {
        _image.sprite = sources[stage];
    }

    private void FixedUpdate()
    {
        if (bgNumber == 0)
        {
            _image.color = GameManager.Instance.Stage == 2 ? new Color(1, 1, 1, 0.73f) : Color.white;
            return;
        }
        
        transform.Translate(Vector3.down * BackGroundManager.Instance.BgSpeed[bgNumber]);

        if (transform.position.y < BackGroundManager.EndY)
        {
            transform.position = new Vector3(0, BackGroundManager.StartY, 0);
        }
    }
}
