using JapaneseCalendarLibrary.Domain.ValueObjects;
using JapaneseCalendarLibrary.Infrastructure.Repositories;
using JapaneseCalendarLibrary.Infrastructure.Services;

namespace JapaneseCalendarLibrary.Tests.Infrastructure.Services;

/// <summary>
/// CalendarConverterクラスのテスト
/// </summary>
public class CalendarConverterTests
{
    private readonly CalendarConverter _converter;

    public CalendarConverterTests()
    {
        var repository = new EraRepository();
        _converter = new CalendarConverter(repository);
    }

    [Fact]
    public void ToJapaneseDate_令和の日付_正しく変換される()
    {
        // Given: 令和の西暦日付
        var gregorianDate = new DateTime(2023, 12, 25);

        // When: 和暦日付に変換
        var result = _converter.ToJapaneseDate(gregorianDate);

        // Then: 正しい和暦日付が返される
        Assert.Equal("令和", result.Era.Name);
        Assert.Equal(5, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(25, result.Day);
    }

    [Fact]
    public void ToJapaneseDate_平成の日付_正しく変換される()
    {
        // Given: 平成の西暦日付
        var gregorianDate = new DateTime(2000, 1, 1);

        // When: 和暦日付に変換
        var result = _converter.ToJapaneseDate(gregorianDate);

        // Then: 正しい和暦日付が返される
        Assert.Equal("平成", result.Era.Name);
        Assert.Equal(12, result.Year);
        Assert.Equal(1, result.Month);
        Assert.Equal(1, result.Day);
    }

    [Fact]
    public void ToJapaneseDate_昭和の日付_正しく変換される()
    {
        // Given: 昭和の西暦日付
        var gregorianDate = new DateTime(1950, 5, 15);

        // When: 和暦日付に変換
        var result = _converter.ToJapaneseDate(gregorianDate);

        // Then: 正しい和暦日付が返される
        Assert.Equal("昭和", result.Era.Name);
        Assert.Equal(25, result.Year);
        Assert.Equal(5, result.Month);
        Assert.Equal(15, result.Day);
    }

    [Fact]
    public void ToJapaneseDate_範囲外の日付_例外が発生する()
    {
        // Given: 明治より前の日付
        var gregorianDate = new DateTime(1850, 1, 1);

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentException>(() => _converter.ToJapaneseDate(gregorianDate));
    }

    [Fact]
    public void ToGregorianDate_正常な和暦日付_正しく変換される()
    {
        // Given: 令和5年12月25日
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var japaneseDate = new JapaneseDate(era, 5, 12, 25);
        var expectedDate = new DateTime(2023, 12, 25);

        // When: 西暦日付に変換
        var result = _converter.ToGregorianDate(japaneseDate);

        // Then: 正しい西暦日付が返される
        Assert.Equal(expectedDate, result);
    }

    [Fact]
    public void ToGregorianDate_Nullの和暦日付_例外が発生する()
    {
        // Given: null和暦日付
        JapaneseDate japaneseDate = null;

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentNullException>(() => _converter.ToGregorianDate(japaneseDate));
    }

    [Fact]
    public void FindEraByDate_令和期間の日付_令和を返す()
    {
        // Given: 令和期間の日付
        var gregorianDate = new DateTime(2023, 12, 25);

        // When: 元号を検索
        var result = _converter.FindEraByDate(gregorianDate);

        // Then: 令和が返される
        Assert.NotNull(result);
        Assert.Equal("令和", result.Name);
    }

    [Fact]
    public void FindEraByDate_範囲外の日付_Nullを返す()
    {
        // Given: 範囲外の日付
        var gregorianDate = new DateTime(1850, 1, 1);

        // When: 元号を検索
        var result = _converter.FindEraByDate(gregorianDate);

        // Then: nullが返される
        Assert.Null(result);
    }

    [Theory]
    [InlineData("令和")]
    [InlineData("平成")]
    [InlineData("昭和")]
    [InlineData("大正")]
    [InlineData("明治")]
    public void FindEraByName_有効な元号名_正しい元号を返す(string eraName)
    {
        // Given: 有効な元号名

        // When: 元号を検索
        var result = _converter.FindEraByName(eraName);

        // Then: 正しい元号が返される
        Assert.NotNull(result);
        Assert.Equal(eraName, result.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("存在しない元号")]
    public void FindEraByName_無効な元号名_Nullを返す(string invalidEraName)
    {
        // Given: 無効な元号名

        // When: 元号を検索
        var result = _converter.FindEraByName(invalidEraName);

        // Then: nullが返される
        Assert.Null(result);
    }

    [Fact]
    public void CalculateGregorianYear_令和3年_2021年を返す()
    {
        // Given: 令和3年
        var eraName = "令和";
        var japaneseYear = 3;

        // When: 西暦年を計算
        var result = _converter.CalculateGregorianYear(eraName, japaneseYear);

        // Then: 2021年が返される
        Assert.Equal(2021, result);
    }

    [Fact]
    public void CalculateGregorianYear_存在しない元号_例外が発生する()
    {
        // Given: 存在しない元号
        var eraName = "存在しない元号";
        var japaneseYear = 1;

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentException>(() => _converter.CalculateGregorianYear(eraName, japaneseYear));
    }

    [Fact]
    public void CalculateJapaneseYear_2021年令和_3年を返す()
    {
        // Given: 2021年（令和）
        var gregorianYear = 2021;
        var eraName = "令和";

        // When: 和暦年を計算
        var result = _converter.CalculateJapaneseYear(gregorianYear, eraName);

        // Then: 3年が返される
        Assert.Equal(3, result);
    }

    [Fact]
    public void CalculateJapaneseYear_範囲外の年_例外が発生する()
    {
        // Given: 令和の範囲外の年
        var gregorianYear = 2018;
        var eraName = "令和";

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentException>(() => _converter.CalculateJapaneseYear(gregorianYear, eraName));
    }

    [Fact]
    public void GetCurrentJapaneseDate_現在の日付を和暦で取得できる()
    {
        // When: 現在の和暦日付を取得
        var result = _converter.GetCurrentJapaneseDate();

        // Then: 令和の日付が返される
        Assert.NotNull(result);
        Assert.Equal("令和", result.Era.Name);
    }

    [Fact]
    public void GetToday_今日の日付を和暦で取得できる()
    {
        // When: 今日の和暦日付を取得
        var result = _converter.GetToday();

        // Then: 令和の日付が返される
        Assert.NotNull(result);
        Assert.Equal("令和", result.Era.Name);
    }

    [Fact]
    public void 元号境界日の変換が正しく動作する()
    {
        // Given: 令和開始日
        var reiwaStartDate = new DateTime(2019, 5, 1);

        // When: 和暦日付に変換
        var result = _converter.ToJapaneseDate(reiwaStartDate);

        // Then: 令和元年5月1日として変換される
        Assert.Equal("令和", result.Era.Name);
        Assert.Equal(1, result.Year);
        Assert.Equal(5, result.Month);
        Assert.Equal(1, result.Day);
        Assert.True(result.IsFirstYear);
    }

    [Fact]
    public void 平成最終日の変換が正しく動作する()
    {
        // Given: 平成最終日
        var heiseiEndDate = new DateTime(2019, 4, 30);

        // When: 和暦日付に変換
        var result = _converter.ToJapaneseDate(heiseiEndDate);

        // Then: 平成31年4月30日として変換される
        Assert.Equal("平成", result.Era.Name);
        Assert.Equal(31, result.Year);
        Assert.Equal(4, result.Month);
        Assert.Equal(30, result.Day);
    }
}