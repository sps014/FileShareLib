## Setup

1. Make sure the paths in csprojs for Asp.NET dlls are correct and corresponds to your computer.


```xml
<Reference Include="C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App\8.0.12\*.dll" Exclude="C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App\8.0.12\aspnetcorev2_inprocess.dll">
	<Private>False</Private>
	<CopyToOutputDirectory>Never</CopyToOutputDirectory>
</Reference>
```

## Local network access

The mobile app exposes two endpoints: `http://localhost:5000`, `https://localhost:5001`. Use Android Debug Bridge (adb) to [forward local ports to the Android emulator](https://developer.android.com/tools/adb#forwardports):

```
adb forward tcp:5000 tcp:5000
adb forward tcp:5001 tcp:5001
```