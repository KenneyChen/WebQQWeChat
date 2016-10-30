﻿using System.Threading.Tasks;
using Utility.HttpAction.Event;

namespace WebWeChat.Im.Module.Interface
{
    public interface ILoginModule: IWeChatModule
    {
        /// <summary>
        /// 登录，扫码方式
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        Task<ActionEventType> Login(ActionEventListener listener = null);
    }
}