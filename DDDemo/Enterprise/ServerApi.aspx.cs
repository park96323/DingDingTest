using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/**
 * 
 * des：当前的页面主要的功能是拿到免登码，换取token之后去测试各个服务器的接口类文件
 * 
 * author:dqk1985
 * 
 * date:2015-10-23
 * 
 * */


public partial class Enterprise_ServerApi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string code = Request["code"];

        //根据CropId与cropSecret去换取AccessToken
        //这里的AccessToken的主要含义是企业令牌，它的意思是说依靠这个令牌可以去拿取与企业相关的数据，
        //根据官方文档介绍这里的有效期是7200秒，
        var tokenModel = EnterpriseBusiness.GetToken(Config.ECorpId, Config.ECorpSecret);
        var access_token = tokenModel.access_token;

        /*
         * 这里拿到企业令牌后，可以将其保存到数据库中，同时设定它的过期时间为当前时间+7200秒，             
         * 每次使有令牌时判断当前时间是否已经超过了有效期，如果超过了有效期，请重新获取新的令牌
         * 为了安全access_token在实际的开发过程当中不建议放到客户端，这个令牌一般禁止用户接触到，一般可放在服务器端的session里             
         */
        Helper.WriteLog("access_token:" + access_token);


        //---------------利用access_token和code去换取当前用户
        var userModel = EnterpriseBusiness.GetCurrentUser(access_token, code);
        Helper.WriteLog("userId:" + userModel.userid);


        //拿到access_token之后。可以参照钉钉开发文档中的-服务端开发文档进行其它api的测试。关于服务端的回调接口，将在isv的开发中提到具体的用法。
        //这里只写一个接口测试。
        string url = "https://oapi.dingtalk.com/department/create?access_token=" + access_token;
        string param = "{\"access_token\":\"" + access_token + "\",\"name\":\"新增部门测试\",\"parentid\":\"1\",\"order\":\"3\",\"createDeptGroup\":\"false\"}";
        string callback = HttpHelper.Post(url, param);
        Helper.WriteLog("创建部门：" + callback);
    }
}