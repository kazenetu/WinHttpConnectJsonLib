using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransferObjects.Response
{
  /// <summary>
  /// Responseのスーパークラス
  /// </summary>
  public class BaseResponse<T> where T : class
  {
    /// <summary>
    /// ステータス列挙型
    /// </summary>
    public enum Results
    {
      OK, NG
    }

    /// <summary>
    /// ステータス
    /// </summary>
    protected Results result;

    /// <summary>
    /// ステータス(文字列)
    /// </summary>
    public string Result
    {
      get
      {
        return result.ToString();
      }
    }

    /// <summary>
    /// エラーメッセージ
    /// </summary>
    public string ErrorMessage { protected set; get; }

    /// <summary>
    /// データ
    /// </summary>
    public T ResponseData { protected set; get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="result">結果</param>
    /// <param name="errorMessage">エラーメッセージ</param>
    public BaseResponse(Results result, string errorMessage)
    {
      this.result = result;
      this.ErrorMessage = errorMessage;
      ResponseData = null;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="result">結果</param>
    /// <param name="errorMessage">エラーメッセージ</param>
    /// <param name="responseData">データ</param>
    public BaseResponse(Results result, string errorMessage, T responseData)
    {
      this.result = result;
      this.ErrorMessage = errorMessage;
      this.ResponseData = responseData;
    }
  }
}
