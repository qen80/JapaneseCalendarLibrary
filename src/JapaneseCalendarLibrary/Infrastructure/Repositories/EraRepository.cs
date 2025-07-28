using JapaneseCalendarLibrary.Domain.Entities;
using JapaneseCalendarLibrary.Domain.ValueObjects;

namespace JapaneseCalendarLibrary.Infrastructure.Repositories;

/// <summary>
/// 元号データへのアクセス機能を提供するリポジトリ実装
/// </summary>
public class EraRepository : IEraRepository
{
    private readonly IReadOnlyList<EraInfo> _eras;

    /// <summary>
    /// EraRepositoryクラスの新しいインスタンスを初期化します
    /// </summary>
    public EraRepository()
    {
        _eras = InitializeEras();
    }

    /// <summary>
    /// 全ての元号情報を取得します
    /// </summary>
    /// <returns>全元号情報のコレクション（時系列順）</returns>
    public IReadOnlyList<EraInfo> GetAll()
    {
        return _eras;
    }

    /// <summary>
    /// 指定されたIDの元号情報を取得します
    /// </summary>
    /// <param name="id">元号ID</param>
    /// <returns>元号情報。見つからない場合はnull</returns>
    public EraInfo? GetById(int id)
    {
        return _eras.FirstOrDefault(era => era.Id == id);
    }

    /// <summary>
    /// 指定された元号名の元号情報を取得します
    /// </summary>
    /// <param name="eraName">元号名</param>
    /// <returns>元号情報。見つからない場合はnull</returns>
    public EraInfo? GetByName(string eraName)
    {
        ArgumentException.ThrowIfNullOrEmpty(eraName);
        return _eras.FirstOrDefault(era => era.Era.Name == eraName);
    }

    /// <summary>
    /// 指定された日付を含む元号情報を取得します
    /// </summary>
    /// <param name="date">検索対象の日付</param>
    /// <returns>元号情報。見つからない場合はnull</returns>
    public EraInfo? GetByDate(DateTime date)
    {
        return _eras.FirstOrDefault(era => era.Contains(date));
    }

    /// <summary>
    /// 現在の元号情報を取得します
    /// </summary>
    /// <returns>現在の元号情報。見つからない場合はnull</returns>
    public EraInfo? GetCurrent()
    {
        return _eras.FirstOrDefault(era => era.Era.IsCurrent);
    }

    /// <summary>
    /// 指定された期間と重複する元号情報を取得します
    /// </summary>
    /// <param name="startDate">開始日</param>
    /// <param name="endDate">終了日</param>
    /// <returns>重複する元号情報のコレクション</returns>
    public IReadOnlyList<EraInfo> GetOverlapping(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
            throw new ArgumentException("開始日は終了日より前である必要があります");

        return _eras.Where(era => 
        {
            var eraStart = era.Era.StartDate;
            var eraEnd = era.Era.EndDate ?? DateTime.MaxValue;
            
            return eraStart <= endDate && eraEnd >= startDate;
        }).ToList();
    }

    /// <summary>
    /// 元号データを初期化します
    /// </summary>
    /// <returns>初期化された元号情報のコレクション</returns>
    private static IReadOnlyList<EraInfo> InitializeEras()
    {
        return new List<EraInfo>
        {
            new(1, new Era("明治", new DateTime(1868, 1, 25), new DateTime(1912, 7, 29)), 1),
            new(2, new Era("大正", new DateTime(1912, 7, 30), new DateTime(1926, 12, 24)), 2),
            new(3, new Era("昭和", new DateTime(1926, 12, 25), new DateTime(1989, 1, 7)), 3),
            new(4, new Era("平成", new DateTime(1989, 1, 8), new DateTime(2019, 4, 30)), 4),
            new(5, new Era("令和", new DateTime(2019, 5, 1)), 5)
        };
    }
}