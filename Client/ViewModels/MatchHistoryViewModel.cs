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
        private readonly DataTable matchHistoryDataTable;
        private readonly int userWinRatio;

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

        public string UserWinRatioText {
            get { return "Stosunek zwycięstw: " + userWinRatio.ToString() + "%"; }
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
            this.userWinRatio = GetUserWinRatio();

        }

        private DataTable GetMatchHistoryDataTable() {
            DataSet ds = new DataSet();
            ds.ReadXml(new StringReader(model.MatchHistoryXML));
            if (ds.Tables.Count == 0) return null;

            DataTable dt = new DataTable("MatchHistory");
            dt.Columns.Add("numer", typeof(int));
            dt.Columns.Add("przeciwnik", typeof(string));
            dt.Columns.Add("wynik", typeof(string));
            dt.Columns.Add("rezultat", typeof(string));
            dt.Columns.Add("elo", typeof(int));
            dt.Columns.Add("zmiana_elo", typeof(int));

            DataRow newRow = dt.NewRow();
            int i = 1;
            bool isP1User;
            foreach (DataRow row in ds.Tables[0].Rows) {
                if ((string)row["p1Name"] == model.User.Username) isP1User = true;
                else isP1User = false;
                newRow["numer"] = i;
                if (isP1User) newRow["przeciwnik"] = row["p2Name"];
                else newRow["przeciwnik"] = row["p1Name"];
                if (isP1User) newRow["wynik"] = row["p1Points"] + ":" + row["p2Points"];
                else newRow["wynik"] = row["p2Points"] + ":" + row["p1Points"];
                if ((string)row["winnerName"] == model.User.Username) newRow["rezultat"] = "Zwycięstwo";
                else newRow["rezultat"] = "Porażka";
                if (isP1User) newRow["elo"] = row["p1Elo"];
                else newRow["elo"] = row["p2Elo"];
                if (isP1User) newRow["zmiana_elo"] = row["p1EloLoss"];
                else newRow["zmiana_elo"] = row["p2EloLoss"];

                dt.Rows.Add(newRow);
            }
            return dt;
        }

        private int GetUserWinRatio() {
            if (matchHistoryDataTable == null) return 0;
            if (matchHistoryDataTable.Rows.Count == 0) return 0;
            return (matchHistoryDataTable.Select("rezultat = 'Zwycięstwo'").Length / matchHistoryDataTable.Rows.Count) * 100;
        }

    }
}
