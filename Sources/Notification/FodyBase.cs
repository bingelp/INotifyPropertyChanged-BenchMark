using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification
{
    [ImplementPropertyChanged]
    public abstract class FodyBase :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
