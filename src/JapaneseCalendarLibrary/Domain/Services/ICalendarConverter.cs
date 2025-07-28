using JapaneseCalendarLibrary.Domain.ValueObjects;

namespace JapaneseCalendarLibrary.Domain.Services;

/// <summary>
/// 西暦と和暦の相互変換機能を提供するドメインサービスのインターフェース
/// </summary>
public interface ICalendarConverter
{
    /// <summary>
    /// 西暦日付を和暦日付に変換します
    /// </summary>
    /// <param name="gregorianDate">変換対象の西暦日付</param>
    /// <returns>変換された和暦日付</returns>
    /// <exception cref="ArgumentException">変換不可能な日付の場合</exception>
    JapaneseDate ToJapaneseDate(DateTime gregorianDate);

    /// <summary>
    /// 和暦日付を西暦日付に変換します
    /// </summary>
    /// <param name="japaneseDate">変換対象の和暦日付</param>
    /// <returns>変換された西暦日付</returns>
    /// <exception cref="ArgumentException">変換不可能な和暦日付の場合</exception>
    DateTime ToGregorianDate(JapaneseDate japaneseDate);

    /// <summary>
    /// 指定された西暦日付に対応する元号を取得します
    /// </summary>
    /// <param name="gregorianDate">検索対象の西暦日付</param>
    /// <returns>対応する元号。見つからない場合はnull</returns>
    Era? FindEraByDate(DateTime gregorianDate);

    /// <summary>
    /// 元号名から元号を取得します
    /// </summary>
    /// <param name="eraName">検索対象の元号名</param>
    /// <returns>対応する元号。見つからない場合はnull</returns>
    Era? FindEraByName(string eraName);

    /// <summary>
    /// 指定された元号名と和暦年から西暦年を計算します
    /// </summary>
    /// <param name="eraName">元号名</param>
    /// <param name="japaneseYear">和暦年</param>
    /// <returns>対応する西暦年</returns>
    /// <exception cref="ArgumentException">元号が見つからない場合</exception>
    int CalculateGregorianYear(string eraName, int japaneseYear);

    /// <summary>
    /// 指定された西暦年から対応する和暦年を計算します
    /// </summary>
    /// <param name="gregorianYear">西暦年</param>
    /// <param name="eraName">元号名</param>
    /// <returns>対応する和暦年</returns>
    /// <exception cref="ArgumentException">元号が見つからない、または年が元号の範囲外の場合</exception>
    int CalculateJapaneseYear(int gregorianYear, string eraName);
}