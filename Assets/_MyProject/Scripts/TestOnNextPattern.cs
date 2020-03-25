using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class TestOnNextPattern : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Test_LoadLevel();
    }

    private static void Test_LoadLevel()
    {
        // use case
        Application.LoadLevelAsync("testscene")
            .AsObservable()
            .Do(x => Debug.Log(x)) // output progress
            .Last() // last sequence is load completed
            .Subscribe();
    }

    private IEnumerator RunAsyncOperation(UnityEngine.AsyncOperation asyncOperation, IObserver<float> observer, CancellationToken cancellationToken)
    {
        while (!asyncOperation.isDone && !cancellationToken.IsCancellationRequested)
        {
            observer.OnNext(asyncOperation.progress);
            yield return null;
        }
        if (!cancellationToken.IsCancellationRequested)
        {
            observer.OnNext(asyncOperation.progress); // push 100%
            observer.OnCompleted();
        }
    }
}