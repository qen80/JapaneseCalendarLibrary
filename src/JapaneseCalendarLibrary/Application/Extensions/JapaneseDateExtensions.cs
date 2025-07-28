using JapaneseCalendarLibrary.Domain.ValueObjects;

namespace JapaneseCalendarLibrary.Application.Extensions;

/// <summary>
/// JapaneseDate型の拡張メソッドを提供するクラス
/// </summary>
public static class JapaneseDateExtensions
{
    /// <summary>
    /// 和暦日付を西暦日付に変換します
    /// </summary>
    /// <param name="japaneseDate">変換対象の和暦日付</param>
    /// <returns>変換された西暦日付</returns>
    /// <exception cref="InvalidOperationException">変換不可能な和暦日付の場合</exception>
    public static DateTime ToGregorianDate(this JapaneseDate japaneseDate)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);
        return japaneseDate.GregorianDate;
    }

    /// <summary>
    /// 和暦日付を西暦の文字列形式で取得します
    /// </summary>
    /// <param name="japaneseDate">変換対象の和暦日付</param>
    /// <returns>「2023年12月25日」形式の文字列</returns>
    public static string ToGregorianDateString(this JapaneseDate japaneseDate)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);
        var gregorian = japaneseDate.GregorianDate;
        return $"{gregorian:yyyy年M月d日}";
    }

    /// <summary>
    /// 和暦日付をISO8601形式の文字列で取得します
    /// </summary>
    /// <param name="japaneseDate">変換対象の和暦日付</param>
    /// <returns>「2023-12-25」形式の文字列</returns>
    public static string ToIsoDateString(this JapaneseDate japaneseDate)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);
        var gregorian = japaneseDate.GregorianDate;
        return gregorian.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 指定された和暦日付との差を日数で計算します
    /// </summary>
    /// <param name="japaneseDate">基準となる和暦日付</param>
    /// <param name="other">比較対象の和暦日付</param>
    /// <returns>日数の差（正の値は未来、負の値は過去）</returns>
    public static int DaysFrom(this JapaneseDate japaneseDate, JapaneseDate other)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);
        ArgumentNullException.ThrowIfNull(other);
        
        var thisDate = japaneseDate.GregorianDate;
        var otherDate = other.GregorianDate;
        
        return (int)(thisDate - otherDate).TotalDays;
    }

    /// <summary>
    /// 現在日付との差を日数で計算します
    /// </summary>
    /// <param name="japaneseDate">基準となる和暦日付</param>
    /// <returns>現在日付からの日数差（正の値は未来、負の値は過去）</returns>
    public static int DaysFromToday(this JapaneseDate japaneseDate)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);
        
        var thisDate = japaneseDate.GregorianDate;
        var today = DateTime.Today;
        
        return (int)(thisDate - today).TotalDays;
    }

    /// <summary>
    /// 和暦日付が今日より未来かどうかを判定します
    /// </summary>
    /// <param name="japaneseDate">判定対象の和暦日付</param>
    /// <returns>未来の場合true、今日以前の場合false</returns>
    public static bool IsFuture(this JapaneseDate japaneseDate)
    {
        return japaneseDate.DaysFromToday() > 0;
    }

    /// <summary>
    /// 和暦日付が今日より過去かどうかを判定します
    /// </summary>
    /// <param name="japaneseDate">判定対象の和暦日付</param>
    /// <returns>過去の場合true、今日以降の場合false</returns>
    public static bool IsPast(this JapaneseDate japaneseDate)
    {
        return japaneseDate.DaysFromToday() < 0;
    }

    /// <summary>
    /// 和暦日付が今日かどうかを判定します
    /// </summary>
    /// <param name="japaneseDate">判定対象の和暦日付</param>
    /// <returns>今日の場合true、そうでなければfalse</returns>
    public static bool IsToday(this JapaneseDate japaneseDate)
    {
        return japaneseDate.DaysFromToday() == 0;
    }

    /// <summary>
    /// 和暦日付が指定された年月日と同じかどうかを判定します
    /// </summary>
    /// <param name="japaneseDate">判定対象の和暦日付</param>
    /// <param name="year">和暦年</param>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    /// <returns>同じ日付の場合true、そうでなければfalse</returns>
    public static bool IsSameDate(this JapaneseDate japaneseDate, int year, int month, int day)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);
        
        return japaneseDate.Year == year && 
               japaneseDate.Month == month && 
               japaneseDate.Day == day;
    }

    /// <summary>
    /// 和暦日付が指定された月日と同じかどうかを判定します（年は無視）
    /// </summary>
    /// <param name="japaneseDate">判定対象の和暦日付</param>
    /// <param name="month">月</param>
    /// <param name="day">日</param>
    /// <returns>同じ月日の場合true、そうでなければfalse</returns>
    public static bool IsSameMonthDay(this JapaneseDate japaneseDate, int month, int day)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);
        
        return japaneseDate.Month == month && japaneseDate.Day == day;
    }

    /// <summary>
    /// 和暦日付から年齢を計算します
    /// </summary>
    /// <param name="birthDate">生年月日（和暦）</param>
    /// <param name="baseDate">基準日（省略時は今日の和暦日付）</param>
    /// <returns>計算された年齢</returns>
    public static int CalculateAge(this JapaneseDate birthDate, JapaneseDate? baseDate = null)
    {
        ArgumentNullException.ThrowIfNull(birthDate);
        
        var target = baseDate ?? DateTime.Today.ToJapaneseDate();
        
        // 西暦で計算する方が正確
        var birthGregorian = birthDate.GregorianDate;
        var targetGregorian = target.GregorianDate;
        
        var age = targetGregorian.Year - birthGregorian.Year;
        
        if (targetGregorian.Month < birthGregorian.Month || 
            (targetGregorian.Month == birthGregorian.Month && targetGregorian.Day < birthGregorian.Day))
        {
            age--;
        }
        
        return age;
    }

    /// <summary>
    /// 和暦日付の詳細な文字列表現を取得します
    /// </summary>
    /// <param name="japaneseDate">対象の和暦日付</param>
    /// <returns>和暦と西暦の両方を含む詳細な文字列</returns>
    public static string ToDetailedString(this JapaneseDate japaneseDate)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);
        
        var gregorian = japaneseDate.GregorianDate;
        return $"{japaneseDate} ({gregorian:yyyy年M月d日})";
    }

    /// <summary>
    /// 和暦日付の曜日を取得します
    /// </summary>
    /// <param name="japaneseDate">対象の和暦日付</param>
    /// <returns>曜日を表す文字列</returns>
    public static string GetDayOfWeekString(this JapaneseDate japaneseDate)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);
        
        var gregorian = japaneseDate.GregorianDate;
        return gregorian.DayOfWeek switch
        {
            DayOfWeek.Sunday => "日",
            DayOfWeek.Monday => "月",
            DayOfWeek.Tuesday => "火",
            DayOfWeek.Wednesday => "水",
            DayOfWeek.Thursday => "木",
            DayOfWeek.Friday => "金",
            DayOfWeek.Saturday => "土",
            _ => "不明"
        };
    }

    /// <summary>
    /// 和暦日付を曜日付きの文字列で取得します
    /// </summary>
    /// <param name="japaneseDate">対象の和暦日付</param>
    /// <returns>「令和5年12月25日（月）」形式の文字列</returns>
    public static string ToStringWithDayOfWeek(this JapaneseDate japaneseDate)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);
        
        var dayOfWeek = japaneseDate.GetDayOfWeekString();
        return $"{japaneseDate}（{dayOfWeek}）";
    }
}