namespace JapaneseCalendarLibrary.Domain.ValueObjects;

/// <summary>
/// 日本の元号を表すバリューオブジェクト
/// </summary>
/// <param name="Name">元号名（例：令和、平成）</param>
/// <param name="StartDate">元号開始日（西暦）</param>
/// <param name="EndDate">元号終了日（西暦、現在の元号の場合はnull）</param>
public record Era(string Name, DateTime StartDate, DateTime? EndDate = null)
{
    /// <summary>
    /// 元号名を取得します
    /// </summary>
    /// <value>元号名（ひらがな表記）</value>
    public string Name { get; } = ValidateName(Name);

    /// <summary>
    /// 元号の開始日を取得します
    /// </summary>
    /// <value>元号が開始された西暦日付</value>
    public DateTime StartDate { get; } = StartDate;

    /// <summary>
    /// 元号の終了日を取得します
    /// </summary>
    /// <value>元号が終了した西暦日付。現在の元号の場合はnull</value>
    public DateTime? EndDate { get; } = EndDate;

    /// <summary>
    /// 現在も継続中の元号かどうかを取得します
    /// </summary>
    /// <value>継続中の場合true、終了済みの場合false</value>
    public bool IsCurrent => EndDate == null;

    /// <summary>
    /// 指定された日付がこの元号の期間内かどうかを判定します
    /// </summary>
    /// <param name="date">判定対象の日付</param>
    /// <returns>期間内の場合true、期間外の場合false</returns>
    public bool Contains(DateTime date)
    {
        ArgumentNullException.ThrowIfNull(date);
        
        if (date < StartDate) return false;
        if (EndDate.HasValue && date > EndDate.Value) return false;
        
        return true;
    }

    /// <summary>
    /// 元号名のバリデーションを行います
    /// </summary>
    /// <param name="name">検証する元号名</param>
    /// <returns>検証済みの元号名</returns>
    /// <exception cref="ArgumentException">元号名が無効な場合</exception>
    private static string ValidateName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        if (name.Length > 10)
            throw new ArgumentException("元号名は10文字以下である必要があります", nameof(name));
            
        return name.Trim();
    }

    /// <summary>
    /// 元号の文字列表現を取得します
    /// </summary>
    /// <returns>元号名と期間を含む文字列</returns>
    public override string ToString()
    {
        var endDateStr = EndDate?.ToString("yyyy年M月d日") ?? "現在";
        return $"{Name}（{StartDate:yyyy年M月d日} - {endDateStr}）";
    }
}