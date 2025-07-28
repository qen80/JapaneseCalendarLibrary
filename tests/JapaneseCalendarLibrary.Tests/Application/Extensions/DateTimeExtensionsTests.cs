using JapaneseCalendarLibrary.Application.Extensions;

namespace JapaneseCalendarLibrary.Tests.Application.Extensions;

/// <summary>
/// DateTimeExtensionsクラスのテスト
/// </summary>
public class DateTimeExtensionsTests
{
    [Fact]
    public void ToJapaneseDate_令和の日付_正しく変換される()
    {
        // Given: 令和の西暦日付
        var gregorianDate = new DateTime(2023, 12, 25);

        // When: 和暦日付に変換
        var result = gregorianDate.ToJapaneseDate();

        // Then: 正しい和暦日付が返される
        Assert.Equal("令和", result.Era.Name);
        Assert.Equal(5, result.Year);
        Assert.Equal(12, result.Month);
        Assert.Equal(25, result.Day);
    }

    [Fact]
    public void GetEra_令和の日付_令和元号を返す()
    {
        // Given: 令和の西暦日付
        var gregorianDate = new DateTime(2023, 12, 25);

        // When: 元号を取得
        var result = gregorianDate.GetEra();

        // Then: 令和元号が返される
        Assert.NotNull(result);
        Assert.Equal("令和", result.Name);
    }

    [Fact]
    public void ToJapaneseDateString_標準形式文字列を返す()
    {
        // Given: 令和の西暦日付
        var gregorianDate = new DateTime(2023, 12, 25);

        // When: 和暦文字列を取得
        var result = gregorianDate.ToJapaneseDateString();

        // Then: 正しい形式で返される
        Assert.Equal("令和5年12月25日", result);
    }

    [Fact]
    public void ToJapaneseShortDateString_短縮形式文字列を返す()
    {
        // Given: 令和の西暦日付
        var gregorianDate = new DateTime(2023, 12, 25);

        // When: 和暦短縮文字列を取得
        var result = gregorianDate.ToJapaneseShortDateString();

        // Then: 正しい短縮形式で返される
        Assert.Equal("R5.12.25", result);
    }

    [Theory]
    [InlineData("2023-12-25", "令和")]
    [InlineData("2000-01-01", "平成")]
    [InlineData("1950-05-15", "昭和")]
    public void IsInEra_指定元号の期間内_Trueを返す(string dateStr, string eraName)
    {
        // Given: 指定元号期間内の日付
        var gregorianDate = DateTime.Parse(dateStr);

        // When: 元号期間内かチェック
        var result = gregorianDate.IsInEra(eraName);

        // Then: trueが返される
        Assert.True(result);
    }

    [Fact]
    public void IsInEra_指定元号の期間外_Falseを返す()
    {
        // Given: 令和期間外の日付
        var gregorianDate = new DateTime(2000, 1, 1);

        // When: 令和期間内かチェック
        var result = gregorianDate.IsInEra("令和");

        // Then: falseが返される
        Assert.False(result);
    }

    [Fact]
    public void IsReiwa_令和の日付_Trueを返す()
    {
        // Given: 令和の日付
        var gregorianDate = new DateTime(2023, 12, 25);

        // When: 令和かチェック
        var result = gregorianDate.IsReiwa();

        // Then: trueが返される
        Assert.True(result);
    }

    [Fact]
    public void IsHeisei_平成の日付_Trueを返す()
    {
        // Given: 平成の日付
        var gregorianDate = new DateTime(2000, 1, 1);

        // When: 平成かチェック
        var result = gregorianDate.IsHeisei();

        // Then: trueが返される
        Assert.True(result);
    }

    [Fact]
    public void IsShowa_昭和の日付_Trueを返す()
    {
        // Given: 昭和の日付
        var gregorianDate = new DateTime(1950, 5, 15);

        // When: 昭和かチェック
        var result = gregorianDate.IsShowa();

        // Then: trueが返される
        Assert.True(result);
    }

    [Fact]
    public void IsTaisho_大正の日付_Trueを返す()
    {
        // Given: 大正の日付
        var gregorianDate = new DateTime(1920, 1, 1);

        // When: 大正かチェック
        var result = gregorianDate.IsTaisho();

        // Then: trueが返される
        Assert.True(result);
    }

    [Fact]
    public void IsMeiji_明治の日付_Trueを返す()
    {
        // Given: 明治の日付
        var gregorianDate = new DateTime(1900, 1, 1);

        // When: 明治かチェック
        var result = gregorianDate.IsMeiji();

        // Then: trueが返される
        Assert.True(result);
    }

    [Fact]
    public void GetJapaneseYear_令和3年_3を返す()
    {
        // Given: 令和3年の日付
        var gregorianDate = new DateTime(2021, 6, 15);

        // When: 令和での和暦年を取得
        var result = gregorianDate.GetJapaneseYear("令和");

        // Then: 3が返される
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetJapaneseYear_範囲外の元号_マイナス1を返す()
    {
        // Given: 令和の範囲外の日付
        var gregorianDate = new DateTime(2000, 1, 1);

        // When: 令和での和暦年を取得
        var result = gregorianDate.GetJapaneseYear("令和");

        // Then: -1が返される
        Assert.Equal(-1, result);
    }

    [Fact]
    public void CalculateJapaneseAge_正しい年齢情報を返す()
    {
        // Given: 生年月日と基準日
        var birthDate = new DateTime(1990, 5, 15);
        var baseDate = new DateTime(2023, 12, 25);

        // When: 和暦年齢を計算
        var result = birthDate.CalculateJapaneseAge(baseDate);

        // Then: 正しい年齢情報が返される
        Assert.Equal("令和", result.Era);
        Assert.Equal(33, result.Age);
    }

    [Fact]
    public void CalculateJapaneseAge_基準日省略時_今日基準で計算される()
    {
        // Given: 生年月日
        var birthDate = new DateTime(1990, 5, 15);

        // When: 和暦年齢を計算（基準日省略）
        var result = birthDate.CalculateJapaneseAge();

        // Then: 現在の元号で年齢が計算される
        Assert.Equal("令和", result.Era);
        Assert.True(result.Age >= 0);
    }

    [Fact]
    public void 誕生日前後で年齢計算が正しく動作する()
    {
        // Given: 5月15日生まれ
        var birthDate = new DateTime(1990, 5, 15);
        var beforeBirthday = new DateTime(2023, 5, 14); // 誕生日前
        var onBirthday = new DateTime(2023, 5, 15);     // 誕生日当日
        var afterBirthday = new DateTime(2023, 5, 16);  // 誕生日後

        // When: それぞれの日付で年齢計算
        var ageBefore = birthDate.CalculateJapaneseAge(beforeBirthday);
        var ageOn = birthDate.CalculateJapaneseAge(onBirthday);
        var ageAfter = birthDate.CalculateJapaneseAge(afterBirthday);

        // Then: 誕生日を境に年齢が変わる
        Assert.Equal(32, ageBefore.Age);
        Assert.Equal(33, ageOn.Age);
        Assert.Equal(33, ageAfter.Age);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void IsInEra_無効な元号名_Falseを返す(string invalidEraName)
    {
        // Given: 任意の日付と無効な元号名
        var gregorianDate = new DateTime(2023, 12, 25);

        // When: 元号期間内かチェック
        var result = gregorianDate.IsInEra(invalidEraName);

        // Then: falseが返される
        Assert.False(result);
    }
}