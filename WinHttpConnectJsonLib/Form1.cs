using DataTransferObjects.Request;
using DataTransferObjects.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinClient.connectLib;

namespace WinHttpConnectJsonLib
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    /// <summary>
    /// WebAPIのルートURLを取得
    /// </summary>
    /// <param name="setApiPath">APIパスの追加</param>
    /// <returns>APIのルートURL</returns>
    private string getWebApiRootAddress(bool setApiPath)
    {
      var url = string.Empty;

      if (ApplicationDeployment.IsNetworkDeployed)
      {
        // ClickOnce実行時のURL
        var updateLocation = ApplicationDeployment.CurrentDeployment.UpdateLocation;
        url = string.Format("{0}://{1}:{2}/", updateLocation.Scheme, updateLocation.Host, updateLocation.Port);
      }
      else
      {
        // デバッグ実行時のURL
        var args = Environment.GetCommandLineArgs();
        if (args.Length >= 2)
        {
          url = args[1];
          if (url[url.Length - 1] != '/')
          {
            url += "/";
          }
        }
        else
        {
          url = "http://localhost:8080/";
        }
      }

      // APIパス追加
      if (setApiPath)
      {
        url += "api/";
      }

      return url;
    }

    /// <summary>
    /// 認証なしのWebAPIの呼び出し
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button1_Click(object sender, EventArgs e)
    {
      var param = new Dictionary<string, object>() { { "id", "test" }, { "test", "test" } };

      var url = "http://localhost:5000/api/account/testPost";
      var result = HttpConnectLib.Post<object>(url, param);
      MessageBox.Show(result.ToString());
    }

    /// <summary>
    /// ログインWebAPI：成功パターン
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button2_Click(object sender, EventArgs e)
    {
      // WebAPIのルートURLを取得
      var rootURL = getWebApiRootAddress(false);

      // GetTokenメソッド発行
      HttpConnectLib.GetToken(string.Format("{0}", rootURL));

      // ログインWebAPI発行
      var param = new LoginRequest() {ID="test",Password="test" };
      var results = HttpConnectLib.Post<LoginResponse>(string.Format("{0}account/login", getWebApiRootAddress(true)), param);

      // 結果を表示
      var sb = new StringBuilder();
      sb.AppendLine($"result = {results.Result}");
      sb.AppendLine($"message = {results.ErrorMessage}");
      sb.AppendLine($"data = {results.ResponseData?.Name}");
      MessageBox.Show(sb.ToString());
    }

    /// <summary>
    /// ログインWebAPI：失敗パターン
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button3_Click(object sender, EventArgs e)
    {
      // WebAPIのルートURLを取得
      var rootURL = getWebApiRootAddress(false);

      // GetTokenメソッド発行
      HttpConnectLib.GetToken(string.Format("{0}", rootURL));

      // ログインWebAPI発行
      var param = new LoginRequest() { ID = "test", Password = "test1" };
      var results = HttpConnectLib.Post<LoginResponse>(string.Format("{0}account/login", getWebApiRootAddress(true)), param);

      // 結果を表示
      var sb = new StringBuilder();
      sb.AppendLine($"result = {results.Result}");
      sb.AppendLine($"message = {results.ErrorMessage}");
      sb.AppendLine($"data = {results.ResponseData?.Name}");
      MessageBox.Show(sb.ToString());
    }
  }
}
