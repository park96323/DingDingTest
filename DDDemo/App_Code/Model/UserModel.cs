using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// UserModel 的摘要说明
/// </summary>
public class UserModel
{
	public UserModel()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public string userid { get; set; }

    public string deviceId { get; set; }

    public string is_sys { get; set; }

    public string sys_level { get; set; }
}