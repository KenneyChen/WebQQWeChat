﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Http
{
    /**
     *
     * HTTP请求
     *
     * @author solosky
     */
    public class QQHttpRequest
    {
        private const string DefaultGetContentType = "application/json; charset=utf-8";
        private const string DefaultPostContentType = "text/plain; charset=UTF-8";

        private Stream _inputStream;

        public QQHttpRequest(string url, string method)
        {
            _url = url;
            Method = method.ToUpper();
            HeaderMap = new Dictionary<string, string>();
            PostMap = new Dictionary<string, string>();
            GetMap = new Dictionary<string, string>();
            FileMap = new Dictionary<string, string>();
            HeaderMap[HttpConstants.ContentType] = Method == HttpConstants.Post ? DefaultPostContentType : DefaultGetContentType;
            HeaderMap[HttpConstants.UserAgent] = QQConstants.USER_AGENT;
            HeaderMap[HttpConstants.Referer] = QQConstants.REFFER;
        }

        private string _url;
        public string Url
        {
            get
            {
                if (GetMap.Count > 0)
                {
                    var query = string.Join("&", GetMap.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
                    return $"{_url}?{query}";
                }
                else
                {
                    return _url;
                }
            }
            set { _url = value; }
        }

        public string Method { get; set; }

        public int Timeout { get; set; }

        public string PostBody { get; set; }

        public Stream OutputStream { get; set; }

        public string Charset { get; set; } = "utf-8";

        public int ConnectTimeout { get; set; } = 10 * 10000;

        public int ReadTimeout { get; set; } = 20 * 10000;

        public Dictionary<string, string> GetMap { get; }

        public Dictionary<string, string> HeaderMap { get; set; }

        public Dictionary<string, string> PostMap { get; set; }

        public Dictionary<string, string> FileMap { get; }

        public string GetPostString()
        {
            return string.Join("&", PostMap.Select(item => $"{item.Key.UrlEncode()}={item.Value.UrlEncode()}"));
        }

        public byte[] GetPostBytes()
        {
            return Encoding.GetEncoding(Charset).GetBytes(GetPostString());
        }

        public Stream GetPostStream()
        {
            return _inputStream ?? new MemoryStream(GetPostBytes());
        }

        public void AddHeader(string key, string value)
        {
            HeaderMap.Add(key, value, AddChoice.Update);
        }

        public void SetBody(Stream inputStream)
        {
            _inputStream = inputStream;
        }

        public void AddPostValue(string key, object value)
        {
            PostMap[key] = value?.ToString() ?? string.Empty;
        }

        public void AddPostFile(string key, string file)
        {
            FileMap[key] = file;
            FileMap.Add(key, file, AddChoice.Update);
        }

        public void AddGetValue(string key, object value)
        {
            GetMap[key] = value?.ToString() ?? string.Empty;
        }

        public string ContentType
        {
            get { return HeaderMap[HttpConstants.ContentType]; }
            set { HeaderMap[HttpConstants.ContentType] = value; }
        }

        public string Refer
        {
            get { return HeaderMap[HttpConstants.Referer]; }
            set { HeaderMap[HttpConstants.ContentType] = value; }
        }

        public string UserAgent
        {
            get { return HeaderMap[HttpConstants.UserAgent]; }
            set { HeaderMap[HttpConstants.UserAgent] = value; }
        }
    }

}
