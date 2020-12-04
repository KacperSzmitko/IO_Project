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
        private readonly DataTable rankingDataTable;
        private readonly int userRankingPlace;

        public string Username {
            get { return model.User.Username; }
        }

        public int Elo {
            get { return model.User.Elo; }
        }

        public DataView RankingDataTableView {
            get { return rankingDataTable.DefaultView; }
        }

        public string UserRankingPlaceText {
            get { return "Twoja pozycja: " + userRankingPlace.ToString(); }
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
            this.rankingDataTable = GetRankingDataTable();
            this.userRankingPlace = GetUserRankingPlace();

        }

        private DataTable GetRankingDataTable() {
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(model.RankingXML));
            DataTable dt = ds.Tables[0].Clone();
            dt.Columns[1].DataType = typeof(Int32);
            dt.Columns[2].DataType = typeof(Int32);
            foreach (DataRow row in ds.Tables[0].Rows) dt.ImportRow(row);
            return dt;
        }

        private int GetUserRankingPlace() {
            string query = "Player = '" + model.User.Username + "'";
            DataRow[] rows = rankingDataTable.Select(query);
            return rows[0].Field<int>(2);
        }


    }
}
