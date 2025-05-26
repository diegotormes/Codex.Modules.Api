<#
.SYNOPSIS
Deploys a Service Fabric application type to a cluster.
#>

param (
    [string]$PublishProfileFile,
    [string]$StartupServicesFile,
    [string]$ApplicationPackagePath,
    [switch]$DeployOnly,
    [string]$OverwriteBehavior = 'SameAppTypeAndVersion'
)

$publishProfile = Get-Content $PublishProfileFile | ConvertFrom-Xml
$PublishParameters = @{
    PublishProfileFile = $PublishProfileFile
    ApplicationPackagePath = $ApplicationPackagePath
    OverwriteBehavior = $OverwriteBehavior
    SkipPackageValidation = $true
}

if ($StartupServicesFile) {
    $PublishParameters['StartupServicesFilePath'] = $StartupServicesFile
    if ($publishProfile.PublishProfile.StartupServiceParameterFile) {
        $PublishParameters['StartupServiceParameterFilePath'] = $publishProfile.PublishProfile.StartupServiceParameterFile
    }
}

$Action = 'RegisterAndCreate'
if ($DeployOnly) {
    $Action = 'Register'
}

$PublishParameters['Action'] = $Action

Publish-NewServiceFabricApplication @PublishParameters
