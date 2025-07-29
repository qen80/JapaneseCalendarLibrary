# GitHub Packages NuGet 配布セットアップガイド

JapaneseCalendarLibraryをGitHub PackagesでNuGetパッケージとして配布するためのセットアップが完了しました。

## 📦 設定完了項目

### ✅ プロジェクトファイルの更新
- NuGetパッケージメタデータを`JapaneseCalendarLibrary.csproj`に追加
- GitHub Packages用の設定を追加
- SourceLink設定によるデバッグサポートを追加

### ✅ GitHub Actionsワークフロー
- `.github/workflows/publish-nuget.yml`を作成
- リリース時とマニュアル実行での自動パブリッシュ機能
- テスト実行とパッケージ作成の自動化

### ✅ NuGet.config
- GitHub Packagesソースの設定
- 認証情報の設定テンプレート

### ✅ テスト確認
- ビルド成功（78テスト全て成功）
- パッケージ作成成功

## 🚀 GitHub Packagesへの配布手順

### 1. GitHubリポジトリの準備

```bash
# 1. GitHubでリポジトリを作成
# 2. ローカルリポジトリをGitHubにプッシュ
git init
git add .
git commit -m "初期コミット: JapaneseCalendarLibrary NuGetパッケージ設定"
git branch -M main
git remote add origin https://github.com/[YOUR_USERNAME]/JapaneseCalendarLibrary.git
git push -u origin main
```

### 2. 設定ファイルの調整

**プロジェクトファイル**の以下の項目を実際の値に更新：
```xml
<Authors>Your Name</Authors>
<Company>Your Company</Company>
<RepositoryUrl>https://github.com/[YOUR_USERNAME]/JapaneseCalendarLibrary</RepositoryUrl>
```

**NuGet.config**の以下の項目を実際の値に更新：
```xml
<add key="github" value="https://nuget.pkg.github.com/[YOUR_USERNAME]/index.json" />
<add key="Username" value="[YOUR_USERNAME]" />
<add key="ClearTextPassword" value="[YOUR_PERSONAL_ACCESS_TOKEN]" />
```

### 3. Personal Access Token の作成

1. GitHub Settings → Developer settings → Personal access tokens
2. **Classic tokens** を選択
3. 必要な権限を付与：
   - `write:packages` - パッケージの書き込み
   - `read:packages` - パッケージの読み取り
   - `repo` - リポジトリアクセス

### 4. パッケージのパブリッシュ

**方法1: リリース作成による自動パブリッシュ**
```bash
# タグを作成してリリース
git tag v1.0.0
git push origin v1.0.0
# GitHubでリリースを作成すると自動でパブリッシュされます
```

**方法2: マニュアル実行**
1. GitHubリポジトリの「Actions」タブ
2. 「Publish NuGet Package to GitHub Packages」を選択
3. 「Run workflow」をクリック
4. バージョン番号を入力して実行

## 📥 パッケージの利用方法

### プロジェクトでの使用

**NuGet.configの設定**
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="github" value="https://nuget.pkg.github.com/[YOUR_USERNAME]/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <github>
      <add key="Username" value="[YOUR_USERNAME]" />
      <add key="ClearTextPassword" value="[YOUR_PERSONAL_ACCESS_TOKEN]" />
    </github>
  </packageSourceCredentials>
</configuration>
```

**パッケージのインストール**
```bash
dotnet add package JapaneseCalendarLibrary --version 1.0.0
```

### 使用例
```csharp
using JapaneseCalendarLibrary.Domain.ValueObjects;
using JapaneseCalendarLibrary.Infrastructure.Services;

// 和暦から西暦へ変換
var era = new Era("令和", new DateTime(2019, 5, 1));
var japaneseDate = new JapaneseDate(era, 6, 1, 15);
var converter = new CalendarConverter();
var gregorianDate = converter.ToGregorianDate(japaneseDate);

Console.WriteLine($"令和6年1月15日 → {gregorianDate:yyyy年M月d日}");
```

## 🔧 トラブルシューティング

### よくある問題

**認証エラー**
- Personal Access Tokenの権限を確認
- トークンの有効期限を確認
- ユーザー名とトークンが正しく設定されているか確認

**パッケージが見つからない**
- パブリッシュが成功しているか確認
- NuGet.configでソースが正しく設定されているか確認
- キャッシュクリア: `dotnet nuget locals all --clear`

**ビルドエラー**
- .NET 8.0 SDKがインストールされているか確認
- `dotnet restore`を実行
- 依存関係の問題がないか確認

## 📝 追加の設定項目

### バージョン管理
- セマンティックバージョニング（SemVer）の採用を推奨
- メジャー.マイナー.パッチ形式（例：1.2.3）

### ライセンス
- 現在はMITライセンスを設定
- 必要に応じてLICENSEファイルを追加

### ドキュメント
- XMLドキュメントコメントが自動で含まれます
- 追加のドキュメントはREADME.mdに記載

## 🎯 次のステップ

1. **実際のGitHubアカウント情報で設定を更新**
2. **リポジトリの作成とプッシュ**
3. **Personal Access Tokenの作成**
4. **初回パブリッシュのテスト**
5. **パッケージの利用テスト**

これで和暦ライブラリがGitHub PackagesでNuGetパッケージとして配布できるようになりました！