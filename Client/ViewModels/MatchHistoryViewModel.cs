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
    class MatchHistoryViewModel : BaseViewModel
    {
        private MatchHistoryModel model;
        private RelayCommand goHomeCommand;
        private DataTable matchHistoryDataTable;

        public string Username {
            get { return model.User.Username; }
        }

        public int Elo {
            get { return model.User.Elo; }
        }

        public DataView MatchHistoryTableView {
            get { return matchHistoryDataTable.DefaultView; }
        }

        public string MatchHistoryTableVisibility {
            get {
                if (matchHistoryDataTable == null) return "Collapsed";
                return "Visible";
            }
        }

        public string NoMatchHistoryInfoVisibility {
            get {
                if (matchHistoryDataTable == null) return "Visible";
                return "Collapsed";
            }
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

        public MatchHistoryViewModel(ServerConnection connection, Navigator navigator, User user) : base(connection, navigator) {
            this.model = new MatchHistoryModel(this.connection, user);
            this.matchHistoryDataTable = GetMatchHistoryDataTable();

        }

        private DataTable GetMatchHistoryDataTable() {
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(model.MatchHistoryXML));
            if (ds.Tables.Count == 0) return null;
            DataTable dt = ds.Tables[0].Clone();
            //dt.Columns[1].DataType = typeof(Int32);
            //dt.Columns[2].DataType = typeof(Int32);
            //foreach (DataRow row in ds.Tables[0].Rows) dt.ImportRow(row);
            return dt;
        }

    }
}
