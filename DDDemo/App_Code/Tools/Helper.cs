using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

/// <summary>
/// Helper 的摘要说明
/// </summary>
public static class Helper
{
    /// <summary>
    /// 记录日志
    /// </summary>
    /// <param name="strMemo"></param>
    public static void WriteLog(string strMemo)
    {
        string directoryPath = HttpContext.Current.Server.MapPath(@"\Logs");
        string fileName = directoryPath + @"\log" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
        StreamWriter sr = null;
        try
        {
            if (!File.Exists(fileName))
            {
                sr = File.CreateText(fileName);
            }
            else
            {
                sr = File.AppendText(fileName);
            }
            sr.WriteLine(DateTime.Now + ": " + strMemo);
        }
        catch (Exception ex)
        {
        }
        finally
        {
            if (sr != null)
                sr.Close();
        }
    }


    /// <summary>
    /// 返回一个八位的随机号，用于签名
    /// </summary>
    /// <returns></returns>
    public static string randNonce()
    {
        var result = "";
        var random = new Random((int)DateTime.Now.Ticks);
        const string textArray = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        for (var i = 0; i < 8; i++)
        {
            result = result + textArray.Substring(random.Next() % textArray.Length, 1);
        }

        return result;
    }

    /// <summary>
    /// 时间戳的随机数
    /// </summary>
    /// <returns></returns>
    public static string timeStamp()
    {
        DateTime dt1 = Convert.ToDateTime("1970-01-01 00:00:00");
        TimeSpan ts = DateTime.Now - dt1;
        return Math.Ceiling(ts.TotalSeconds).ToString();
    }

    /// <summary>
    /// state 随机数
    /// </summary>
    /// <returns></returns>
    public static string state()
    {
        Random ran = new Random();
        int RandKey = ran.Next(100, 999);
        return RandKey.ToString();
    }



}