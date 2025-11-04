using System;

namespace FarmsteadMap.WPF 
{
    public class NavigateEventArgs : EventArgs
    {
        public string TargetView { get; set; }
        public object Data { get; set; }
    }
}