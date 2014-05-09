using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Notification.Wpf.Models
{
    public class FodyModel: FodyBase, IDisplayText
    {
        public const string Name = "Fody";

        public string DisplayText { get; private set; }

        public DateTime Time {get; set;}

        public bool Status {get; set;}


        public int Count
        {
            get;
            private set;
        }

        public FodyModel(int id)
        {
            PropertyChanged += FodyModel_PropertyChanged;
            DisplayText = String.Format("{0} Model : {1}", Name, id);
        }

        void FodyModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName != "Count")
                Count += 1;
        }

        public static IDisplayText Create(int i)
        {
            return new FodyModel(i);
        }
    }
}
