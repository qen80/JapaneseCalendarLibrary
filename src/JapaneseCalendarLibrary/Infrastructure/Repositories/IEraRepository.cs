using JapaneseCalendarLibrary.Domain.Entities;
using JapaneseCalendarLibrary.Domain.ValueObjects;

namespace JapaneseCalendarLibrary.Infrastructure.Repositories;

/// <summary>
/// 元号データへのアクセス機能を提供するリポジトリインターフェース
/// </summary>
public interface IEraRepository
{
    /// <summary>
    /// 全ての元号情報を取得します
    /// </summary>
    /// <returns>全元号情報のコレクション（時系列順）</returns>
    IReadOnlyList<EraInfo> GetAll();

    /// <summary>
    /// 指定されたIDの元号情報を取得します
    /// </summary>
    /// <param name="id">元号ID</param>
    /// <returns>元号情報。見つからない場合はnull</returns>
    EraInfo? GetById(int id);

    /// <summary>
    /// 指定された元号名の元号情報を取得します
    /// </summary>
    /// <param name="eraName">元号名</param>
    /// <returns>元号情報。見つからない場合はnull</returns>
    EraInfo? GetByName(string eraName);

    /// <summary>
    /// 指定された日付を含む元号情報を取得します
    /// </summary>
    /// <param name="date">検索対象の日付</param>
    /// <returns>元号情報。見つからない場合はnull</returns>
    EraInfo? GetByDate(DateTime date);

    /// <summary>
    /// 現在の元号情報を取得します
    /// </summary>
    /// <returns>現在の元号情報。見つからない場合はnull</returns>
    EraInfo? GetCurrent();

    /// <summary>
    /// 指定された期間と重複する元号情報を取得します
    /// </summary>
    /// <param name="startDate">開始日</param>
    /// <param name="endDate">終了日</param>
    /// <returns>重複する元号情報のコレクション</returns>
    IReadOnlyList<EraInfo> GetOverlapping(DateTime startDate, DateTime endDate);
}