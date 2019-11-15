using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    private CompositeDisposable _disposables = new CompositeDisposable();

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
        //var cancel = Observable.FromCoroutine(AsyncA)
        //    .SelectMany(AsyncB)
        //    .SelectMany(AsyncD)
        //    .SelectMany(AsyncC)
        //    .Subscribe();
        //_disposables.Add(cancel);

        // you can stop a coroutine by calling your subscription's Dispose.
        //cancel.Dispose();

        //var testCustomYeild = Observable.FromCoroutine(TestNewCustomYieldInstruction).Subscribe();
        //_disposables.Add(testCustomYeild);

        //var testObserved = transform.ObserveEveryValueChanged(myTransForm => myTransForm.position)
        //    .FirstOrDefault(position => position.y >= 4f)
        //    .Subscribe();

        //_disposables.Add(testObserved);

        //var getObserved = GetWWW("http://unity3d.com/").Subscribe();
        //_disposables.Add(getObserved);
    }

    // public method
    public IObservable<string> GetWWW(string url)
    {
        // convert coroutine to IObservable
        return Observable.FromCoroutine<string>((observer, cancellationToken) => GetWWWCore(url, observer, cancellationToken));
    }

    // IObserver is a callback publisher
    // Note: IObserver's basic scheme is "OnNext* (OnError | Oncompleted)?" 
    private IEnumerator GetWWWCore(string url, IObserver<string> observer, CancellationToken cancellationToken)
    {
        var www = new UnityEngine.WWW(url);
        while (!www.isDone && !cancellationToken.IsCancellationRequested)
        {
            yield return null;
        }

        if (cancellationToken.IsCancellationRequested) yield break;

        if (www.error != null)
        {
            observer.OnError(new Exception(www.error));
        }
        else
        {
            observer.OnNext(www.text);
            observer.OnCompleted(); // IObserver needs OnCompleted after OnNext!

            Debug.Log(www.text);
        }
    }

    private IEnumerator TestNewCustomYieldInstruction()
    {
        // wait Rx Observable.
        yield return Observable.Timer(TimeSpan.FromSeconds(1)).ToYieldInstruction();
        Debug.Log(1);
        // you can change the scheduler(this is ignore Time.scale)
        yield return Observable.Timer(TimeSpan.FromSeconds(1), Scheduler.MainThreadIgnoreTimeScale).ToYieldInstruction();
        Debug.Log(2);
        // get return value from ObservableYieldInstruction
        var observe = ObservableWWW.Get("http://unity3d.com/").ToYieldInstruction(throwOnError: false);
        yield return observe;

        if (observe.HasError) { Debug.Log(observe.Error.ToString()); }
        if (observe.HasResult) { Debug.Log(observe.Result); }
        Debug.Log("Done get link");

        // other sample(wait until transform.position.y >= 4)
        // observe object transform whenever >= 4 with ObserveEveryValueChanged
        yield return this.transform.ObserveEveryValueChanged(myTransForm => myTransForm.position).FirstOrDefault(p => p.y >= 4f).ToYieldInstruction();
        Debug.Log("Done transform");
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
        _disposables.Clear();
    }
}
