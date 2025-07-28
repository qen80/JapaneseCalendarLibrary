using JapaneseCalendarLibrary.Domain.ValueObjects;

namespace JapaneseCalendarLibrary.Tests.Domain.ValueObjects;

/// <summary>
/// JapaneseDateクラスのテスト
/// </summary>
public class JapaneseDateTests
{
    [Fact]
    public void 正常な値で和暦日付を作成すると正しく初期化される()
    {
        // Given: 正常な和暦日付データ
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var year = 5;
        var month = 12;
        var day = 25;

        // When: 和暦日付を作成
        var japaneseDate = new JapaneseDate(era, year, month, day);

        // Then: 正しく初期化される
        Assert.Equal(era, japaneseDate.Era);
        Assert.Equal(5, japaneseDate.Year);
        Assert.Equal(12, japaneseDate.Month);
        Assert.Equal(25, japaneseDate.Day);
        Assert.False(japaneseDate.IsFirstYear);
    }

    [Fact]
    public void 元年の和暦日付を作成するとIsFirstYearがTrueになる()
    {
        // Given: 元年の和暦日付データ
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var year = 1;
        var month = 5;
        var day = 1;

        // When: 和暦日付を作成
        var japaneseDate = new JapaneseDate(era, year, month, day);

        // Then: 元年として認識される
        Assert.True(japaneseDate.IsFirstYear);
    }

    [Fact]
    public void Nullの元号を渡すと例外が発生する()
    {
        // Given: null元号
        Era era = null;

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentNullException>(() => new JapaneseDate(era, 1, 1, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(201)]
    public void 無効な年を渡すと例外が発生する(int invalidYear)
    {
        // Given: 無効な年
        var era = new Era("令和", new DateTime(2019, 5, 1));

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentOutOfRangeException>(() => new JapaneseDate(era, invalidYear, 1, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void 無効な月を渡すと例外が発生する(int invalidMonth)
    {
        // Given: 無効な月
        var era = new Era("令和", new DateTime(2019, 5, 1));

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentOutOfRangeException>(() => new JapaneseDate(era, 1, invalidMonth, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(32)]
    public void 無効な日を渡すと例外が発生する(int invalidDay)
    {
        // Given: 無効な日
        var era = new Era("令和", new DateTime(2019, 5, 1));

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentOutOfRangeException>(() => new JapaneseDate(era, 1, 1, invalidDay));
    }

    [Fact]
    public void GregorianDate_正しい西暦日付に変換される()
    {
        // Given: 令和5年12月25日
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var japaneseDate = new JapaneseDate(era, 5, 12, 25);
        var expectedDate = new DateTime(2023, 12, 25);

        // When: 西暦日付を取得
        var gregorianDate = japaneseDate.GregorianDate;

        // Then: 正しい西暦日付が返される
        Assert.Equal(expectedDate, gregorianDate);
    }

    [Fact]
    public void GregorianDate_元年の日付が正しく変換される()
    {
        // Given: 令和元年5月1日
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var japaneseDate = new JapaneseDate(era, 1, 5, 1);
        var expectedDate = new DateTime(2019, 5, 1);

        // When: 西暦日付を取得
        var gregorianDate = japaneseDate.GregorianDate;

        // Then: 正しい西暦日付が返される
        Assert.Equal(expectedDate, gregorianDate);
    }

    [Fact]
    public void ToString_標準形式で文字列表現を返す()
    {
        // Given: 令和5年12月25日
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var japaneseDate = new JapaneseDate(era, 5, 12, 25);

        // When: 文字列表現を取得
        var result = japaneseDate.ToString();

        // Then: 正しい形式で返される
        Assert.Equal("令和5年12月25日", result);
    }

    [Fact]
    public void ToString_元年は元と表示される()
    {
        // Given: 令和元年5月1日
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var japaneseDate = new JapaneseDate(era, 1, 5, 1);

        // When: 文字列表現を取得
        var result = japaneseDate.ToString();

        // Then: 「元」と表示される
        Assert.Equal("令和元年5月1日", result);
    }

    [Fact]
    public void ToShortString_短縮形式で文字列表現を返す()
    {
        // Given: 令和5年12月25日
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var japaneseDate = new JapaneseDate(era, 5, 12, 25);

        // When: 短縮形文字列を取得
        var result = japaneseDate.ToShortString();

        // Then: 正しい短縮形式で返される
        Assert.Equal("R5.12.25", result);
    }

    [Theory]
    [InlineData("令和", "R")]
    [InlineData("平成", "H")]
    [InlineData("昭和", "S")]
    [InlineData("大正", "T")]
    [InlineData("明治", "M")]
    public void ToShortString_各元号の略称が正しく表示される(string eraName, string expectedAbbr)
    {
        // Given: 各元号の和暦日付
        var era = new Era(eraName, new DateTime(1900, 1, 1));
        var japaneseDate = new JapaneseDate(era, 5, 1, 1);

        // When: 短縮形文字列を取得
        var result = japaneseDate.ToShortString();

        // Then: 正しい略称で表示される
        Assert.StartsWith(expectedAbbr, result);
    }

    [Fact]
    public void Record型として等価性が正しく動作する()
    {
        // Given: 同じ内容の2つの和暦日付
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var date1 = new JapaneseDate(era, 5, 12, 25);
        var date2 = new JapaneseDate(era, 5, 12, 25);
        var date3 = new JapaneseDate(era, 5, 12, 26);

        // When & Then: 等価性が正しく判定される
        Assert.Equal(date1, date2);
        Assert.NotEqual(date1, date3);
        Assert.True(date1 == date2);
        Assert.False(date1 == date3);
    }

    [Fact]
    public void うるう年の2月29日の日付が正しく処理される()
    {
        // Given: うるう年の2月29日（令和2年 = 2020年）
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var japaneseDate = new JapaneseDate(era, 2, 2, 29);

        // When: 西暦日付を取得
        var gregorianDate = japaneseDate.GregorianDate;

        // Then: 正しい日付が返される
        Assert.Equal(new DateTime(2020, 2, 29), gregorianDate);
    }

    [Fact]
    public void 平年の2月29日を指定すると例外が発生する()
    {
        // Given: 平年の2月29日（令和3年 = 2021年）
        var era = new Era("令和", new DateTime(2019, 5, 1));

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentOutOfRangeException>(() => new JapaneseDate(era, 3, 2, 29));
    }
}