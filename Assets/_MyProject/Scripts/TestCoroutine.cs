using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // two coroutines
        IEnumerator AsyncA()
        {
            Debug.Log("a start");
            yield return new WaitForSeconds(3);
            Debug.Log("a end");
        }

        IEnumerator AsyncB()
        {
            Debug.Log("b start");
            yield return new WaitForEndOfFrame();
            Debug.Log("b end");
        }

        IEnumerator AsyncC()
        {
            Debug.Log("c start");
            yield return new WaitForEndOfFrame();
            Debug.Log("c end");
        }

        IEnumerator AsyncD()
        {
            Debug.Log("d start");
            yield return new WaitForSecondsRealtime(5);
            Debug.Log("d end");
        }

        // main code
        // Observable.FromCoroutine converts IEnumerator to Observable<Unit>.
        // You can also use the shorthand, AsyncA().ToObservable()

        // after AsyncA completes, run AsyncB as a continuous routine.
        // UniRx expands SelectMany(IEnumerator) as SelectMany(IEnumerator.ToObservable())
        var cancel = Observable.FromCoroutine(AsyncA)
            .SelectMany(AsyncB)
            .SelectMany(AsyncD)
            .SelectMany(AsyncC)
            .Subscribe();

        // you can stop a coroutine by calling your subscription's Dispose.
        //cancel.Dispose();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
