using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MapGeneratorBenchmark
{

    Stopwatch watch;

    public MapGeneratorBenchmark() {
        watch = new Stopwatch();
    }

    public void Start() {
        watch.Start();
    }

    public void Stop() {
        watch.Stop();
    }

    public string PrintTime() {
        return "Elapsed Time: " + watch.ElapsedMilliseconds + "ms";
    }

    public float GetTimeMs() {
        return watch.ElapsedMilliseconds;
    }

}
