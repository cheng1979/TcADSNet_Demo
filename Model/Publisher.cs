﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcADSNet_Demo.Model
{
    public static class Publisher
    {
        private static Int16 OnConnectTotalSubTasks = 3;
        private static Int16 OnConnectSubTasksCount = 0;

        public static event EventHandler<String> evPublisher;
        public static event EventHandler<Boolean> evOnConnectSubTasksCompleted;
        public static event EventHandler evEnableConnectButton;

        public static void Publish(String msg)
        {
            evPublisher?.Invoke(null, msg);
        }

        public static void OnConnectSubTaskDone()
        {
            OnConnectSubTasksCount++;
            //Console.WriteLine("Sub Tasks Count " + OnConnectSubTasksCount);
            if(OnConnectSubTasksCount >= OnConnectTotalSubTasks)
            {
                evOnConnectSubTasksCompleted?.Invoke(null, true);
                OnConnectSubTasksCount = 0;
                //Console.WriteLine("Sub Tasks Count " + OnConnectSubTasksCount);
            }
        }

        public static void EnableConnectButton()
        {
            evEnableConnectButton?.Invoke(null, EventArgs.Empty);
        }

    }///Class
}
