# https://github.com/microsoft/dotnet-framework-docker/blob/main/samples/aspnetmvcapp/Dockerfile
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2022 AS build
ENV PATH="$PATH;C:\Windows\system32;C:\Windows;C:\Windows\System32\Wbem;C:\Windows\System32\WindowsPowerShell\v1.0\;C:\Windows\System32\OpenSSH\;C:\Program Files\dotnet\;C:\Users\ContainerAdministrator\AppData\Local\Microsoft\WindowsApps;C:\Users\ContainerAdministrator\.dotnet\tools;C:\Program Files\NuGet;C:\Program Files (x86)\Microsoft Visual Studio\2022\TestAgent\Common7\IDE\CommonExtensions\Microsoft\TestWindow;C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\amd64;C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools;C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool;"
SHELL ["powershell"]

RUN Invoke-WebRequest -Uri https://nodejs.org/dist/v22.13.0/node-v22.13.0-x64.msi -OutFile node.msi; \
Start-Process msiexec.exe -Wait -ArgumentList '/i node.msi /quiet /norestart'; \
Remove-Item -Path node.msi

COPY *.sln *.csproj packages.config ./
RUN nuget restore

COPY . .
RUN msbuild ARMExplorer.csproj /nologo /verbosity:m /t:Build /t:pipelinePreDeployCopyAllFilesToOneFolder /p:'_PackageTempDir=/out;AutoParameterizationWebConfigConnectionStrings=false;Configuration=Release;UseSharedCompilation=false'

FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2022
ENV PATH="$PATH;C:\Windows\system32;C:\Windows;C:\Windows\System32\Wbem;C:\Windows\System32\WindowsPowerShell\v1.0\;C:\Windows\System32\OpenSSH\;C:\Program Files\dotnet\;C:\Users\ContainerAdministrator\AppData\Local\Microsoft\WindowsApps;C:\Users\ContainerAdministrator\.dotnet\tools;C:\Program Files\NuGet;C:\Program Files (x86)\Microsoft Visual Studio\2022\TestAgent\Common7\IDE\CommonExtensions\Microsoft\TestWindow;C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\amd64;C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools;C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool;"
SHELL ["powershell"]
WORKDIR /inetpub/wwwroot

RUN [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; mkdir C:\LogMonitor; \
Invoke-WebRequest -Uri https://github.com/microsoft/windows-container-tools/releases/download/v2.1.1/LogMonitor.exe -OutFile C:\LogMonitor\LogMonitor.exe -Verbose; \
Invoke-WebRequest -Uri https://raw.githubusercontent.com/microsoft/iis-docker/master/windowsservercore-insider/LogMonitorConfig.json -OutFile C:\LogMonitor\LogMonitorConfig.json -Verbose
RUN C:\Windows\System32\inetsrv\appcmd.exe set config -section:system.applicationHost/sites '/[name=''Default Web Site''].traceFailedRequestsLogging.enabled:True' /commit:apphost; \
    C:\Windows\System32\inetsrv\appcmd.exe set config -section:system.applicationHost/sites '/[name=''Default Web Site''].traceFailedRequestsLogging.maxLogFiles:10' /commit:apphost; \
    C:\Windows\System32\inetsrv\appcmd.exe set config -section:system.applicationHost/sites '/[name=''Default Web Site''].logFile.logTargetW3C:"File,ETW"' /commit:apphost

RUN Invoke-WebRequest -Uri https://download.microsoft.com/download/1/2/8/128E2E22-C1B9-44A4-BE2A-5859ED1D4592/rewrite_amd64_en-US.msi -OutFile rewrite.msi; \
Start-Process msiexec.exe -Wait -ArgumentList '/i rewrite.msi /quiet /norestart'; \
Remove-Item -Path rewrite.msi

COPY --from=build /out/. ./

ENTRYPOINT ["C:\\LogMonitor\\LogMonitor.exe", "C:\\ServiceMonitor.exe", "w3svc"]
