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

        private const int SleepTime = 500;

        public TestBenchViewModel()
        {
            _worker = new BackgroundWorker();
            _worker.DoWork += (s, e) => { ShowResults = false; Run(); };
            _worker.RunWorkerCompleted += (s,e) =>
            {
                RaisePropertyChanged(ResultsWithCount1Property);
                RaisePropertyChanged(ResultsWithCount10Property);
                RaisePropertyChanged(ResultsWithCount100Property);
                RaisePropertyChanged(ResultsWithCount1000Property);
                RaisePropertyChanged(ResultsWithCount10000Property);
                RaisePropertyChanged(ResultsWithCount100000Property);
                RaisePropertyChanged(ResultsUsingCMNProperty);
                RaisePropertyChanged(ResultsUsingDelegateSetterProperty);
                RaisePropertyChanged(ResultsUsingField2Property);
                RaisePropertyChanged(ResultsUsingFieldProperty);
                RaisePropertyChanged(ResultsUsingFieldWeakEventProperty);
                RaisePropertyChanged(ResultsUsingLambdaFieldProperty);
                RaisePropertyChanged(ResultsUsingLambdaProperty);
                RaisePropertyChanged(ResultsUsingSetterProperty);
                RaisePropertyChanged(ResultsUsingSimpleProperty);
                RaisePropertyChanged(ResultsUsingFodyProperty);
                Status = "Done";
                ShowResults = true;
            };
            _worker.WorkerReportsProgress = true;
            _worker.ProgressChanged += _worker_ProgressChanged;
            RunCommand = new RelayCommand(_worker.RunWorkerAsync, (s) => {return !_worker.IsBusy;});
            ShowResults = false;
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
        public const string ResultsWithCount1Property = "ResultsWithCount1";
        public const string ResultsWithCount10Property = "ResultsWithCount10";
        public const string ResultsWithCount100Property = "ResultsWithCount100";
        public const string ResultsWithCount1000Property = "ResultsWithCount1000";
        public const string ResultsWithCount10000Property = "ResultsWithCount10000";
        public const string ResultsWithCount100000Property = "ResultsWithCount100000";

        public const string ResultsUsingCMNProperty = "ResultsUsingCMN";
        public const string ResultsUsingDelegateSetterProperty = "ResultsUsingDelegateSetter";
        public const string ResultsUsingField2Property = "ResultsUsingField2";
        public const string ResultsUsingFieldProperty = "ResultsUsingField";
        public const string ResultsUsingFieldWeakEventProperty = "ResultsUsingFieldWeakEvent";
        public const string ResultsUsingLambdaFieldProperty = "ResultsUsingLambdaField";
        public const string ResultsUsingLambdaProperty = "ResultsUsingLambda";
        public const string ResultsUsingSetterProperty = "ResultsUsingSetter";
        public const string ResultsUsingSimpleProperty = "ResultsUsingSimple";
        public const string ResultsUsingFodyProperty = "ResultsUsingFody";
        public const string ShowResultsProperty = "ShowResults";

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

        public IEnumerable<TestResult> ResultsWithCount1
        {
            get { return Results.Where(r => r.ObjectCount == 1); }
        }

        public IEnumerable<TestResult> ResultsWithCount10
        {
            get { return Results.Where(r => r.ObjectCount == 10); }
        }

        public IEnumerable<TestResult> ResultsWithCount100
        {
            get { return Results.Where(r => r.ObjectCount == 100); }
        }

        public IEnumerable<TestResult> ResultsWithCount1000
        {
            get { return Results.Where(r => r.ObjectCount == 1000); }
        }

        public IEnumerable<TestResult> ResultsWithCount10000
        {
            get { return Results.Where(r => r.ObjectCount == 10000); }
        }

        public IEnumerable<TestResult> ResultsWithCount100000
        {
            get { return Results.Where(r => r.ObjectCount == 100000); }
        }

        public IEnumerable<TestResult> ResultsUsingCMN
        {
            get { return Results.Where(r => r.Method == CMNModel.Name); }
        }
        public IEnumerable<TestResult> ResultsUsingDelegateSetter
        {
            get { return Results.Where(r => r.Method == DelegateSetterModel.Name); }
        }
        public IEnumerable<TestResult> ResultsUsingField2
        {
            get { return Results.Where(r => r.Method == Field2Model.Name); }
        }
        public IEnumerable<TestResult> ResultsUsingField
        {
            get { return Results.Where(r => r.Method == FieldModel.Name); }
        }
        public IEnumerable<TestResult> ResultsUsingFieldWeakEvent
        {
            get { return Results.Where(r => r.Method == FieldWeakEventModel.Name); }
        }
        public IEnumerable<TestResult> ResultsUsingLambdaField
        {
            get { return Results.Where(r => r.Method == LambdaFieldModel.Name); }
        }
        public IEnumerable<TestResult> ResultsUsingLambda
        {
            get { return Results.Where(r => r.Method == LambdaModel.Name); }
        }
        public IEnumerable<TestResult> ResultsUsingSetter
        {
            get { return Results.Where(r => r.Method == SetterModel.Name); }
        }
        public IEnumerable<TestResult> ResultsUsingSimple
        {
            get { return Results.Where(r => r.Method == SimpleModel.Name); }
        }
        public IEnumerable<TestResult> ResultsUsingFody
        {
            get { return Results.Where(r => r.Method == FodyModel.Name); }
        }

        private bool _showResults;

        public bool ShowResults
        {
            get { return _showResults; }
            set { _showResults = value; RaisePropertyChanged(ShowResultsProperty); }
        }


        public void Run()
        {
                _worker.ReportProgress(ResetResults);
                RunTest(SimpleModel.Name, SimpleModel.Create);
                Thread.Sleep(SleepTime);
                RunTest(SetterModel.Name, SetterModel.Create);
                Thread.Sleep(SleepTime);
                RunTest(LambdaModel.Name, LambdaModel.Create);
                Thread.Sleep(SleepTime);
                RunTest(FieldModel.Name, FieldModel.Create);
                Thread.Sleep(SleepTime);
                RunTest(LambdaFieldModel.Name, LambdaFieldModel.Create);
                Thread.Sleep(SleepTime);
                RunTest(Field2Model.Name, Field2Model.Create);
                Thread.Sleep(SleepTime);
                RunTest(FieldWeakEventModel.Name, FieldWeakEventModel.Create);
                Thread.Sleep(SleepTime);
                RunTest(DelegateSetterModel.Name, DelegateSetterModel.Create);
                Thread.Sleep(SleepTime);
                RunTest(CMNModel.Name, CMNModel.Create);
                Thread.Sleep(SleepTime);
                RunTest(FodyModel.Name, FodyModel.Create);
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
        /// <summary>
        /// A command whose sole purpose is to 
        /// relay its functionality to other
        /// objects by invoking delegates. The
        /// default return value for the CanExecute
        /// method is 'true'.
        /// </summary>
        public class RelayCommand : ICommand
        {
            #region Fields

            readonly Action<object> _execute;
            readonly Predicate<object> _canExecute;

            #endregion // Fields

            #region Constructors

            /// <summary>
            /// Creates a new command that can always execute.
            /// </summary>
            /// <param name="execute">The execution logic.</param>
            public RelayCommand(Action<object> execute)
                : this(execute, null)
            {
            }

            /// <summary>
            /// Creates a new command.
            /// </summary>
            /// <param name="execute">The execution logic.</param>
            /// <param name="canExecute">The execution status logic.</param>
            public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");

                _execute = execute;
                _canExecute = canExecute;
            }

            #endregion // Constructors

            #region ICommand Members

            [DebuggerStepThrough]
            public bool CanExecute(object parameter)
            {
                return _canExecute == null ? true : _canExecute(parameter);
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }

            #endregion // ICommand Members
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
