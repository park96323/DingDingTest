﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Lanxin.Messages
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadMediaResult : JsonResult
    {
        private DateTime dateInit = new DateTime(1970, 1, 1);
        /// <summary>
        /// 媒体文件类型：分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 媒体文件上传后获取的唯一标识 
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        /// 媒体文件上传时间戳
        /// </summary>
        public double created_at { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateCreated
        {
            get
            {
                if (created_at > Math.Pow(10, 10))
                {
                    return dateInit.AddMilliseconds(created_at).ToLocalTime();
                }
                return dateInit.AddSeconds(created_at).ToLocalTime();
            }
            set
            {
                var startTime = TimeZone.CurrentTimeZone.ToLocalTime(dateInit);
                created_at = (int)(value - startTime).TotalSeconds;
                //created_at = (value - dateInit).TotalSeconds;
            }
        }
    }
}