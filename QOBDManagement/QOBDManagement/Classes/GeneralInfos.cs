using QOBDCommon.Classes;
using QOBDCommon.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Configuration;
using System.Globalization;
using QOBDCommon.Enum;

namespace QOBDManagement.Classes
{
    public class GeneralInfos : BindBase
    {
        private int _SelectedlistSize;
        private List<int> _listSizeList;
        private List<string> imageSizeList;

        public GeneralInfos()
        {
            _listSizeList = new List<int>();
            generateListSizeList();
        }

        public List<int> ListSizeList
        {
            get { return _listSizeList; }
            set { setProperty(ref _listSizeList, value); }
        }
        
        public int TxtSelectedListSize
        {
            get { return _SelectedlistSize; }
            set { setProperty(ref _SelectedlistSize, value); }
        }

        private void generateListSizeList()
        {
            for (int i = 1; i < 200; i++)
            {
                _listSizeList.Add(i);
            }
        }

        public Dictionary<string, Info> retrieveInfoDataDictionaryFromList(List<Info> infosList, List<string> requestedDataList)
        {
            return getInfoDictionary(infosList, requestedDataList);
        }

        public List<Info> retrieveInfoDataListFromDictionary(Dictionary<string, Info> infoDictionary)
        {
            return getInfoList(infoDictionary);
        }

        private Dictionary<string, Info> getInfoDictionary(List<Info> infosList, List<string> requestedDataList)
        {
            var outputDictionary = new Dictionary<string, Info>();
            foreach (var filter in requestedDataList)
            {
                var match = infosList.Where(x => x.Name.Equals(filter)).ToList();
                if (match.Count() > 0)
                    outputDictionary[filter] = match[0];
                else
                    outputDictionary[filter] = new Info { Name = filter };
            }
            return outputDictionary;
        }

        private List<Info> getInfoList(Dictionary<string, Info> infoDictionary)
        {
            var infosList = new List<Info>();
            foreach (var infoDict in infoDictionary)
                infosList.Add(infoDict.Value);
            return infosList;
        }        

        public List<string> getGeneratedImageSizeList()
        {
            imageSizeList = new List<string>();
            int step = 5;
            for (int i = 5; i <= 800; i = i + step)
            {
                if (i >= 50)
                    step = 25;

                imageSizeList.Add(i.ToString());
            }
            return imageSizeList;
        }

        //======================[ Pair ]=====================
        
        public class Pair<T1, T2>
        {
            public T1 PairID { get; set; }
            public T2 PairValue { get; set; }
        }

        
    }
}
