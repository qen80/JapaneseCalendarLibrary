# JapaneseCalendarLibrary

日本の元号と西暦の相互変換を行うC#クラスライブラリです。軽量DDD設計により、保守性と拡張性を重視した実装となっています。

## 特徴

- ✅ **西暦⇔和暦の完全な相互変換**
- ✅ **元号管理**: 明治・大正・昭和・平成・令和に対応
- ✅ **軽量DDD設計**: ValueObject、Entity、Repository、DomainServiceを採用
- ✅ **プライマリコンストラクタ対応**: 最新のC#記法に準拠
- ✅ **豊富な拡張メソッド**: DateTime型とJapaneseDate型の拡張
- ✅ **完全なXMLドキュメント**: IntelliSense対応
- ✅ **78個のテスト**: 境界値・エラーケースを含む包括的テスト
- ✅ **.NET 8.0対応**: 最新の.NETランタイム

## インストール

```bash
git clone https://github.com/[あなたのユーザー名]/JapaneseCalendarLibrary.git
cd JapaneseCalendarLibrary
dotnet build
```

## 基本的な使用方法

### 1. 西暦から和暦への変換

```csharp
using JapaneseCalendarLibrary.Application.Extensions;

var gregorianDate = new DateTime(2023, 12, 25);
var japaneseDate = gregorianDate.ToJapaneseDate();

Console.WriteLine(japaneseDate.ToString()); // 出力: 令和5年12月25日
Console.WriteLine(japaneseDate.ToShortString()); // 出力: R5.12.25
```

### 2. 和暦から西暦への変換

```csharp
using JapaneseCalendarLibrary.Domain.ValueObjects;

var era = new Era("令和", new DateTime(2019, 5, 1));
var japaneseDate = new JapaneseDate(era, 5, 12, 25);
var gregorianDate = japaneseDate.GregorianDate;

Console.WriteLine(gregorianDate.ToString("yyyy年M月d日")); // 出力: 2023年12月25日
```

### 3. 元号の判定

```csharp
var date = new DateTime(2023, 1, 1);

Console.WriteLine(date.IsReiwa());  // 出力: True
Console.WriteLine(date.IsHeisei()); // 出力: False
Console.WriteLine(date.GetEra()?.Name); // 出力: 令和
```

### 4. 年齢計算

```csharp
var birthDate = new DateTime(1990, 5, 15);
var age = birthDate.CalculateJapaneseAge();

Console.WriteLine($"{age.Age}歳 ({age.Era}時代)"); // 出力: 33歳 (令和時代)
```

## 高度な使用方法

### サービスクラスの使用

```csharp
using JapaneseCalendarLibrary.Infrastructure.Repositories;
using JapaneseCalendarLibrary.Infrastructure.Services;

var repository = new EraRepository();
var converter = new CalendarConverter(repository);

// 元号境界日の変換
var reiwaStart = new DateTime(2019, 5, 1);
var japaneseDate = converter.ToJapaneseDate(reiwaStart);
Console.WriteLine(japaneseDate); // 出力: 令和元年5月1日

// 現在の和暦日付を取得
var today = converter.GetToday();
Console.WriteLine(today.ToStringWithDayOfWeek()); // 出力: 令和5年12月25日（月）
```

### リポジトリの活用

```csharp
var repository = new EraRepository();

// 現在の元号情報を取得
var currentEra = repository.GetCurrent();
Console.WriteLine(currentEra?.Era.Name); // 出力: 令和

// 特定期間の元号を検索
var overlapping = repository.GetOverlapping(
    new DateTime(1985, 1, 1), 
    new DateTime(1995, 1, 1)
);
// 昭和と平成が取得される
```

## プロジェクト構成

```
JapaneseCalendarLibrary/
├── src/JapaneseCalendarLibrary/
│   ├── Domain/                    # ドメイン層
│   │   ├── ValueObjects/         # Era, JapaneseDate
│   │   ├── Entities/             # EraInfo
│   │   └── Services/             # ICalendarConverter
│   ├── Infrastructure/           # インフラ層
│   │   ├── Repositories/         # EraRepository
│   │   └── Services/             # CalendarConverter
│   └── Application/              # アプリケーション層
│       └── Extensions/           # 拡張メソッド
├── tests/JapaneseCalendarLibrary.Tests/  # テスト
├── samples/                      # 使用例
└── README.md
```

## API リファレンス

### 主要クラス

| クラス | 説明 |
|--------|------|
| `Era` | 元号を表すValueObject |
| `JapaneseDate` | 和暦日付を表すValueObject |
| `EraInfo` | 元号情報を管理するEntity |
| `CalendarConverter` | 西暦⇔和暦変換サービス |
| `EraRepository` | 元号データアクセス |

### 拡張メソッド

| メソッド | 説明 |
|----------|------|
| `DateTime.ToJapaneseDate()` | 西暦→和暦変換 |
| `DateTime.IsReiwa()` | 令和かどうか判定 |
| `DateTime.CalculateJapaneseAge()` | 和暦年齢計算 |
| `JapaneseDate.ToStringWithDayOfWeek()` | 曜日付き文字列 |
| `JapaneseDate.DaysFromToday()` | 今日からの日数差 |

## 対応元号

| 元号 | 期間 | 略称 |
|------|------|------|
| 明治 | 1868年1月25日 - 1912年7月29日 | M |
| 大正 | 1912年7月30日 - 1926年12月24日 | T |
| 昭和 | 1926年12月25日 - 1989年1月7日 | S |
| 平成 | 1989年1月8日 - 2019年4月30日 | H |
| 令和 | 2019年5月1日 - 現在 | R |

## テスト

```bash
# 全テスト実行
dotnet test

# テスト結果詳細表示
dotnet test --logger console --verbosity normal
```

**テスト結果**: 78個のテスト全て成功 ✅

## 要件

- .NET 8.0以上
- C# 12.0対応コンパイラ

## ライセンス

MIT License

## 貢献

プルリクエストや課題報告を歓迎します。

## 使用例

詳細な使用例は`samples/Usage.cs`をご覧ください。

```bash
# サンプル実行
cd samples
dotnet run
```

## 作者

Claude Code Assistant によって生成されました。

---

このライブラリは日本の暦制度を正確に扱うために作成されました。ビジネスアプリケーションでの和暦表示、帳票システム、官公庁システムなどでご活用ください。