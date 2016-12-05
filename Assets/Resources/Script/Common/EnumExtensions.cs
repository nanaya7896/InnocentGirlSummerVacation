using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Enum型の拡張メソッド
/// </summary>
public static class EnumExtensions{

    /// <summary>
    /// フラグが立っているかどうか判定して結果を返してくれる
    /// </summary>
    /// <returns><c>true</c>, if flag was hased, <c>false</c> otherwise.</returns>
    /// <param name="self">Self.</param>
    /// <param name="flag">Flag.</param>
    public static bool HasFlag(this Enum self, Enum flag)
    {
        if (self.GetType() != flag.GetType())
        {
            throw new ArgumentException("flag型が現在のインスタンスの型と異なっています");
        }
        var selfValue = Convert.ToUInt64(self);
        var flagValue = Convert.ToUInt64(flag);

        return (selfValue & flagValue) == flagValue;
    }
}
