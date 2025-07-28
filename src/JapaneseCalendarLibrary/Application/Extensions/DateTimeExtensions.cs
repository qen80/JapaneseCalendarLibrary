using JapaneseCalendarLibrary.Domain.Services;
using JapaneseCalendarLibrary.Domain.ValueObjects;
using JapaneseCalendarLibrary.Infrastructure.Repositories;
using JapaneseCalendarLibrary.Infrastructure.Services;

namespace JapaneseCalendarLibrary.Application.Extensions;

/// <summary>
/// DateTime型の拡張メソッドを提供するクラス
/// </summary>
public static class DateTimeExtensions
{
    private static readonly Lazy<ICalendarConverter> _converter = new(() => 
        new CalendarConverter(new EraRepository()));

    /// <summary>
    /// 西暦日付を和暦日付に変換します
    /// </summary>
    /// <param name="gregorianDate">変換対象の西暦日付</param>
    /// <returns>変換された和暦日付</returns>
    /// <exception cref="ArgumentException">変換不可能な日付の場合</exception>
    public static JapaneseDate ToJapaneseDate(this DateTime gregorianDate)
    {
        return _converter.Value.ToJapaneseDate(gregorianDate);
    }

    /// <summary>
    /// 西暦日付から元号を取得します
    /// </summary>
    /// <param name="gregorianDate">検索対象の西暦日付</param>
    /// <returns>対応する元号。見つからない場合はnull</returns>
    public static Era? GetEra(this DateTime gregorianDate)
    {
        return _converter.Value.FindEraByDate(gregorianDate);
    }

    /// <summary>
    /// 西暦日付を和暦の文字列形式で取得します
    /// </summary>
    /// <param name="gregorianDate">変換対象の西暦日付</param>
    /// <returns>「令和5年12月25日」形式の文字列</returns>
    /// <exception cref="ArgumentException">変換不可能な日付の場合</exception>
    public static string ToJapaneseDateString(this DateTime gregorianDate)
    {
        return gregorianDate.ToJapaneseDate().ToString();
    }

    /// <summary>
    /// 西暦日付を和暦の短縮形文字列で取得します
    /// </summary>
    /// <param name="gregorianDate">変換対象の西暦日付</param>
    /// <returns>「R5.12.25」形式の文字列</returns>
    /// <exception cref="ArgumentException">変換不可能な日付の場合</exception>
    public static string ToJapaneseShortDateString(this DateTime gregorianDate)
    {
        return gregorianDate.ToJapaneseDate().ToShortString();
    }

    /// <summary>
    /// 指定された元号の範囲内かどうかを判定します
    /// </summary>
    /// <param name="gregorianDate">判定対象の西暦日付</param>
    /// <param name="eraName">元号名</param>
    /// <returns>範囲内の場合true、範囲外の場合false</returns>
    public static bool IsInEra(this DateTime gregorianDate, string eraName)
    {
        if (string.IsNullOrEmpty(eraName)) return false;
        
        var era = _converter.Value.FindEraByName(eraName);
        return era?.Contains(gregorianDate) ?? false;
    }

    /// <summary>
    /// 現在の元号（令和）かどうかを判定します
    /// </summary>
    /// <param name="gregorianDate">判定対象の西暦日付</param>
    /// <returns>令和の場合true、そうでなければfalse</returns>
    public static bool IsReiwa(this DateTime gregorianDate)
    {
        return gregorianDate.IsInEra("令和");
    }

    /// <summary>
    /// 平成かどうかを判定します
    /// </summary>
    /// <param name="gregorianDate">判定対象の西暦日付</param>
    /// <returns>平成の場合true、そうでなければfalse</returns>
    public static bool IsHeisei(this DateTime gregorianDate)
    {
        return gregorianDate.IsInEra("平成");
    }

    /// <summary>
    /// 昭和かどうかを判定します
    /// </summary>
    /// <param name="gregorianDate">判定対象の西暦日付</param>
    /// <returns>昭和の場合true、そうでなければfalse</returns>
    public static bool IsShowa(this DateTime gregorianDate)
    {
        return gregorianDate.IsInEra("昭和");
    }

    /// <summary>
    /// 大正かどうかを判定します
    /// </summary>
    /// <param name="gregorianDate">判定対象の西暦日付</param>
    /// <returns>大正の場合true、そうでなければfalse</returns>
    public static bool IsTaisho(this DateTime gregorianDate)
    {
        return gregorianDate.IsInEra("大正");
    }

    /// <summary>
    /// 明治かどうかを判定します
    /// </summary>
    /// <param name="gregorianDate">判定対象の西暦日付</param>
    /// <returns>明治の場合true、そうでなければfalse</returns>
    public static bool IsMeiji(this DateTime gregorianDate)
    {
        return gregorianDate.IsInEra("明治");
    }

    /// <summary>
    /// 指定された元号の和暦年を取得します
    /// </summary>
    /// <param name="gregorianDate">対象の西暦日付</param>
    /// <param name="eraName">元号名</param>
    /// <returns>和暦年。元号の範囲外の場合は-1</returns>
    public static int GetJapaneseYear(this DateTime gregorianDate, string eraName)
    {
        try
        {
            return _converter.Value.CalculateJapaneseYear(gregorianDate.Year, eraName);
        }
        catch (ArgumentException)
        {
            return -1;
        }
    }

    /// <summary>
    /// 和暦での年齢を計算します
    /// </summary>
    /// <param name="birthDate">生年月日</param>
    /// <param name="baseDate">基準日（省略時は今日）</param>
    /// <returns>和暦での年齢情報を含むタプル</returns>
    public static (string Era, int Age) CalculateJapaneseAge(this DateTime birthDate, DateTime? baseDate = null)
    {
        var target = baseDate ?? DateTime.Today;
        var age = target.Year - birthDate.Year;
        
        if (target.Month < birthDate.Month || 
            (target.Month == birthDate.Month && target.Day < birthDate.Day))
        {
            age--;
        }

        var era = target.GetEra()?.Name ?? "不明";
        return (era, age);
    }
}