@echo off


@set Define="UNITY_5_3_OR_NEWER;UNITY_2017_1_OR_NEWER;Release;UNITY_2019_2_OR_NEWER"

cd %~dp0

@set DllPath=%cd%\UnityDll

@set GameSrcPath=%cd%\Assets\Scripts\Core

@set Output=%cd%\OutDll\MeshPlus.Dll

@set XmlPath=%cd%\OutDll\MeshPlus.xml

 rem set path=%GameSrcPath%;

rem echo %path%

rem pause
rem pause

call %cd%\Compile.bat

pause






