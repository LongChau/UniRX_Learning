using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TestMultithread : MonoBehaviour
{
    private CompositeDisposable _disposables = new CompositeDisposable();

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        // Observable.Start is start factory methods on specified scheduler
        // default is on ThreadPool
        var heavyMethod = Observable.Start(() =>
        {
            // heavy method...
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            return 10;
        });

        var heavyMethod2 = Observable.Start(() =>
        {
            // heavy method...
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
            return 10;
        });

        // Join and await two other thread values
        var threadObservable = Observable.WhenAll(heavyMethod, heavyMethod2)
            .ObserveOnMainThread() // return to main thread
            .Subscribe(xs =>
            {
                // Unity can't touch GameObject from other thread
                // but use ObserveOnMainThread, you can touch GameObject naturally.
                //(GameObject.Find("myGuiText")).GetComponent<GUIText>.text = xs[0] + ":" + xs[1];
                Debug.Log($"{xs[0]} : {xs[1]}");
            });

        _disposables.Add(threadObservable);
    }
}
