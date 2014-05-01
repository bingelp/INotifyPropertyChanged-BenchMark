using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Notification.Wpf.Models;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Notification.Wpf.ViewModels
{
    public class TestBenchViewModel : INotifyPropertyChanged
    {
        private const int ExecutionCount = 1000000;
        private const int ResetResults = 1;
        private const int ResetModels = 2;
        private const int UpdateStatus = 3;
        private const int AddResult = 4;

        public TestBenchViewModel()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += (s, e) => { Run(); };
            _worker.RunWorkerCompleted += (s,e) => { Status = "Done";};
            _worker.WorkerReportsProgress = true;
            _worker.ProgressChanged += _worker_ProgressChanged;
            RunCommand = new RelayCommand(_worker.RunWorkerAsync);
        }

        void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == ResetResults)
            {
                Results.Clear();
            }
            else if (e.ProgressPercentage == ResetModels)
            {
                Models = null;
            }
            else if (e.ProgressPercentage == UpdateStatus)
            {
                Status = e.UserState.ToString();
            }
            else if (e.ProgressPercentage == AddResult)
            {
                Results.Add((TestResult)e.UserState);
            }
        }


        public const string StatusProperty = "Status";
        public const string ObjectCountProperty = "ObjectCount";
        public const string ModelsProperty = "Models";
        public const string ResultsProperty = "Results";
        public const string RunCommandProperty = "RunCommand";

        private BackgroundWorker _worker;

        private string _status = "Press the run button to run tests";

        public string Status
        {
            get { return _status; }
            set
            {
                if (value == _status) { return; }
                _status = value;
                RaisePropertyChanged(StatusProperty);
            }
        }

        private int _objectCount = 100;
        public int ObjectCount
        {
            get { return _objectCount; }
            set
            {
                if (value == _objectCount) { return; }
                _objectCount = value;
                RaisePropertyChanged(ObjectCountProperty);
            }
        }

        public IEnumerable<int> ObjectCountOptions
        {
            get
            {
                for (int i = 1; i < ExecutionCount; i *= 10)
                {
                    yield return i;
                }
            }
        }

        private ObservableCollection<IDisplayText> _models;
        public ObservableCollection<IDisplayText> Models
        {
            get { return _models; }
            set
            {
                if (value == _models) { return; }
                _models = value;
                RaisePropertyChanged(ModelsProperty);
            }
        }

        private ObservableCollection<TestResult> _results = new ObservableCollection<TestResult>();
        public ObservableCollection<TestResult> Results
        {
            get { return _results; }
            set
            {
                if (value == _results) { return; }
                _results = value;
                RaisePropertyChanged(ResultsProperty);
            }
        }

        public void Run()
        {
                _worker.ReportProgress(ResetResults);
                RunTest(SimpleModel.Name, SimpleModel.Create);
                Thread.Sleep(1000);
                RunTest(SetterModel.Name, SetterModel.Create);
                Thread.Sleep(1000);
                RunTest(LambdaModel.Name, LambdaModel.Create);
                Thread.Sleep(1000);
                RunTest(FieldModel.Name, FieldModel.Create);
                Thread.Sleep(1000);
                RunTest(LambdaFieldModel.Name, LambdaFieldModel.Create);
                Thread.Sleep(1000);
                RunTest(Field2Model.Name, Field2Model.Create);
                Thread.Sleep(1000);
                RunTest(FieldWeakEventModel.Name, FieldWeakEventModel.Create);
                Thread.Sleep(1000);
                RunTest(DelegateSetterModel.Name, DelegateSetterModel.Create);
                _worker.ReportProgress(ResetModels);
        }

        private void RunTest(string description, Func<int, IDisplayText> factory)
        {
            for (int i = 1; i <= ObjectCount; i *= 10)
            {
                _worker.ReportProgress(AddResult,RunTest(description, factory, i));
            }
        }

        private TestResult RunTest(string description, Func<int, IDisplayText> factory, int objectCount)
        {
            var watch = new Stopwatch();
            var result = new TestResult();
            result.Method = description;
            _worker.ReportProgress(UpdateStatus,"Running Test on " + description + " Implementation ...");

            result.ObjectCount = objectCount;
            result.IterationCount = ExecutionCount / objectCount;
            watch.Restart();
            var models = CreateModels(factory, objectCount);
            watch.Stop();
            result.ConstructionMilliseconds = watch.ElapsedMilliseconds;
            Models = new ObservableCollection<IDisplayText>(models);

            watch.Restart();
            UpdateValue(models, result.ObjectCount, result.IterationCount);
            watch.Stop();
            result.ExecutionMilliseconds = watch.ElapsedMilliseconds;
            _worker.ReportProgress(UpdateStatus, "Done");

            return result;
        }

        private IDisplayText[] CreateModels(Func<int, IDisplayText> factory, int count)
        {
            var result = new IDisplayText[count];

            for (var i = 0; i < count; ++i)
            {
                result[i] = factory(i + 1);
            }

            return result;
        }

        private void UpdateValue(IDisplayText[] models, int objectCount, int iterationCount)
        {
            for (var i = 0; i < iterationCount; ++i)
            {
                for (var j = 0; j < objectCount; ++j)
                {
                    var model = models[j];
                    model.Time = DateTime.Now;
                    model.Status = !model.Status;
                }
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region RunCommand
        public class RelayCommand : ICommand
        {
            private readonly Action _fn;

            public RelayCommand(Action fn)
            {
                _fn = fn;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _fn();
            }
        }

        private ICommand _runCommand;
        public ICommand RunCommand
        {
            get
            {
                return _runCommand;
            }
            set
            {
                _runCommand = value;
                RaisePropertyChanged(RunCommandProperty);
            }
        }
        #endregion
    }
}
