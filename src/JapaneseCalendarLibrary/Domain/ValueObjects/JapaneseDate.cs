namespace JapaneseCalendarLibrary.Domain.ValueObjects;

/// <summary>
/// 和暦の日付を表すバリューオブジェクト
/// </summary>
/// <param name="Era">元号</param>
/// <param name="Year">和暦年（元号からの経過年数）</param>
/// <param name="Month">月（1-12）</param>
/// <param name="Day">日（1-31）</param>
public record JapaneseDate(Era Era, int Year, int Month, int Day)
{
    /// <summary>
    /// 元号を取得します
    /// </summary>
    /// <value>この日付が属する元号</value>
    public Era Era { get; } = Era ?? throw new ArgumentNullException(nameof(Era));

    /// <summary>
    /// 和暦年を取得します
    /// </summary>
    /// <value>元号からの経過年数（1年から開始）</value>
    public int Year { get; } = ValidateYear(Year);

    /// <summary>
    /// 月を取得します
    /// </summary>
    /// <value>月（1-12の範囲）</value>
    public int Month { get; } = ValidateMonth(Month);

    /// <summary>
    /// 日を取得します
    /// </summary>
    /// <value>日（1-31の範囲、月によって上限が異なる）</value>
    public int Day { get; } = ValidateDay(Day, Month, Year, Era);

    /// <summary>
    /// 対応する西暦日付を取得します
    /// </summary>
    /// <value>西暦での日付表現</value>
    /// <exception cref="InvalidOperationException">無効な和暦日付の場合</exception>
    public DateTime GregorianDate
    {
        get
        {
            try
            {
                var gregorianYear = Era.StartDate.Year + Year - 1;
                return new DateTime(gregorianYear, Month, Day);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"無効な和暦日付です: {this}", ex);
            }
        }
    }

    /// <summary>
    /// 年が元年かどうかを取得します
    /// </summary>
    /// <value>元年の場合true、そうでなければfalse</value>
    public bool IsFirstYear => Year == 1;

    /// <summary>
    /// 和暦年のバリデーションを行います
    /// </summary>
    /// <param name="year">検証する年</param>
    /// <returns>検証済みの年</returns>
    /// <exception cref="ArgumentOutOfRangeException">年が無効な場合</exception>
    private static int ValidateYear(int year)
    {
        if (year < 1 || year > 200)
            throw new ArgumentOutOfRangeException(nameof(year), year, "年は1以上200以下である必要があります");
            
        return year;
    }

    /// <summary>
    /// 月のバリデーションを行います
    /// </summary>
    /// <param name="month">検証する月</param>
    /// <returns>検証済みの月</returns>
    /// <exception cref="ArgumentOutOfRangeException">月が無効な場合</exception>
    private static int ValidateMonth(int month)
    {
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month), month, "月は1以上12以下である必要があります");
            
        return month;
    }

    /// <summary>
    /// 日のバリデーションを行います
    /// </summary>
    /// <param name="day">検証する日</param>
    /// <param name="month">月</param>
    /// <param name="year">年</param>
    /// <param name="era">元号</param>
    /// <returns>検証済みの日</returns>
    /// <exception cref="ArgumentOutOfRangeException">日が無効な場合</exception>
    private static int ValidateDay(int day, int month, int year, Era era)
    {
        if (day < 1 || day > 31)
            throw new ArgumentOutOfRangeException(nameof(day), day, "日は1以上31以下である必要があります");

        // より詳細な日付検証は西暦変換時に行う（うるう年考慮等）
        try
        {
            var gregorianYear = era.StartDate.Year + year - 1;
            var daysInMonth = DateTime.DaysInMonth(gregorianYear, month);
            if (day > daysInMonth)
                throw new ArgumentOutOfRangeException(nameof(day), day, 
                    $"{era.Name}{year}年{month}月は{daysInMonth}日までです");
        }
        catch (ArgumentOutOfRangeException ex) when (ex.ParamName != nameof(day))
        {
            // 西暦変換でエラーが発生した場合は、基本的な範囲チェックのみ実施
            if (day > 31)
                throw new ArgumentOutOfRangeException(nameof(day), day, "日は31以下である必要があります");
        }

        return day;
    }

    /// <summary>
    /// 和暦日付の標準的な文字列表現を取得します
    /// </summary>
    /// <returns>「令和5年12月25日」形式の文字列</returns>
    public override string ToString()
    {
        var yearStr = IsFirstYear ? "元" : Year.ToString();
        return $"{Era.Name}{yearStr}年{Month}月{Day}日";
    }

    /// <summary>
    /// 和暦日付の短縮形文字列表現を取得します
    /// </summary>
    /// <returns>「R5.12.25」形式の文字列</returns>
    public string ToShortString()
    {
        var eraAbbr = GetEraAbbreviation(Era.Name);
        var yearStr = IsFirstYear ? "1" : Year.ToString();
        return $"{eraAbbr}{yearStr}.{Month}.{Day}";
    }

    /// <summary>
    /// 元号の略称を取得します
    /// </summary>
    /// <param name="eraName">元号名</param>
    /// <returns>元号の略称</returns>
    private static string GetEraAbbreviation(string eraName) => eraName switch
    {
        "令和" => "R",
        "平成" => "H",
        "昭和" => "S",
        "大正" => "T",
        "明治" => "M",
        _ => eraName[0].ToString()
    };
}