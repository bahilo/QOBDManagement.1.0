using System;
using System.Collections.Generic;
using System.Linq;
using QOBDBusiness;
using QOBDManagement.Classes;
using LiveCharts.Defaults;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows;
using QOBDCommon.Entities;
using QOBDManagement.Command;
using System.ComponentModel;
using QOBDManagement.Models;
using System.IO;
using System.Xml.Serialization;
using QOBDManagement.Interfaces;
using QOBDCommon.Enum;

namespace QOBDManagement.ViewModel
{
    public class HomeViewModel : BindBase
    {
        private SeriesCollection _purchaseAndSalePriceseriesCollection;
        private SeriesCollection _payReceivedSeries;
        private ChartValues<DateTimePoint> _chartValueList;
        private List<StatisticModel> _statisticList;
        private List<ToDo> _toDoList;
        private string _newTask;

        //private Func<double, string> _xFormatter;
        //private Func<double, string> _yFormatter;

        //----------------------------[ Models ]------------------

        private ItemModel _firstBestSeller;
        private ItemModel _secondBestSeller;
        private ItemModel _ThirdBestSeller;
        private ItemModel _fourthBestSeller;
        private SeriesCollection _creditSeries;
        private IMainWindowViewModel _main;
        private Func<object, object> _page;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<ToDo> DeleteToDoTaskCommand { get; set; }

        public HomeViewModel()
        {
            instances();
            instancesCommand();
        }

        public HomeViewModel(IMainWindowViewModel mainWindowViewModel)
        {
            _main = mainWindowViewModel;
            _page = _main.navigation;
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onNewTaskChange_SaveToToDoList;
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged += onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
            }
        }
        private void instances()
        {
            _toDoList = new List<ToDo>();
            _firstBestSeller = new ItemModel();
            _secondBestSeller = new ItemModel();
            _ThirdBestSeller = new ItemModel();
            _fourthBestSeller = new ItemModel();
            _purchaseAndSalePriceseriesCollection = new SeriesCollection();
            _payReceivedSeries = new SeriesCollection();            
        }

        private void instancesCommand()
        {
            DeleteToDoTaskCommand = new ButtonCommand<ToDo>(deleteTask, canDeleteTask);
        }

        //----------------------------[ Properties ]------------------

        public BusinessLogic Bl
        {
            get { return Startup.Bl; }
            set { Startup.Bl = value; onPropertyChange("Bl"); }
        }

        public List<StatisticModel> StatisticDataList
        {
            get { return _statisticList; }
            set { _statisticList = value; onPropertyChange("StatisticDataList"); }
        }

        public ItemModel FirstBestItemModelSeller
        {
            get { return _firstBestSeller; }
            set { _firstBestSeller  = value; onPropertyChange("FirstBestItemModelSeller"); }
        }

        public ItemModel SecondBestItemModelSeller
        {
            get { return _secondBestSeller; }
            set { _secondBestSeller = value; onPropertyChange("SecondBestItemModelSeller"); }
        }

        public ItemModel ThirdBestItemModelSeller
        {
            get { return _ThirdBestSeller; }
            set { _ThirdBestSeller = value; onPropertyChange("ThirdBestItemModelSeller"); }
        }

        public ItemModel FourthBestItemModelSeller
        {
            get { return _fourthBestSeller; }
            set { _fourthBestSeller = value; onPropertyChange("FourthBestItemModelSeller"); }
        }

        public List<ToDo> ToDoList
        {
            get { return _toDoList; }
            set { _toDoList = value; onPropertyChange("ToDoList"); }
        }

        public string TxtNewTask
        {
            get { return _newTask; }
            set { _newTask = value; onPropertyChange("TxtNewTask"); }
        }

        public SeriesCollection PurchaseAndIncomeSeriesCollection
        {
            get { return _purchaseAndSalePriceseriesCollection; }
            set { _purchaseAndSalePriceseriesCollection = value; onPropertyChange("PurchaseAndIncomeSeriesCollection"); }
        }

        public SeriesCollection PayReceivedSeriesCollection
        {
            get { return _payReceivedSeries; }
            set { setProperty(ref _payReceivedSeries, value, "PayReceivedSeriesCollection"); }
        }

        public SeriesCollection CreditSeriesCollection
        {
            get { return _creditSeries; }
            set { setProperty(ref _creditSeries, value, "CreditSeriesCollection"); }
        }

        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public string[] PurchaseAndIncomeLabels { get; set; }
        public string[] PayReceivedAndBillLabels { get; set; }

        private string readTxtNewTask()
        {
            return _newTask;
        }

        private void setTxtNewTask(string task)
        {
            _newTask = task;
        }
        //----------------------------[ Actions ]------------------
        
        public void loadData()
        {
            Application.Current.Dispatcher.Invoke(async()=> {
                Dialog.showSearch("Loading...");
                StatisticDataList = (await Bl.BlStatisitc.searchStatisticAsync(new Statistic { Option = 1 }, ESearchOption.AND)).Select(x => new StatisticModel { Statistic = x }).ToList();
                ToDoList = getToDoTasks();
                loadUIData();
                Dialog.IsDialogOpen = false;
            });
            
        }


        private void loadUIData()
        {
            loadDataGauge();
            //loadChartPayreceivedData();
            loadPurchaseAndIncomeChart();
            loadPayReceivedAndBillChart();
        }

        private void loadPayReceivedAndBillChart()
        {
            var payReceivedChartValue = new ChartValues<decimal>();
            var invoiceAmountChartValue = new ChartValues<decimal>();

            payReceivedChartValue.AddRange(StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x => x.Statistic.Pay_received).ToList());
            invoiceAmountChartValue.AddRange(StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x => x.Statistic.Total_tax_included).ToList());

            CreditSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Pay received",
                    Values = payReceivedChartValue
                },
                new LineSeries
                {
                    Title = "Invoice",
                    Values = invoiceAmountChartValue
                }
            };

            PayReceivedAndBillLabels = StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x => x.Statistic.InvoiceDate.ToString("MMM")).ToArray();

        }

        private void loadPurchaseAndIncomeChart()
        {
            var purchaseChartValue = new ChartValues<decimal>();
            var IncomeChartValue = new ChartValues<decimal>();
            var BillAmountChartValue = new ChartValues<decimal>();

            purchaseChartValue.AddRange(StatisticDataList.OrderBy(x=>x.Statistic.ID).Select(x=>x.Statistic.Price_purchase_total).ToList());
            IncomeChartValue.AddRange(StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x => x.Statistic.Income).ToList());
            BillAmountChartValue.AddRange(StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x => x.Statistic.Total_tax_included).ToList());

            PurchaseAndIncomeSeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Sale",
                    Values = BillAmountChartValue
                },
                new LineSeries
                {
                    Title = "Purchase",
                    Values = purchaseChartValue
                },
                new LineSeries
                {
                    Title = "Income",
                    Values = IncomeChartValue
                }
            };

            PurchaseAndIncomeLabels = StatisticDataList.OrderBy(x => x.Statistic.ID).Select(x=>x.Statistic.InvoiceDate.ToString("MMMM") ).ToArray();
            //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
        }

        private void loadDataGauge()
        {
            getBestSellers();
        }

        public async void getBestSellers()
        {
            // getting four best sellers
            // option = 1 (10 bestseller)
            var itemBestSsellerList = await Bl.BlItem.searchItemAsync(new Item { Option = 1  }, ESearchOption.AND);
            FirstBestItemModelSeller = itemBestSsellerList.OrderByDescending(x => x.Number_of_sale).Select(x => new ItemModel { Item = x }).FirstOrDefault() ?? new ItemModel();
            SecondBestItemModelSeller = itemBestSsellerList.OrderByDescending(x => x.Number_of_sale).Where(x => x.Number_of_sale < FirstBestItemModelSeller.Item.Number_of_sale).Select(x => new ItemModel { Item = x }).FirstOrDefault() ?? new ItemModel();
            ThirdBestItemModelSeller = itemBestSsellerList.OrderByDescending(x => x.Number_of_sale).Where(x => x.Number_of_sale < SecondBestItemModelSeller.Item.Number_of_sale).Select(x => new ItemModel { Item = x }).FirstOrDefault() ?? new ItemModel();
            FourthBestItemModelSeller = itemBestSsellerList.OrderByDescending(x => x.Number_of_sale).Where(x => x.Number_of_sale < ThirdBestItemModelSeller.Item.Number_of_sale).Select(x => new ItemModel { Item = x }).FirstOrDefault() ?? new ItemModel();
                        
        }

        private void loadChartPayreceivedData()
        {
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };

            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(33, 148, 241), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));

            PayReceivedSeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = GetDataPayreceived(),
                        Fill = gradientBrush,
                        StrokeThickness = 1,
                        PointGeometrySize = 0
                    }
                };


            XFormatter = val => new DateTime((long)val).ToString("dd MMM");
            YFormatter = val => val.ToString("C");
        }


        private ChartValues<DateTimePoint> GetDataPayreceived()
        {
            loadStatisticPayReceived();
            return _chartValueList;
        }        

        private void loadStatisticPayReceived()
        {            
            _chartValueList = new ChartValues<DateTimePoint>();
            var statistics = StatisticDataList.OrderBy(x => x.Statistic.Date_limit).ToList();
            foreach (var statisticModel in statistics)
            {
                _chartValueList.Add(new DateTimePoint(statisticModel.Statistic.Pay_date, (double)statisticModel.Statistic.Pay_received));
            }
        }

        private void setNewTask()
        {
            var toDo = new ToDo();
            toDo.PropertyChanged += onToDoListIsDoneChange;
            toDo.TxtTask = TxtNewTask;
            ToDoList.Add(toDo);
            ToDoList = new List<ToDo>(ToDoList);
        }

        private void saveToDoTasks(List<ToDo> taskList)
        {
            string path = Directory.GetCurrentDirectory();
            string fileName = "tasks.xml";
            string fullFileName = string.Format(@"{0}\Docs\Files\{1}", path, fileName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //if (!File.Exists(fullFileName))
            //    File.Create(fullFileName);
            if (taskList.Count == 0)
                return;

            using (StreamWriter sw = new StreamWriter(fullFileName))
            {
                XmlSerializer xs = new XmlSerializer(taskList.GetType());
                xs.Serialize(sw, taskList);
            }
        }

        private List<ToDo> getToDoTasks()
        {
            string path = Directory.GetCurrentDirectory();
            string fileName = "tasks.xml";
            string fullFileName = string.Format(@"{0}\Docs\Files\{1}", path, fileName);

            List<ToDo> results = new List<ToDo>();
            if (File.Exists(fullFileName))
            {
                using (StreamReader sr = new StreamReader(fullFileName))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<ToDo>));
                    results = (List<ToDo>)xs.Deserialize(sr);
                }
                foreach (var todo in results)
                    todo.PropertyChanged += onToDoListIsDoneChange;
            }
            return results;
        }

        public override void Dispose()
        {
            PropertyChanged -= onNewTaskChange_SaveToToDoList;
        }

        //----------------------------[ Event Handler ]------------------

        private void onNewTaskChange_SaveToToDoList(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtNewTask"))
            {
                setNewTask();
                saveToDoTasks(ToDoList);
            }
        }

        private void onToDoListIsDoneChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsDone"))
            {
                saveToDoTasks(ToDoList);
            }
        }

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                Startup = (_main.getObject("main") as BindBase).Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                Dialog = (_main.getObject("main") as BindBase).Dialog;
            }
        }

        //----------------------------[ Action Commands ]------------------


        private void deleteTask(ToDo obj)
        {
            ToDoList.Remove(obj);
            ToDoList = new List<ToDo>(ToDoList);
            saveToDoTasks(ToDoList);
        }

        private bool canDeleteTask(ToDo arg)
        {
            return true;
        }

        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "home":
                    _page(this);
                    break;
                default:
                    goto case "home";
            }
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }


    }
}
