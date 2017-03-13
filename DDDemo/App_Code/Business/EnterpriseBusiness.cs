using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;

/// <summary>
/// EnterpriseBusiness 的摘要说明
/// </summary>
public class EnterpriseBusiness
{
	public EnterpriseBusiness()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 拿到企业令牌
    /// </summary>
    /// <param name="CorpId"></param>
    /// <param name="CorpSecret"></param>
    /// <returns></returns>
    public static TokenModel GetToken(string CorpId, string CorpSecret)
    {
        string tagUrl = "https://oapi.dingtalk.com/gettoken?corpid=" + CorpId + "&corpsecret=" + CorpSecret;
        string result = HttpHelper.Get(tagUrl);

        var tokenModel = JsonConvert.DeserializeObject<TokenModel>(result);
        return tokenModel;
    }

    /// <summary>
    /// 拿到当前登录的用户
    /// </summary>
    /// <param name="access_token"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    public static UserModel GetCurrentUser(string access_token, string code)
    {
        string tagUrl = "https://oapi.dingtalk.com/user/getuserinfo?access_token=" + access_token + "&code=" + code;
        string result = HttpHelper.Get(tagUrl);

        var userModel = JsonConvert.DeserializeObject<UserModel>(result);
        return userModel;
    }

    /// <summary>
    /// 拿到Tickets
    /// </summary>
    /// <param name="CorpId"></param>
    /// <param name="CorpSecret"></param>
    /// <returns></returns>
    public static string GetTickets(string access_token)
    {
        string url = "https://oapi.dingtalk.com/get_jsapi_ticket?access_token={0}&type=jsapi";
        url = string.Format(url, access_token);
        string result =  HttpHelper.Get(url);
        var jsApiTicket = JsonConvert.DeserializeObject<JsApiTicket>(result);

        return jsApiTicket.ticket;
    }

}