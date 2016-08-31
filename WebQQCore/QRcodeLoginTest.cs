﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Event;

namespace iQQ.Net.WebQQCore
{
    /**
     * 使用二维码登录WebQQ
     * <p/>
     * <p/>
     * Created by Tony on 10/6/15.
     */
    public class QRcodeLoginTest
    {
        private static Process QRcodeProcess = null;

        static readonly IQQClient mClient = new WebQQClient("", "", (sender, Event) =>
        {
            switch (Event.Type)
            {
                case QQNotifyEventType.CHAT_MSG:
                    var revMsg = (QQMsg)Event.Target;
                    SendMsg(revMsg.From);
                    break;

                case QQNotifyEventType.QRCODE_READY:
                    {
                        var verify = (Image)Event.Target;
                        const string path = "verify.png";
                        verify.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                        Console.WriteLine("请扫描在项目根目录下qrcode.png图片");
                        QRcodeProcess = Process.Start(path);
                        break;
                    }

                case QQNotifyEventType.QRCODE_SUCCESS:
                case QQNotifyEventType.QRCODE_INVALID:
                    Console.WriteLine(Event);
                    QRcodeProcess?.Kill();
                    break;
            }

        }, new ThreadActorDispatcher());

        public static void Main(string[] args)
        {
            // 获取二维码
            mClient.GetQRCode((sender, @event) =>
            {
                
            });

            //// 检查二维码状态
            //mClient.CheckQRCode((sender, Event) =>
            //{
            //    switch (Event.Type)
            //    {
            //        case QQActionEventType.EVT_OK:
            //            {
            //                // 扫描通过,登录成功
            //                // 
            //                mClient.GetBuddyList((s, e) =>
            //                {
            //                    if (e.Type == QQActionEventType.EVT_OK)
            //                    {
            //                        Console.WriteLine("加载好友列表成功");
            //                    }
            //                });
            //                mClient.GetGroupList((s, e) =>
            //                {
            //                    if (e.Type == QQActionEventType.EVT_OK)
            //                    {
            //                        Console.WriteLine("加载群列表成功");
            //                    }
            //                });
            //                mClient.GetSelfInfo((s, e) =>
            //                {
            //                    if (e.Type == QQActionEventType.EVT_OK)
            //                    {
            //                        Console.WriteLine("获取个人信息成功");
            //                    }
            //                });

            //                mClient.BeginPollMsg();
            //                break;
            //            }

            //        case QQActionEventType.EVT_ERROR:
            //            {
            //                var ex = (QQException)(Event.Target);
            //                var code = ex.ErrorCode;
            //                switch (code)
            //                {
            //                    // 二维码有效,等待用户扫描
            //                    // 二维码已经扫描,等用户允许登录
            //                    case QQErrorCode.QRCODE_OK:
            //                    case QQErrorCode.QRCODE_AUTH:
            //                        Thread.Sleep(5000);
            //                        break;
            //                        // 发现检查二维码状态
            //                }
            //                break;
            //            }
            //    }
            //});

            Console.ReadKey();
        }

        public static void SendMsg(QQUser user)
        {
            Console.WriteLine("sendMsg " + user);

            // 组装QQ消息发送回去
            var sendMsg = new QQMsg
            {
                To = user,
                Type = QQMsgType.BUDDY_MSG
            };
            sendMsg.AddContentItem(new TextItem("hello from iqq")); // 添加文本内容
            sendMsg.AddContentItem(new FaceItem(0));            // QQ id为0的表情
            sendMsg.AddContentItem(new FontItem());             // 使用默认字体
            mClient.SendMsg(sendMsg, null);                     // 调用接口发送消息
        }
    }
}
