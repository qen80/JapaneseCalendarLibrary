using JapaneseCalendarLibrary.Application.Extensions;
using JapaneseCalendarLibrary.Domain.Services;
using JapaneseCalendarLibrary.Domain.ValueObjects;
using JapaneseCalendarLibrary.Infrastructure.Repositories;
using JapaneseCalendarLibrary.Infrastructure.Services;

namespace JapaneseCalendarLibrary.Samples;

/// <summary>
/// 日本暦ライブラリの使用例
/// </summary>
public static class UsageExamples
{
    /// <summary>
    /// 基本的な使用例を実行します
    /// </summary>
    public static void RunBasicExamples()
    {
        Console.WriteLine("=== 日本暦ライブラリ使用例 ===\n");

        // 1. 西暦から和暦への変換（拡張メソッド使用）
        Console.WriteLine("1. 西暦から和暦への変換");
        var gregorianDate = new DateTime(2023, 12, 25);
        var japaneseDate = gregorianDate.ToJapaneseDate();
        
        Console.WriteLine($"西暦: {gregorianDate:yyyy年M月d日}");
        Console.WriteLine($"和暦: {japaneseDate}");
        Console.WriteLine($"短縮形: {japaneseDate.ToShortString()}");
        Console.WriteLine();

        // 2. サービスを使った変換
        Console.WriteLine("2. サービスを使った変換");
        var repository = new EraRepository();
        var converter = new CalendarConverter(repository);
        
        var date1 = new DateTime(2019, 5, 1); // 令和開始日
        var japanese1 = converter.ToJapaneseDate(date1);
        Console.WriteLine($"{date1:yyyy年M月d日} → {japanese1}");
        
        var date2 = new DateTime(2019, 4, 30); // 平成最終日
        var japanese2 = converter.ToJapaneseDate(date2);
        Console.WriteLine($"{date2:yyyy年M月d日} → {japanese2}");
        Console.WriteLine();

        // 3. 和暦から西暦への変換
        Console.WriteLine("3. 和暦から西暦への変換");
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var japDate = new JapaneseDate(era, 5, 12, 25);
        var gregDate = japDate.GregorianDate;
        
        Console.WriteLine($"和暦: {japDate}");
        Console.WriteLine($"西暦: {gregDate:yyyy年M月d日}");
        Console.WriteLine();

        // 4. 元号の判定
        Console.WriteLine("4. 元号の判定");
        var testDates = new[]
        {
            new DateTime(2023, 1, 1),
            new DateTime(2000, 1, 1),
            new DateTime(1980, 1, 1),
            new DateTime(1920, 1, 1),
            new DateTime(1900, 1, 1)
        };

        foreach (var testDate in testDates)
        {
            Console.WriteLine($"{testDate:yyyy年M月d日}:");
            Console.WriteLine($"  令和: {testDate.IsReiwa()}");
            Console.WriteLine($"  平成: {testDate.IsHeisei()}");
            Console.WriteLine($"  昭和: {testDate.IsShowa()}");
            Console.WriteLine($"  大正: {testDate.IsTaisho()}");
            Console.WriteLine($"  明治: {testDate.IsMeiji()}");
            Console.WriteLine($"  元号: {testDate.GetEra()?.Name ?? "不明"}");
        }
        Console.WriteLine();

        // 5. 年齢計算
        Console.WriteLine("5. 年齢計算");
        var birthDate = new DateTime(1990, 5, 15);
        var age = birthDate.CalculateJapaneseAge();
        Console.WriteLine($"生年月日: {birthDate:yyyy年M月d日} ({birthDate.ToJapaneseDateString()})");
        Console.WriteLine($"現在の年齢: {age.Age}歳 ({age.Era}時代)");
        Console.WriteLine();

        // 6. 拡張メソッドの活用
        Console.WriteLine("6. 拡張メソッドの活用");
        var today = DateTime.Today;
        var todayJapanese = today.ToJapaneseDate();
        
        Console.WriteLine($"今日: {today.ToJapaneseDateString()}");
        Console.WriteLine($"短縮形: {today.ToJapaneseShortDateString()}");
        Console.WriteLine($"詳細: {todayJapanese.ToDetailedString()}");
        Console.WriteLine($"曜日付き: {todayJapanese.ToStringWithDayOfWeek()}");
        Console.WriteLine();

        // 7. 日付の比較と計算
        Console.WriteLine("7. 日付の比較と計算");
        var newYear = new DateTime(2024, 1, 1).ToJapaneseDate();
        var christmas = new DateTime(2023, 12, 25).ToJapaneseDate();
        
        Console.WriteLine($"元旦: {newYear}");
        Console.WriteLine($"クリスマス: {christmas}");
        Console.WriteLine($"クリスマスから元旦まで: {newYear.DaysFrom(christmas)}日");
        Console.WriteLine($"今日からの日数: {newYear.DaysFromToday()}日");
        Console.WriteLine($"未来の日付?: {newYear.IsFuture()}");
        Console.WriteLine();

        // 8. エラーハンドリング例
        Console.WriteLine("8. エラーハンドリング例");
        try
        {
            // 存在しない日付
            var invalidDate = new JapaneseDate(era, 1, 2, 29); // 令和元年は平年
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"エラー: {ex.Message}");
        }

        try
        {
            // 範囲外の日付
            var outOfRange = new DateTime(1800, 1, 1);
            var result = converter.ToJapaneseDate(outOfRange);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"エラー: {ex.Message}");
        }
    }

    /// <summary>
    /// 高度な使用例を実行します
    /// </summary>
    public static void RunAdvancedExamples()
    {
        Console.WriteLine("\n=== 高度な使用例 ===\n");

        var repository = new EraRepository();
        var converter = new CalendarConverter(repository);

        // 1. 元号の期間一覧
        Console.WriteLine("1. 元号の期間一覧");
        var allEras = repository.GetAll();
        foreach (var eraInfo in allEras)
        {
            Console.WriteLine($"{eraInfo.Era.Name}: {eraInfo.Era.StartDate:yyyy年M月d日} - " +
                            $"{(eraInfo.Era.EndDate?.ToString("yyyy年M月d日") ?? "現在")} " +
                            $"({eraInfo.DurationInYears}年間)");
        }
        Console.WriteLine();

        // 2. 特定期間の元号検索
        Console.WriteLine("2. 特定期間の元号検索");
        var searchStart = new DateTime(1985, 1, 1);
        var searchEnd = new DateTime(1995, 1, 1);
        var overlapping = repository.GetOverlapping(searchStart, searchEnd);
        
        Console.WriteLine($"期間 {searchStart:yyyy年M月d日} - {searchEnd:yyyy年M月d日} に重複する元号:");
        foreach (var eraInfo in overlapping)
        {
            Console.WriteLine($"  {eraInfo.Era}");
        }
        Console.WriteLine();

        // 3. 年代別統計
        Console.WriteLine("3. 年代別統計");
        var decades = new[] { 1970, 1980, 1990, 2000, 2010, 2020 };
        
        foreach (var decade in decades)
        {
            var decadeStart = new DateTime(decade, 1, 1);
            var era = converter.FindEraByDate(decadeStart);
            var japaneseYear = converter.CalculateJapaneseYear(decade, era?.Name ?? "不明");
            
            Console.WriteLine($"{decade}年代: {era?.Name ?? "不明"}{(japaneseYear > 0 ? japaneseYear.ToString() : "")}年");
        }
        Console.WriteLine();

        // 4. 現在の元号情報
        Console.WriteLine("4. 現在の元号情報");
        var currentEra = repository.GetCurrent();
        if (currentEra != null)
        {
            var currentJapanese = converter.GetToday();
            Console.WriteLine($"現在の元号: {currentEra.Era}");
            Console.WriteLine($"今日の和暦: {currentJapanese}");
            Console.WriteLine($"元号開始からの日数: {DateTime.Today.Subtract(currentEra.Era.StartDate).Days}日");
        }
        Console.WriteLine();

        // 5. 境界日のテスト
        Console.WriteLine("5. 元号境界日のテスト");
        var boundaryDates = new[]
        {
            new DateTime(2019, 4, 30), // 平成最終日
            new DateTime(2019, 5, 1),  // 令和開始日
        };

        foreach (var boundaryDate in boundaryDates)
        {
            var japDate = converter.ToJapaneseDate(boundaryDate);
            Console.WriteLine($"{boundaryDate:yyyy年M月d日} → {japDate}");
        }
    }
}

/// <summary>
/// メインプログラム
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            UsageExamples.RunBasicExamples();
            UsageExamples.RunAdvancedExamples();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"エラーが発生しました: {ex.Message}");
        }

        Console.WriteLine("\nEnterキーを押して終了...");
        Console.ReadLine();
    }
}