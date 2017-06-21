using QOBDCommon.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using QOBDCommon.Classes;
using QOBDCommon.Enum;
using System.Data.SqlServerCe;

namespace QOBDDAL.Helper.ChannelHelper
{

    public static class DALHelper
    {
        public static string convertDateToStringFormat(string dateToConvert, string dateFormat)
        {
            string output = "0000-00-00 00:00:00";
            DateTime date = Utility.convertToDateTime(dateToConvert);
            if (date > Utility.DateTimeMinValueInSQL2005)
                return date.ToString(dateFormat);
            return output;
        }

        public static string getAllDataSqlText(this string tableName, Dictionary<string, string> columsDict)
        {
            string output = "SELECT ";

            // append column 
            foreach (var dictElement in columsDict)
                output += "[" + dictElement.Key + "], ";

            output += "FROM [" + tableName + "];";

            output = output.Replace(", [" + tableName, " [" + tableName).Replace(", FROM", " FROM");

            return output;
        }

        public static string getInsertSqlText(this string tableName, Dictionary<string, string> columsDict)
        {
            string output = "INSERT INTO [" + tableName + "] (";

            // append column 
            foreach (var dictElement in columsDict)
                output += "[" + dictElement.Key + "], ";

            output += ") VALUES ('";

            // append values
            foreach (var dictElement in columsDict)
                output += dictElement.Value.Replace("'", "''") + "', '";

            output += ");";
            output = output.Replace(", )", ")").Replace(", ')", ")");//.Replace("'", "''");

            return output;
        }

        public static string getUpdateSqlText(this string tableName, Dictionary<string, string> columsDict)
        {
            string output = "UPDATE [" + tableName + "] SET ";
            var IDDict = columsDict.Where(x => x.Key == "ID").SingleOrDefault();
            columsDict = columsDict.Where(x => x.Key != "ID").ToDictionary(x => x.Key, x => x.Value);

            // append column 
            foreach (var dictElement in columsDict)
                output += "[" + dictElement.Key + "] = '" + dictElement.Value.Replace("'", "''") + "', ";

            output += " WHERE [" + IDDict.Key + "] = '" + IDDict.Value + "';";
            output = output.Replace(",  WHERE", " WHERE");

            return output;
        }

        public static string getDeleteSqlText(this string tableName, Dictionary<string, string> columsDict)
        {
            string output = "DELETE FROM [" + tableName + "] ";
            var IDDict = columsDict.Where(x => x.Key == "ID").SingleOrDefault();

            output += " WHERE [" + IDDict.Key + "] = '" + IDDict.Value + "';";

            return output;
        }

        //====================================================================================
        //===============================[ Sql CE Commands ]=====================================
        //====================================================================================

        public static DataTable getDataTableFromSqlCEQuery(this string sql)
        {
            object _lock = new object();

            string _constr = System.Configuration.ConfigurationManager.ConnectionStrings["QCBDDatabaseCEConnectionString"].ConnectionString;
            _constr = _constr.Replace("|DataDirectory|", Utility.getDirectory("App_Data"));

            DataSet dataSet = new DataSet("QOBDData");
            DataTable dataTable = new DataTable("QOBDTable");

            using (SqlCeConnection connection = new SqlCeConnection(_constr))
            {
                using (SqlCeCommand cmd = new SqlCeCommand(sql, connection))
                {
                    try
                    {
                        cmd.Connection.Open();
                        cmd.CommandTimeout = 0;
                        dataTable.Load(cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection));
                    }
                    catch (Exception ex)
                    {
                        Log.error(ex.Message, EErrorFrom.HELPER);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Dispose();
                    }
                }
            }
            return dataTable;
        }


        //====================================================================================
        //===============================[ Auto_ref ]===========================================
        //====================================================================================

        public static List<Auto_ref> DataTableTypeToAuto_ref(this DataTable auto_refDataTable)
        {
            object _lock = new object(); List<Auto_ref> returnList = new List<Auto_ref>();

            for (int i = 0; i < auto_refDataTable.Rows.Count; i++)
            {
                Auto_ref auto_ref = new Auto_ref();
                auto_ref.ID = Utility.intTryParse(auto_refDataTable.Rows[i].ItemArray[auto_refDataTable.Columns["ID"].Ordinal].ToString());
                auto_ref.RefId = Utility.intTryParse(auto_refDataTable.Rows[i].ItemArray[auto_refDataTable.Columns["RefId"].Ordinal].ToString());

                lock (_lock) returnList.Add(auto_ref);
            }

            return returnList;
        }

        public static List<Auto_ref> filterDataTableToAuto_refType(this Auto_ref Auto_ref, ESearchOption filterOperator)
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

                return DataTableTypeToAuto_ref(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Auto_ref>();
        }

        public static Dictionary<string, string> getColumDictionary(this Auto_ref auto_ref)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = auto_ref.ID.ToString();
            output["RefId"] = auto_ref.RefId.ToString();

            return output;
        }
        //====================================================================================
        //===============================[ Currency ]===========================================
        //====================================================================================

        public static List<Currency> DataTableTypeToCurrency(this DataTable currencyDataTable)
        {
            object _lock = new object(); List<Currency> returnList = new List<Currency>();

            for (int i = 0; i < currencyDataTable.Rows.Count; i++)
            {
                Currency currency = new Currency();
                currency.ID = Utility.intTryParse(currencyDataTable.Rows[i].ItemArray[currencyDataTable.Columns["ID"].Ordinal].ToString());
                currency.IsDefault = (Utility.intTryParse(currencyDataTable.Rows[i].ItemArray[currencyDataTable.Columns["IsDefault"].Ordinal].ToString()) == 1)? true : false;
                currency.Name = (currencyDataTable.Rows[i].ItemArray[currencyDataTable.Columns["Name"].Ordinal] ?? "").ToString();
                currency.Rate = Utility.decimalTryParse(currencyDataTable.Rows[i].ItemArray[currencyDataTable.Columns["Rate"].Ordinal].ToString());
                currency.Symbol = (currencyDataTable.Rows[i].ItemArray[currencyDataTable.Columns["Symbol"].Ordinal] ?? "").ToString();
                currency.CountryCode = (currencyDataTable.Rows[i].ItemArray[currencyDataTable.Columns["Country_code"].Ordinal] ?? "").ToString();
                currency.CurrencyCode = (currencyDataTable.Rows[i].ItemArray[currencyDataTable.Columns["Currency_code"].Ordinal] ?? "").ToString();
                currency.Country = (currencyDataTable.Rows[i].ItemArray[currencyDataTable.Columns["Country"].Ordinal] ?? "").ToString();
                currency.Date = Utility.convertToDateTime(currencyDataTable.Rows[i].ItemArray[currencyDataTable.Columns["Date"].Ordinal].ToString());

                lock (_lock) returnList.Add(currency);
            }

            return returnList;
        }

        public static List<Currency> filterDataTableToCurrencyType(this Currency Currency, ESearchOption filterOperator)
        {
            if (Currency != null)
            {
                string baseSqlString = "SELECT * FROM Currencies WHERE ";
                string defaultSqlString = "SELECT * FROM Currencies WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Currency.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Currency.ID);
                if (!string.IsNullOrEmpty(Currency.CurrencyCode))
                    query = string.Format(query + " {0} Currency_code LIKE '{1}' ", filterOperator.ToString(), Currency.CurrencyCode);
                if (Currency.IsDefault)
                    query = string.Format(query + " {0} IsDefault LIKE '{1}' ", filterOperator.ToString(), Currency.IsDefault ? 1 : 0);
                if (!string.IsNullOrEmpty(Currency.Name))
                    query = string.Format(query + " {0} Name LIKE '{1}' ", filterOperator.ToString(), Currency.Name);
                if (!string.IsNullOrEmpty(Currency.Symbol))
                    query = string.Format(query + " {0} Symbol LIKE '{1}' ", filterOperator.ToString(), Currency.Symbol);
                if (!string.IsNullOrEmpty(Currency.CountryCode))
                    query = string.Format(query + " {0} Country_code LIKE '{1}' ", filterOperator.ToString(), Currency.CountryCode);
                if (!string.IsNullOrEmpty(Currency.Country))
                    query = string.Format(query + " {0} Country LIKE '{1}' ", filterOperator.ToString(), Currency.Country);
                if (Currency.Rate != 0)
                    query = string.Format(query + " {0} Rate LIKE '{1}' ", filterOperator.ToString(), Currency.Rate);
                
                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToCurrency(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Currency>();
        }

        public static Dictionary<string, string> getColumDictionary(this Currency currency)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = currency.ID.ToString();
            output["IsDefault"] = (currency.IsDefault) ? "1" : "0";
            output["Date"] = currency.Date.ToString("yyyy-MM-dd H:mm:ss");
            output["Rate"] = currency.Rate.ToString();
            output["Symbol"] = (currency.Symbol ?? "").ToString();
            output["Name"] = (currency.Name ?? "").ToString();
            output["Country_code"] = (currency.CountryCode ?? "").ToString();
            output["Currency_code"] = (currency.CurrencyCode ?? "").ToString();
            output["Country"] = (currency.Country ?? "").ToString();

            return output;
        }


        //====================================================================================
        //===============================[ Notification ]===========================================
        //====================================================================================

        public static List<Notification> DataTableTypeToNotification(this DataTable NotificationDataTable)
        {
            object _lock = new object(); List<Notification> returnList = new List<Notification>();

            for (int i = 0; i < NotificationDataTable.Rows.Count; i++)
            {
                Notification notification = new Notification();
                notification.ID = Utility.intTryParse(NotificationDataTable.Rows[i].ItemArray[NotificationDataTable.Columns["ID"].Ordinal].ToString());
                notification.BillId = Utility.intTryParse(NotificationDataTable.Rows[i].ItemArray[NotificationDataTable.Columns["BillId"].Ordinal].ToString());
                notification.Reminder1 = Utility.convertToDateTime(NotificationDataTable.Rows[i].ItemArray[NotificationDataTable.Columns["Reminder1"].Ordinal].ToString());
                notification.Reminder2 = Utility.convertToDateTime(NotificationDataTable.Rows[i].ItemArray[NotificationDataTable.Columns["Reminder2"].Ordinal].ToString());

                lock (_lock) returnList.Add(notification);
            }

            return returnList;
        }

        public static List<Notification> filterDataTableToNotificationType(this Notification Notification, ESearchOption filterOperator)
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

                return DataTableTypeToNotification(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Notification>();
        }

        public static Dictionary<string, string> getColumDictionary(this Notification notification)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = notification.ID.ToString();
            output["BillId"] = notification.BillId.ToString();
            output["Reminder1"] = convertDateToStringFormat(notification.Reminder1.ToString(), "yyyy-MM-dd H:mm:ss");
            output["Reminder2"] = convertDateToStringFormat(notification.Reminder2.ToString(), "yyyy-MM-dd H:mm:ss");

            return output;
        }

        //====================================================================================
        //===============================[ Statistic ]===========================================
        //====================================================================================

        public static List<Statistic> DataTableTypeToStatistic(this DataTable statisticDataTable)
        {
            object _lock = new object(); List<Statistic> returnList = new List<Statistic>();
            for (int i = 0; i < statisticDataTable.Rows.Count; i++)
            {
                Statistic statistic = new Statistic();
                statistic.ID = Utility.intTryParse(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["ID"].Ordinal].ToString());
                statistic.BillId = Utility.intTryParse(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["BillId"].Ordinal].ToString());
                statistic.Company = (statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Company"].Ordinal] ?? "").ToString();
                statistic.Income = Utility.decimalTryParse(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Income"].Ordinal].ToString());
                statistic.Income_percent = Utility.doubleTryParse(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Income_percent"].Ordinal].ToString());
                statistic.Pay_received = Utility.decimalTryParse(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Pay_received"].Ordinal].ToString());
                statistic.Price_purchase_total = Utility.decimalTryParse(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Price_purchase_total"].Ordinal].ToString());
                statistic.Tax_value = Utility.doubleTryParse(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Tax_value"].Ordinal].ToString());
                statistic.Total = Utility.decimalTryParse(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Total"].Ordinal].ToString());
                statistic.Total_tax_included = Utility.decimalTryParse(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Total_tax_included"].Ordinal].ToString());
                statistic.Bill_datetime = Utility.convertToDateTime(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Bill_datetime"].Ordinal].ToString());
                statistic.Date_limit = Utility.convertToDateTime(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Date_limit"].Ordinal].ToString());
                statistic.Pay_date = Utility.convertToDateTime(statisticDataTable.Rows[i].ItemArray[statisticDataTable.Columns["Pay_date"].Ordinal].ToString());

                lock (_lock) returnList.Add(statistic);
            }

            return returnList;
        }

        public static List<Statistic> filterDataTableToStatisticType(this Statistic Statistic, ESearchOption filterOperator)
        {
            if (Statistic != null)
            {
                string baseSqlString = "SELECT * FROM [Statistics] WHERE ";
                string defaultSqlString = "SELECT * FROM [Statistics] WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Statistic.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Statistic.ID);
                if (Statistic.BillId != 0)
                    query = string.Format(query + " {0} BillId LIKE '{1}' ", filterOperator.ToString(), Statistic.BillId);
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

                return DataTableTypeToStatistic(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Statistic>();
        }

        public static Dictionary<string, string> getColumDictionary(this Statistic statistic)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = statistic.ID.ToString();
            output["BillId"] = statistic.BillId.ToString();
            output["Bill_datetime"] = convertDateToStringFormat(statistic.Bill_datetime.ToString(), "yyyy-MM-dd H:mm:ss");
            output["Company"] = (statistic.Company ?? "").ToString();
            output["Date_limit"] = convertDateToStringFormat(statistic.Date_limit.ToString(), "yyyy-MM-dd H:mm:ss");
            output["Income"] = statistic.Income.ToString();
            output["Income_percent"] = statistic.Income_percent.ToString();
            output["Pay_datetime"] = convertDateToStringFormat(statistic.Pay_date.ToString(), "yyyy-MM-dd H:mm:ss");
            output["Pay_received"] = statistic.Pay_received.ToString();
            output["Price_purchase_total"] = statistic.Price_purchase_total.ToString();
            output["Tax_value"] = statistic.Tax_value.ToString();
            output["Total"] = statistic.Total.ToString();
            output["Total_tax_included"] = statistic.Total_tax_included.ToString();

            return output;
        }

        //====================================================================================
        //===============================[ Infos ]===========================================
        //====================================================================================

        public static List<Info> DataTableTypeToInfos(this DataTable InfosDataTable)
        {
            object _lock = new object(); List<Info> returnList = new List<Info>();
            for (int i = 0; i < InfosDataTable.Rows.Count; i++)
            {
                Info infos = new Info();
                infos.ID = Utility.intTryParse(InfosDataTable.Rows[i].ItemArray[InfosDataTable.Columns["ID"].Ordinal].ToString());
                infos.Name = (InfosDataTable.Rows[i].ItemArray[InfosDataTable.Columns["Name"].Ordinal] ?? "").ToString();
                infos.Value = (InfosDataTable.Rows[i].ItemArray[InfosDataTable.Columns["Value"].Ordinal] ?? "").ToString();

                lock (_lock) returnList.Add(infos);
            }
            return returnList;
        }

        public static List<Info> filterDataTableToInfoType(this Info Info, ESearchOption filterOperator)
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

                return DataTableTypeToInfos(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Info>();
        }

        public static Dictionary<string, string> getColumDictionary(this Info info)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = info.ID.ToString();
            output["Name"] = (info.Name ?? "").ToString();
            output["Value"] = (info.Value ?? "").ToString();

            return output;
        }

        //====================================================================================
        //===============================[ Agent ]===========================================
        //====================================================================================

        public static List<Agent> DataTableTypeToAgent(this DataTable agentDataTable)
        {
            object _lock = new object(); List<Agent> returnList = new List<Agent>();
            for (int i = 0; i < agentDataTable.Rows.Count; i++)
            {
                Agent agent = new Agent();
                agent.ID = Utility.intTryParse(agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["ID"].Ordinal].ToString());
                agent.FirstName = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["FirstName"].Ordinal] ?? "").ToString();
                agent.LastName = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["LastName"].Ordinal] ?? "").ToString();
                agent.UserName = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["Login"].Ordinal] ?? "").ToString();
                agent.HashedPassword = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["Password"].Ordinal] ?? "").ToString();
                agent.Picture = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["Picture"].Ordinal] ?? "").ToString();
                agent.Phone = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["Phone"].Ordinal] ?? "").ToString();
                agent.Status = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["Status"].Ordinal] ?? "").ToString();
                agent.IPAddress = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["IPAddress"].Ordinal] ?? "").ToString();
                agent.Admin = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["Admin"].Ordinal] ?? "").ToString();
                agent.Email = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["Email"].Ordinal] ?? "").ToString();
                agent.Fax = (agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["Fax"].Ordinal] ?? "").ToString();
                agent.ListSize = Utility.intTryParse(agentDataTable.Rows[i].ItemArray[agentDataTable.Columns["ListSize"].Ordinal].ToString());

                lock (_lock) returnList.Add(agent);
            }

            return returnList;
        }

        public static List<Agent> filterDataTableToAgentType(this Agent agent, ESearchOption filterOperator)
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
                if (!string.IsNullOrEmpty(agent.Picture))
                    query = string.Format(query + " {0} Picture LIKE '{1}' ", filterOperator.ToString(), agent.Picture.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Admin))
                    query = string.Format(query + " {0} Admin LIKE '{1}' ", filterOperator.ToString(), agent.Admin.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Status))
                    query = string.Format(query + " {0} Status LIKE '{1}' ", filterOperator.ToString(), agent.Status.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.IPAddress))
                    query = string.Format(query + " {0} IPAddress LIKE '{1}' ", filterOperator.ToString(), agent.IPAddress);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToAgent(baseSqlString.getDataTableFromSqlCEQuery());
            }
            return new List<Agent>();
        }

        public static Dictionary<string, string> getColumDictionary(this Agent agent)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = agent.ID.ToString();
            output["FirstName"] = (agent.FirstName ?? "").ToString();
            output["LastName"] = (agent.LastName ?? "").ToString();
            output["Login"] = (agent.UserName ?? "").ToString();
            output["Password"] = (agent.HashedPassword ?? "").ToString();
            output["Picture"] = (agent.Picture ?? "").ToString();
            output["Phone"] = (agent.Phone ?? "").ToString();
            output["Status"] = (agent.Status ?? "").ToString();
            output["IPAddress"] = (agent.IPAddress ?? "").ToString();
            output["Admin"] = (agent.Admin ?? "").ToString();
            output["Email"] = (agent.Email ?? "").ToString();
            output["Fax"] = (agent.Fax ?? "").ToString();
            output["ListSize"] = agent.ListSize.ToString();

            return output;
        }


        //====================================================================================
        //===============================[ Order ]===========================================
        //====================================================================================

        public static List<Order> DataTableTypeToOrder(this DataTable OrderDataTable)
        {
            object _lock = new object(); List<Order> returnList = new List<Order>();

            for (int i = 0; i < OrderDataTable.Rows.Count; i++)
            {
                Order Order = new Order();
                Order.ID = Utility.intTryParse(OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["ID"].Ordinal].ToString());
                Order.AgentId = Utility.intTryParse(OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["AgentId"].Ordinal].ToString());
                Order.BillAddress = Utility.intTryParse(OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["BillAddress"].Ordinal].ToString());
                Order.ClientId = Utility.intTryParse(OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["ClientId"].Ordinal].ToString());
                Order.CurrencyId = Utility.intTryParse(OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["CurrencyId"].Ordinal].ToString());
                Order.Comment1 = (OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["Comment1"].Ordinal] ?? "").ToString();
                Order.Comment2 = (OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["Comment2"].Ordinal] ?? "").ToString();
                Order.Comment3 = (OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["Comment3"].Ordinal] ?? "").ToString();
                Order.Status = (OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["Status"].Ordinal] ?? "").ToString();
                Order.Date = Utility.convertToDateTime(OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["Date"].Ordinal].ToString());
                Order.DeliveryAddress = Utility.intTryParse(OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["DeliveryAddress"].Ordinal].ToString());
                Order.Tax = Utility.decimalTryParse(OrderDataTable.Rows[i].ItemArray[OrderDataTable.Columns["Tax"].Ordinal].ToString());

                lock (_lock) returnList.Add(Order);
            }
            return returnList;
        }

        public static List<Order> filterDataTableToOrderType(this Order order, ESearchOption filterOperator)
        {
            string baseSqlString = "SELECT * FROM Orders WHERE ";
            string defaultSqlString = "SELECT * FROM Orders WHERE 1=0 ";
            string orderBy = " ORDER BY ID DESC";
            object _lock = new object(); string query = "";

            if (order != null)
            {
                if (order.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), order.ID);
                if (order.AgentId != 0)
                    query = string.Format(query + " {0} AgentId LIKE '{1}' ", filterOperator.ToString(), order.AgentId);
                if (order.CurrencyId != 0)
                    query = string.Format(query + " {0} CurrencyId LIKE '{1}' ", filterOperator.ToString(), order.CurrencyId);
                if (!string.IsNullOrEmpty(order.Comment1))
                    query = string.Format(query + " {0} Comment1 LIKE '{1}' ", filterOperator.ToString(), order.Comment1.Replace("'", "''"));
                if (!string.IsNullOrEmpty(order.Comment2))
                    query = string.Format(query + " {0} Comment2 LIKE '{1}' ", filterOperator.ToString(), order.Comment2.Replace("'", "''"));
                if (!string.IsNullOrEmpty(order.Comment3))
                    query = string.Format(query + " {0} Comment3 LIKE '{1}' ", filterOperator.ToString(), order.Comment3.Replace("'", "''"));
                if (!string.IsNullOrEmpty(order.Status))
                    query = string.Format(query + " {0} Status LIKE '{1}' ", filterOperator.ToString(), order.Status.Replace("'", "''"));
                if (order.Tax != 0)
                    query = string.Format(query + " {0} Tax LIKE '{1}' ", filterOperator.ToString(), order.Tax);
                if (order.ClientId != 0)
                    query = string.Format(query + " {0} ClientId LIKE '{1}' ", filterOperator.ToString(), order.ClientId);
                if (order.BillAddress != 0)
                    query = string.Format(query + " {0} BillAddress LIKE '{1}' ", filterOperator.ToString(), order.BillAddress);
                if (order.DeliveryAddress != 0)
                    query = string.Format(query + " {0} DeliveryAddress LIKE '{1}' ", filterOperator.ToString(), order.DeliveryAddress);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length) + orderBy;
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToOrder(baseSqlString.getDataTableFromSqlCEQuery());
            }
            return new List<Order>();
        }

        public static Dictionary<string, string> getColumDictionary(this Order order)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["AgentId"] = order.AgentId.ToString();
            output["BillAddress"] = order.BillAddress.ToString();
            output["ClientId"] = order.ClientId.ToString();
            output["CurrencyId"] = order.CurrencyId.ToString();
            output["Comment1"] = (order.Comment1 ?? "").ToString();
            output["Comment2"] = (order.Comment2 ?? "").ToString();
            output["Comment3"] = (order.Comment3 ?? "").ToString();
            output["Date"] = convertDateToStringFormat(order.Date.ToString(), "yyyy-MM-dd H:mm:ss");
            output["DeliveryAddress"] = order.DeliveryAddress.ToString();
            output["ID"] = order.ID.ToString();
            output["Status"] = (order.Status ?? "").ToString();
            output["Tax"] = order.Tax.ToString();

            return output;
        }


        //====================================================================================
        //===============================[ Tax_order ]======================================
        //====================================================================================

        public static List<Tax_order> DataTableTypeToTax_order(this DataTable Tax_OrderDataTableList)
        {
            object _lock = new object(); List<Tax_order> returnList = new List<Tax_order>();

            for (int i = 0; i < Tax_OrderDataTableList.Rows.Count; i++)
            {
                Tax_order Tax_order = new Tax_order();
                Tax_order.ID = Utility.intTryParse(Tax_OrderDataTableList.Rows[i].ItemArray[Tax_OrderDataTableList.Columns["ID"].Ordinal].ToString());
                Tax_order.OrderId = Utility.intTryParse(Tax_OrderDataTableList.Rows[i].ItemArray[Tax_OrderDataTableList.Columns["OrderId"].Ordinal].ToString());
                Tax_order.Target = Tax_OrderDataTableList.Rows[i].ItemArray[Tax_OrderDataTableList.Columns["Target"].Ordinal].ToString();
                Tax_order.Tax_value = Utility.doubleTryParse(Tax_OrderDataTableList.Rows[i].ItemArray[Tax_OrderDataTableList.Columns["Tax_value"].Ordinal].ToString());
                Tax_order.TaxId = Utility.intTryParse(Tax_OrderDataTableList.Rows[i].ItemArray[Tax_OrderDataTableList.Columns["TaxId"].Ordinal].ToString());
                Tax_order.Date_insert = Utility.convertToDateTime(Tax_OrderDataTableList.Rows[i].ItemArray[Tax_OrderDataTableList.Columns["Date_insert"].Ordinal].ToString());

                lock (_lock) returnList.Add(Tax_order);
            }

            return returnList;
        }

        public static List<Tax_order> filterDataTableToTax_orderType(this Tax_order Tax_order, ESearchOption filterOperator)
        {
            string baseSqlString = "SELECT * FROM Tax_orders WHERE ";
            string defaultSqlString = "SELECT * FROM Tax_orders WHERE 1=0 ";
            object _lock = new object(); string query = "";

            if (Tax_order != null)
            {
                if (Tax_order.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Tax_order.ID);
                if (Tax_order.OrderId != 0)
                    query = string.Format(query + " {0} OrderId LIKE '{1}' ", filterOperator.ToString(), Tax_order.OrderId);
                if (Tax_order.TaxId != 0)
                    query = string.Format(query + " {0} TaxId LIKE '{1}' ", filterOperator.ToString(), Tax_order.TaxId);
                if (Tax_order.Tax_value != 0)
                    query = string.Format(query + " {0} Tax_value LIKE '{1}' ", filterOperator.ToString(), Tax_order.Tax_value);
                if (!string.IsNullOrEmpty(Tax_order.Target))
                    query = string.Format(query + " {0} Target LIKE '{1}' ", filterOperator.ToString(), Tax_order.Target);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToTax_order(baseSqlString.getDataTableFromSqlCEQuery());

            }

            return new List<Tax_order>();
        }

        public static Dictionary<string, string> getColumDictionary(this Tax_order tax_order)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["OrderId"] = tax_order.OrderId.ToString();
            output["Date_insert"] = convertDateToStringFormat(tax_order.Date_insert.ToString(), "yyyy-MM-dd H:mm:ss");
            output["Target"] = (tax_order.Target ?? "").ToString();
            output["Tax_value"] = tax_order.Tax_value.ToString();
            output["TaxId"] = tax_order.TaxId.ToString();
            output["ID"] = tax_order.ID.ToString();

            return output;
        }

        //====================================================================================
        //===============================[ Client ]===========================================
        //====================================================================================

        public static List<Client> DataTableTypeToClient(this DataTable clientDataTable)
        {
            object _lock = new object(); List<Client> returnList = new List<Client>();
            if (clientDataTable != null)
            {
                for (int i = 0; i < clientDataTable.Rows.Count; i++)
                {
                    Client client = new Client();
                    client.ID = Utility.intTryParse(clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["ID"].Ordinal].ToString());
                    client.AgentId = Utility.intTryParse(clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["AgentId"].Ordinal].ToString());
                    client.FirstName = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["FirstName"].Ordinal] ?? "").ToString();
                    client.LastName = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["LastName"].Ordinal] ?? "").ToString();
                    client.Comment = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["Comment"].Ordinal] ?? "").ToString();
                    client.Phone = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["Phone"].Ordinal] ?? "").ToString();
                    client.Status = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["Status"].Ordinal] ?? "").ToString();
                    client.Company = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["Company"].Ordinal] ?? "").ToString();
                    client.Email = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["Email"].Ordinal] ?? "").ToString();
                    client.Fax = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["Fax"].Ordinal] ?? "").ToString();
                    client.CompanyName = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["CompanyName"].Ordinal] ?? "").ToString();
                    client.CRN = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["CRN"].Ordinal] ?? "").ToString();
                    client.MaxCredit = Utility.intTryParse(clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["MaxCredit"].Ordinal].ToString());
                    client.Rib = (clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["Rib"].Ordinal] ?? "").ToString();
                    client.PayDelay = Utility.intTryParse(clientDataTable.Rows[i].ItemArray[clientDataTable.Columns["PayDelay"].Ordinal].ToString());

                    lock (_lock) returnList.Add(client);
                }
            }
            return returnList;
        }

        public static List<Client> filterDataTableToClientType(this Client client, ESearchOption filterOperator)
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

                return DataTableTypeToClient(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Client>();
        }

        public static Dictionary<string, string> getColumDictionary(this Client client)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = client.ID.ToString();
            output["AgentId"] = client.AgentId.ToString();
            output["FirstName"] = (client.FirstName ?? "").ToString();
            output["LastName"] = (client.LastName ?? "").ToString();
            output["Comment"] = (client.Comment ?? "").ToString();
            output["Phone"] = (client.Phone ?? "").ToString();
            output["Status"] = (client.Status ?? "").ToString();
            output["Company"] = (client.Company ?? "").ToString();
            output["Email"] = (client.Email ?? "").ToString();
            output["Fax"] = (client.Fax ?? "").ToString();
            output["CompanyName"] = (client.CompanyName ?? "").ToString();
            output["CRN"] = (client.CRN ?? "").ToString();
            output["MaxCredit"] = client.MaxCredit.ToString();
            output["Rib"] = (client.Rib ?? "").ToString();
            output["PayDelay"] = client.PayDelay.ToString();

            return output;
        }

        //====================================================================================
        //===============================[ Contact ]===========================================
        //====================================================================================

        public static List<Contact> DataTableTypeToContact(this DataTable contactDataTable)
        {
            object _lock = new object(); List<Contact> returnList = new List<Contact>();
            if (contactDataTable != null)
            {
                for (int i = 0; i < contactDataTable.Rows.Count; i++)
                {
                    Contact contact = new Contact();
                    contact.ID = Utility.intTryParse(contactDataTable.Rows[i].ItemArray[contactDataTable.Columns["ID"].Ordinal].ToString());
                    contact.Cellphone = (contactDataTable.Rows[i].ItemArray[contactDataTable.Columns["Cellphone"].Ordinal] ?? "").ToString();
                    contact.ClientId = Utility.intTryParse(contactDataTable.Rows[i].ItemArray[contactDataTable.Columns["ClientId"].Ordinal].ToString());
                    contact.LastName = (contactDataTable.Rows[i].ItemArray[contactDataTable.Columns["LastName"].Ordinal] ?? "").ToString();
                    contact.Comment = (contactDataTable.Rows[i].ItemArray[contactDataTable.Columns["Comment"].Ordinal] ?? "").ToString();
                    contact.Phone = (contactDataTable.Rows[i].ItemArray[contactDataTable.Columns["Phone"].Ordinal] ?? "").ToString();
                    contact.Firstname = (contactDataTable.Rows[i].ItemArray[contactDataTable.Columns["Firstname"].Ordinal] ?? "").ToString();
                    contact.Position = (contactDataTable.Rows[i].ItemArray[contactDataTable.Columns["Position"].Ordinal] ?? "").ToString();
                    contact.Email = (contactDataTable.Rows[i].ItemArray[contactDataTable.Columns["Email"].Ordinal] ?? "").ToString();
                    contact.Fax = (contactDataTable.Rows[i].ItemArray[contactDataTable.Columns["Fax"].Ordinal] ?? "").ToString();

                    lock (_lock) returnList.Add(contact);
                }
            }
            return returnList;
        }

        public static List<Contact> filterDataTableToContactType(this Contact Contact, ESearchOption filterOperator)
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

                return DataTableTypeToContact(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Contact>();
        }

        public static Dictionary<string, string> getColumDictionary(this Contact contact)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = contact.ID.ToString();
            output["Position"] = (contact.Position ?? "").ToString();
            output["ClientId"] = contact.ClientId.ToString();
            output["LastName"] = (contact.LastName ?? "").ToString();
            output["Comment"] = (contact.Comment ?? "").ToString();
            output["Phone"] = (contact.Phone ?? "").ToString();
            output["Cellphone"] = (contact.Cellphone ?? "").ToString();
            output["Firstname"] = (contact.Firstname ?? "").ToString();
            output["Email"] = (contact.Email ?? "").ToString();
            output["Fax"] = (contact.Fax ?? "").ToString();

            return output;
        }



        //====================================================================================
        //===============================[ Address ]===========================================
        //====================================================================================

        public static List<Address> DataTableTypeToAddress(this DataTable addressesDataTable)
        {
            object _lock = new object(); List<Address> returnList = new List<Address>();
            if (addressesDataTable != null)
            {
                for (int i = 0; i < addressesDataTable.Rows.Count; i++)
                {
                    Address address = new Address();
                    address.ID = (int)addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["ID"].Ordinal];
                    address.AddressName = addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["Address"].Ordinal].ToString();
                    address.ClientId = Utility.intTryParse(addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["ClientId"].Ordinal].ToString());
                    address.Comment = (addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["Comment"].Ordinal] ?? "").ToString();
                    address.Email = (addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["Email"].Ordinal] ?? "").ToString();
                    address.Phone = (addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["Phone"].Ordinal] ?? "").ToString();
                    address.CityName = (addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["CityName"].Ordinal] ?? "").ToString();
                    address.Country = (addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["Country"].Ordinal] ?? "").ToString();
                    address.LastName = (addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["LastName"].Ordinal] ?? "").ToString();
                    address.FirstName = (addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["FirstName"].Ordinal] ?? "").ToString();
                    address.Name = (addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["Name"].Ordinal] ?? "").ToString();
                    address.Name2 = (addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["Name2"].Ordinal] ?? "").ToString();
                    address.Postcode = (addressesDataTable.Rows[i].ItemArray[addressesDataTable.Columns["Postcode"].Ordinal] ?? "").ToString();

                    lock (_lock) returnList.Add(address);
                }
            }
            return returnList;
        }

        public static List<Address> filterDataTableToAddressType(this Address Address, ESearchOption filterOperator)
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

                return DataTableTypeToAddress(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Address>();
        }

        public static Dictionary<string, string> getColumDictionary(this Address address)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = address.ID.ToString();
            output["Address"] = (address.AddressName ?? "").ToString();
            output["ClientId"] = address.ClientId.ToString();
            output["LastName"] = (address.LastName ?? "").ToString();
            output["Comment"] = (address.Comment ?? "").ToString();
            output["Phone"] = (address.Phone ?? "").ToString();
            output["ClientId"] = address.ClientId.ToString();
            output["FirstName"] = (address.FirstName ?? "").ToString();
            output["Email"] = (address.Email ?? "").ToString();
            output["CityName"] = (address.CityName ?? "").ToString();
            output["Country"] = (address.Country ?? "").ToString();
            output["Name"] = (address.Name ?? "").ToString();
            output["Name2"] = (address.Name2 ?? "").ToString();
            output["Postcode"] = (address.Postcode ?? "").ToString();

            return output;
        }


        //====================================================================================
        //===============================[ Bill ]===========================================
        //====================================================================================

        public static List<Bill> DataTableTypeToBill(this DataTable BillDataTable)
        {
            object _lock = new object(); List<Bill> returnList = new List<Bill>();
            if (BillDataTable != null)
            {
                for (int i = 0; i < BillDataTable.Rows.Count; i++)
                {
                    Bill bill = new Bill();
                    bill.ID = Utility.intTryParse(BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["ID"].Ordinal].ToString());
                    bill.OrderId = Utility.intTryParse(BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["OrderId"].Ordinal].ToString());
                    bill.ClientId = Utility.intTryParse(BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["ClientId"].Ordinal].ToString());
                    bill.Date = Utility.convertToDateTime(BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["Date"].Ordinal].ToString());
                    bill.DatePay = Utility.convertToDateTime(BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["DatePay"].Ordinal].ToString());
                    bill.DateLimit = Utility.convertToDateTime(BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["DateLimit"].Ordinal].ToString());
                    bill.Pay = Utility.decimalTryParse(BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["Pay"].Ordinal].ToString());
                    bill.Comment1 = (BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["Comment1"].Ordinal] ?? "").ToString();
                    bill.Comment2 = (BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["Comment2"].Ordinal] ?? "").ToString();
                    bill.PayMod = BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["PayMod"].Ordinal].ToString();
                    bill.PayReceived = Utility.decimalTryParse(BillDataTable.Rows[i].ItemArray[BillDataTable.Columns["PayReceived"].Ordinal].ToString());

                    lock (_lock) returnList.Add(bill);
                }
            }
            return returnList;
        }

        public static List<Bill> filterDataTableToBillType(this Bill Bill, ESearchOption filterOperator)
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
                    query = string.Format(query + " {0} OrderId LIKE '{1}' ", filterOperator.ToString(), Bill.OrderId);
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

                return DataTableTypeToBill(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Bill>();
        }

        public static Bill LastBill()
        {
            string baseSqlString = "SELECT TOP 1 * FROM Bills ORDER BY ID DESC ";

            var FoundList = DataTableTypeToBill(baseSqlString.getDataTableFromSqlCEQuery());
            if (FoundList.Count > 0)
                return FoundList[0];

            return new Bill();
        }

        public static Dictionary<string, string> getColumDictionary(this Bill bill)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = bill.ID.ToString();
            output["OrderId"] = bill.OrderId.ToString();
            output["ClientId"] = bill.ClientId.ToString();
            output["Comment1"] = (bill.Comment1 ?? "").ToString();
            output["Comment2"] = (bill.Comment2 ?? "").ToString();
            output["Pay"] = bill.Pay.ToString();
            output["Date"] = convertDateToStringFormat(bill.Date.ToString(), "yyyy-MM-dd H:mm:ss");
            output["DatePay"] = convertDateToStringFormat(bill.DatePay.ToString(), "yyyy-MM-dd H:mm:ss");
            output["DateLimit"] = convertDateToStringFormat(bill.DateLimit.ToString(), "yyyy-MM-dd H:mm:ss");
            output["PayMod"] = (bill.PayMod ?? "").ToString();
            output["PayReceived"] = bill.PayReceived.ToString();

            return output;
        }

        //====================================================================================
        //===============================[ Delivery ]===========================================
        //====================================================================================

        public static List<Delivery> DataTableTypeToDelivery(this DataTable DeliveryDataTable)
        {
            object _lock = new object(); List<Delivery> returnList = new List<Delivery>();
            if (DeliveryDataTable != null)
            {
                for (int i = 0; i < DeliveryDataTable.Rows.Count; i++)
                {
                    Delivery delivery = new Delivery();
                    delivery.ID = Utility.intTryParse(DeliveryDataTable.Rows[i].ItemArray[DeliveryDataTable.Columns["ID"].Ordinal].ToString());
                    delivery.OrderId = Utility.intTryParse(DeliveryDataTable.Rows[i].ItemArray[DeliveryDataTable.Columns["OrderId"].Ordinal].ToString());
                    delivery.BillId = Utility.intTryParse(DeliveryDataTable.Rows[i].ItemArray[DeliveryDataTable.Columns["BillId"].Ordinal].ToString());
                    delivery.Date = Utility.convertToDateTime(DeliveryDataTable.Rows[i].ItemArray[DeliveryDataTable.Columns["Date"].Ordinal].ToString());
                    delivery.Package = Utility.intTryParse(DeliveryDataTable.Rows[i].ItemArray[DeliveryDataTable.Columns["Package"].Ordinal].ToString());
                    delivery.Status = (DeliveryDataTable.Rows[i].ItemArray[DeliveryDataTable.Columns["Status"].Ordinal] ?? "").ToString();

                    lock (_lock) returnList.Add(delivery);
                }
            }
            return returnList;
        }

        public static List<Delivery> filterDataTableToDeliveryType(this Delivery Delivery, ESearchOption filterOperator)
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
                    query = string.Format(query + " {0} OrderId LIKE '{1}' ", filterOperator.ToString(), Delivery.OrderId);
                if (Delivery.BillId != 0)
                    query = string.Format(query + " {0} BillId LIKE '{1}' ", filterOperator.ToString(), Delivery.BillId);
                if (Delivery.Package != 0)
                    query = string.Format(query + " {0} Package LIKE '{1}' ", filterOperator.ToString(), Delivery.Package);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToDelivery(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Delivery>();
        }

        public static Dictionary<string, string> getColumDictionary(this Delivery delivery)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = delivery.ID.ToString();
            output["OrderId"] = delivery.OrderId.ToString();
            output["BillId"] = delivery.BillId.ToString();
            output["Date"] = convertDateToStringFormat(delivery.Date.ToString(), "yyyy-MM-dd H:mm:ss");
            output["Package"] = delivery.Package.ToString();
            output["Status"] = (delivery.Status ?? "").ToString();

            return output;
        }

        //====================================================================================
        //================================[ Order_item ]====================================
        //====================================================================================

        public static List<Order_item> DataTableTypeToOrder_item(this DataTable Order_itemDataTable)
        {
            object _lock = new object(); List<Order_item> returnList = new List<Order_item>();
            if (Order_itemDataTable != null)
            {
                for (int i = 0; i < Order_itemDataTable.Rows.Count; i++)
                {
                    Order_item Order_item = new Order_item();
                    Order_item.ID = Utility.intTryParse(Order_itemDataTable.Rows[i].ItemArray[Order_itemDataTable.Columns["ID"].Ordinal].ToString());
                    Order_item.OrderId = Utility.intTryParse(Order_itemDataTable.Rows[i].ItemArray[Order_itemDataTable.Columns["OrderId"].Ordinal].ToString());
                    Order_item.Comment_Purchase_Price = (Order_itemDataTable.Rows[i].ItemArray[Order_itemDataTable.Columns["Comment_Purchase_Price"].Ordinal] ?? "").ToString();
                    Order_item.Item_ref = (Order_itemDataTable.Rows[i].ItemArray[Order_itemDataTable.Columns["Item_ref"].Ordinal] ?? "").ToString();
                    Order_item.Rank = Utility.intTryParse(Order_itemDataTable.Rows[i].ItemArray[Order_itemDataTable.Columns["Rank"].Ordinal].ToString());
                    Order_item.Price = Utility.decimalTryParse(Order_itemDataTable.Rows[i].ItemArray[Order_itemDataTable.Columns["Price"].Ordinal].ToString());
                    Order_item.Price_purchase = Utility.decimalTryParse(Order_itemDataTable.Rows[i].ItemArray[Order_itemDataTable.Columns["Price_purchase"].Ordinal].ToString());
                    Order_item.Quantity = Utility.intTryParse(Order_itemDataTable.Rows[i].ItemArray[Order_itemDataTable.Columns["Quantity"].Ordinal].ToString());
                    Order_item.Quantity_current = Utility.intTryParse(Order_itemDataTable.Rows[i].ItemArray[Order_itemDataTable.Columns["Quantity_current"].Ordinal].ToString());
                    Order_item.Quantity_delivery = Utility.intTryParse(Order_itemDataTable.Rows[i].ItemArray[Order_itemDataTable.Columns["Quantity_delivery"].Ordinal].ToString());

                    lock (_lock) returnList.Add(Order_item);
                }
            }
            var test = returnList.Where(x => x.OrderId == 3410).ToList();
            return returnList;
        }

        public static List<Order_item> filterDataTableToOrder_itemType(this Order_item Order_item, ESearchOption filterOperator)
        {
            if (Order_item != null)
            {
                string baseSqlString = "SELECT * FROM Order_items WHERE ";
                string defaultSqlString = "SELECT * FROM Order_items WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Order_item.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Order_item.ID);
                if (Order_item.OrderId != 0)
                    query = string.Format(query + " {0} OrderId LIKE '{1}' ", filterOperator.ToString(), Order_item.OrderId);
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
                    query = string.Format(query + " {0} [Rank] LIKE '{1}' ", filterOperator.ToString(), Order_item.Rank);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToOrder_item(baseSqlString.getDataTableFromSqlCEQuery());

            }

            return new List<Order_item>();
        }

        public static Dictionary<string, string> getColumDictionary(this Order_item order_item)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["OrderId"] = order_item.OrderId.ToString();
            output["Price"] = order_item.Price.ToString();
            output["Price_purchase"] = order_item.Price_purchase.ToString();
            output["Comment_Purchase_Price"] = (order_item.Comment_Purchase_Price ?? "").ToString();
            output["ItemId"] = order_item.ItemId.ToString();
            output["Item_ref"] = (order_item.Item_ref ?? "").ToString();
            output["Quantity"] = order_item.Quantity.ToString();
            output["Quantity_current"] = order_item.Quantity_current.ToString();
            output["Quantity_delivery"] = order_item.Quantity_delivery.ToString();
            output["Rank"] = order_item.Rank.ToString();
            output["ID"] = order_item.ID.ToString();

            return output;
        }


        //====================================================================================
        //==================================[ Tax ]===========================================
        //====================================================================================

        public static List<Tax> DataTableTypeToTax(this DataTable TaxDataTable)
        {
            object _lock = new object(); List<Tax> returnList = new List<Tax>();
            if (TaxDataTable != null)
            {
                for (int i = 0; i < TaxDataTable.Rows.Count; i++)
                {
                    Tax tax = new Tax();
                    tax.ID = Utility.intTryParse(TaxDataTable.Rows[i].ItemArray[TaxDataTable.Columns["ID"].Ordinal].ToString());
                    tax.Tax_current = Utility.intTryParse(TaxDataTable.Rows[i].ItemArray[TaxDataTable.Columns["Tax_current"].Ordinal].ToString());
                    tax.Type = (TaxDataTable.Rows[i].ItemArray[TaxDataTable.Columns["Type"].Ordinal] ?? "").ToString();
                    tax.Value = Utility.decimalTryParse(TaxDataTable.Rows[i].ItemArray[TaxDataTable.Columns["Value"].Ordinal].ToString());
                    tax.Date_insert = Utility.convertToDateTime(TaxDataTable.Rows[i].ItemArray[TaxDataTable.Columns["Date_insert"].Ordinal].ToString());
                    tax.Comment = (TaxDataTable.Rows[i].ItemArray[TaxDataTable.Columns["Comment"].Ordinal] ?? "").ToString();

                lock (_lock) returnList.Add(tax);
                }
            }
            return returnList;
        }

        public static List<Tax> filterDataTableToTaxType(this Tax Tax, ESearchOption filterOperator)
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

                return DataTableTypeToTax(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Tax>();
        }

        public static Dictionary<string, string> getColumDictionary(this Tax tax)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["Comment"] = (tax.Comment ?? "").ToString();
            output["Date_insert"] = convertDateToStringFormat(tax.Date_insert.ToString(), "yyyy-MM-dd H:mm:ss");
            output["Tax_current"] = tax.Tax_current.ToString();
            output["Type"] = (tax.Type ?? "").ToString();
            output["Value"] = tax.Value.ToString();
            output["ID"] = tax.ID.ToString();

            return output;
        }



        //====================================================================================
        //===============================[ Provider_item ]===========================================
        //====================================================================================

        public static List<Provider_item> DataTableTypeToProvider_item(this DataTable provider_itemDataTable)
        {
            object _lock = new object(); List<Provider_item> returnList = new List<Provider_item>();
            if (provider_itemDataTable != null)
            {
                for (int i = 0; i < provider_itemDataTable.Rows.Count; i++)
                {
                    Provider_item provider_item = new Provider_item();
                    provider_item.ID = Utility.intTryParse(provider_itemDataTable.Rows[i].ItemArray[provider_itemDataTable.Columns["ID"].Ordinal].ToString());
                    provider_item.ItemId = Utility.intTryParse(provider_itemDataTable.Rows[i].ItemArray[provider_itemDataTable.Columns["ItemId"].Ordinal].ToString());
                    provider_item.ProviderId = Utility.intTryParse(provider_itemDataTable.Rows[i].ItemArray[provider_itemDataTable.Columns["ProviderId"].Ordinal].ToString());
                    provider_item.CurrencyId = Utility.intTryParse(provider_itemDataTable.Rows[i].ItemArray[provider_itemDataTable.Columns["CurrencyId"].Ordinal].ToString());

                    lock (_lock) returnList.Add(provider_item);
                }
            }
            return returnList;
        }

        public static List<Provider_item> filterDataTableToProvider_itemType(this Provider_item Provider_item, ESearchOption filterOperator)
        {
            if (Provider_item != null)
            {
                string baseSqlString = "SELECT * FROM Provider_items WHERE ";
                string defaultSqlString = "SELECT * FROM Provider_items WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Provider_item.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator.ToString(), Provider_item.ID);
                if (Provider_item.ProviderId != 0)
                    query = string.Format(query + " {0} ProviderId LIKE '{1}' ", filterOperator.ToString(), Provider_item.ProviderId);
                if (Provider_item.ItemId != 0)
                    query = string.Format(query + " {0} ItemId LIKE '{1}' ", filterOperator.ToString(), Provider_item.ItemId);
                if (Provider_item.CurrencyId != 0)
                    query = string.Format(query + " {0} CurrencyId LIKE '{1}' ", filterOperator.ToString(), Provider_item.CurrencyId);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToProvider_item(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Provider_item>();
        }

        public static Dictionary<string, string> getColumDictionary(this Provider_item provider_item)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = provider_item.ID.ToString();
            output["ItemId"] = provider_item.ItemId.ToString();
            output["ProviderId"] = provider_item.ProviderId.ToString();
            output["CurrencyId"] = provider_item.CurrencyId.ToString();

            return output;
        }

        //====================================================================================
        //===============================[ Provider ]===========================================
        //====================================================================================

        public static List<Provider> DataTableTypeToProvider(this DataTable providerDataTable)
        {
            object _lock = new object(); List<Provider> returnList = new List<Provider>();
            if (providerDataTable != null)
            {
                for (int i = 0; i < providerDataTable.Rows.Count; i++)
                {
                    Provider provider = new Provider();
                    provider.ID = Utility.intTryParse(providerDataTable.Rows[i].ItemArray[providerDataTable.Columns["ID"].Ordinal].ToString());
                    provider.Name = (providerDataTable.Rows[i].ItemArray[providerDataTable.Columns["Name"].Ordinal] ?? "").ToString();
                    provider.AddressId = Utility.intTryParse(providerDataTable.Rows[i].ItemArray[providerDataTable.Columns["AddressId"].Ordinal].ToString());
                    provider.Source = Utility.intTryParse(providerDataTable.Rows[i].ItemArray[providerDataTable.Columns["Source"].Ordinal].ToString());

                    lock (_lock) returnList.Add(provider);
                }
            }
            return returnList;
        }

        public static List<Provider> filterDataTableToProviderType(this Provider Provider, ESearchOption filterOperator)
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
                if (Provider.AddressId != 0)
                    query = string.Format(query + " {0} AddressId LIKE '{1}' ", filterOperator.ToString(), Provider.AddressId);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToProvider(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Provider>();
        }

        public static Dictionary<string, string> getColumDictionary(this Provider provider)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = provider.ID.ToString();
            output["Name"] = (provider.Name ?? "").ToString();
            output["Source"] = provider.Source.ToString();
            output["AddressId"] = provider.AddressId.ToString();

            return output;
        }

        //====================================================================================
        //===============================[ Item ]===========================================
        //====================================================================================

        public static List<Item> DataTableTypeToItem(this DataTable itemDataTable)
        {
            object _lock = new object(); List<Item> returnList = new List<Item>();
            if (itemDataTable != null)
            {
                for (int i = 0; i < itemDataTable.Rows.Count; i++)
                {
                    Item Item = new Item();
                    Item.ID = Utility.intTryParse(itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["ID"].Ordinal].ToString());
                    Item.Comment = (itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Comment"].Ordinal] ?? "").ToString();
                    Item.Erasable = (itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Erasable"].Ordinal] ?? "").ToString();
                    Item.Name = (itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Name"].Ordinal] ?? "").ToString();
                    Item.Price_purchase = Utility.decimalTryParse(itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Price_purchase"].Ordinal].ToString());
                    Item.Price_sell = Utility.decimalTryParse(itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Price_sell"].Ordinal].ToString());
                    Item.Ref = (itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Ref"].Ordinal] ?? "").ToString();
                    Item.Type_sub = (itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Type_sub"].Ordinal] ?? "").ToString();
                    Item.Source = Utility.intTryParse(itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Source"].Ordinal].ToString());
                    Item.Type = (itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Type"].Ordinal] ?? "").ToString();
                    Item.Picture = (itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Picture"].Ordinal] ?? "").ToString();
                    Item.Stock = Utility.intTryParse(itemDataTable.Rows[i].ItemArray[itemDataTable.Columns["Stock"].Ordinal].ToString());

                    lock (_lock) returnList.Add(Item);
                }
            }
            return returnList;
        }

        public static List<Item> filterDataTableToItemType(this Item item, ESearchOption filterOperator)
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
                if (item.Stock != 0)
                    query = string.Format(query + " {0} Stock LIKE '{1}' ", filterOperator.ToString(), item.Stock);
                if (item.Source != 0)
                    query = string.Format(query + " {0} Source LIKE '{1}' ", filterOperator.ToString(), item.Source);
                if (!string.IsNullOrEmpty(item.Comment))
                    query = string.Format(query + " {0} Comment LIKE '%{1}%' ", filterOperator.ToString(), item.Comment.Replace("'", "''"));
                if (!string.IsNullOrEmpty(item.Erasable))
                    query = string.Format(query + " {0} Erasable LIKE '{1}' ", filterOperator.ToString(), item.Erasable);
                if (!string.IsNullOrEmpty(item.Picture))
                    query = string.Format(query + " {0} Picture LIKE '{1}' ", filterOperator.ToString(), item.Picture);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator.ToString()) + filterOperator.ToString().Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToItem(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Item>();
        }

        public static Dictionary<string, string> getColumDictionary(this Item item)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = item.ID.ToString();
            output["Comment"] = (item.Comment ?? "").ToString();
            output["Erasable"] = (item.Erasable ?? "").ToString();
            output["Name"] = (item.Name ?? "").ToString();
            output["Price_purchase"] = item.Price_purchase.ToString();
            output["Price_sell"] = item.Price_sell.ToString();
            output["Ref"] = (item.Ref ?? "").ToString();
            output["Type_sub"] = (item.Type_sub ?? "").ToString();
            output["Source"] = item.Source.ToString();
            output["Type"] = (item.Type ?? "").ToString();
            output["Picture"] = (item.Picture ?? "").ToString();
            output["Stock"] = item.Stock.ToString();

            return output;
        }

        //====================================================================================
        //===============================[ Item_delivery ]===========================================
        //====================================================================================


        public static List<Item_delivery> DataTableTypeToItem_delivery(this DataTable item_deliveryDataTable)
        {
            object _lock = new object(); List<Item_delivery> returnList = new List<Item_delivery>();
            if (item_deliveryDataTable != null)
            {
                for (int i = 0; i < item_deliveryDataTable.Rows.Count; i++)
                {
                    Item_delivery item_delivery = new Item_delivery();
                    item_delivery.ID = Utility.intTryParse(item_deliveryDataTable.Rows[i].ItemArray[item_deliveryDataTable.Columns["ID"].Ordinal].ToString());
                    item_delivery.DeliveryId = Utility.intTryParse(item_deliveryDataTable.Rows[i].ItemArray[item_deliveryDataTable.Columns["DeliveryId"].Ordinal].ToString());
                    item_delivery.Item_ref = (item_deliveryDataTable.Rows[i].ItemArray[item_deliveryDataTable.Columns["Item_ref"].Ordinal] ?? "").ToString();
                    item_delivery.Quantity_delivery = Utility.intTryParse(item_deliveryDataTable.Rows[i].ItemArray[item_deliveryDataTable.Columns["Quantity_delivery"].Ordinal].ToString());

                    lock (_lock) returnList.Add(item_delivery);
                }
            }
            return returnList;
        }

        public static List<Item_delivery> filterDataTableToItem_deliveryType(this Item_delivery Item_delivery, ESearchOption filterOperator)
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

                return DataTableTypeToItem_delivery(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Item_delivery>();
        }

        public static Dictionary<string, string> getColumDictionary(this Item_delivery item_delivery)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = item_delivery.ID.ToString();
            output["DeliveryId"] = item_delivery.DeliveryId.ToString();
            output["Item_ref"] = (item_delivery.Item_ref ?? "").ToString();
            output["Quantity_delivery"] = item_delivery.Quantity_delivery.ToString();

            return output;
        }

        //====================================================================================
        //===============================[ Tax_item ]===========================================
        //====================================================================================


        public static List<Tax_item> DataTableTypeToTax_item(this DataTable tax_itemDataTable)
        {
            object _lock = new object(); List<Tax_item> returnList = new List<Tax_item>();
            if (tax_itemDataTable != null)
            {
                for (int i = 0; i < tax_itemDataTable.Rows.Count; i++)
                {
                    Tax_item tax_item = new Tax_item();
                    tax_item.ID = Utility.intTryParse(tax_itemDataTable.Rows[i].ItemArray[tax_itemDataTable.Columns["ID"].Ordinal].ToString());
                    tax_item.Item_ref = (tax_itemDataTable.Rows[i].ItemArray[tax_itemDataTable.Columns["Item_ref"].Ordinal] ?? "").ToString();
                    tax_item.Tax_value = Utility.doubleTryParse(tax_itemDataTable.Rows[i].ItemArray[tax_itemDataTable.Columns["Tax_value"].Ordinal].ToString());
                    tax_item.Tax_type = tax_itemDataTable.Rows[i].ItemArray[tax_itemDataTable.Columns["Tax_type"].Ordinal].ToString();
                    tax_item.TaxId = Utility.intTryParse(tax_itemDataTable.Rows[i].ItemArray[tax_itemDataTable.Columns["TaxId"].Ordinal].ToString());

                    lock (_lock) returnList.Add(tax_item);
                }
            }
            return returnList;
        }

        public static List<Tax_item> filterDataTableToTax_itemType(this Tax_item Tax_item, ESearchOption filterOperator)
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

                return DataTableTypeToTax_item(baseSqlString.getDataTableFromSqlCEQuery());

            }
            return new List<Tax_item>();
        }

        public static Dictionary<string, string> getColumDictionary(this Tax_item tax_item)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            output["ID"] = tax_item.ID.ToString();
            output["Tax_value"] = tax_item.Tax_value.ToString();
            output["Item_ref"] = (tax_item.Item_ref ?? "").ToString();
            output["Tax_type"] = (tax_item.Tax_type ?? "").ToString();
            output["TaxId"] = tax_item.TaxId.ToString();

            return output;
        }








    }
}
