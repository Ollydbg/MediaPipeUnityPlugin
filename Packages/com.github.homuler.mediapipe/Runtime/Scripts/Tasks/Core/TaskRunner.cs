// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using Google.Protobuf;

namespace Mediapipe.Tasks.Core
{
  public class TaskRunner : MpResourceHandle
  {
    public delegate void NativePacketsCallback(IntPtr status, IntPtr packetMap);

    public static TaskRunner Create(CalculatorGraphConfig config, NativePacketsCallback packetsCallback = null)
    {
      var bytes = config.ToByteArray();
      UnsafeNativeMethods.mp_tasks_core_TaskRunner_Create__PKc_i_PF(bytes, bytes.Length, packetsCallback, out var statusPtr, out var taskRunnerPtr).Assert();

      var status = new Status(statusPtr);
      status.AssertOk();

      return new TaskRunner(taskRunnerPtr);
    }

    private TaskRunner(IntPtr ptr) : base(ptr) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_tasks_core_TaskRunner__delete(ptr);
    }

    public PacketMap Process(PacketMap inputs)
    {
      UnsafeNativeMethods.mp_tasks_core_TaskRunner__Process__Ppm(mpPtr, inputs.mpPtr, out var statusPtr, out var packetMapPtr).Assert();
      GC.KeepAlive(this);

      var status = new Status(statusPtr);
      status.AssertOk();

      return new PacketMap(packetMapPtr, true);
    }

    public void Send(PacketMap inputs)
    {
      UnsafeNativeMethods.mp_tasks_core_TaskRunner__Send__Ppm(mpPtr, inputs.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(this);

      var status = new Status(statusPtr);
      status.AssertOk();
    }

    public void Close()
    {
      UnsafeNativeMethods.mp_tasks_core_TaskRunner__Close(mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(this);

      var status = new Status(statusPtr);
      status.AssertOk();
    }

    public void Restart()
    {
      UnsafeNativeMethods.mp_tasks_core_TaskRunner__Restart(mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(this);

      var status = new Status(statusPtr);
      status.AssertOk();
    }

    public CalculatorGraphConfig GetGraphConfig(ExtensionRegistry extensionRegistry = null)
    {
      UnsafeNativeMethods.mp_tasks_core_TaskRunner__GetGraphConfig(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var parser = extensionRegistry == null ? CalculatorGraphConfig.Parser : CalculatorGraphConfig.Parser.WithExtensionRegistry(extensionRegistry);
      var config = serializedProto.Deserialize(parser);
      serializedProto.Dispose();

      return config;
    }
  }
}
