using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// TokenModel 的摘要说明
/// </summary>
public class TokenModel
{
	public TokenModel()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public string access_token { get; set; }

    public string errcode { get; set; }

    public string errmsg { get; set; }
}