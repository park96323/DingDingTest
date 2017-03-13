<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JsAPI.aspx.cs" Inherits="Enterprise_JsAPI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>利用jsapi实现免登</title>
    <script type="text/javascript" src="http://g.alicdn.com/ilw/ding/0.5.1/scripts/dingtalk.js" ></script>
    <script type="text/javascript">

        var _config = {
            appId: '<%=appId%>',
            corpId: '<%=corpId%>',
            timeStamp: '<%=timestamp%>',
            nonce: '<%=nonceStr%>',
            signature: '<%=signature%>'
        };


        //jsapi的配置。我注销之后代码仍然可正确执行。这里我没有了解到具体的函义与用法
        dd.config({
            appId: _config.appId,
            corpId: _config.corpId,
            timeStamp: _config.timeStamp,
            nonceStr: _config.nonce,
            signature: _config.signature,
            jsApiList: ['runtime.info',
                'biz.contact.choose',
                'device.notification.confirm',
                'device.notification.alert',
                'device.notification.prompt',
                'biz.ding.post',
            'runtime.permission.requestAuthCode',
            'device.geolocation.get',
            'biz.ding.post',
            'biz.contact.complexChoose']
        });



        dd.ready(function () {

            //获取免登授权码 -- 注销获取免登服务，可以测试jsapi的一些方法
            dd.runtime.permission.requestAuthCode({
                corpId: _config.corpId,
                onSuccess: function (result) {
                    location.href="ServerApi.aspx?code=" + result["code"];
                },
                onFail: function (err) { }

            });



            //这里写一个简单的jsapi的弹用，其它api的调用请参照钉钉开发文档-客户端开发文档
            dd.device.notification.alert({
                message: "测试弹窗",
                title: "提示",//可传空
                buttonName: "收到",
                onSuccess: function () {
                    /*回调*/
                },
                onFail: function (err) { }
            });

        });
    </script>


</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
