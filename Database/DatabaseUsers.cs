using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.DatabaseDataSetTableAdapters;

namespace Database
{
    public class DatabaseUsers
    {
        public DatabaseUsers()
        {
            var adapter = new UsersTableAdapter();
            adapter.Fill(new DatabaseDataSet.UsersDataTable());

        }
    }
}
