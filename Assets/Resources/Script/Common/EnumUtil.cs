
using System.Collections.Generic;
using System;

/// <summary>
/// Enum拡張
/// </summary>
public static class EnumUtil{

    /// <summary>
    /// ストリング型をEnumに変換
    /// </summary>
    /// <returns>The enum.</returns>
    /// <param name="val">Value.</param>
    /// <typeparam name="TEnum">The 1st type parameter.</typeparam>
    public static TEnum ConvertoEnum<TEnum>(string val)
    {
        return (TEnum)Enum.Parse(typeof(TEnum), val);
    }

    /// <summary>
    /// 数値をenum型に変換
    /// </summary>
    /// <returns>The to enum.</returns>
    /// <param name="number">Number.</param>
    /// <typeparam name="TEnum">The 1st type parameter.</typeparam>
    public static TEnum ConvertToEnum<TEnum>(int number)
    {
        return (TEnum)Enum.ToObject(typeof(TEnum), number);
    }
}
