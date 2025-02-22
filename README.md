# MediaPipe.NET

Bringing the best of MediaPipe to the .NET ecosystem! Based on [MediaPipeUnity](https://github.com/homuler/MediaPipeUnityPlugin), MediaPipe.NET brings [Google's MediaPipe](https://mediapipe.dev/) into the .NET ecosystem in its fullest.

## Installation

You may install via NuGet (not yet released):

```
dotnet add --project <Project> Mediapipe.Net
```

Since this library is mapping off native methods from MediaPipe to the CLR realm, the runtime is not explicitly included in the NuGet package or in this repository.
Interested parties who may want to head to the [MediaPipe.NET.Runtime](https://github.com/vignetteapp/MediaPipe.NET.Runtime) repository, where the native code resides.

However, if you just want things to work, You will want to use `MediaPipe.Net.Runtime`, you have the option of using CPU or GPU:

```shell
$ dotnet add --project <Project> Mediapipe.Net.Runtime.Gpu # if you want to use GPU
$ dotnet add --project <Project> Mediapipe.Net.Runtime.Cpu # if you want to use CPU
```

> :warning: **NOTE** :warning: : Certain features may not work at the moment in GPU, please consult the [compatibility matrix](#compatibility-matrix).

## Compatibility Matrix

We share the same compatibility matrix with MediaPipeUnityPlugin, so this will change depending on whichever party implements what's missing.

|                             |   Linux (x86_64)   |   macOS (x86_64)   |   macOS (ARM64)    |  Windows (x86_64)  |      Android       |        iOS         |
| :-------------------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: |
|     Linux (AMD64) [^1]      | :heavy_check_mark: |                    |                    |                    | :heavy_check_mark: |                    |
|          Intel Mac          |                    | :heavy_check_mark: |                    |                    | :heavy_check_mark: | :heavy_check_mark: |
|         M1 Mac [^2]         |                    |                    | :heavy_check_mark: |                    | :heavy_check_mark: | :heavy_check_mark: |
| Windows 10 (AMD64) [^3][^4] | :heavy_check_mark: |                    |                    | :heavy_check_mark: | :heavy_check_mark: |                    |

[^1]: Tested on Arch Linux.
[^2]: Experimental, because MediaPipe does not support M1 Mac.
[^3]: Windows 11 will be also OK.
[^4]: Running MediaPipe on Windows is [experimental](https://google.github.io/mediapipe/getting_started/install.html#installing-on-windows).

### Examples

Coming soon!

## Copyright

MediaPipe.NET is Copyright &copy; The Vignette Authors, Licensed under MIT. Parts of this code is based on MediaPipeUnity, Copyright &copy; homuler, Licensed under MIT.
