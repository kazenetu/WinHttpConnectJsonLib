# WinHttpConnectJsonLib
WinCEJsonLibのバージョンアップ版。Cookie対応。  
ASP.NET Core2のWebAPIで動作できる機能を追加。

- [ライブラリ本体：connectLib/HttpConnectLib.cs](https://github.com/kazenetu/WinHttpConnectJsonLib/blob/master/WinHttpConnectJsonLib/connectLib/HttpConnectLib.cs)
- [必須JSONライブラリ：Newtonsoft.Json.Compact.dll](https://github.com/kazenetu/WinHttpConnectJsonLib/tree/master/WinHttpConnectJsonLib/connectLib/lib)

# 開発環境
- Visual Studio 2017  
  ※「netstandard2.0」のビルドで必要
- .NET Core 2.0以上

# 実行手順
## WebAPI実行手順
- Visual Studio 2017で実行する場合
   1. `./ASPdotNETCore2/ASPdotNETCoreTest.sln`をVisual Studio 2017で開く
   1. スタートアッププロジェクトを'WebApi'に設定する
   1. デバッグ実行を行う
- コマンドプロンプトで実行する場合
   1. コマンドプロンプトを起動する
   1. `./ASPdotNETCore2/WebApi`に移動する  
      例）`cd ルートフォルダ/ASPdotNETCore2/WebApi`
   1. `dotnet run`を実行する

## Windowsアプリ実行手順
Visual Studio 2017上で実行する。  
1. `./WinHttpConnectJsonLib.sln`をVisual Studio 2017で開く
1. WebAPIを実行する  
   ※表示されるURLが`http://localhost:5000/`以外の場合は  
   1. WinHttpConnectJsonLibプロジェクトのプロパティを開く
   1. デバッグの「コマンドライン引数」にURLを設定する
1. WinHttpConnectJsonLibプロジェクトのデバッグ実行を行う