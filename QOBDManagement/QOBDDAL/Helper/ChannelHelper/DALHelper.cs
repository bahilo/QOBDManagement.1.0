using QOBDCommon.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using QOBDCommon.Classes;
using QOBDDAL.App_Data;
using QOBDCommon.Enum;

namespace QOBDDAL.Helper.ChannelHelper
{

    public static class DALHelper
    {

        public static Task<TResult> doActionAsync<TInput, TResult>(this Func<TInput, TResult> func, TInput param, [CallerMemberName] string callerName = null) where TResult : new()
        {
            TResult result = new TResult();
            QOBDCommon.Classes.NotifyTaskCompletion<TResult> ntc = new QOBDCommon.Classes.NotifyTaskCompletion<TResult>();
            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            Task.Factory
                    .StartNew(() =>
                    {
                        TResult taskResult = new TResult();
                        try
                        {
                            taskResult = (TResult)func(param);
                            taskCompletionSource.SetResult(taskResult);
                        }
                        catch (Exception ex)
                        {
                            string ErrorMessage = string.Format("Custom [{0}]: One Error occured - {1}", callerName, ex.InnerException.Message);
                            taskCompletionSource.SetException(new Exception(ErrorMessage));
                            //Log.error(ErrorMessage, EErrorFrom.);
                        }
                        return taskResult;
                    });

            ntc.initializeNewTask(taskCompletionSource.Task);
            return ntc.Task;
        }

        public static Task<TResult> doActionAsync<TResult>(this Func<TResult> func, [CallerMemberName] string callerName = null) where TResult : new()
        {
            TResult result = new TResult();
            QOBDCommon.Classes.NotifyTaskCompletion<TResult> ntc = new QOBDCommon.Classes.NotifyTaskCompletion<TResult>();
            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            Task.Factory
                    .StartNew(() =>
                    {
                        TResult taskResult = new TResult();
                        try
                        {
                            taskResult = (TResult)func();
                            taskCompletionSource.SetResult(taskResult);
                        }
                        catch (Exception ex)
                        {
                            string ErrorMessage = string.Format("Custom [{0}]: One Error occured - {1}", callerName, ex.InnerException.Message);
                            taskCompletionSource.SetException(new Exception(ErrorMessage));
                            //Log.error(ErrorMessage);
                        }
                        return taskResult;
                    });

            ntc.initializeNewTask(taskCompletionSource.Task);
            return ntc.Task;
        }

        public static void doActionAsync(this System.Action action, [CallerMemberName]string callerName = null)
        {
            QOBDCommon.Classes.NotifyTaskCompletion<object> ntc = new QOBDCommon.Classes.NotifyTaskCompletion<object>();
            TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();

            Task<object>.Factory.StartNew(() =>
            {
                try
                {
                    action();
                    taskCompletionSource.SetResult(null);
                }
                catch (Exception ex)
                {
                    string ErrorMessage = string.Format("Custom [{0}]: One Error occured - {1}", callerName, (ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
                    taskCompletionSource.SetException(new Exception(ErrorMessage));
                    //Log.error(ErrorMessage);
                }
                return null;
            });

            ntc.initializeNewTask(taskCompletionSource.Task);
        }


        //====================================================================================
        //===============================[ Sql Commands ]=====================================
        //====================================================================================

        public static DataTable getDataTableFromSqlQuery<T>(string sql) where T : new()
        {
            DataTable table = new DataTable();
            object _lock = new object();
            string _constr = "";
            SqlCommand cmd = new SqlCommand();

            _constr = System.Configuration.ConfigurationManager.ConnectionStrings["QOBDDAL.Properties.Settings.QCBDDatabaseConnectionString"].ConnectionString;

            using (var connection = new SqlConnection(_constr))
            {         
                using (cmd = new SqlCommand(sql, connection))
                {
                    try
                    {
                        if (typeof(T).Equals(typeof(QOBDSet.billsDataTable)))
                            table = new QOBDSet.billsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.tax_commandsDataTable)))
                            table = new QOBDSet.tax_commandsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.taxesDataTable)))
                            table = new QOBDSet.taxesDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.deliveriesDataTable)))
                            table = new QOBDSet.deliveriesDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.commandsDataTable)))
                            table = new QOBDSet.commandsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.command_itemsDataTable)))
                            table = new QOBDSet.command_itemsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.clientsDataTable)))
                            table = new QOBDSet.clientsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.contactsDataTable)))
                            table = new QOBDSet.contactsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.addressesDataTable)))
                            table = new QOBDSet.addressesDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.itemsDataTable)))
                            table = new QOBDSet.itemsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.item_deliveriesDataTable)))
                            table = new QOBDSet.item_deliveriesDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.providersDataTable)))
                            table = new QOBDSet.providersDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.provider_itemsDataTable)))
                            table = new QOBDSet.provider_itemsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.agentsDataTable)))
                            table = new QOBDSet.agentsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.auto_refsDataTable)))
                            table = new QOBDSet.auto_refsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.tax_itemsDataTable)))
                            table = new QOBDSet.tax_itemsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.actionRecordsDataTable)))
                            table = new QOBDSet.actionRecordsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.actionRecordsDataTable)))
                            table = new QOBDSet.actionRecordsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.infosDataTable)))
                            table = new QOBDSet.infosDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.statisticsDataTable)))
                            table = new QOBDSet.statisticsDataTable();
                        else if (typeof(T).Equals(typeof(QOBDSet.notificationsDataTable)))
                            table = new QOBDSet.notificationsDataTable();

                        cmd.Connection.Open();
                        cmd.CommandTimeout = 0;
                        table.Load(cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection));
                    }
                    catch (Exception ex)
                    {
                        Log.error(ex.Message, EErrorFrom.HELPER);
                    }
                }
            }
            return table;
        }


        //====================================================================================
        //===============================[ Auto_ref ]===========================================
        //====================================================================================

        public static List<Auto_ref> DataTableTypeToAuto_ref(this QOBDSet.auto_refsDataTable Auto_refDataTable)
        {
            object _lock = new object(); List<Auto_ref> returnList = new List<Auto_ref>();

            foreach (var Auto_refQCBD in Auto_refDataTable)
            {
                Auto_ref Auto_ref = new Auto_ref();
                Auto_ref.ID = Auto_refQCBD.ID;
                Auto_ref.RefId = Auto_refQCBD.RefId;

                lock (_lock) returnList.Add(Auto_ref);
            }

            return returnList;
        }

        public static QOBDSet.auto_refsDataTable Auto_refTypeToDataTable(this List<Auto_ref> Auto_refList)
        {
            object _lock = new object();
            QOBDSet.auto_refsDataTable returnQCBDDataTable = new QOBDSet.auto_refsDataTable();

            foreach (var Auto_ref in Auto_refList)
            {
                QOBDSet.auto_refsRow Auto_refQCBD = returnQCBDDataTable.Newauto_refsRow();
                Auto_refQCBD.ID = Auto_ref.ID;
                Auto_refQCBD.RefId = Auto_ref.RefId;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(Auto_refQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(Auto_refQCBD);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.auto_refsDataTable Auto_refTypeToDataTable(this List<Auto_ref> auto_refList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (auto_refList != null)
            {
                if (dataSet != null && dataSet.auto_refs.Count > 0)
                {
                    foreach (var Auto_ref in auto_refList)
                    {
                        QOBDSet.auto_refsRow Auto_refQCBD = dataSet.auto_refs.Where(x => x.ID == Auto_ref.ID).First();
                        Auto_refQCBD.RefId = Auto_ref.RefId;
                    }
                }
            }

            return dataSet.auto_refs;
        }

        public static List<Auto_ref> Auto_refTypeToFilterDataTable(this Auto_ref Auto_ref, ESearchOption filterOperator)
        {
            if (Auto_ref != null)
            {
                string baseSqlString = "SELECT * FROM Auto_refs WHERE ";
                string defaultSqlString = "SELECT * FROM Auto_refs WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Auto_ref.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Auto_ref.ID);
                if (Auto_ref.RefId != 0)
                    query = string.Format(query + " {0} RefId LIKE '{1}' ", filterOperator.ToString(), Auto_ref.RefId);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToAuto_ref((QOBDSet.auto_refsDataTable)getDataTableFromSqlQuery<QOBDSet.auto_refsDataTable>(baseSqlString));

            }
            return new List<Auto_ref>();
        }


        //====================================================================================
        //===============================[ Notification ]===========================================
        //====================================================================================

        public static List<Notification> DataTableTypeToNotification(this QOBDSet.notificationsDataTable NotificationDataTable)
        {
            object _lock = new object(); List<Notification> returnList = new List<Notification>();

            foreach (var NotificationQCBD in NotificationDataTable)
            {
                Notification Notification = new Notification();
                Notification.ID = NotificationQCBD.ID;
                Notification.BillId = NotificationQCBD.BillId;
                Notification.Reminder1 = NotificationQCBD.Reminder1;
                Notification.Reminder2 = NotificationQCBD.Reminder2;

                lock (_lock) returnList.Add(Notification);
            }

            return returnList;
        }

        public static QOBDSet.notificationsDataTable NotificationTypeToDataTable(this List<Notification> NotificationList)
        {
            object _lock = new object();
            QOBDSet.notificationsDataTable returnQCBDDataTable = new QOBDSet.notificationsDataTable();

            foreach (var Notification in NotificationList)
            {
                QOBDSet.notificationsRow NotificationQCBD = returnQCBDDataTable.NewnotificationsRow();
                NotificationQCBD.ID = Notification.ID;
                NotificationQCBD.BillId = Notification.BillId;
                NotificationQCBD.Reminder1 = Notification.Reminder1;
                NotificationQCBD.Reminder2 = Notification.Reminder2;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(NotificationQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(NotificationQCBD);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.notificationsDataTable NotificationTypeToDataTable(this List<Notification> notificationList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (notificationList != null)
            {
                if (dataSet != null && dataSet.notifications.Count > 0)
                {
                    foreach (var Notification in notificationList)
                    {
                        QOBDSet.notificationsRow NotificationQCBD = dataSet.notifications.Where(x => x.ID == Notification.ID).First();
                        if (NotificationQCBD.ID != Notification.ID && Notification.ID != 0)
                            NotificationQCBD.ID = Notification.ID;
                        NotificationQCBD.BillId = Notification.BillId;
                        NotificationQCBD.Reminder1 = Notification.Reminder1;
                        NotificationQCBD.Reminder2 = Notification.Reminder2;
                    }
                }
            }

            return dataSet.notifications;
        }

        public static List<Notification> NotificationTypeToFilterDataTable(this Notification Notification, ESearchOption filterOperator)
        {
            if (Notification != null)
            {
                string baseSqlString = "SELECT * FROM Notifications WHERE ";
                string defaultSqlString = "SELECT * FROM Notifications WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Notification.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Notification.ID);
                if (Notification.BillId != 0)
                    query = string.Format(query + " {0} BillId LIKE '{1}' ", filterOperator.ToString(), Notification.BillId);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToNotification((QOBDSet.notificationsDataTable)getDataTableFromSqlQuery<QOBDSet.notificationsDataTable>(baseSqlString));

            }
            return new List<Notification>();
        }

        //====================================================================================
        //===============================[ Statistic ]===========================================
        //====================================================================================

        public static List<Statistic> DataTableTypeToStatistic(this QOBDSet.statisticsDataTable StatisticDataTable)
        {
            object _lock = new object(); List<Statistic> returnList = new List<Statistic>();

            foreach (var statisticQCBD in StatisticDataTable)
            {
                Statistic statistic = new Statistic();
                statistic.ID = statisticQCBD.ID;
                statistic.InvoiceId = statisticQCBD.BillId;
                statistic.InvoiceDate = statisticQCBD.Bill_datetime;
                statistic.Company = statisticQCBD.Company;
                statistic.Date_limit = statisticQCBD.Date_limit;
                statistic.Income = statisticQCBD.Income;
                statistic.Income_percent = statisticQCBD.Income_percent;
                statistic.Pay_date = statisticQCBD.Pay_datetime;
                statistic.Pay_received = statisticQCBD.Pay_received;
                statistic.Price_purchase_total = statisticQCBD.Price_purchase_total;
                statistic.Tax_value = statisticQCBD.Tax_value;
                statistic.Total = statisticQCBD.Total;
                statistic.Total_tax_included = statisticQCBD.Total_tax_included;

                lock (_lock) returnList.Add(statistic);
            }

            return returnList;
        }

        public static QOBDSet.statisticsDataTable StatisticTypeToDataTable(this List<Statistic> StatisticList)
        {
            object _lock = new object();
            QOBDSet.statisticsDataTable returnQCBDDataTable = new QOBDSet.statisticsDataTable();

            foreach (var statistic in StatisticList)
            {
                QOBDSet.statisticsRow statisticQCBD = returnQCBDDataTable.NewstatisticsRow();
                statisticQCBD.ID = statistic.ID;
                statisticQCBD.BillId = statistic.InvoiceId;
                statisticQCBD.Bill_datetime = statistic.InvoiceDate;
                statisticQCBD.Company = statistic.Company;
                statisticQCBD.Date_limit = statistic.Date_limit;
                statisticQCBD.Income = statistic.Income;
                statisticQCBD.Income_percent = statistic.Income_percent;
                statisticQCBD.Pay_datetime = statistic.Pay_date;
                statisticQCBD.Pay_received = statistic.Pay_received;
                statisticQCBD.Price_purchase_total = statistic.Price_purchase_total;
                statisticQCBD.Tax_value = statistic.Tax_value;
                statisticQCBD.Total = statistic.Total;
                statisticQCBD.Total_tax_included = statistic.Total_tax_included;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(statisticQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(statisticQCBD);
                    }
                }


            }

            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.statisticsDataTable StatisticTypeToDataTable(this List<Statistic> statisticsList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (statisticsList != null)
            {
                if (dataSet != null && dataSet.statistics.Count > 0)
                {
                    foreach (var statistic in statisticsList)
                    {
                        QOBDSet.statisticsRow statisticQCBD = dataSet.statistics.Where(x => x.ID == statistic.ID).First();
                        statisticQCBD.BillId = statistic.InvoiceId;
                        statisticQCBD.Bill_datetime = statistic.InvoiceDate;
                        statisticQCBD.Company = statistic.Company;
                        statisticQCBD.Date_limit = statistic.Date_limit;
                        statisticQCBD.Income = statistic.Income;
                        statisticQCBD.Income_percent = statistic.Income_percent;
                        statisticQCBD.Pay_datetime = statistic.Pay_date;
                        statisticQCBD.Pay_received = statistic.Pay_received;
                        statisticQCBD.Price_purchase_total = statistic.Price_purchase_total;
                        statisticQCBD.Tax_value = statistic.Tax_value;
                        statisticQCBD.Total = statistic.Total;
                        statisticQCBD.Total_tax_included = statistic.Total_tax_included;
                    }
                }
            }

            return dataSet.statistics;
        }

        public static List<Statistic> FilterDataTableToStatisticType(this Statistic Statistic, ESearchOption filterOperator)
        {
            if (Statistic != null)
            {
                string baseSqlString = "SELECT * FROM Statistics WHERE ";
                string defaultSqlString = "SELECT * FROM Statistics WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Statistic.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Statistic.ID);
                if (Statistic.InvoiceId != 0)
                    query = string.Format(query + " {0} BillId LIKE '{1}' ", filterOperator.ToString(), Statistic.InvoiceId);
                if (Statistic.Pay_received != 0)
                    query = string.Format(query + " {0} Pay_received LIKE '{1}' ", filterOperator.ToString(), Statistic.Pay_received);
                if (Statistic.Price_purchase_total != 0)
                    query = string.Format(query + " {0} Price_purchase_total LIKE '{1}' ", filterOperator.ToString(), Statistic.Price_purchase_total);
                if (Statistic.Total != 0)
                    query = string.Format(query + " {0} Total LIKE '{1}' ", filterOperator.ToString(), Statistic.Total);
                if (Statistic.Total_tax_included != 0)
                    query = string.Format(query + " {0} Total_tax_included LIKE '{1}' ", filterOperator.ToString(), Statistic.Total_tax_included);
                if (Statistic.Tax_value != 0)
                    query = string.Format(query + " {0} Tax_value LIKE '{1}' ", filterOperator.ToString(), Statistic.Tax_value);
                if (Statistic.Income != 0)
                    query = string.Format(query + " {0} Income LIKE '{1}' ", filterOperator.ToString(), Statistic.Income);
                if (Statistic.Income_percent != 0)
                    query = string.Format(query + " {0} Income_percent LIKE '{1}' ", filterOperator.ToString(), Statistic.Income_percent);
                if (string.IsNullOrEmpty(Statistic.Company))
                    query = string.Format(query + " {0} Company LIKE '{1}' ", filterOperator.ToString(), Statistic.Company);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToStatistic((QOBDSet.statisticsDataTable)getDataTableFromSqlQuery<QOBDSet.statisticsDataTable>(baseSqlString));

            }
            return new List<Statistic>();
        }

        //====================================================================================
        //===============================[ Infos ]===========================================
        //====================================================================================

        public static List<Info> DataTableTypeToInfos(this QOBDSet.infosDataTable InfosDataTable)
        {
            object _lock = new object(); List<Info> returnList = new List<Info>();

            foreach (var InfosQCBD in InfosDataTable)
            {
                Info Infos = new Info();
                Infos.ID = InfosQCBD.ID;
                Infos.Name = InfosQCBD.Name;
                Infos.Value = InfosQCBD.Value;

                lock (_lock) returnList.Add(Infos);
            }
            return returnList;
        }

        public static QOBDSet.infosDataTable InfosTypeToDataTable(this List<Info> InfosList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.infosDataTable returnQCBDDataTable = new QOBDSet.infosDataTable();

            foreach (var Infos in InfosList)
            {
                QOBDSet.infosRow InfosQCBD = returnQCBDDataTable.NewinfosRow();
                InfosQCBD.ID = Infos.ID;
                InfosQCBD.Name = Infos.Name;
                InfosQCBD.Value = Infos.Value;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(InfosQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(InfosQCBD);
                        idList.Add(InfosQCBD.ID);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.infosDataTable InfosTypeToDataTable(this List<Info> infoList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (infoList != null)
            {
                if (dataSet != null && dataSet.infos.Count > 0)
                {
                    foreach (var Infos in infoList)
                    {
                        QOBDSet.infosRow InfosQCBD = dataSet.infos.Where(x => x.ID == Infos.ID).First();
                        InfosQCBD.Name = Infos.Name;
                        InfosQCBD.Value = Infos.Value;
                    }
                }
            }

            return dataSet.infos;
        }

        public static List<Info> FilterDataTableToInfoType(this Info Info, ESearchOption filterOperator)
        {
            if (Info != null)
            {
                string baseSqlString = "SELECT * FROM Infos WHERE ";
                string defaultSqlString = "SELECT * FROM Infos WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Info.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Info.ID);
                if (!string.IsNullOrEmpty(Info.Name))
                    query = string.Format(query + " {0} Name LIKE '%{1}%' ", filterOperator.ToString(), Info.Name.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Info.Value))
                    query = string.Format(query + " {0} Value LIKE '{1}' ", filterOperator.ToString(), Info.Value.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToInfos((QOBDSet.infosDataTable)getDataTableFromSqlQuery<QOBDSet.infosDataTable>(baseSqlString));

            }
            return new List<Info>();
        }


        //====================================================================================
        //===============================[ ActionRecord ]===========================================
        //====================================================================================

        public static List<ActionRecord> DataTableTypeToActionRecord(this QOBDSet.actionRecordsDataTable ActionRecordDataTable)
        {
            object _lock = new object(); List<ActionRecord> returnList = new List<ActionRecord>();

            foreach (var ActionRecordQCBD in ActionRecordDataTable)
            {
                ActionRecord ActionRecord = new ActionRecord();
                ActionRecord.ID = ActionRecordQCBD.ID;
                ActionRecord.TargetName = ActionRecordQCBD.TargetName;
                ActionRecord.AgentId = ActionRecordQCBD.AgentId;
                ActionRecord.TargetId = ActionRecordQCBD.TargetId;
                ActionRecord.Action = ActionRecordQCBD.Action;

                lock (_lock) returnList.Add(ActionRecord);
            }

            return returnList;
        }

        public static QOBDSet.actionRecordsDataTable ActionRecordTypeToDataTable(this List<ActionRecord> ActionRecordList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.actionRecordsDataTable returnQCBDDataTable = new QOBDSet.actionRecordsDataTable();

            foreach (var ActionRecord in ActionRecordList)
            {
                QOBDSet.actionRecordsRow ActionRecordQCBD = returnQCBDDataTable.NewactionRecordsRow();
                ActionRecordQCBD.ID = ActionRecord.ID;
                ActionRecordQCBD.TargetName = ActionRecord.TargetName;
                ActionRecordQCBD.AgentId = ActionRecord.AgentId;
                ActionRecordQCBD.TargetId = ActionRecord.TargetId;
                ActionRecordQCBD.Action = ActionRecord.Action;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(ActionRecordQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(ActionRecordQCBD);
                        idList.Add(ActionRecordQCBD.ID);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.actionRecordsDataTable ActionRecordTypeToDataTable(this List<ActionRecord> actionRecordsList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (actionRecordsList != null)
            {
                if (dataSet != null && dataSet.actionRecords.Count > 0)
                {
                    foreach (var ActionRecord in actionRecordsList)
                    {
                        QOBDSet.actionRecordsRow ActionRecordQCBD = dataSet.actionRecords.Where(x => x.ID == ActionRecord.ID).First();
                        ActionRecordQCBD.TargetName = ActionRecord.TargetName;
                        ActionRecordQCBD.AgentId = ActionRecord.AgentId;
                        ActionRecordQCBD.TargetId = ActionRecord.TargetId;
                        ActionRecordQCBD.Action = ActionRecord.Action;
                    }
                }
            }
            return dataSet.actionRecords;
        }

        public static List<ActionRecord> ActionRecordTypeToFilterDataTable(this ActionRecord ActionRecord, ESearchOption filterOperator)
        {
            if (ActionRecord != null)
            {
                string baseSqlString = "SELECT * FROM ActionRecords WHERE ";
                string defaultSqlString = "SELECT * FROM ActionRecords WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (ActionRecord.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), ActionRecord.ID);
                if (ActionRecord.AgentId != 0)
                    query = string.Format(query + " {0} AgentId LIKE '{1}' ", filterOperator.ToString(), ActionRecord.AgentId);
                if (ActionRecord.TargetId != 0)
                    query = string.Format(query + " {0} TargetId LIKE '{1}' ", filterOperator.ToString(), ActionRecord.TargetId);
                if (!string.IsNullOrEmpty(ActionRecord.TargetName))
                    query = string.Format(query + " {0} TargetName LIKE '{1}' ", filterOperator.ToString(), ActionRecord.TargetName);
                if (!string.IsNullOrEmpty(ActionRecord.Action))
                    query = string.Format(query + " {0} Action LIKE '{1}' ", filterOperator.ToString(), ActionRecord.Action);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToActionRecord((QOBDSet.actionRecordsDataTable)getDataTableFromSqlQuery<QOBDSet.actionRecordsDataTable>(baseSqlString));

            }
            return new List<ActionRecord>();
        }

        //====================================================================================
        //===============================[ Agent ]===========================================
        //====================================================================================

        public static List<Agent> DataTableTypeToAgent(this QOBDSet.agentsDataTable agentDataTable)
        {
            object _lock = new object(); List<Agent> returnList = new List<Agent>();

            foreach (var agentQCBD in agentDataTable)
            {
                Agent agent = new Agent();
                agent.ID = agentQCBD.ID;
                agent.FirstName = agentQCBD.FirstName;
                agent.LastName = agentQCBD.LastName;
                agent.UserName = agentQCBD.Login;
                agent.HashedPassword = agentQCBD.Password;
                agent.Phone = agentQCBD.Phone;
                agent.Status = agentQCBD.Status;
                agent.Admin = agentQCBD.Admin;
                agent.Email = agentQCBD.Email;
                agent.Fax = agentQCBD.Fax;
                agent.ListSize = agentQCBD.ListSize;

                lock (_lock) returnList.Add(agent);
            }

            return returnList;
        }

        public static QOBDSet.agentsDataTable AgentTypeToDataTable(this List<Agent> agentList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.agentsDataTable outputQCBDDataTable = new QOBDSet.agentsDataTable();
            if (agentList != null)
            {
                foreach (var agent in agentList)
                {
                    QOBDSet.agentsRow agentQCBD = outputQCBDDataTable.NewagentsRow();
                    agentQCBD.FirstName = agent.FirstName;
                    agentQCBD.LastName = agent.LastName;
                    agentQCBD.Login = agent.UserName;
                    agentQCBD.Password = agent.HashedPassword;
                    agentQCBD.Phone = agent.Phone;
                    agentQCBD.Status = agent.Status;
                    agentQCBD.Admin = agent.Admin;
                    agentQCBD.Email = agent.Email;
                    agentQCBD.Fax = agent.Fax;
                    agentQCBD.ListSize = agent.ListSize;

                    lock (_lock)
                    {
                        if (!idList.Contains(agentQCBD.ID))
                        {
                            outputQCBDDataTable.Rows.Add(agentQCBD);
                            idList.Add(agentQCBD.ID);
                        }
                    }
                }
            }
            return outputQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.agentsDataTable AgentTypeToDataTable(this List<Agent> agentList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (agentList != null)
            {
                if (dataSet != null && dataSet.agents.Count > 0)
                {
                    foreach (var agent in agentList)
                    {
                        QOBDSet.agentsRow agentQCBD = dataSet.agents.Where(x => x.ID == agent.ID).First();
                        agentQCBD.FirstName = agent.FirstName;
                        agentQCBD.LastName = agent.LastName;
                        agentQCBD.Login = agent.UserName;
                        agentQCBD.Password = agent.HashedPassword;
                        agentQCBD.Phone = agent.Phone;
                        agentQCBD.Status = agent.Status;
                        agentQCBD.Admin = agent.Admin;
                        agentQCBD.Email = agent.Email;
                        agentQCBD.Fax = agent.Fax;
                        agentQCBD.ListSize = agent.ListSize;
                    }
                }
            }
            return dataSet.agents;
        }

        public static List<Agent> AgentTypeToFilterDataTable(this Agent agent, ESearchOption filterOperator)
        {
            if (agent != null)
            {
                string baseSqlString = "SELECT * FROM Agents WHERE ";
                string defaultSqlString = "SELECT * FROM Agents WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (agent.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), agent.ID);
                if (agent.ListSize != 0)
                    query = string.Format(query + " {0} ListSize LIKE '{1}' ", filterOperator.ToString(), agent.ListSize);
                if (!string.IsNullOrEmpty(agent.LastName))
                    query = string.Format(query + " {0} LastName LIKE '{1}' ", filterOperator.ToString(), agent.LastName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.FirstName))
                    query = string.Format(query + " {0} FirstName LIKE '{1}' ", filterOperator.ToString(), agent.FirstName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Phone))
                    query = string.Format(query + " {0} Phone LIKE '{1}' ", filterOperator.ToString(), agent.Phone.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Fax))
                    query = string.Format(query + " {0} Fax LIKE '{1}' ", filterOperator.ToString(), agent.Fax.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Email))
                    query = string.Format(query + " {0} Email LIKE '{1}' ", filterOperator.ToString(), agent.Email.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.UserName))
                    query = string.Format(query + " {0} Login LIKE '{1}' ", filterOperator.ToString(), agent.UserName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.HashedPassword))
                    query = string.Format(query + " {0} Password LIKE '{1}' ", filterOperator.ToString(), agent.HashedPassword.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Admin))
                    query = string.Format(query + " {0} Admin LIKE '{1}' ", filterOperator.ToString(), agent.Admin.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Status))
                    query = string.Format(query + " {0} Status LIKE '{1}' ", filterOperator.ToString(), agent.Status.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToAgent((QOBDSet.agentsDataTable)DALHelper.getDataTableFromSqlQuery<QOBDSet.agentsDataTable>(baseSqlString));

            }
            return new List<Agent>();
        }


        //====================================================================================
        //===============================[ Order ]===========================================
        //====================================================================================

        public static List<Order> DataTableTypeToOrder(this QOBDSet.commandsDataTable OrderDataTable)
        {
            object _lock = new object(); List<Order> returnList = new List<Order>();

            Parallel.ForEach(OrderDataTable, (OrderQCBD) =>
            {
                Order Order = new Order();
                Order.ID = OrderQCBD.ID;
                Order.AgentId = OrderQCBD.AgentId;
                Order.BillAddress = OrderQCBD.BillAddress;
                Order.ClientId = OrderQCBD.ClientId;
                Order.Comment1 = OrderQCBD.Comment1;
                Order.Comment2 = OrderQCBD.Comment2;
                Order.Comment3 = OrderQCBD.Comment3;
                Order.Status = OrderQCBD.Status;
                Order.Date = OrderQCBD.Date;
                Order.DeliveryAddress = OrderQCBD.DeliveryAddress;
                Order.Tax = OrderQCBD.Tax;

                lock (_lock) returnList.Add(Order);
            });

            return returnList;
        }

        public static QOBDSet.commandsDataTable OrderTypeToDataTable(this List<Order> OrderList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.commandsDataTable returnQCBDDataTable = new QOBDSet.commandsDataTable();
            if (OrderList != null)
            {
                foreach (var Order in OrderList)
                {
                    QOBDSet.commandsRow OrderQCBD = returnQCBDDataTable.NewcommandsRow();
                    OrderQCBD.ID = Order.ID;
                    OrderQCBD.AgentId = Order.AgentId;
                    OrderQCBD.BillAddress = Order.BillAddress;
                    OrderQCBD.ClientId = Order.ClientId;
                    OrderQCBD.Comment1 = Order.Comment1;
                    OrderQCBD.Comment2 = Order.Comment2;
                    OrderQCBD.Comment3 = Order.Comment3;
                    OrderQCBD.Status = Order.Status;
                    OrderQCBD.Date = Order.Date;
                    OrderQCBD.DeliveryAddress = Order.DeliveryAddress;
                    OrderQCBD.Tax = Order.Tax;

                    lock (_lock)
                    {
                        if (!idList.Contains(OrderQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(OrderQCBD);
                            idList.Add(OrderQCBD.ID);
                        }
                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.commandsDataTable OrderTypeToDataTable(this List<Order> ordersList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (ordersList != null)
            {
                if (dataSet != null && dataSet.commands.Count > 0)
                {
                    foreach (var Order in ordersList)
                    {
                        QOBDSet.commandsRow OrderQCBD = dataSet.commands.Where(x => x.ID == Order.ID).First();
                        OrderQCBD.AgentId = Order.AgentId;
                        OrderQCBD.BillAddress = Order.BillAddress;
                        OrderQCBD.ClientId = Order.ClientId;
                        OrderQCBD.Comment1 = Order.Comment1;
                        OrderQCBD.Comment2 = Order.Comment2;
                        OrderQCBD.Comment3 = Order.Comment3;
                        OrderQCBD.Status = Order.Status;
                        OrderQCBD.Date = Order.Date;
                        OrderQCBD.DeliveryAddress = Order.DeliveryAddress;
                        OrderQCBD.Tax = Order.Tax;
                    }
                }
            }
            return dataSet.commands;
        }

        public static List<Order> orderTypeToFilterDataTable(this Order command, ESearchOption filterOperator)
        {
            string baseSqlString = "SELECT * FROM Commands WHERE ";
            string defaultSqlString = "SELECT * FROM Commands WHERE 1=0 ";
            string orderBy = " ORDER BY ID DESC";
            object _lock = new object(); string query = "";

            if (command != null)
            {
                if (command.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), command.ID);
                if (command.AgentId != 0)
                    query = string.Format(query + " {0} AgentId LIKE '{1}' ", filterOperator.ToString(), command.AgentId);
                if (!string.IsNullOrEmpty(command.Comment1))
                    query = string.Format(query + " {0} Comment1 LIKE '{1}' ", filterOperator.ToString(), command.Comment1.Replace("'", "''"));
                if (!string.IsNullOrEmpty(command.Comment2))
                    query = string.Format(query + " {0} Comment2 LIKE '{1}' ", filterOperator.ToString(), command.Comment2.Replace("'", "''"));
                if (!string.IsNullOrEmpty(command.Comment3))
                    query = string.Format(query + " {0} Comment3 LIKE '{1}' ", filterOperator.ToString(), command.Comment3.Replace("'", "''"));
                if (!string.IsNullOrEmpty(command.Status))
                    query = string.Format(query + " {0} Status LIKE '{1}' ", filterOperator.ToString(), command.Status.Replace("'", "''"));
                if (command.Tax != 0)
                    query = string.Format(query + " {0} Tax LIKE '{1}' ", filterOperator.ToString(), command.Tax);
                if (command.ClientId != 0)
                    query = string.Format(query + " {0} ClientId LIKE '{1}' ", filterOperator.ToString(), command.ClientId);
                if (command.BillAddress != 0)
                    query = string.Format(query + " {0} BillAddress LIKE '{1}' ", filterOperator.ToString(), command.BillAddress);
                if (command.DeliveryAddress != 0)
                    query = string.Format(query + " {0} DeliveryAddress LIKE '{1}' ", filterOperator.ToString(), command.DeliveryAddress);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length) + orderBy;
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToOrder((QOBDSet.commandsDataTable)getDataTableFromSqlQuery<QOBDSet.commandsDataTable>(baseSqlString));
            }
            return new List<Order>();
        }


        //====================================================================================
        //===============================[ Tax_command ]======================================
        //====================================================================================

        public static List<Tax_order> DataTableTypeToTax_order(this QOBDSet.tax_commandsDataTable Tax_OrderDataTableList)
        {
            object _lock = new object(); List<Tax_order> returnList = new List<Tax_order>();

            foreach (var Tax_commandQCBD in Tax_OrderDataTableList)
            {
                Tax_order Tax_command = new Tax_order();
                Tax_command.ID = Tax_commandQCBD.ID;
                Tax_command.OrderId = Tax_commandQCBD.CommandId;
                Tax_command.Date_insert = Tax_commandQCBD.Date_insert;
                Tax_command.Target = Tax_commandQCBD.Target;
                Tax_command.Tax_value = Tax_commandQCBD.Tax_value;
                Tax_command.TaxId = Tax_commandQCBD.TaxId;

                lock (_lock) returnList.Add(Tax_command);
            }

            return returnList;
        }

        public static QOBDSet.tax_commandsDataTable Tax_orderTypeToDataTable(this List<Tax_order> Tax_commandList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.tax_commandsDataTable returnQCBDDataTable = new QOBDSet.tax_commandsDataTable();

            foreach (var Tax_order in Tax_commandList)
            {
                QOBDSet.tax_commandsRow Tax_orderQCBD = returnQCBDDataTable.Newtax_commandsRow();
                Tax_orderQCBD.ID = Tax_order.ID;
                Tax_orderQCBD.CommandId = Tax_order.OrderId;
                Tax_orderQCBD.Date_insert = Tax_order.Date_insert;
                Tax_orderQCBD.Target = Tax_order.Target;
                Tax_orderQCBD.Tax_value = Tax_order.Tax_value;
                Tax_orderQCBD.TaxId = Tax_order.TaxId;

                lock (_lock)
                {
                    if (!idList.Contains(Tax_order.ID))
                    {
                        returnQCBDDataTable.Rows.Add(Tax_order);
                        idList.Add(Tax_order.ID);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.tax_commandsDataTable Tax_orderTypeToDataTable(this List<Tax_order> tax_ordersList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (tax_ordersList != null)
            {
                if (dataSet != null && dataSet.tax_commands.Count > 0)
                {
                    foreach (var Tax_order in tax_ordersList)
                    {
                        QOBDSet.tax_commandsRow Tax_orderQCBD = dataSet.tax_commands.Where(x => x.ID == Tax_order.ID).First();
                        Tax_orderQCBD.CommandId = Tax_order.OrderId;
                        Tax_orderQCBD.Date_insert = Tax_order.Date_insert;
                        Tax_orderQCBD.Target = Tax_order.Target;
                        Tax_orderQCBD.Tax_value = Tax_order.Tax_value;
                        Tax_orderQCBD.TaxId = Tax_order.TaxId;
                    }
                }
            }
            return dataSet.tax_commands;
        }

        public static List<Tax_order> Tax_orderTypeToFilterDataTable(this Tax_order Tax_command, ESearchOption filterOperator)
        {
            string baseSqlString = "SELECT * FROM Tax_commands WHERE ";
            string defaultSqlString = "SELECT * FROM Tax_commands WHERE 1=0 ";
            object _lock = new object(); string query = "";

            if (Tax_command != null)
            {
                if (Tax_command.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Tax_command.ID);
                if (Tax_command.OrderId != 0)
                    query = string.Format(query + " {0} CommandId LIKE '{1}' ", filterOperator.ToString(), Tax_command.OrderId);
                if (Tax_command.TaxId != 0)
                    query = string.Format(query + " {0} TaxId LIKE '{1}' ", filterOperator.ToString(), Tax_command.TaxId);
                if (Tax_command.Tax_value != 0)
                    query = string.Format(query + " {0} Tax_value LIKE '{1}' ", filterOperator.ToString(), Tax_command.Tax_value);
                if (!string.IsNullOrEmpty(Tax_command.Target))
                    query = string.Format(query + " {0} Target LIKE '{1}' ", filterOperator.ToString(), Tax_command.Target);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToTax_order((QOBDSet.tax_commandsDataTable)getDataTableFromSqlQuery<QOBDSet.tax_commandsDataTable>(baseSqlString));

            }

            return new List<Tax_order>();
        }

        //====================================================================================
        //===============================[ Client ]===========================================
        //====================================================================================

        public static List<Client> DataTableTypeToClient(this QOBDSet.clientsDataTable ClientDataTable)
        {
            object _lock = new object(); List<Client> returnList = new List<Client>();
            if (ClientDataTable != null)
            {
                //foreach (var ClientQCBD in ClientDataTable)
                Parallel.ForEach(ClientDataTable, (ClientQCBD) =>
                {
                    Client Client = new Client();
                    Client.ID = ClientQCBD.ID;
                    Client.FirstName = ClientQCBD.FirstName;
                    Client.LastName = ClientQCBD.LastName;
                    Client.AgentId = ClientQCBD.AgentId;
                    Client.Comment = ClientQCBD.Comment;
                    Client.Phone = ClientQCBD.Phone;
                    Client.Status = ClientQCBD.Status;
                    Client.Company = ClientQCBD.Company;
                    Client.Email = ClientQCBD.Email;
                    Client.Fax = ClientQCBD.Fax;
                    Client.CompanyName = ClientQCBD.CompanyName;
                    Client.CRN = ClientQCBD.CRN;
                    Client.MaxCredit = ClientQCBD.MaxCredit;
                    Client.Rib = ClientQCBD.Rib;
                    Client.PayDelay = ClientQCBD.PayDelay;

                    lock (_lock) returnList.Add(Client);
                });
            }
            return returnList;
        }

        // insert new row into database
        public static QOBDSet.clientsDataTable ClientTypeToDataTable(this List<Client> ClientList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.clientsDataTable returnQCBDDataTable = new QOBDSet.clientsDataTable();
            if (ClientList != null)
            {
                foreach (var Client in ClientList)
                {
                    QOBDSet.clientsRow ClientQCBD = returnQCBDDataTable.NewclientsRow();
                    ClientQCBD.ID = Client.ID;
                    ClientQCBD.FirstName = Client.FirstName;
                    ClientQCBD.LastName = Client.LastName;
                    ClientQCBD.AgentId = Client.AgentId;
                    ClientQCBD.Comment = Client.Comment;
                    ClientQCBD.Phone = Client.Phone;
                    ClientQCBD.Status = Client.Status;
                    ClientQCBD.Company = Client.Company;
                    ClientQCBD.Email = Client.Email;
                    ClientQCBD.Fax = Client.Fax;
                    ClientQCBD.CompanyName = Client.CompanyName;
                    ClientQCBD.CRN = Client.CRN;
                    ClientQCBD.MaxCredit = Client.MaxCredit;
                    ClientQCBD.Rib = Client.Rib;
                    ClientQCBD.PayDelay = Client.PayDelay;

                    lock (_lock)
                    {
                        if (!idList.Contains(ClientQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(ClientQCBD);
                            idList.Add(ClientQCBD.ID);
                        }
                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.clientsDataTable ClientTypeToDataTable(this List<Client> ClientList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (ClientList != null)
            {
                if (dataSet != null && dataSet.clients.Count > 0)
                {
                    foreach (var Client in ClientList)
                    {
                        QOBDSet.clientsRow ClientQCBD = dataSet.clients.Where(x => x.ID == Client.ID).First();
                        ClientQCBD.FirstName = Client.FirstName;
                        ClientQCBD.LastName = Client.LastName;
                        ClientQCBD.AgentId = Client.AgentId;
                        ClientQCBD.Comment = Client.Comment;
                        ClientQCBD.Phone = Client.Phone;
                        ClientQCBD.Status = Client.Status;
                        ClientQCBD.Company = Client.Company;
                        ClientQCBD.Email = Client.Email;
                        ClientQCBD.Fax = Client.Fax;
                        ClientQCBD.CompanyName = Client.CompanyName;
                        ClientQCBD.CRN = Client.CRN;
                        ClientQCBD.MaxCredit = Client.MaxCredit;
                        ClientQCBD.Rib = Client.Rib;
                        ClientQCBD.PayDelay = Client.PayDelay;
                    }
                }
            }

            return dataSet.clients;
        }

        public static List<Client> ClientTypeToFilterDataTable(this Client client, ESearchOption filterOperator)
        {
            if (client != null)
            {
                string baseSqlString = "SELECT * FROM Clients WHERE ";
                string defaultSqlString = "SELECT * FROM Clients WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (client.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), client.ID);
                if (client.AgentId != 0)
                    query = string.Format(query + " {0} AgentId LIKE '{1}' ", filterOperator.ToString(), client.AgentId);
                if (!string.IsNullOrEmpty(client.FirstName))
                    query = string.Format(query + " {0} LastName LIKE '%{1}%' ", filterOperator.ToString(), client.LastName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.LastName))
                    query = string.Format(query + " {0} FirstName LIKE '%{1}%' ", filterOperator.ToString(), client.FirstName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Company))
                    query = string.Format(query + " {0} Company LIKE '%{1}%' ", filterOperator.ToString(), client.Company.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Email))
                    query = string.Format(query + " {0} Email LIKE '%{1}%' ", filterOperator.ToString(), client.Email.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Phone))
                    query = string.Format(query + " {0} Phone LIKE '{1}' ", filterOperator.ToString(), client.Phone.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Fax))
                    query = string.Format(query + " {0} Fax LIKE '{1}' ", filterOperator.ToString(), client.Fax.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Rib))
                    query = string.Format(query + " {0} Rib LIKE '{1}' ", filterOperator.ToString(), client.Rib.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Rib))
                    query = string.Format(query + " {0} Rib LIKE '{1}' ", filterOperator.ToString(), client.Rib.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.CRN))
                    query = string.Format(query + " {0} CRN LIKE '%{1}%' ", filterOperator.ToString(), client.CRN.Replace("'", "''"));
                if (client.PayDelay > 0)
                    query = string.Format(query + " {0} PayDelay LIKE '{1}' ", filterOperator.ToString(), client.PayDelay);
                if (!string.IsNullOrEmpty(client.Comment))
                    query = string.Format(query + " {0} Comment LIKE '%{1}%' ", filterOperator.ToString(), client.Comment.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Status))
                    query = string.Format(query + " {0} Status LIKE '{1}' ", filterOperator.ToString(), client.Status.Replace("'", "''"));
                if (client.MaxCredit > 0)
                    query = string.Format(query + " {0} MaxCredit LIKE '{1}' ", filterOperator.ToString(), client.MaxCredit);
                if (!string.IsNullOrEmpty(client.CompanyName))
                    query = string.Format(query + " {0} CompanyName LIKE '{1}' ", filterOperator.ToString(), client.CompanyName.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToClient((QOBDSet.clientsDataTable)getDataTableFromSqlQuery<QOBDSet.clientsDataTable>(baseSqlString));

            }
            return new List<Client>();
        }

        //====================================================================================
        //===============================[ Contact ]===========================================
        //====================================================================================

        public static List<Contact> DataTableTypeToContact(this QOBDSet.contactsDataTable ContactDataTable)
        {
            object _lock = new object(); List<Contact> returnList = new List<Contact>();
            if (ContactDataTable != null)
            {
                foreach (var ContactQCBD in ContactDataTable)
                {
                    Contact Contact = new Contact();
                    Contact.ID = ContactQCBD.ID;
                    Contact.Cellphone = ContactQCBD.Cellphone;
                    Contact.ClientId = ContactQCBD.ClientId;
                    Contact.Comment = ContactQCBD.Comment;
                    Contact.Email = ContactQCBD.Email;
                    Contact.Phone = ContactQCBD.Phone;
                    Contact.Fax = ContactQCBD.Fax;
                    Contact.Firstname = ContactQCBD.Firstname;
                    Contact.LastName = ContactQCBD.LastName;
                    Contact.Position = ContactQCBD.Position;

                    lock (_lock) returnList.Add(Contact);
                }
            }
            return returnList;
        }

        public static QOBDSet.contactsDataTable ContactTypeToDataTable(this List<Contact> ContactList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.contactsDataTable returnQCBDDataTable = new QOBDSet.contactsDataTable();
            if (ContactList != null)
            {
                foreach (var Contact in ContactList)
                {
                    QOBDSet.contactsRow ContactQCBD = returnQCBDDataTable.NewcontactsRow();
                    ContactQCBD.ID = Contact.ID;
                    ContactQCBD.Position = Contact.Position;
                    ContactQCBD.LastName = Contact.LastName;
                    ContactQCBD.Firstname = Contact.Firstname;
                    ContactQCBD.Comment = Contact.Comment;
                    ContactQCBD.Phone = Contact.Phone;
                    ContactQCBD.ClientId = Contact.ClientId;
                    ContactQCBD.Cellphone = Contact.Cellphone;
                    ContactQCBD.Email = Contact.Email;
                    ContactQCBD.Fax = Contact.Fax;

                    lock (_lock)
                    {
                        if (!idList.Contains(ContactQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(ContactQCBD);
                            idList.Add(ContactQCBD.ID);
                        }
                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.contactsDataTable ContactTypeToDataTable(this List<Contact> contactList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (contactList != null)
            {
                if (dataSet != null && dataSet.contacts.Count > 0)
                {
                    foreach (var Contact in contactList)
                    {
                        QOBDSet.contactsRow ContactQCBD = dataSet.contacts.Where(x => x.ID == Contact.ID).First();
                        ContactQCBD.Position = Contact.Position;
                        ContactQCBD.LastName = Contact.LastName;
                        ContactQCBD.Firstname = Contact.Firstname;
                        ContactQCBD.Comment = Contact.Comment;
                        ContactQCBD.Phone = Contact.Phone;
                        ContactQCBD.ClientId = Contact.ClientId;
                        ContactQCBD.Cellphone = Contact.Cellphone;
                        ContactQCBD.Email = Contact.Email;
                        ContactQCBD.Fax = Contact.Fax;
                    }
                }
            }

            return dataSet.contacts;
        }

        public static List<Contact> ContactTypeToFilterDataTable(this Contact Contact, ESearchOption filterOperator)
        {
            if (Contact != null)
            {
                string baseSqlString = "SELECT * FROM Contacts WHERE ";
                string defaultSqlString = "SELECT * FROM Contacts WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Contact.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Contact.ID);
                if (Contact.ClientId != 0)
                    query = string.Format(query + " {0} ClientId LIKE '{1}' ", filterOperator.ToString(), Contact.ClientId);
                if (!string.IsNullOrEmpty(Contact.Firstname))
                    query = string.Format(query + " {0} Firstname LIKE '{1}' ", filterOperator.ToString(), Contact.Firstname.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.LastName))
                    query = string.Format(query + " {0} LastName LIKE '{1}' ", filterOperator.ToString(), Contact.LastName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Position))
                    query = string.Format(query + " {0} Position LIKE '{1}' ", filterOperator.ToString(), Contact.Position.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Email))
                    query = string.Format(query + " {0} Email LIKE '{1}' ", filterOperator.ToString(), Contact.Email.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Phone))
                    query = string.Format(query + " {0} Phone LIKE '{1}' ", filterOperator.ToString(), Contact.Phone.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Cellphone))
                    query = string.Format(query + " {0} Cellphone LIKE '{1}' ", filterOperator.ToString(), Contact.Cellphone.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Fax))
                    query = string.Format(query + " {0} Fax LIKE '{1}' ", filterOperator.ToString(), Contact.Fax.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Comment))
                    query = string.Format(query + " {0} Comment LIKE '{1}' ", filterOperator.ToString(), Contact.Comment.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToContact((QOBDSet.contactsDataTable)getDataTableFromSqlQuery<QOBDSet.contactsDataTable>(baseSqlString));

            }
            return new List<Contact>();
        }



        //====================================================================================
        //===============================[ Address ]===========================================
        //====================================================================================

        public static List<Address> DataTableTypeToAddress(this QOBDSet.addressesDataTable AddressesDataTable)
        {
            object _lock = new object(); List<Address> returnList = new List<Address>();
            if (AddressesDataTable != null)
            {
                foreach (var AddressQCBD in AddressesDataTable)
                {
                    Address Address = new Address();
                    Address.ID = AddressQCBD.ID;
                    Address.AddressName = AddressQCBD.Address;
                    Address.ClientId = AddressQCBD.ClientId;
                    Address.Comment = AddressQCBD.Comment;
                    Address.Email = AddressQCBD.Email;
                    Address.Phone = AddressQCBD.Phone;
                    Address.CityName = AddressQCBD.CityName;
                    Address.Country = AddressQCBD.Country;
                    Address.LastName = AddressQCBD.LastName;
                    Address.FirstName = AddressQCBD.FirstName;
                    Address.Name = AddressQCBD.Name;
                    Address.Name2 = AddressQCBD.Name2;
                    Address.Postcode = AddressQCBD.Postcode;

                    lock (_lock) lock (_lock) returnList.Add(Address);
                }
            }
            return returnList;
        }

        public static QOBDSet.addressesDataTable AddressTypeToDataTable(this List<Address> AddressList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.addressesDataTable returnQCBDDataTable = new QOBDSet.addressesDataTable();
            if (AddressList != null)
            {
                foreach (var Address in AddressList)
                {
                    QOBDSet.addressesRow AddressQCBD = returnQCBDDataTable.NewaddressesRow();
                    AddressQCBD.ID = Address.ID;
                    AddressQCBD.Address = Address.AddressName;
                    AddressQCBD.ClientId = Address.ClientId;
                    AddressQCBD.Comment = Address.Comment;
                    AddressQCBD.Email = Address.Email;
                    AddressQCBD.Phone = Address.Phone;
                    AddressQCBD.CityName = Address.CityName;
                    AddressQCBD.Country = Address.Country;
                    AddressQCBD.LastName = Address.LastName;
                    AddressQCBD.FirstName = Address.FirstName;
                    AddressQCBD.Name = Address.Name;
                    AddressQCBD.Name2 = Address.Name2;
                    AddressQCBD.Postcode = Address.Postcode;

                    lock (_lock)
                    {
                        if (!idList.Contains(AddressQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(AddressQCBD);
                            idList.Add(AddressQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.addressesDataTable AddressTypeToDataTable(this List<Address> addressesList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (addressesList != null)
            {
                if (dataSet != null && dataSet.addresses.Count > 0)
                {
                    foreach (var Address in addressesList)
                    {
                        QOBDSet.addressesRow AddressQCBD = dataSet.addresses.Where(x => x.ID == Address.ID).First();
                        AddressQCBD.Address = Address.AddressName;
                        AddressQCBD.ClientId = Address.ClientId;
                        AddressQCBD.Comment = Address.Comment;
                        AddressQCBD.Email = Address.Email;
                        AddressQCBD.Phone = Address.Phone;
                        AddressQCBD.CityName = Address.CityName;
                        AddressQCBD.Country = Address.Country;
                        AddressQCBD.LastName = Address.LastName;
                        AddressQCBD.FirstName = Address.FirstName;
                        AddressQCBD.Name = Address.Name;
                        AddressQCBD.Name2 = Address.Name2;
                        AddressQCBD.Postcode = Address.Postcode;
                    }
                }
            }
            return dataSet.addresses;
        }

        public static List<Address> AddressTypeToFilterDataTable(this Address Address, ESearchOption filterOperator)
        {
            if (Address != null)
            {
                string baseSqlString = "SELECT * FROM Addresses WHERE ";
                string defaultSqlString = "SELECT * FROM Addresses WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Address.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Address.ID);
                if (Address.ClientId != 0)
                    query = string.Format(query + " {0} ClientId LIKE '{1}' ", filterOperator.ToString(), Address.ClientId);
                if (!string.IsNullOrEmpty(Address.Name))
                    query = string.Format(query + " {0} Name LIKE '{1}' ", filterOperator.ToString(), Address.Name.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Name2))
                    query = string.Format(query + " {0} Name2 LIKE '{1}' ", filterOperator.ToString(), Address.Name2.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.CityName))
                    query = string.Format(query + " {0} CityName LIKE '{1}' ", filterOperator.ToString(), Address.CityName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.AddressName))
                    query = string.Format(query + " {0} Address LIKE '{1}' ", filterOperator.ToString(), Address.AddressName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Postcode))
                    query = string.Format(query + " {0} Postcode LIKE '{1}' ", filterOperator.ToString(), Address.Postcode.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Country))
                    query = string.Format(query + " {0} Country LIKE '{1}' ", filterOperator.ToString(), Address.Country.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Comment))
                    query = string.Format(query + " {0} Comment LIKE '{1}' ", filterOperator.ToString(), Address.Comment.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.FirstName))
                    query = string.Format(query + " {0} FirstName LIKE '{1}' ", filterOperator.ToString(), Address.FirstName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.LastName))
                    query = string.Format(query + " {0} LastName LIKE '{1}' ", filterOperator.ToString(), Address.LastName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Phone))
                    query = string.Format(query + " {0} Phone LIKE '{1}' ", filterOperator.ToString(), Address.Phone.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Email))
                    query = string.Format(query + " {0} Email LIKE '{1}' ", filterOperator.ToString(), Address.Email.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToAddress((QOBDSet.addressesDataTable)getDataTableFromSqlQuery<QOBDSet.addressesDataTable>(baseSqlString));

            }
            return new List<Address>();
        }


        //====================================================================================
        //===============================[ Bill ]===========================================
        //====================================================================================

        public static List<Bill> DataTableTypeToBill(this QOBDSet.billsDataTable BillDataTable)
        {
            object _lock = new object(); List<Bill> returnList = new List<Bill>();
            if (BillDataTable != null)
            {
                foreach (var BillQCBD in BillDataTable)
                {
                    Bill Bill = new Bill();
                    Bill.ID = BillQCBD.ID;
                    Bill.ClientId = BillQCBD.ClientId;
                    Bill.OrderId = BillQCBD.CommandId;
                    Bill.Comment1 = BillQCBD.Comment1;
                    Bill.Comment2 = BillQCBD.Comment2;
                    Bill.Date = BillQCBD.Date;
                    Bill.DateLimit = BillQCBD.DateLimit;
                    Bill.Pay = BillQCBD.Pay;
                    Bill.PayDate = BillQCBD.DatePay;
                    Bill.PayMod = BillQCBD.PayMod;
                    Bill.PayReceived = BillQCBD.PayReceived;

                    lock (_lock) returnList.Add(Bill);
                }
            }
            return returnList;
        }

        public static QOBDSet.billsDataTable BillTypeToDataTable(this List<Bill> BillList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.billsDataTable returnQCBDDataTable = new QOBDSet.billsDataTable();
            if (BillList != null)
            {
                foreach (var Bill in BillList)
                {
                    QOBDSet.billsRow BillQCBD = returnQCBDDataTable.NewbillsRow();
                    BillQCBD.ID = Bill.ID;
                    BillQCBD.ClientId = Bill.ClientId;
                    BillQCBD.CommandId = Bill.OrderId;
                    BillQCBD.Comment1 = Bill.Comment1;
                    BillQCBD.Comment2 = Bill.Comment2;
                    BillQCBD.Date = Bill.Date;
                    BillQCBD.DateLimit = Bill.DateLimit;
                    BillQCBD.Pay = Bill.Pay;
                    BillQCBD.DatePay = Bill.PayDate;
                    BillQCBD.PayMod = Bill.PayMod;
                    BillQCBD.PayReceived = Bill.PayReceived;

                    lock (_lock)
                    {
                        if (!idList.Contains(BillQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(BillQCBD);
                            idList.Add(BillQCBD.ID);
                        }
                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.billsDataTable BillTypeToDataTable(this List<Bill> billList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (billList != null)
            {
                if (dataSet != null && dataSet.bills.Count > 0)
                {
                    foreach (var Bill in billList)
                    {
                        QOBDSet.billsRow BillQCBD = dataSet.bills.Where(x => x.ID == Bill.ID).First();
                        BillQCBD.ClientId = Bill.ClientId;
                        BillQCBD.CommandId = Bill.OrderId;
                        BillQCBD.Comment1 = Bill.Comment1;
                        BillQCBD.Comment2 = Bill.Comment2;
                        BillQCBD.Date = Bill.Date;
                        BillQCBD.DateLimit = Bill.DateLimit;
                        BillQCBD.Pay = Bill.Pay;
                        BillQCBD.DatePay = Bill.PayDate;
                        BillQCBD.PayMod = Bill.PayMod;
                        BillQCBD.PayReceived = Bill.PayReceived;
                    }
                }
            }
            return dataSet.bills;
        }

        public static List<Bill> BillTypeToFilterDataTable(this Bill Bill, ESearchOption filterOperator)
        {
            if (Bill != null)
            {
                string baseSqlString = "SELECT * FROM Bills WHERE ";
                string defaultSqlString = "SELECT * FROM Bills WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Bill.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Bill.ID);
                if (Bill.ClientId != 0)
                    query = string.Format(query + " {0} ClientId LIKE '{1}' ", filterOperator.ToString(), Bill.ClientId);
                if (Bill.OrderId != 0)
                    query = string.Format(query + " {0} CommandId LIKE '{1}' ", filterOperator.ToString(), Bill.OrderId);
                if (Bill.Pay != 0)
                    query = string.Format(query + " {0} Pay LIKE '{1}' ", filterOperator.ToString(), Bill.Pay);
                if (!string.IsNullOrEmpty(Bill.PayMod))
                    query = string.Format(query + " {0} PayMod LIKE '{1}' ", filterOperator.ToString(), Bill.PayMod);
                if (Bill.PayReceived != 0)
                    query = string.Format(query + " {0} PayReceived LIKE '{1}' ", filterOperator.ToString(), Bill.PayReceived);
                if (!string.IsNullOrEmpty(Bill.Comment2))
                    query = string.Format(query + " {0} Comment2 LIKE '{1}' ", filterOperator.ToString(), Bill.Comment2.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Bill.Comment1))
                    query = string.Format(query + " {0} Comment1 LIKE '{1}' ", filterOperator.ToString(), Bill.Comment1.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToBill((QOBDSet.billsDataTable)DALHelper.getDataTableFromSqlQuery<QOBDSet.billsDataTable>(baseSqlString));

            }
            return new List<Bill>();
        }

        public static Bill LastBill()
        {
            string baseSqlString = "SELECT TOP 1 * FROM Bills ORDER BY ID DESC ";

            var FoundList = DataTableTypeToBill((QOBDSet.billsDataTable)DALHelper.getDataTableFromSqlQuery<QOBDSet.billsDataTable>(baseSqlString));
            if (FoundList.Count > 0)
                return FoundList[0];

            return new Bill();
        }

        //====================================================================================
        //===============================[ Delivery ]===========================================
        //====================================================================================

        public static List<Delivery> DataTableTypeToDelivery(this QOBDSet.deliveriesDataTable DeliveryDataTable)
        {
            object _lock = new object(); List<Delivery> returnList = new List<Delivery>();
            if (DeliveryDataTable != null)
            {
                foreach (var DeliveryQCBD in DeliveryDataTable)
                {
                    Delivery Delivery = new Delivery();
                    Delivery.ID = DeliveryQCBD.ID;
                    Delivery.BillId = DeliveryQCBD.BillId;
                    Delivery.OrderId = DeliveryQCBD.CommandId;
                    Delivery.Date = DeliveryQCBD.Date;
                    Delivery.Package = DeliveryQCBD.Package;
                    Delivery.Status = DeliveryQCBD.Status;

                    lock (_lock) returnList.Add(Delivery);
                }
            }
            return returnList;
        }

        public static QOBDSet.deliveriesDataTable DeliveryTypeToDataTable(this List<Delivery> DeliveryList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.deliveriesDataTable returnQCBDDataTable = new QOBDSet.deliveriesDataTable();
            if (DeliveryList != null)
            {
                foreach (var Delivery in DeliveryList)
                {
                    QOBDSet.deliveriesRow DeliveryQCBD = returnQCBDDataTable.NewdeliveriesRow();
                    DeliveryQCBD.ID = Delivery.ID;
                    DeliveryQCBD.BillId = Delivery.BillId;
                    DeliveryQCBD.CommandId = Delivery.OrderId;
                    DeliveryQCBD.Date = Delivery.Date;
                    DeliveryQCBD.Package = Delivery.Package;
                    DeliveryQCBD.Status = Delivery.Status;

                    lock (_lock)
                    {
                        if (!idList.Contains(DeliveryQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(DeliveryQCBD);
                            idList.Add(DeliveryQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.deliveriesDataTable DeliveryTypeToDataTable(this List<Delivery> deliveriesList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (deliveriesList != null)
            {
                if (dataSet != null && dataSet.deliveries.Count > 0)
                {
                    foreach (var Delivery in deliveriesList)
                    {
                        QOBDSet.deliveriesRow DeliveryQCBD = dataSet.deliveries.Where(x => x.ID == Delivery.ID).First();
                        DeliveryQCBD.BillId = Delivery.BillId;
                        DeliveryQCBD.CommandId = Delivery.OrderId;
                        DeliveryQCBD.Date = Delivery.Date;
                        DeliveryQCBD.Package = Delivery.Package;
                        DeliveryQCBD.Status = Delivery.Status;
                    }
                }
            }
            return dataSet.deliveries;
        }

        public static List<Delivery> DeliveryTypeToFilterDataTable(this Delivery Delivery, ESearchOption filterOperator)
        {
            if (Delivery != null)
            {
                string baseSqlString = "SELECT * FROM Deliveries WHERE ";
                string defaultSqlString = "SELECT * FROM Deliveries WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Delivery.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Delivery.ID);
                if (!string.IsNullOrEmpty(Delivery.Status))
                    query = string.Format(query + " {0} Status LIKE '{1}' ", filterOperator.ToString(), Delivery.Status);
                if (Delivery.OrderId != 0)
                    query = string.Format(query + " {0} CommandId LIKE '{1}' ", filterOperator.ToString(), Delivery.OrderId);
                if (Delivery.BillId != 0)
                    query = string.Format(query + " {0} BillId LIKE '{1}' ", filterOperator.ToString(), Delivery.BillId);
                if (Delivery.Package != 0)
                    query = string.Format(query + " {0} Package LIKE '{1}' ", filterOperator.ToString(), Delivery.Package);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToDelivery((QOBDSet.deliveriesDataTable)DALHelper.getDataTableFromSqlQuery<QOBDSet.deliveriesDataTable>(baseSqlString));

            }
            return new List<Delivery>();
        }

        //====================================================================================
        //================================[ Order_item ]====================================
        //====================================================================================

        public static List<Order_item> DataTableTypeToOrder_item(this QOBDSet.command_itemsDataTable Order_itemDataTable)
        {
            object _lock = new object(); List<Order_item> returnList = new List<Order_item>();
            if (Order_itemDataTable != null)
            {
                Parallel.ForEach(Order_itemDataTable, (Action<QOBDSet.command_itemsRow>)((Order_itemQCBD) =>
                {
                    Order_item Order_item = new Order_item();
                    Order_item.ID = Order_itemQCBD.ID;
                    Order_item.OrderId = Order_itemQCBD.CommandId;
                    Order_item.Comment_Purchase_Price = Order_itemQCBD.Comment_Purchase_Price;
                    Order_item.Item_ref = Order_itemQCBD.Item_ref;
                    Order_item.Rank = Order_itemQCBD.Order;
                    Order_item.Price = Order_itemQCBD.Price;
                    Order_item.Price_purchase = Order_itemQCBD.Price_purchase;
                    Order_item.Quantity = Order_itemQCBD.Quantity;
                    Order_item.Quantity_current = Order_itemQCBD.Quantity_current;
                    Order_item.Quantity_delivery = Order_itemQCBD.Quantity_delivery;

                    lock (_lock) returnList.Add(Order_item);
                }));
            }
            var test = returnList.Where(x => x.OrderId == 3410).ToList(); ;
            return returnList;
        }

        public static QOBDSet.command_itemsDataTable Order_itemTypeToDataTable(this List<Order_item> Order_itemList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.command_itemsDataTable returnQCBDDataTable = new QOBDSet.command_itemsDataTable();
            if (Order_itemList != null)
            {
                foreach (var Order_item in Order_itemList)
                {
                    QOBDSet.command_itemsRow Order_itemQCBD = returnQCBDDataTable.Newcommand_itemsRow();
                    Order_itemQCBD.ID = Order_item.ID;
                    Order_itemQCBD.CommandId = Order_item.OrderId;
                    Order_itemQCBD.Comment_Purchase_Price = Order_item.Comment_Purchase_Price;
                    Order_itemQCBD.Item_ref = Order_item.Item_ref;
                    Order_itemQCBD.Order = Order_item.Rank;
                    Order_itemQCBD.Price = Order_item.Price;
                    Order_itemQCBD.Price_purchase = Order_item.Price_purchase;
                    Order_itemQCBD.Quantity = Order_item.Quantity;
                    Order_itemQCBD.Quantity_current = Order_item.Quantity_current;
                    Order_itemQCBD.Quantity_delivery = Order_item.Quantity_delivery;

                    lock (_lock)
                    {
                        if (!idList.Contains(Order_itemQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(Order_itemQCBD);
                            idList.Add(Order_itemQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.command_itemsDataTable Order_itemTypeToDataTable(this List<Order_item> order_itemsList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (order_itemsList != null)
            {
                if (dataSet != null && dataSet.command_items.Count > 0)
                {
                    foreach (var Order_item in order_itemsList)
                    {
                        QOBDSet.command_itemsRow Order_itemQCBD = dataSet.command_items.Where(x => x.ID == Order_item.ID).First();
                        Order_itemQCBD.CommandId = Order_item.OrderId;
                        Order_itemQCBD.Comment_Purchase_Price = Order_item.Comment_Purchase_Price;
                        Order_itemQCBD.Item_ref = Order_item.Item_ref;
                        Order_itemQCBD.Order = Order_item.Rank;
                        Order_itemQCBD.Price = Order_item.Price;
                        Order_itemQCBD.Price_purchase = Order_item.Price_purchase;
                        Order_itemQCBD.Quantity = Order_item.Quantity;
                        Order_itemQCBD.Quantity_current = Order_item.Quantity_current;
                        Order_itemQCBD.Quantity_delivery = Order_item.Quantity_delivery;
                    }
                }
            }
            return dataSet.command_items;
        }

        public static List<Order_item> order_itemTypeToFilterDataTable(this Order_item Order_item, ESearchOption filterOperator)
        {
            if (Order_item != null)
            {
                string baseSqlString = "SELECT * FROM Command_items WHERE ";
                string defaultSqlString = "SELECT * FROM Command_items WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Order_item.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Order_item.ID);
                if (Order_item.OrderId != 0)
                    query = string.Format(query + " {0} CommandId LIKE '{1}' ", filterOperator.ToString(), Order_item.OrderId);
                if (Order_item.Quantity != 0)
                    query = string.Format(query + " {0} Quantity LIKE '{1}' ", filterOperator.ToString(), Order_item.Quantity);
                if (Order_item.Quantity_delivery != 0)
                    query = string.Format(query + " {0} Quantity_delivery LIKE '{1}' ", filterOperator.ToString(), Order_item.Quantity_delivery);
                if (!string.IsNullOrEmpty(Order_item.Item_ref))
                    query = string.Format(query + " {0} Item_ref LIKE '{1}' ", filterOperator.ToString(), Order_item.Item_ref);
                if (Order_item.ItemId != 0)
                    query = string.Format(query + " {0} ItemId LIKE '{1}' ", filterOperator.ToString(), Order_item.ItemId);
                if (Order_item.Quantity_current != 0)
                    query = string.Format(query + " {0} Quantity_current LIKE '{1}' ", filterOperator.ToString(), Order_item.Quantity_current);
                if (Order_item.Price != 0)
                    query = string.Format(query + " {0} Price LIKE '{1}' ", filterOperator.ToString(), Order_item.Price);
                if (Order_item.Price_purchase != 0)
                    query = string.Format(query + " {0} Price_purchase LIKE '{1}' ", filterOperator.ToString(), Order_item.Price_purchase);
                if (!string.IsNullOrEmpty(Order_item.Comment_Purchase_Price))
                    query = string.Format(query + " {0} Comment_Purchase_Price LIKE '{1}' ", filterOperator.ToString(), Order_item.Comment_Purchase_Price.Replace("'", "''"));
                if (Order_item.Rank != 0)
                    query = string.Format(query + " {0} [Order] LIKE '{1}' ", filterOperator.ToString(), Order_item.Rank);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToOrder_item((QOBDSet.command_itemsDataTable)getDataTableFromSqlQuery<QOBDSet.command_itemsDataTable>(baseSqlString));

            }

            return new List<Order_item>();

        }


        //====================================================================================
        //==================================[ Tax ]===========================================
        //====================================================================================

        public static List<Tax> DataTableTypeToTax(this QOBDSet.taxesDataTable TaxDataTable)
        {
            object _lock = new object(); List<Tax> returnList = new List<Tax>();
            if (TaxDataTable != null)
            {
                foreach (var TaxQCBD in TaxDataTable)
                {
                    Tax Tax = new Tax();
                    Tax.ID = TaxQCBD.ID;
                    Tax.Tax_current = TaxQCBD.Tax_current;
                    Tax.Type = TaxQCBD.Type;
                    Tax.Value = TaxQCBD.Value;
                    Tax.Date_insert = TaxQCBD.Date_insert;
                    Tax.Comment = TaxQCBD.Comment;

                    lock (_lock) returnList.Add(Tax);
                }
            }
            return returnList;
        }

        public static QOBDSet.taxesDataTable TaxTypeToDataTable(this List<Tax> TaxList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.taxesDataTable returnQCBDDataTable = new QOBDSet.taxesDataTable();
            if (TaxList != null)
            {
                foreach (var Tax in TaxList)
                {
                    QOBDSet.taxesRow TaxQCBD = returnQCBDDataTable.NewtaxesRow();
                    TaxQCBD.ID = Tax.ID;
                    TaxQCBD.Tax_current = Tax.Tax_current;
                    TaxQCBD.Type = Tax.Type;
                    TaxQCBD.Value = Tax.Value;
                    TaxQCBD.Date_insert = Tax.Date_insert;
                    TaxQCBD.Comment = Tax.Comment;

                    lock (_lock)
                    {
                        if (!idList.Contains(TaxQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(TaxQCBD);
                            idList.Add(TaxQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.taxesDataTable TaxTypeToDataTable(this List<Tax> taxesList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (taxesList != null)
            {
                if (dataSet != null && dataSet.taxes.Count > 0)
                {
                    foreach (var Tax in taxesList)
                    {
                        QOBDSet.taxesRow TaxQCBD = dataSet.taxes.Where(x => x.ID == Tax.ID).First();
                        TaxQCBD.Tax_current = Tax.Tax_current;
                        TaxQCBD.Type = Tax.Type;
                        TaxQCBD.Value = Tax.Value;
                        TaxQCBD.Date_insert = Tax.Date_insert;
                        TaxQCBD.Comment = Tax.Comment;
                    }
                }
            }
            return dataSet.taxes;
        }

        public static List<Tax> TaxTypeToFilterDataTable(this Tax Tax, ESearchOption filterOperator)
        {
            if (Tax != null)
            {
                string baseSqlString = "SELECT * FROM Taxes WHERE ";
                string defaultSqlString = "SELECT * FROM Taxes WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Tax.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Tax.ID);
                if (!string.IsNullOrEmpty(Tax.Type))
                    query = string.Format(query + " {0} Type LIKE '{1}' ", filterOperator.ToString(), Tax.Type);
                if (Tax.Value != 0)
                    query = string.Format(query + " {0} Value LIKE '{1}' ", filterOperator.ToString(), Tax.Value);
                if (Tax.Tax_current != 0)
                    query = string.Format(query + " {0} Tax_current LIKE '{1}' ", filterOperator.ToString(), Tax.Tax_current);
                if (!string.IsNullOrEmpty(Tax.Comment))
                    query = string.Format(query + " {0} Comment LIKE '{1}' ", filterOperator.ToString(), Tax.Comment.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToTax((QOBDSet.taxesDataTable)DALHelper.getDataTableFromSqlQuery<QOBDSet.taxesDataTable>(baseSqlString));

            }
            return new List<Tax>();
        }



        //====================================================================================
        //===============================[ Provider_item ]===========================================
        //====================================================================================

        public static List<Provider_item> DataTableTypeToProvider_item(this QOBDSet.provider_itemsDataTable Provider_itemDataTable)
        {
            object _lock = new object(); List<Provider_item> returnList = new List<Provider_item>();
            if (Provider_itemDataTable != null)
            {
                foreach (var Provider_itemQCBD in Provider_itemDataTable)
                {
                    Provider_item Provider_item = new Provider_item();
                    Provider_item.ID = Provider_itemQCBD.ID;
                    Provider_item.Item_ref = Provider_itemQCBD.Item_ref;
                    Provider_item.Provider_name = Provider_itemQCBD.Provider_name;

                    lock (_lock) returnList.Add(Provider_item);
                }
            }
            return returnList;
        }

        public static QOBDSet.provider_itemsDataTable Provider_itemTypeToDataTable(this List<Provider_item> Provider_itemList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.provider_itemsDataTable returnQCBDDataTable = new QOBDSet.provider_itemsDataTable();
            if (Provider_itemList != null)
            {
                foreach (var Provider_item in Provider_itemList)
                {
                    QOBDSet.provider_itemsRow Provider_itemQCBD = returnQCBDDataTable.Newprovider_itemsRow();
                    Provider_itemQCBD.ID = Provider_item.ID;
                    Provider_itemQCBD.Item_ref = Provider_item.Item_ref;
                    Provider_itemQCBD.Provider_name = Provider_item.Provider_name;

                    lock (_lock)
                    {
                        if (!idList.Contains(Provider_itemQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(Provider_itemQCBD);
                            idList.Add(Provider_itemQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.provider_itemsDataTable Provider_itemTypeToDataTable(this List<Provider_item> provider_itemsList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (provider_itemsList != null)
            {
                if (dataSet != null && dataSet.provider_items.Count > 0)
                {
                    foreach (var Provider_item in provider_itemsList)
                    {
                        QOBDSet.provider_itemsRow Provider_itemQCBD = dataSet.provider_items.Where(x => x.ID == Provider_item.ID).First();
                        Provider_itemQCBD.Item_ref = Provider_item.Item_ref;
                        Provider_itemQCBD.Provider_name = Provider_item.Provider_name;
                    }
                }
            }
            return dataSet.provider_items;
        }

        public static List<Provider_item> Provider_itemTypeToFilterDataTable(this Provider_item Provider_item, ESearchOption filterOperator)
        {
            if (Provider_item != null)
            {
                string baseSqlString = "SELECT * FROM Provider_items WHERE ";
                string defaultSqlString = "SELECT * FROM Provider_items WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Provider_item.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Provider_item.ID);
                if (!string.IsNullOrEmpty(Provider_item.Provider_name))
                    query = string.Format(query + " {0} Provider_name LIKE '{1}' ", filterOperator.ToString(), Provider_item.Provider_name.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Provider_item.Item_ref))
                    query = string.Format(query + " {0} Item_ref LIKE '{1}' ", filterOperator.ToString(), Provider_item.Item_ref.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToProvider_item((QOBDSet.provider_itemsDataTable)DALHelper.getDataTableFromSqlQuery<QOBDSet.provider_itemsDataTable>(baseSqlString));

            }
            return new List<Provider_item>();
        }

        //====================================================================================
        //===============================[ Provider ]===========================================
        //====================================================================================

        public static List<Provider> DataTableTypeToProvider(this QOBDSet.providersDataTable ProviderDataTable)
        {
            object _lock = new object(); List<Provider> returnList = new List<Provider>();
            if (ProviderDataTable != null)
            {
                foreach (var ProviderQCBD in ProviderDataTable)
                {
                    Provider Provider = new Provider();
                    Provider.ID = ProviderQCBD.ID;
                    Provider.Name = ProviderQCBD.Name;
                    Provider.Source = ProviderQCBD.Source;

                    lock (_lock) returnList.Add(Provider);
                }
            }
            return returnList;
        }

        public static QOBDSet.providersDataTable ProviderTypeToDataTable(this List<Provider> ProviderList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.providersDataTable returnQCBDDataTable = new QOBDSet.providersDataTable();
            if (ProviderList != null)
            {
                foreach (var Provider in ProviderList)
                {
                    QOBDSet.providersRow ProviderQCBD = returnQCBDDataTable.NewprovidersRow();
                    ProviderQCBD.ID = Provider.ID;
                    ProviderQCBD.Name = Provider.Name;
                    ProviderQCBD.Source = Provider.Source;

                    lock (_lock)
                    {
                        if (!idList.Contains(ProviderQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(ProviderQCBD);
                            idList.Add(ProviderQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.providersDataTable ProviderTypeToDataTable(this List<Provider> providersList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (providersList != null)
            {
                if (dataSet != null && dataSet.providers.Count > 0)
                {
                    foreach (var Provider in providersList)
                    {
                        QOBDSet.providersRow ProviderQCBD = dataSet.providers.Where(x => x.ID == Provider.ID).First();
                        ProviderQCBD.Name = Provider.Name;
                        ProviderQCBD.Source = Provider.Source;
                    }
                }
            }
            return dataSet.providers;
        }

        public static List<Provider> ProviderTypeToFilterDataTable(this Provider Provider, ESearchOption filterOperator)
        {
            if (Provider != null)
            {
                string baseSqlString = "SELECT * FROM Providers WHERE ";
                string defaultSqlString = "SELECT * FROM Providers WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Provider.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Provider.ID);
                if (!string.IsNullOrEmpty(Provider.Name))
                    query = string.Format(query + " {0} Name LIKE '{1}' ", filterOperator.ToString(), Provider.Name.Replace("'", "''"));
                if (Provider.Source != 0)
                    query = string.Format(query + " {0} Source LIKE '{1}' ", filterOperator.ToString(), Provider.Source);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToProvider((QOBDSet.providersDataTable)DALHelper.getDataTableFromSqlQuery<QOBDSet.providersDataTable>(baseSqlString));

            }
            return new List<Provider>();
        }

        //====================================================================================
        //===============================[ Item ]===========================================
        //====================================================================================

        public static List<Item> DataTableTypeToItem(this QOBDSet.itemsDataTable ItemDataTable)
        {
            object _lock = new object(); List<Item> returnList = new List<Item>();
            if (ItemDataTable != null)
            {
                Parallel.ForEach(ItemDataTable, (ItemQCBD) =>
                {
                    Item Item = new Item();
                    Item.ID = ItemQCBD.ID;
                    Item.Comment = ItemQCBD.Comment;
                    Item.Erasable = ItemQCBD.Erasable;
                    Item.Name = ItemQCBD.Name;
                    Item.Price_purchase = ItemQCBD.Price_purchase;
                    Item.Price_sell = ItemQCBD.Price_sell;
                    Item.Ref = ItemQCBD.Ref;
                    Item.Type_sub = ItemQCBD.Type_sub;
                    Item.Source = ItemQCBD.Source;
                    Item.Type = ItemQCBD.Type;

                    lock (_lock) returnList.Add(Item);
                });
            }
            return returnList;
        }

        public static QOBDSet.itemsDataTable ItemTypeToDataTable(this List<Item> ItemList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.itemsDataTable returnQCBDDataTable = new QOBDSet.itemsDataTable();
            if (ItemList != null)
            {
                foreach (var Item in ItemList)
                {
                    QOBDSet.itemsRow ItemQCBD = returnQCBDDataTable.NewitemsRow();
                    ItemQCBD.ID = Item.ID;
                    ItemQCBD.Comment = Item.Comment;
                    ItemQCBD.Erasable = Item.Erasable;
                    ItemQCBD.Name = Item.Name;
                    ItemQCBD.Price_purchase = Item.Price_purchase;
                    ItemQCBD.Price_sell = Item.Price_sell;
                    ItemQCBD.Ref = Item.Ref;
                    ItemQCBD.Type_sub = Item.Type_sub;
                    ItemQCBD.Source = Item.Source;
                    ItemQCBD.Type = Item.Type;

                    lock (_lock)
                    {
                        if (!idList.Contains(ItemQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(ItemQCBD);
                            idList.Add(ItemQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.itemsDataTable ItemTypeToDataTable(this List<Item> itemsList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (itemsList != null)
            {
                if (dataSet != null && dataSet.items.Count > 0)
                {
                    foreach (var Item in itemsList)
                    {
                        QOBDSet.itemsRow ItemQCBD = dataSet.items.Where(x => x.ID == Item.ID).First();
                        ItemQCBD.Comment = Item.Comment;
                        ItemQCBD.Erasable = Item.Erasable;
                        ItemQCBD.Name = Item.Name;
                        ItemQCBD.Price_purchase = Item.Price_purchase;
                        ItemQCBD.Price_sell = Item.Price_sell;
                        ItemQCBD.Ref = Item.Ref;
                        ItemQCBD.Type_sub = Item.Type_sub;
                        ItemQCBD.Source = Item.Source;
                        ItemQCBD.Type = Item.Type;
                    }
                }
            }
            return dataSet.items;
        }

        public static List<Item> ItemTypeToFilterDataTable(this Item item, ESearchOption filterOperator)
        {
            if (item != null)
            {
                string baseSqlString = "SELECT * FROM items WHERE ";
                string defaultSqlString = "SELECT * FROM items WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (item.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), item.ID);
                if (item.Price_purchase != 0)
                    query = string.Format(query + " {0} Price_purchase LIKE '{1}' ", filterOperator.ToString(), item.Price_purchase);
                if (!string.IsNullOrEmpty(item.Ref))
                    query = string.Format(query + " {0} Ref LIKE '{1}' ", filterOperator.ToString(), item.Ref.Replace("'", "''"));
                if (!string.IsNullOrEmpty(item.Name))
                    query = string.Format(query + " {0} Name LIKE '%{1}%' ", filterOperator.ToString(), item.Name.Replace("'", "''"));
                if (!string.IsNullOrEmpty(item.Type))
                    query = string.Format(query + " {0} Type LIKE '{1}' ", filterOperator.ToString(), item.Type);
                if (!string.IsNullOrEmpty(item.Type_sub))
                    query = string.Format(query + " {0} Type_sub LIKE '{1}' ", filterOperator.ToString(), item.Type_sub);
                if (item.Price_sell != 0)
                    query = string.Format(query + " {0} Price_sell LIKE '{1}' ", filterOperator.ToString(), item.Price_sell);
                if (item.Source != 0)
                    query = string.Format(query + " {0} Source LIKE '{1}' ", filterOperator.ToString(), item.Source);
                if (!string.IsNullOrEmpty(item.Comment))
                    query = string.Format(query + " {0} Comment LIKE '%{1}%' ", filterOperator.ToString(), item.Comment.Replace("'", "''"));
                if (!string.IsNullOrEmpty(item.Erasable))
                    query = string.Format(query + " {0} Erasable LIKE '{1}' ", filterOperator.ToString(), item.Erasable);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToItem((QOBDSet.itemsDataTable)DALHelper.getDataTableFromSqlQuery<QOBDSet.itemsDataTable>(baseSqlString));

            }
            return new List<Item>();
        }




        //====================================================================================
        //===============================[ Item_delivery ]===========================================
        //====================================================================================


        public static List<Item_delivery> DataTableTypeToItem_delivery(this QOBDSet.item_deliveriesDataTable Item_deliveryDataTable)
        {
            object _lock = new object(); List<Item_delivery> returnList = new List<Item_delivery>();
            if (Item_deliveryDataTable != null)
            {
                foreach (var Item_deliveryQCBD in Item_deliveryDataTable)
                {
                    Item_delivery Item_delivery = new Item_delivery();
                    Item_delivery.ID = Item_deliveryQCBD.ID;
                    Item_delivery.DeliveryId = Item_deliveryQCBD.DeliveryId;
                    Item_delivery.Item_ref = Item_deliveryQCBD.Item_ref;
                    Item_delivery.Quantity_delivery = Item_deliveryQCBD.Quantity_delivery;

                    lock (_lock) returnList.Add(Item_delivery);
                }
            }
            return returnList;
        }

        public static QOBDSet.item_deliveriesDataTable Item_deliveryTypeToDataTable(this List<Item_delivery> Item_deliveryList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.item_deliveriesDataTable returnQCBDDataTable = new QOBDSet.item_deliveriesDataTable();
            if (Item_deliveryList != null)
            {
                foreach (var Item_delivery in Item_deliveryList)
                {
                    QOBDSet.item_deliveriesRow Item_deliveryQCBD = returnQCBDDataTable.Newitem_deliveriesRow();
                    Item_deliveryQCBD.ID = Item_delivery.ID;
                    Item_deliveryQCBD.DeliveryId = Item_delivery.DeliveryId;
                    Item_deliveryQCBD.Item_ref = Item_delivery.Item_ref;
                    Item_deliveryQCBD.Quantity_delivery = Item_delivery.Quantity_delivery;

                    lock (_lock)
                    {
                        if (!idList.Contains(Item_deliveryQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(Item_deliveryQCBD);
                            idList.Add(Item_deliveryQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.item_deliveriesDataTable Item_deliveryTypeToDataTable(this List<Item_delivery> item_deliveriesList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (item_deliveriesList != null)
            {
                if (dataSet != null && dataSet.item_deliveries.Count > 0)
                {
                    foreach (var Item_delivery in item_deliveriesList)
                    {
                        QOBDSet.item_deliveriesRow Item_deliveryQCBD = dataSet.item_deliveries.Where(x => x.ID == Item_delivery.ID).First();
                        Item_deliveryQCBD.DeliveryId = Item_delivery.DeliveryId;
                        Item_deliveryQCBD.Item_ref = Item_delivery.Item_ref;
                        Item_deliveryQCBD.Quantity_delivery = Item_delivery.Quantity_delivery;
                    }
                }
            }
            return dataSet.item_deliveries;
        }

        public static List<Item_delivery> Item_deliveryTypeToFilterDataTable(this Item_delivery Item_delivery, ESearchOption filterOperator)
        {
            if (Item_delivery != null)
            {
                string baseSqlString = "SELECT * FROM Item_deliveries WHERE ";
                string defaultSqlString = "SELECT * FROM Item_deliveries WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Item_delivery.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Item_delivery.ID);
                if (Item_delivery.DeliveryId != 0)
                    query = string.Format(query + " {0} DeliveryId LIKE '{1}' ", filterOperator.ToString(), Item_delivery.DeliveryId);
                if (!string.IsNullOrEmpty(Item_delivery.Item_ref))
                    query = string.Format(query + " {0} Item_ref LIKE '{1}' ", filterOperator.ToString(), Item_delivery.Item_ref.Replace("'", "''"));
                if (Item_delivery.Quantity_delivery != 0)
                    query = string.Format(query + " {0} Quantity_delivery LIKE '{1}' ", filterOperator.ToString(), Item_delivery.Quantity_delivery);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToItem_delivery((QOBDSet.item_deliveriesDataTable)DALHelper.getDataTableFromSqlQuery<QOBDSet.item_deliveriesDataTable>(baseSqlString));

            }
            return new List<Item_delivery>();
        }

        //====================================================================================
        //===============================[ Tax_item ]===========================================
        //====================================================================================


        public static List<Tax_item> DataTableTypeToTax_item(this QOBDSet.tax_itemsDataTable Tax_itemDataTable)
        {
            object _lock = new object(); List<Tax_item> returnList = new List<Tax_item>();
            if (Tax_itemDataTable != null)
            {
                foreach (var Tax_itemQCBD in Tax_itemDataTable)
                {
                    Tax_item Tax_item = new Tax_item();
                    Tax_item.ID = Tax_itemQCBD.ID;
                    Tax_item.Item_ref = Tax_itemQCBD.Item_ref;
                    Tax_item.Tax_value = Tax_itemQCBD.Tax_value;
                    Tax_item.Tax_type = Tax_itemQCBD.Tax_type;
                    Tax_item.TaxId = Tax_itemQCBD.TaxId;

                    lock (_lock) returnList.Add(Tax_item);
                }
            }
            return returnList;
        }

        public static QOBDSet.tax_itemsDataTable Tax_itemTypeToDataTable(this List<Tax_item> Tax_itemList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QOBDSet.tax_itemsDataTable returnQCBDDataTable = new QOBDSet.tax_itemsDataTable();
            if (Tax_itemList != null)
            {
                foreach (var Tax_item in Tax_itemList)
                {
                    QOBDSet.tax_itemsRow Tax_itemQCBD = returnQCBDDataTable.Newtax_itemsRow();
                    Tax_itemQCBD.ID = Tax_item.ID;
                    Tax_itemQCBD.Item_ref = Tax_item.Item_ref;
                    Tax_itemQCBD.Tax_value = Tax_item.Tax_value;
                    Tax_itemQCBD.Tax_type = Tax_item.Tax_type;
                    Tax_itemQCBD.TaxId = Tax_item.TaxId;

                    lock (_lock)
                    {
                        if (!idList.Contains(Tax_itemQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(Tax_itemQCBD);
                            idList.Add(Tax_itemQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        // update the given dataset 
        public static QOBDSet.tax_itemsDataTable Tax_itemTypeToDataTable(this List<Tax_item> tax_itemsList, QOBDSet dataSet)
        {
            object _lock = new object();
            if (tax_itemsList != null)
            {
                if (dataSet != null && dataSet.tax_items.Count > 0)
                {
                    foreach (var Tax_item in tax_itemsList)
                    {
                        QOBDSet.tax_itemsRow Tax_itemQCBD = dataSet.tax_items.Where(x => x.ID == Tax_item.ID).First();
                        Tax_itemQCBD.Item_ref = Tax_item.Item_ref;
                        Tax_itemQCBD.Tax_value = Tax_item.Tax_value;
                        Tax_itemQCBD.Tax_type = Tax_item.Tax_type;
                        Tax_itemQCBD.TaxId = Tax_item.TaxId;
                    }
                }
            }
            return dataSet.tax_items;
        }

        public static List<Tax_item> Tax_itemTypeToFilterDataTable(this Tax_item Tax_item, ESearchOption filterOperator)
        {
            if (Tax_item != null)
            {
                string baseSqlString = "SELECT * FROM tax_items WHERE ";
                string defaultSqlString = "SELECT * FROM tax_items WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Tax_item.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Tax_item.ID);
                if (Tax_item.Tax_value != 0)
                    query = string.Format(query + " {0} Tax_value LIKE '{1}' ", filterOperator.ToString(), Tax_item.Tax_value);
                if (!string.IsNullOrEmpty(Tax_item.Item_ref))
                    query = string.Format(query + " {0} Item_ref LIKE '{1}' ", filterOperator.ToString(), Tax_item.Item_ref.Replace("'", "''"));
                if (Tax_item.TaxId != 0)
                    query = string.Format(query + " {0} TaxId LIKE '{1}' ", filterOperator.ToString(), Tax_item.TaxId);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToTax_item((QOBDSet.tax_itemsDataTable)DALHelper.getDataTableFromSqlQuery<QOBDSet.tax_itemsDataTable>(baseSqlString));

            }
            return new List<Tax_item>();
        }








    }
}
