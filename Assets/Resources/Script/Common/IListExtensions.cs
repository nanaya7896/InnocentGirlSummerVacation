
using System.Collections.Generic;

/// <summary>
/// リストの中身がnullか空か調べる
/// </summary>
public static class IListExtensions{


	/// <summary>
	/// 指定された配列がnullか要素数0かどうか調べる
	/// </summary>
	/// <returns><c>true</c> 空あるいは要素数０, <c>false</c>.</returns>
	/// <param name="list">リストや配列.</param>
	/// <typeparam name="TItem">アイテム要素型の宣言.</typeparam>
	public static bool IsNullOrEmpty<TItem>(IEnumerable<TItem> list)
	{

		if (list == null) {
			return true;
		}

		foreach (var item in list) {
			return false;
		}
		return true;

	}

}
