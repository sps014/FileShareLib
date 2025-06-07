## Setup

1. Make sure the paths in csprojs for Asp.NET dlls are correct and corresponds to your computer.


```xml
<Reference Include="C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App\8.0.12\*.dll" Exclude="C:\Program Files\dotnet\shared\Microsoft.AspNetCore.App\8.0.12\aspnetcorev2_inprocess.dll">
	<Private>False</Private>
	<CopyToOutputDirectory>Never</CopyToOutputDirectory>
</Reference>
```