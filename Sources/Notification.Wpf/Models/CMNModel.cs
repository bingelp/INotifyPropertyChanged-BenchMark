using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Wpf.Models
{
    public class CMNModel : CMNBase, IDisplayText
    {
        public const string Name = "Caller Member Name";
        public const string StatusProperty = "Status";
        public const string CountProperty = "Count";

        public CMNModel(int id)
        {
            PropertyChanged += OnPropertyChanged;
            _displayText = String.Format("{0} Model : {1}", Name, id);
        }

        private string _displayText;
        public string DisplayText
        {
            get { return _displayText; }
        }

        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set
            {
                Set(ref _time, value);
            }
        }
        private bool _status;
        public bool Status
        {
            get { return _status; }
            set { Set(ref _status, value); }
        }
        private int _count;
        public int Count
        {
            get { return _count; }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == StatusProperty)
            {
                _count += 1;
                RaisePropertyChanged(CountProperty);
            }
        }

        public static IDisplayText Create(int i)
        {
            return new CMNModel(i);
        }
    }
}
