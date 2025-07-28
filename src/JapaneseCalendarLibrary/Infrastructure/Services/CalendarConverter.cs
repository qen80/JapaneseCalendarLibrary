using JapaneseCalendarLibrary.Domain.Services;
using JapaneseCalendarLibrary.Domain.ValueObjects;
using JapaneseCalendarLibrary.Infrastructure.Repositories;

namespace JapaneseCalendarLibrary.Infrastructure.Services;

/// <summary>
/// 西暦と和暦の相互変換機能を提供するドメインサービス実装
/// </summary>
/// <param name="eraRepository">元号データリポジトリ</param>
public class CalendarConverter(IEraRepository eraRepository) : ICalendarConverter
{
    private readonly IEraRepository _eraRepository = eraRepository ?? throw new ArgumentNullException(nameof(eraRepository));

    /// <summary>
    /// 西暦日付を和暦日付に変換します
    /// </summary>
    /// <param name="gregorianDate">変換対象の西暦日付</param>
    /// <returns>変換された和暦日付</returns>
    /// <exception cref="ArgumentException">変換不可能な日付の場合</exception>
    public JapaneseDate ToJapaneseDate(DateTime gregorianDate)
    {
        var eraInfo = _eraRepository.GetByDate(gregorianDate);
        if (eraInfo == null)
            throw new ArgumentException($"指定された日付（{gregorianDate:yyyy年M月d日}）に対応する元号が見つかりません", nameof(gregorianDate));

        return eraInfo.ToJapaneseDate(gregorianDate);
    }

    /// <summary>
    /// 和暦日付を西暦日付に変換します
    /// </summary>
    /// <param name="japaneseDate">変換対象の和暦日付</param>
    /// <returns>変換された西暦日付</returns>
    /// <exception cref="ArgumentException">変換不可能な和暦日付の場合</exception>
    public DateTime ToGregorianDate(JapaneseDate japaneseDate)
    {
        ArgumentNullException.ThrowIfNull(japaneseDate);

        try
        {
            return japaneseDate.GregorianDate;
        }
        catch (InvalidOperationException ex)
        {
            throw new ArgumentException("無効な和暦日付のため変換できません", nameof(japaneseDate), ex);
        }
    }

    /// <summary>
    /// 指定された西暦日付に対応する元号を取得します
    /// </summary>
    /// <param name="gregorianDate">検索対象の西暦日付</param>
    /// <returns>対応する元号。見つからない場合はnull</returns>
    public Era? FindEraByDate(DateTime gregorianDate)
    {
        var eraInfo = _eraRepository.GetByDate(gregorianDate);
        return eraInfo?.Era;
    }

    /// <summary>
    /// 元号名から元号を取得します
    /// </summary>
    /// <param name="eraName">検索対象の元号名</param>
    /// <returns>対応する元号。見つからない場合はnull</returns>
    public Era? FindEraByName(string eraName)
    {
        if (string.IsNullOrEmpty(eraName))
            return null;

        var eraInfo = _eraRepository.GetByName(eraName);
        return eraInfo?.Era;
    }

    /// <summary>
    /// 指定された元号名と和暦年から西暦年を計算します
    /// </summary>
    /// <param name="eraName">元号名</param>
    /// <param name="japaneseYear">和暦年</param>
    /// <returns>対応する西暦年</returns>
    /// <exception cref="ArgumentException">元号が見つからない場合</exception>
    public int CalculateGregorianYear(string eraName, int japaneseYear)
    {
        var era = FindEraByName(eraName);
        if (era == null)
            throw new ArgumentException($"元号「{eraName}」が見つかりません", nameof(eraName));

        if (japaneseYear < 1)
            throw new ArgumentOutOfRangeException(nameof(japaneseYear), japaneseYear, "和暦年は1以上である必要があります");

        return era.StartDate.Year + japaneseYear - 1;
    }

    /// <summary>
    /// 指定された西暦年から対応する和暦年を計算します
    /// </summary>
    /// <param name="gregorianYear">西暦年</param>
    /// <param name="eraName">元号名</param>
    /// <returns>対応する和暦年</returns>
    /// <exception cref="ArgumentException">元号が見つからない、または年が元号の範囲外の場合</exception>
    public int CalculateJapaneseYear(int gregorianYear, string eraName)
    {
        var era = FindEraByName(eraName);
        if (era == null)
            throw new ArgumentException($"元号「{eraName}」が見つかりません", nameof(eraName));

        var targetDate = new DateTime(gregorianYear, 1, 1);
        if (!era.Contains(targetDate))
            throw new ArgumentException($"西暦{gregorianYear}年は元号「{eraName}」の範囲外です", nameof(gregorianYear));

        return gregorianYear - era.StartDate.Year + 1;
    }

    /// <summary>
    /// 現在の日付を和暦で取得します
    /// </summary>
    /// <returns>現在の和暦日付</returns>
    /// <exception cref="InvalidOperationException">現在の元号が見つからない場合</exception>
    public JapaneseDate GetCurrentJapaneseDate()
    {
        var now = DateTime.Now;
        return ToJapaneseDate(now);
    }

    /// <summary>
    /// 今日の日付を和暦で取得します
    /// </summary>
    /// <returns>今日の和暦日付</returns>
    /// <exception cref="InvalidOperationException">現在の元号が見つからない場合</exception>
    public JapaneseDate GetToday()
    {
        var today = DateTime.Today;
        return ToJapaneseDate(today);
    }
}