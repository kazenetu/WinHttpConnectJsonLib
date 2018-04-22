using System;
using System.Collections.Generic;
using Domain.Service.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebApi.Utilities;
using static Domain.Service.ServiceBase;
using WebApi.DTO;
using DataTransferObjects.Request;
using DataTransferObjects.Response;

namespace WebApi.Controllers
{
  [Route("api/account")]
  public class AccountController : ControllerBase
  {
    #region プライベートフィールド

    /// <summary>
    /// サービスインスタンス
    /// </summary>
    private readonly IUserService service;

    /// <summary>
    /// ログインスタンス
    /// </summary>
    private readonly ILogger logger;

    #endregion

    #region プライベート定数フィールド
    private static string ErrorLoginNG = "ログイン失敗";
    private static string ErrorPasswordNG = "パスワード失敗";
    #endregion

    #region コンストラクタ

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="service">サービスインスタンス</param>
    /// <param name="logger">ログインスタンス</param>
    public AccountController(IUserService service, ILogger<AccountController> logger)
    {
      this.service = service;
      this.logger = logger;
    }

    #endregion

    #region パブリックメソッド

    /// <summary>
    /// テストメソッド
    /// </summary>
    /// <param name="param"></param>
    /// <remarks>AutoValidateAntiforgeryToken対象外</remarks>
    /// <returns></returns>
    [HttpPost("testPost")]
    [IgnoreAntiforgeryToken]
    public IActionResult TestPost([FromBody]Dictionary<string, object> param)
    {
      var result = new ResponseDTO(ResponseDTO.Results.OK, string.Empty, param);
      return Json(result);
    }


    #region ログイン・ログアウト

    /// <summary>
    /// ログイン
    /// </summary>
    /// <param name="param">入力情報</param>
    /// <returns>結果(json)</returns>
    /// <remarks>POST api/user/login</remarks>
    [HttpPost("login")]
    public IActionResult Login([FromBody]LoginRequest param)
    {
      // 入力チェック
      if (string.IsNullOrEmpty(param.ID))
      {
        logger.LogError("Pram[{0}]が未設定", nameof(param.ID));
        return BadRequest();
      }
      if (string.IsNullOrEmpty(param.Password))
      {
        logger.LogError("Pram[{0}]が未設定", nameof(param.Password));
        return BadRequest();
      }

      var data = new LoginResponse.LoginResponseParam();
      var serviceResult = false;
      try
      {
        var passwordHash = string.Empty;

        var model = service.Find(param.ID);
        if (model != null && model.EntryDate.HasValue)
        {
          // パスワードのハッシュ取得
          passwordHash = HashUtility.Create(model.UserID, param.Password, model.EntryDate.Value);
        }

        // ログインチェック
        if (service.Login(param.ID, passwordHash) != null)
        {
          // セッション破棄
          refreshSession();

          // セッションキー設定
          session.SetString(SessionKeyUserID, param.ID);

          serviceResult = true;
          data.Name = model.UserName;
        }
      }
      catch (Exception ex)
      {
        logger.LogCritical("{0}", ex.Message);
        return BadRequest();
      }

      LoginResponse result = null;
      if (serviceResult)
      {
        result = new LoginResponse(LoginResponse.Results.OK, string.Empty, data);
      }
      else
      {
        result = new LoginResponse(LoginResponse.Results.NG, ErrorLoginNG);
      }

      return Json(result);
    }

    /// <summary>
    /// ログアウト
    /// </summary>
    /// <param name="param">入力情報</param>
    /// <returns>結果(json)</returns>
    /// <remarks>POST api/user/logout</remarks>
    [HttpPost("logout")]
    public IActionResult Logout([FromBody]Dictionary<string, object> param)
    {

      // セッション破棄
      HttpContext.Response.Cookies.Delete(ControllerBase.SessionCookieName);
      session.Clear();

      ResponseDTO result = new ResponseDTO(ResponseDTO.Results.OK, string.Empty, null);

      return Json(result);
    }
    #endregion

    #region パスワード変更

    /// <summary>
    /// パスワード変更
    /// </summary>
    /// <param name="param">入力情報</param>
    /// <returns>結果(json)</returns>
    /// <remarks>POST api/user/passwordChange</remarks>
    [HttpPost("passwordChange")]
    public IActionResult PasswordChange([FromBody]Dictionary<string, object> param)
    {
      var paramNameUserId = "id";
      var paramNamePassword = "password";
      var paramNameNewPassword = "newPassword";

      // 入力チェック
      if (!param.ContainsKey(paramNameUserId))
      {
        logger.LogError("Pram[{0}]が未設定", paramNameUserId);
        return BadRequest();
      }
      if (!param.ContainsKey(paramNamePassword))
      {
        logger.LogError("Pram[{0}]が未設定", paramNamePassword);
        return BadRequest();
      }
      if (!param.ContainsKey(paramNameNewPassword))
      {
        logger.LogError("Pram[{0}]が未設定", paramNameNewPassword);
        return BadRequest();
      }

      var userId = param[paramNameUserId].ToString();
      var password = param[paramNamePassword].ToString();
      var newPassword = param[paramNameNewPassword].ToString();

      var data = new Dictionary<string, object>();
      var serviceResult = UpdateResult.Error;
      try
      {
        var model = service.Find(userId);
        if (model != null && model.EntryDate.HasValue)
        {
          // パスワードのハッシュ取得
          var passwordHash = HashUtility.Create(model.UserID, password, model.EntryDate.Value);
          var newPasswordHash = HashUtility.Create(model.UserID, newPassword, model.EntryDate.Value);

          // パスワード変更
          serviceResult = service.ChangePassword(userId, passwordHash, newPasswordHash);
        }

      }
      catch (Exception ex)
      {
        logger.LogCritical("{0}", ex.Message);
        return BadRequest();
      }

      ResponseDTO result = null;
      if (serviceResult == UpdateResult.OK)
      {
        result = new ResponseDTO(ResponseDTO.Results.OK, string.Empty, data);
      }
      else
      {
        result = new ResponseDTO(ResponseDTO.Results.NG, ErrorPasswordNG);
      }

      return Json(result);
    }
    #endregion

    #endregion
  }
}