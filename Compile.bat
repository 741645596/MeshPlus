"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\Roslyn\csc" ^
/r:%DllPath%\UnityEngine.CoreModule.dll ^
/r:%DllPath%\UnityEngine.dll ^
/r:%DllPath%\Graphics.Dll ^
/define:%Define% /optimize /target:library /out:%Output% /doc:%XmlPath% ^
/recurse:%GameSrcPath%\*.cs

REM /define:%Define% /debug /target:library /out:%Output% /unsafe ^

if ERRORLEVEL 1 (
echo build dll failed!
exit /B 1
) else (
echo build dll sucess!
)