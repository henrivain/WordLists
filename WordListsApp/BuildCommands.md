# LIST OF COMMAND LINE COMMANDS USED TO BUILD VERSION V3.1.1

WINDOWS 
--------------------------------------------------------

msbuild /t:build /p:Configuration=Release /p:TargetFramework=net7.0-windows10.0.19041.0 /p:WindowsAppSDKSelfContained=true /p:Platform=x64 /p:WindowsPackageType=None /p:RuntimeIdentifier=win10-x64




ANDROID
--------------------------------------------------------

dotnet publish -f:net7.0-android -c:Release /p:AndroidSdkDirectory="D:\Android\android-sdk" /p:AndroidSigningKeyPass=#####password##### /p:AndroidSigningStorePass=#####password#####