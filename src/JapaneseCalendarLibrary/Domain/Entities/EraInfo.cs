using JapaneseCalendarLibrary.Domain.ValueObjects;

namespace JapaneseCalendarLibrary.Domain.Entities;

/// <summary>
/// 元号情報を管理するエンティティ
/// </summary>
/// <param name="id">元号ID</param>
/// <param name="era">元号バリューオブジェクト</param>
/// <param name="order">元号の順序（新しいほど大きい値）</param>
public class EraInfo(int id, Era era, int order)
{
    /// <summary>
    /// 元号の一意識別子を取得します
    /// </summary>
    /// <value>元号を識別するためのID</value>
    public int Id { get; } = ValidateId(id);

    /// <summary>
    /// 元号情報を取得します
    /// </summary>
    /// <value>元号のバリューオブジェクト</value>
    public Era Era { get; } = era ?? throw new ArgumentNullException(nameof(era));

    /// <summary>
    /// 元号の順序を取得します
    /// </summary>
    /// <value>元号の時系列順序（新しいほど大きい値）</value>
    public int Order { get; } = ValidateOrder(order);

    /// <summary>
    /// 元号の継続期間（年数）を取得します
    /// </summary>
    /// <value>元号が継続した年数。現在継続中の場合は現在までの年数</value>
    public int DurationInYears
    {
        get
        {
            var endDate = Era.EndDate ?? DateTime.Now;
            return endDate.Year - Era.StartDate.Year + 1;
        }
    }

    /// <summary>
    /// 指定された西暦日付がこの元号の期間内かどうかを判定します
    /// </summary>
    /// <param name="gregorianDate">判定対象の西暦日付</param>
    /// <returns>期間内の場合true、期間外の場合false</returns>
    public bool Contains(DateTime gregorianDate)
    {
        return Era.Contains(gregorianDate);
    }

    /// <summary>
    /// 指定された西暦日付をこの元号の和暦日付に変換します
    /// </summary>
    /// <param name="gregorianDate">変換対象の西暦日付</param>
    /// <returns>変換された和暦日付</returns>
    /// <exception cref="ArgumentException">指定された日付がこの元号の期間外の場合</exception>
    public JapaneseDate ToJapaneseDate(DateTime gregorianDate)
    {
        if (!Contains(gregorianDate))
            throw new ArgumentException($"指定された日付（{gregorianDate:yyyy年M月d日}）は{Era.Name}の期間外です", nameof(gregorianDate));

        var japaneseYear = gregorianDate.Year - Era.StartDate.Year + 1;
        return new JapaneseDate(Era, japaneseYear, gregorianDate.Month, gregorianDate.Day);
    }

    /// <summary>
    /// この元号が指定された元号より新しいかどうかを判定します
    /// </summary>
    /// <param name="other">比較対象の元号情報</param>
    /// <returns>この元号の方が新しい場合true、そうでなければfalse</returns>
    public bool IsNewerThan(EraInfo other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Order > other.Order;
    }

    /// <summary>
    /// この元号が指定された元号より古いかどうかを判定します
    /// </summary>
    /// <param name="other">比較対象の元号情報</param>
    /// <returns>この元号の方が古い場合true、そうでなければfalse</returns>
    public bool IsOlderThan(EraInfo other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return Order < other.Order;
    }

    /// <summary>
    /// IDのバリデーションを行います
    /// </summary>
    /// <param name="id">検証するID</param>
    /// <returns>検証済みのID</returns>
    /// <exception cref="ArgumentOutOfRangeException">IDが無効な場合</exception>
    private static int ValidateId(int id)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id), id, "IDは1以上である必要があります");
            
        return id;
    }

    /// <summary>
    /// 順序のバリデーションを行います
    /// </summary>
    /// <param name="order">検証する順序</param>
    /// <returns>検証済みの順序</returns>
    /// <exception cref="ArgumentOutOfRangeException">順序が無効な場合</exception>
    private static int ValidateOrder(int order)
    {
        if (order < 0)
            throw new ArgumentOutOfRangeException(nameof(order), order, "順序は0以上である必要があります");
            
        return order;
    }

    /// <summary>
    /// エンティティの等価性を判定します（IDベース）
    /// </summary>
    /// <param name="obj">比較対象のオブジェクト</param>
    /// <returns>同じエンティティの場合true、そうでなければfalse</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not EraInfo other) return false;
        if (ReferenceEquals(this, other)) return true;
        
        return Id == other.Id;
    }

    /// <summary>
    /// エンティティのハッシュコードを取得します
    /// </summary>
    /// <returns>IDベースのハッシュコード</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// 元号情報の文字列表現を取得します
    /// </summary>
    /// <returns>元号情報を含む文字列</returns>
    public override string ToString()
    {
        return $"[{Id}] {Era} (順序: {Order})";
    }
}