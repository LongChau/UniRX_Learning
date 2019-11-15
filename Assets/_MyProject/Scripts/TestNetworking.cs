using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public class TestNetworking : MonoBehaviour
{
    CompositeDisposable _disposables = new CompositeDisposable(); // field

    // Start is called before the first frame update
    void Start()
    {
        //ObservableWWW.Get("http://google.co.jp/")
        //    .Subscribe(
        //        x => OnNext(x), // onSuccess
        //        ex => OnError(ex)); // onError

        //Rx is composable and cancelable.You can also query with LINQ expressions:
        // composing asynchronous sequence with LINQ query expressions
        var query = from google in ObservableWWW.Get("http://google.com/")
                    from bing in ObservableWWW.Get("http://bing.com/")
                    //from unknown in ObservableWWW.Get(google + bing)
                    //select new { google, bing, unknown };
                    select new { google, bing };

        var cancel = query.Subscribe(x => OnNext(x.google, x.bing));

        //var cancel = query.Subscribe(x => OnNext(x.google, x.bing));

        // Call Dispose is cancel.
        //cancel.Dispose();

        _disposables.Add(cancel);
    }

    [ContextMenu("StartNetworking")]
    private void StartNetworking()
    {
        var query = from google in ObservableWWW.Get("http://google.com/")
                    from bing in ObservableWWW.Get("http://bing.com/")
                        //from unknown in ObservableWWW.Get(google + bing)
                        //select new { google, bing, unknown };
                    select new { google, bing };

        var cancel = query.Subscribe(x => OnNext(x.google, x.bing));

        _disposables.Add(cancel);
    }

    private void OnNext(string x)
    {
        Debug.Log(x.Substring(0, 100)); // onSuccess
    }

    private void OnNext(string x, string y)
    {
        Debug.Log(x.Substring(0, 100)); // onSuccess
        Debug.Log(y.Substring(0, 100)); // onSuccess

        _disposables.Dispose();
    }

    private void OnError(Exception ex)
    {
        Debug.LogException(ex); // onError
    }
}
