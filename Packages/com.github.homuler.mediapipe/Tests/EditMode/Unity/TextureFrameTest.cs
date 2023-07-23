// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Threading;
using Mediapipe.Unity.Experimental;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Mediapipe.Tests
{
  public class TextureFramePoolTest
  {
    #region TryGetTextureFrame
    [Test, Performance]
    public void TryGetTextureFrame_Performance_When_5_Threads_Running()
    {
      var textureFramePool = new TextureFramePool(640, 480, TextureFormat.RGBA32, 20);
      Measure.Method(() =>
      {
        var i = 0;
        while (i < 100)
        {
          if (!textureFramePool.TryGetTextureFrame(out var textureFrame))
          {
            Thread.Sleep(1);
            continue;
          }
          i++;
          new Thread(() =>
          {
            textureFrame.Release();
          }).Start();
        }
      }).WarmupCount(1)
      .MeasurementCount(20)
      .IterationsPerMeasurement(100)
      .Run();
    }
    #endregion
  }
}
