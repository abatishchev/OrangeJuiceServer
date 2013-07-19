# Debug
#$OctopusWebSiteName = "Test"
#$OctopusPackageDirectoryPath = "c:\inetpub\www\"

# Settings
$appPoolName = "DefaultAppPool12"
$appPoolIdentity = 2 # NetworkService

# Installation
Import-Module WebAdministration

#cd IIS:\

Write-Host "Checking app pool..."

$appPoolPath = ("IIS:\AppPools\" + $appPoolName)
$appPool = Get-Item $appPoolPath -ErrorAction SilentlyContinue
if (!$appPool) {
	Write-Host "App pool does not exist, creating..."

	$appPool = New-WebAppPool -name $appPoolName
	Set-ItemProperty $appPoolPath -name processModel.identityType -value $appPoolIdentity
	Set-ItemProperty $appPoolPath -name managedRuntimeVersion -value v4.0
} else {
	Write-Host "App pool exists."
}

Write-Host "Checking web site..."

$sitePath = ("IIS:\Sites\" + $OctopusWebSiteName)
$site = Get-Item $sitePath -ErrorAction SilentlyContinue
if (!$site) {
	Write-Host "Site does not exist, creating..."

	$id = (dir IIS:\Sites | foreach {$_.id} | sort -Descending | select -first 1) + 1
	$site = New-Website $OctopusWebSiteName -id $id -physicalPath $OctopusPackageDirectoryPath
	Set-ItemProperty $sitePath -name bindings -value @{protocol="http";bindingInformation="*:80:"}
	Set-ItemProperty $sitePath -name applicationPool -value $appPoolName
} else {
    Write-Host "Site exists."
}

Write-Host "IIS configuration complete."