using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/**
 * 
 * des：当前的页面主要的功能是利用oauth协议实现免登
 * 
 * author:dqk1985
 * 
 * date:2015-10-23
 * 
 * */
public partial class Enterprise_OAuth : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //页面初始载入判断是否已存登录用户
        if (string.IsNullOrEmpty(Request["code"]))
        { 
            //用户未登录，通过oauth授权去钉钉服务器拿取授权

            //去拿授权成功后带着code与state的回调地址，可以是当前项目中的任意其它地址，这里使用当前页面
            string redirecturi = Server.UrlEncode(Config.WebUrl + "Enterprise/OAuth.aspx");


            //state 在 oauth中是为了随止跨站攻击的，所以回调之后一定要比较回调来的state与这个session["state]是否相等
            //具体的是什么原理可以参考oauth中关于state的介绍
            string state = Helper.state();
            Session["state"] = state;
            Helper.WriteLog("CorpId:" + Config.ECorpId);

            //这里的含义是说，我当前的网站没登录，我带着我的合法的认证（Config.SCorpId）去钉钉要一个当前登录用户分配的code，拿到这个code可以去换取当前的用户信息，来实现免登
            string url = "https://oapi.dingtalk.com/connect/oauth2/authorize?appid=" + Config.ECorpId + "&redirect_uri=" + redirecturi + "&response_type=code&scope=snsapi_base&state=" + state;
            Response.Redirect(url);
        }
        else if (!string.IsNullOrEmpty(Request["code"]))
        {
            //钉钉服务器根据上面的回调地址回传了code与state
            /*
             * 
             * code的用途是配合AccessToken去换取用户的信息，这样可以做到免登
             * code只允许使用一次
             * code应该也有有效期，但在官方文档中暂未看到说明；
             * 建议拿到code后就去换取用户实现免登
             * 
             * 
             * */
            string code = Request["code"].ToString();
            string state = Request["state"].ToString();

            //判断来源是否有效，是否是跨站
            if (Session["state"].ToString() == state)
            {
                Helper.WriteLog("有效的访问");
            }
            else
            {
                Helper.WriteLog("无效的访问");
                throw new Exception("无效的访问");
            }

            //在日志中打印code查看参数是否接收到
            Helper.WriteLog("code:" + code);

            Response.Redirect("ServerApi.aspx?code=" + code);

            
        }



    }
}