using JapaneseCalendarLibrary.Domain.ValueObjects;

namespace JapaneseCalendarLibrary.Tests.Domain.ValueObjects;

/// <summary>
/// Eraクラスのテスト
/// </summary>
public class EraTests
{
    [Fact]
    public void 正常な値で元号を作成すると正しく初期化される()
    {
        // Given: 正常な元号データ
        var name = "令和";
        var startDate = new DateTime(2019, 5, 1);
        var endDate = (DateTime?)null;

        // When: 元号を作成
        var era = new Era(name, startDate, endDate);

        // Then: 正しく初期化される
        Assert.Equal("令和", era.Name);
        Assert.Equal(new DateTime(2019, 5, 1), era.StartDate);
        Assert.Null(era.EndDate);
        Assert.True(era.IsCurrent);
    }

    [Fact]
    public void 終了日付ありの元号を作成すると正しく初期化される()
    {
        // Given: 終了日付ありの元号データ
        var name = "平成";
        var startDate = new DateTime(1989, 1, 8);
        var endDate = new DateTime(2019, 4, 30);

        // When: 元号を作成
        var era = new Era(name, startDate, endDate);

        // Then: 正しく初期化される
        Assert.Equal("平成", era.Name);
        Assert.Equal(new DateTime(1989, 1, 8), era.StartDate);
        Assert.Equal(new DateTime(2019, 4, 30), era.EndDate);
        Assert.False(era.IsCurrent);
    }

    [Fact]
    public void 空文字の元号名を渡すと例外が発生する()
    {
        // Given: 空文字の元号名
        var invalidName = "";
        var startDate = new DateTime(2019, 5, 1);

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentException>(() => new Era(invalidName, startDate));
    }

    [Fact]
    public void Nullの元号名を渡すと例外が発生する()
    {
        // Given: null元号名
        string invalidName = null;
        var startDate = new DateTime(2019, 5, 1);

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentNullException>(() => new Era(invalidName, startDate));
    }

    [Fact]
    public void 長すぎる元号名を渡すと例外が発生する()
    {
        // Given: 11文字の元号名
        var longName = "とても長い元号名ですね"; // 11文字
        var startDate = new DateTime(2019, 5, 1);

        // When & Then: 例外が発生する
        Assert.Throws<ArgumentException>(() => new Era(longName, startDate));
    }

    [Fact]
    public void Contains_元号期間内の日付_Trueを返す()
    {
        // Given: 平成の元号と期間内の日付
        var era = new Era("平成", new DateTime(1989, 1, 8), new DateTime(2019, 4, 30));
        var targetDate = new DateTime(2000, 1, 1);

        // When: 日付が期間内かチェック
        var result = era.Contains(targetDate);

        // Then: trueが返される
        Assert.True(result);
    }

    [Fact]
    public void Contains_元号期間外の日付_Falseを返す()
    {
        // Given: 平成の元号と期間外の日付
        var era = new Era("平成", new DateTime(1989, 1, 8), new DateTime(2019, 4, 30));
        var targetDate = new DateTime(2019, 5, 1);

        // When: 日付が期間内かチェック
        var result = era.Contains(targetDate);

        // Then: falseが返される
        Assert.False(result);
    }

    [Fact]
    public void Contains_現在の元号で未来の日付_Trueを返す()
    {
        // Given: 現在の元号（令和）と未来の日付
        var era = new Era("令和", new DateTime(2019, 5, 1));
        var futureDate = new DateTime(2030, 1, 1);

        // When: 日付が期間内かチェック
        var result = era.Contains(futureDate);

        // Then: trueが返される
        Assert.True(result);
    }

    [Fact]
    public void ToString_正しい文字列表現を返す()
    {
        // Given: 平成の元号
        var era = new Era("平成", new DateTime(1989, 1, 8), new DateTime(2019, 4, 30));

        // When: 文字列表現を取得
        var result = era.ToString();

        // Then: 正しい形式で返される
        Assert.Equal("平成（1989年1月8日 - 2019年4月30日）", result);
    }

    [Fact]
    public void ToString_現在の元号_現在と表示される()
    {
        // Given: 現在の元号（令和）
        var era = new Era("令和", new DateTime(2019, 5, 1));

        // When: 文字列表現を取得
        var result = era.ToString();

        // Then: 現在と表示される
        Assert.Equal("令和（2019年5月1日 - 現在）", result);
    }

    [Fact]
    public void Record型として等価性が正しく動作する()
    {
        // Given: 同じ内容の2つの元号
        var era1 = new Era("令和", new DateTime(2019, 5, 1));
        var era2 = new Era("令和", new DateTime(2019, 5, 1));
        var era3 = new Era("平成", new DateTime(1989, 1, 8), new DateTime(2019, 4, 30));

        // When & Then: 等価性が正しく判定される
        Assert.Equal(era1, era2);
        Assert.NotEqual(era1, era3);
        Assert.True(era1 == era2);
        Assert.False(era1 == era3);
    }
}