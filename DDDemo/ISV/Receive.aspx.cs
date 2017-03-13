using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/**
 * 
 * des：实现目标如下：
 * 1、创建套件时的回调url验证有效性
 * 2、回调事件的处理
 * 
 * author:dqk1985
 * 
 * date:2015-10-23
 * 
 * 
 * 
 * 
 * suite_ticket：推送过来的，二十分钟一次，套件的票，用来得到套件的访问令牌suiteAccessToken
 * 
 * suiteAccessToken：套件的访问令牌
 * 
 * tmp_auth_code:推送过来的，临时授权码，使用此授权码加套件的访问令牌（suiteAccessToken）可以拿到企业永久的授权码（permanent_code）和用户corpid
 * 
 * 通过permanent_code  corpid   suiteAccessToken获取企业用户的accessToken
 * 
 * 拿到accessToken就可以去调用相关的接口信息了
 * 
 * */
public partial class ISV_Receive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //正确的获取参数
        //签名的来源是使用时间戳，随机数，内容体和token 通过算法加密得到的
        //系统拿到这样几个参数后，首先要计算出签名与传递过来的签名是否相同，相同则说明是正确执行，不相同则说明传递的消息有可能被篡改
        string signature = Request["signature"];//消息签名
        string timeStamp = Request["timestamp"];//时间戳
        string nonce = Request["nonce"];//随机号
        string encrypt = "";

        #region 得到内容体
        Stream sm = Request.InputStream;//获取post正文
        int len = (int)sm.Length;//post数据长度
        byte[] inputByts = new byte[len];//字节数据,用于存储post数据
        sm.Read(inputByts, 0, len);//将post数据写入byte数组中
        sm.Close();//关闭IO流
        string data = Encoding.UTF8.GetString(inputByts);//转为String
        encrypt = data.Replace("{\"encrypt\":\"", "").Replace("\"}", "");
        #endregion

        #region 日志
        Helper.WriteLog("===========================1、服务器方发送回来的值===========================");
        Helper.WriteLog("1、signature:" + signature);
        Helper.WriteLog("2、timeStamp:" + timeStamp);
        Helper.WriteLog("3、nonce:" + nonce);
        Helper.WriteLog("4、encrypt:" + encrypt);
        Helper.WriteLog("==========================================================================");
        #endregion

        #region 验证签名
        string newSignature = "";
        DingTalkCrypt.GenerateSignature(Config.Token, timeStamp, nonce, encrypt, ref newSignature);
        Helper.WriteLog("===========================3、新签名===========================");
        Helper.WriteLog("1、newSignature:" + newSignature + "\r\n");

        if (signature == newSignature)
        {
            Helper.WriteLog("太好了！签名验证通过！");
        }
        else
        {
            Helper.WriteLog("消息有可能被篡改！签名验证错误！");
            return;
        }
        Helper.WriteLog("==========================================================================");
        #endregion

        #region   解密服务器端发送回来的数据
        DingTalkCrypt dingTalk = new DingTalkCrypt(Config.Token, Config.ENCODING_AES_KEY, Config.SUITE_KEY);

        string plainText = "";
        dingTalk.DecryptMsg(signature, timeStamp, nonce, encrypt, ref plainText);
        Hashtable tb = (Hashtable)JsonConvert.DeserializeObject(plainText, typeof(Hashtable));
        string eventType = tb["EventType"].ToString();

        Helper.WriteLog("===========================4、解密服务器端发送回来的数据===========================");
        Helper.WriteLog("plainText:" + plainText);
        Helper.WriteLog("eventType:" + eventType);

        Helper.WriteLog("==========================================================================");
        #endregion


        //根据回调的不同类型做出相应的响应，这里要求要细看开发文档中的   接入指南-ISV接入-5：回调接口（分为五个回调类型）这一篇
        string res = "";

        if (eventType == "check_create_suite_url")
        { 
            //验证有效性
            string random = tb["Random"].ToString();
            res = random;

        }
        else if (eventType == "suite_ticket")
        {
            //验证Ticket推送状态
            string ticket = tb["SuiteTicket"].ToString();   //回调推送过来的票证

            //拿票去换取后台的套件访问令牌
            string surl = "https://oapi.dingtalk.com/service/get_suite_token";
            string param = "{\"suite_key\":\"" + Config.SUITE_KEY + "\",\"suite_secret\":\"" + Config.SUITE_KEY_SECRET + "\",\"suite_ticket\":\"" + ticket + "\"}";
            string suite_access_token_result = HttpHelper.Post(surl, param);//套件的访问令牌
            Hashtable tb_suite_access_token = (Hashtable)JsonConvert.DeserializeObject(suite_access_token_result, typeof(Hashtable));

            string suite_access_token = tb_suite_access_token["suite_access_token"].ToString();
            Application["suite_access_token"] = suite_access_token;
            Helper.WriteLog("suite_access_token：" + suite_access_token);
            res = "success";
        }
        else if (eventType == "tmp_auth_code")
        {
            //模拟授权
            string tmp_auth_code = tb["AuthCode"].ToString();
            Helper.WriteLog("临时授权码：" + tmp_auth_code);


            #region 激活授权套件
            if (Application["suite_access_token"] != null)
            {
                string suite_access_token = Application["suite_access_token"].ToString();

                //获取永久的授权码
                string surl = "https://oapi.dingtalk.com/service/get_permanent_code?suite_access_token=" + suite_access_token;
                string param = "{\"tmp_auth_code\":\"" + tmp_auth_code + "\"}";
                string permanentcodestr = HttpHelper.Post(surl, param);


                Helper.WriteLog("企业的永久授权码：" + permanentcodestr);
                Hashtable tb_permanentcode = (Hashtable)JsonConvert.DeserializeObject(permanentcodestr, typeof(Hashtable));
                Helper.WriteLog("permanent_code:" + tb_permanentcode["permanent_code"]);
                Helper.WriteLog("auth_corp_info:" + tb_permanentcode["auth_corp_info"]);
                string permanent_code = tb_permanentcode["permanent_code"].ToString();
                string auth_corp_info = tb_permanentcode["auth_corp_info"].ToString();
                Hashtable tb_auth_corp_info = (Hashtable)JsonConvert.DeserializeObject(auth_corp_info, typeof(Hashtable));
                //拿到授权企业的corpId
                string corpid = tb_auth_corp_info["corpid"].ToString();
                Helper.WriteLog("corpid:" + corpid);


                //得到授权企业的access_token    
                surl = "https://oapi.dingtalk.com/service/get_corp_token?suite_access_token=" + suite_access_token;
                string param1 = "{\"auth_corpid\":\"" + corpid + "\",\"permanent_code\":\"" + permanent_code + "\"}";
                string accesstoken = HttpHelper.Post(surl, param1);//获取企业授权的access_token
                Helper.WriteLog("企业access_token：" + accesstoken);
                Hashtable tb_access_token = (Hashtable)JsonConvert.DeserializeObject(accesstoken, typeof(Hashtable));
                Helper.WriteLog("access_token:" + tb_access_token["access_token"]);
                Helper.WriteLog("expires_in:" + tb_access_token["expires_in"]);

                //激活企业授权套件
                surl = "https://oapi.dingtalk.com/service/activate_suite?suite_access_token=" + suite_access_token;
                string param2 = "{\"suite_key\":\"" + Config.SUITE_KEY + "\",\"auth_corpid\":\"" + corpid + "\",\"permanent_code\":\"" + permanent_code + "\"}";
                string oauth_suite = HttpHelper.Post(surl, param2);//套件的访问令牌
                Helper.WriteLog("永久授权套件：" + oauth_suite);
                Hashtable tb_suite_access_token = (Hashtable)JsonConvert.DeserializeObject(oauth_suite, typeof(Hashtable));

                res = "success";
            }
            else
            {
                Helper.WriteLog("暂时未拿到套件的授权码！");
            }



            #endregion
        }
        else
        { 
            //还有其它的看具体的文档具体写。激活套件之后的开发与企业的开发基本一样
        }



        //以下是返回结果
        nonce = Helper.randNonce();
        timeStamp = Helper.timeStamp();
        string encrypt1 = "";
        string signature1 = "";

        dingTalk.EncryptMsg(res, timeStamp, nonce, ref encrypt1, ref signature1);
        Hashtable jsonMap = new Hashtable
                {
                    {"msg_signature", signature1},
                    {"encrypt", encrypt1},
                    {"timeStamp", timeStamp},
                    {"nonce", nonce}
                };
        string result = JsonConvert.SerializeObject(jsonMap);
        Response.Write(result);


    }
}