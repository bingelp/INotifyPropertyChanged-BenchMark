﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Wpf.Models
{
    public class FieldWeakEventModel : FieldBase, IDisplayText
    {
        public const string Name = "Field With Weak Event Handler";
        public const string TimeProperty = "Time";
        public const string StatusProperty = "Status";
        public const string CountProperty = "Count";
        public const string DisplayTextProperty = "DisplayText";

        private readonly Field<string> _displayText = new Field<string>(DisplayTextProperty);
        public string DisplayText
        {
            get { return _displayText; }
        }

        private readonly Field<DateTime> _time = new Field<DateTime>(TimeProperty);
        public DateTime Time
        {
            get { return _time; }
            set { Set(_time, value); }
        }

        private readonly Field<bool> _status = new Field<bool>(StatusProperty);
        public bool Status
        {
            get { return _status; }
            set { Set(_status, value); }
        }

        private readonly Field<int> _count = new Field<int>(CountProperty);
        public int Count
        {
            get { return _count; }
        }

        public FieldWeakEventModel(int id)
        {
            PropertyChangedEventManager.AddHandler(this, OnPropertyChanged, StatusProperty);
            _displayText.Value = String.Format("{0} Model : {1}", Name, id);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == StatusProperty)
            {
                Set(_count, _count + 1);
            }
        }

        public static IDisplayText Create(int i)
        {
            return new FieldWeakEventModel(i);
        }
    }
}
