using Client.Commands;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Xml;

namespace Client.ViewModels
{
    class RankingViewModel : BaseViewModel
    {
        private RankingModel model;
        private RelayCommand goHomeCommand;
        private DataView rankingDataView;

        public string Username {
            get { return model.User.Username; }
        }

        public int Elo {
            get { return model.User.Elo; }
        }

        public DataView RankingDataView {
            get { return rankingDataView; }
        }

        public ICommand GoHomeCommand {
            get {
                if (goHomeCommand == null) {
                    goHomeCommand = new RelayCommand(_ => {
                        navigator.CurrentViewModel = new HomeViewModel(connection, navigator, model.User);
                    }, _ => true);
                }
                return goHomeCommand;
            }
        }





        public RankingViewModel(ServerConnection connection, Navigator navigator, User user) : base(connection, navigator) {
            this.model = new RankingModel(this.connection, user);
            this.rankingDataView = GetRankingDataView();

        }

        private DataView GetRankingDataView() {
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(model.RankingXML));
            DataTable dt = ds.Tables[0].Clone();
            dt.Columns[1].DataType = typeof(Int32);
            dt.Columns[2].DataType = typeof(Int32);
            foreach (DataRow row in ds.Tables[0].Rows) dt.ImportRow(row);
            DataView dv = dt.DefaultView;
            return dv;
        }


    }
}
