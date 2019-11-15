using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TestUniRX : MonoBehaviour
{
    private int mouseCount = 0;

    [SerializeField]
    private GameObject _cube;
    [SerializeField]
    private GameObject _sphere;

    CompositeDisposable _disposables = new CompositeDisposable(); // field

    private bool _isEnable;

    //public bool IsEnable
    //{
    //    get
    //    {
    //        //return gameObject != null && gameObject.activeSelf; // will occur error
    //        return IsEnable;
    //    }
    //    set
    //    {
    //        IsEnable = value;
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        _isEnable = gameObject != null && gameObject.activeSelf;
        //IsEnable = gameObject != null && gameObject.activeSelf;
        ClickStream();
    }

    private void ClickStream()
    {
        var clickStream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Where(_ =>
            {
                //return gameObject != null && gameObject.activeSelf;    //cannot use due to variable capture -> gameObject was destroy but trying to access it.
                return _isEnable;
            });

        clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
            .Where(clicks => clicks.Count == 2)
            .Subscribe(clicks =>
            {
                Debug.Log("DoubleClick Detected! Count:" + clicks.Count);
                var go = Instantiate(_cube, new Vector3(0f, 4f, 0f), Quaternion.identity);
                go.GetComponent<MeshRenderer>().material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            });

        clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(450)))
           .Where(clicks => clicks.Count == 3)
           .Subscribe(clicks =>
           {
               Debug.Log("TrippleClick Detected! Count:" + clicks.Count);
               var go = Instantiate(_sphere, new Vector3(0f, 4f, 0f), Quaternion.identity);
               go.GetComponent<MeshRenderer>().material.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
           });
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //if (Input.GetMouseButtonDown(0))
    //    //{
    //    //    mouseCount++;
    //    //    Debug.Log($"Mouse Clicked {mouseCount}");
    //    //}
    //}

    //private void OnDisable()
    //{
    //    _disposables.Dispose();
    //}

    private void OnDestroy()
    {
        _isEnable = false;
        _disposables.Dispose();
        _disposables.Clear();
    }
}
