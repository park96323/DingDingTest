using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// Config 的摘要说明
/// </summary>
public static class Config
{
    //企业CorpID
    private static string _ECorpId;

    //企业CorpSecret
    private static string _ECorpSecret;

    //具体应用的appId
    private static string _EAgentID;


    //当前网站的weburl
    private static string _webUrl;


    //isv参数
    private static string _SUITE_KEY;

    

    private static string _SUITE_KEY_SECRET;


    private static string _ENCODING_AES_KEY;


    private static string _Token;


    public static string ECorpId
    {
        get { return Config._ECorpId; }
        set { Config._ECorpId = value; }
    }

    public static string ECorpSecret
    {
        get { return Config._ECorpSecret; }
        set { Config._ECorpSecret = value; }
    }


    public static string EAgentID
    {
        get { return Config._EAgentID; }
        set { Config._EAgentID = value; }
    }

    public static string WebUrl
    {
        get { return Config._webUrl; }
        set { Config._webUrl = value; }
    }

    public static string SUITE_KEY
    {
        get { return Config._SUITE_KEY; }
        set { Config._SUITE_KEY = value; }
    }

    public static string SUITE_KEY_SECRET
    {
        get { return Config._SUITE_KEY_SECRET; }
        set { Config._SUITE_KEY_SECRET = value; }
    }

    public static string Token
    {
        get { return Config._Token; }
        set { Config._Token = value; }
    }


    public static string ENCODING_AES_KEY
    {
        get { return Config._ENCODING_AES_KEY; }
        set { Config._ENCODING_AES_KEY = value; }
    }




	static Config()
	{
        _ECorpId = ConfigurationManager.AppSettings[ConfigurationKeys.ECorpId];
        _ECorpSecret = ConfigurationManager.AppSettings[ConfigurationKeys.ECorpSecret];
        _EAgentID = ConfigurationManager.AppSettings[ConfigurationKeys.EAgentID];
        _webUrl = ConfigurationManager.AppSettings[ConfigurationKeys.WebUrl];

        _SUITE_KEY = ConfigurationManager.AppSettings[ConfigurationKeys.SUITE_KEY];
        _SUITE_KEY_SECRET = ConfigurationManager.AppSettings[ConfigurationKeys.SUITE_KEY_SECRET];
        _Token = ConfigurationManager.AppSettings[ConfigurationKeys.Token];
        _ENCODING_AES_KEY = ConfigurationManager.AppSettings[ConfigurationKeys.ENCODING_AES_KEY];
	}




    private static class ConfigurationKeys
    {
        public const string ECorpId = "E.CorpId";
        public const string ECorpSecret = "E.CorpSecret";
        public const string EAgentID = "E.AgentID";
        public const string WebUrl = "webUrl";


        public const string SUITE_KEY = "SUITE_KEY";
        public const string SUITE_KEY_SECRET = "SUITE_KEY_SECRET";
        public const string Token = "Token";
        public const string ENCODING_AES_KEY = "ENCODING_AES_KEY";

    }
}