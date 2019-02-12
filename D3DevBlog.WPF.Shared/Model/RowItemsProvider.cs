using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace D3DevBlog.WPF.Shared.Model
{
    public class RowItemsProvider : ViewModelBase
    {
        static RowItemsProvider _Instance;
        public static RowItemsProvider Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new RowItemsProvider();
                return _Instance;
            }
        }

        private IEnumerable<RowItem> _RowItems;
        public IEnumerable<RowItem> RowItems
        {
            get { return _RowItems; }
            set { SetValue<IEnumerable<RowItem>>(ref _RowItems, value); }
        }

        public IList<string> Cities { private set; get; }
        public IList<string> Countries { private set; get; }
        public IList<string> CountryCodes { private set; get; }

        Random _Random;

        private RowItemsProvider()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<RowItem>));
            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes("Data.xml")))
            {
                RowItems = (IEnumerable<RowItem>)xmlSerializer.Deserialize(ms);
            }

            Cities = RowItems.Select(r => r.City).OrderBy(r => r).Distinct().ToList();
            Countries = RowItems.Select(r => r.Country).OrderBy(r => r).Distinct().ToList();
            CountryCodes = RowItems.Select(r => r.CountryCode).OrderBy(r => r).Distinct().ToList();

            _Random = new Random(DateTime.Now.Millisecond);
        }

        public RowItem GetRandomRowItem()
        {
            return RowItems.ElementAt(_Random.Next(0, RowItems.Count()));
        }
    }
}
