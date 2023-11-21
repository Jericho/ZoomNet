# Inspired by: https://github.com/PowerShell/PSScriptAnalyzer/blob/master/tools/appveyor.psm1

$ErrorActionPreference = 'Stop'

# Implements the AppVeyor 'install' step and installs the desired .NET SDK if not already installed.
function Invoke-AppVeyorInstall {

    Write-Verbose -Verbose "Determining the desired version of .NET SDK"
    $globalDotJson = Get-Content (Join-Path $PSScriptRoot 'global.json') -Raw | ConvertFrom-Json
    $desiredDotNetCoreSDKVersion = $globalDotJson.sdk.version
    Write-Verbose -Verbose "We have determined that the desired version of the .NET SDK is $desiredDotNetCoreSDKVersion"

    Write-Verbose -Verbose "Checking availability of .NET SDK $desiredDotNetCoreSDKVersion"
    $desiredDotNetCoreSDKVersionPresent = (dotnet --list-sdks) -match $desiredDotNetCoreSDKVersion

    if (-not $desiredDotNetCoreSDKVersionPresent) {
        Write-Verbose -Verbose "We have determined that the desired version of the .NET SDK is not present on this machine"
        Write-Verbose -Verbose "Installing .NET SDK $desiredDotNetCoreSDKVersion"
        $originalSecurityProtocol = [Net.ServicePointManager]::SecurityProtocol
        try {
            [Net.ServicePointManager]::SecurityProtocol = [Net.ServicePointManager]::SecurityProtocol -bor [Net.SecurityProtocolType]::Tls12
            if ($IsLinux -or $isMacOS) {
                Invoke-WebRequest 'https://dot.net/v1/dotnet-install.sh' -OutFile dotnet-install.sh
                bash dotnet-install.sh --version $desiredDotNetCoreSDKVersion
                [System.Environment]::SetEnvironmentVariable('PATH', "/home/appveyor/.dotnet$([System.IO.Path]::PathSeparator)$PATH")
            }
            else {
                Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile dotnet-install.ps1
                .\dotnet-install.ps1 -Version $desiredDotNetCoreSDKVersion
            }
        }
        finally {
            [Net.ServicePointManager]::SecurityProtocol = $originalSecurityProtocol
            Remove-Item .\dotnet-install.*
        }
        Write-Verbose -Verbose "Installed .NET SDK $desiredDotNetCoreSDKVersion"
    }
    else {
        Write-Verbose -Verbose "We have determined that the desired version of the .NET SDK is already installed on this machine"
    }
}
